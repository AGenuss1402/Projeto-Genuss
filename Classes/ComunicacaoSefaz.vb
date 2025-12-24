Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Policy
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Xml.Schema
Imports Aspose.Pdf
Imports iTextSharp.text.xml.simpleparser
Imports Org.BouncyCastle.Tls
Imports System.Data.OleDb
Imports System.Security.Cryptography.Xml

Public Class ComunicacaoSefaz
    Implements IDisposable

    Private ReadOnly _logger As LoggerNFe
    Private ReadOnly _assinatura As AssinaturaDigitalNFCE
    Private _disposed As Boolean = False

    Public Sub New()
        _logger = LoggerNFe.Instance
        _assinatura = New AssinaturaDigitalNFCE()
    End Sub

    Public Async Function AutorizarNFCeAsync(xmlNFCe As String) As Task(Of ResultadoAutorizacao)

        'VERIFICAÇÃO FINAL ANTES DO ENVIO
        _logger.LogInfo("=== AutorizarNFCeAsync ===")
        _logger.LogInfo($"XML Recebido - Tem declaração: {xmlNFCe.StartsWith("<?xml")}")

        Try
            _logger.LogInfo("Iniciando autorização NFCe no SEFAZ")

            'VERIFICAÇÃO DE CONECTIVIDADE REAL ↓↓↓
            Dim sefazOnline As Boolean = Await TestarConexaoRealSefaz()
            If Not sefazOnline Then
                _logger.LogWarning("⚠️ Falha na verificação de status SEFAZ - tentando envio direto")
                ' Não entrar em contingência, apenas registrar o aviso e continuar
            End If

            ' VERIFICAÇÃO ESPECÍFICA
            _logger.LogInfo("=== VERIFICANDO XML ANTES DE CRIAR LOTE ===")

            ' Conta namespaces no XML de entrada
            Dim countNsEntrada = xmlNFCe.Split(New String() {"xmlns=""http://www.portalfiscal.inf.br/nfe"""}, StringSplitOptions.None).Length - 1
            _logger.LogInfo($"XML entrada tem {countNsEntrada} namespaces")

            ' Salva entrada
            File.WriteAllText("ENTRADA_AutorizarNFCeAsync.xml", xmlNFCe)

            ' Validar assinatura localmente e aplicar fallback de re-assinatura se necessário
            Try
                Dim docCheck As New XmlDocument()
                docCheck.PreserveWhitespace = True
                docCheck.LoadXml(xmlNFCe)
                Dim nsmgr As New XmlNamespaceManager(docCheck.NameTable)
                nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")
                Dim sigNode As XmlNode = docCheck.SelectSingleNode("//ds:Signature", nsmgr)
                If sigNode IsNot Nothing Then
                    Dim sx As New SignedXml(docCheck)
                    sx.LoadXml(CType(sigNode, XmlElement))
                    Dim certificado As X509Certificate2 = CarregarCertificado()
                    If certificado IsNot Nothing AndAlso sx.CheckSignature(certificado, True) Then
                        _logger.LogInfo("✅ Assinatura válida localmente antes do envio")
                    Else
                        _logger.LogWarning("⚠️ Assinatura inválida localmente. Aplicando re-assinatura de fallback.")
                        Dim xmlReassinado = _assinatura.AssinarXmlNFCe(xmlNFCe, certificado)
                        If Not String.IsNullOrEmpty(xmlReassinado) AndAlso xmlReassinado.Contains("<Signature") Then
                            xmlNFCe = xmlReassinado
                            File.WriteAllText("ENTRADA_AutorizarNFCeAsync_REASSINADO.xml", xmlNFCe)
                            _logger.LogInfo("✅ Re-assinatura aplicada com sucesso")
                        Else
                            _logger.LogError("❌ Falha na re-assinatura de fallback")
                        End If
                    End If
                Else
                    _logger.LogWarning("⚠️ Signature não encontrada. Aplicando assinatura.")
                    Dim certificado As X509Certificate2 = CarregarCertificado()
                    Dim xmlAssinado2 = _assinatura.AssinarXmlNFCe(xmlNFCe, certificado)
                    If Not String.IsNullOrEmpty(xmlAssinado2) AndAlso xmlAssinado2.Contains("<Signature") Then
                        xmlNFCe = xmlAssinado2
                        File.WriteAllText("ENTRADA_AutorizarNFCeAsync_ASSINADO.xml", xmlNFCe)
                        _logger.LogInfo("✅ Assinatura aplicada com sucesso")
                    Else
                        _logger.LogError("❌ Falha ao aplicar assinatura")
                    End If
                End If
            Catch exVal As Exception
                _logger.LogWarning("Falha ao validar/assinar localmente antes do envio: " & exVal.Message)
            End Try

            ' Garantir que o QR Code esteja 100% coerente com a nota antes de montar o lote
            Try
                Dim xmlQrValidado = GarantirQrCodeValido(xmlNFCe)
                If Not String.IsNullOrEmpty(xmlQrValidado) Then
                    xmlNFCe = xmlQrValidado
                    File.WriteAllText("ENTRADA_AutorizarNFCeAsync_QR_FIX.xml", xmlNFCe)
                    _logger.LogInfo("✅ QR Code verificado/reconstruído antes do envio")
                End If
            Catch exQr As Exception
                _logger.LogWarning("Falha ao garantir QR Code válido: " & exQr.Message)
            End Try

            ' Criar lote de envio (XML já vem assinado do gerador)
            Dim xmlLoteAssinado = CriarLoteEnvio(xmlNFCe)

            ' Enviar para SEFAZ usando URLs de NFCe
            Dim resultado = Await EnviarParaSefazAsync(xmlLoteAssinado, "NFeAutorizacao4", True)

            ' Processar resposta
            Return ProcessarRespostaAutorizacao(resultado)

        Catch ex As Exception
            _logger.LogError("Erro ao autorizar NFCe", ex)

            ' Retornar objeto padrão com erro
            Return New ResultadoAutorizacao With {
            .Sucesso = False,
            .Mensagem = $"Erro na autorização: {ex.Message}",
            .CodigoStatus = "EXC",
            .NumeroRecibo = "",
            .XmlCompleto = Nothing,
            .Autorizada = False
        }
        End Try
    End Function

    Private Function ExtrairUfDoXml(xml As String) As String
        Try
            If String.IsNullOrEmpty(xml) Then Return strEmitente_estado
            Dim doc As New System.Xml.XmlDocument()
            doc.PreserveWhitespace = True
            doc.LoadXml(xml)
            Dim nsmgr As New System.Xml.XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")
            Dim ufNode As System.Xml.XmlNode = doc.SelectSingleNode("//nfe:emit/nfe:enderEmit/nfe:UF", nsmgr)
            If ufNode Is Nothing Then ufNode = doc.SelectSingleNode("//*[local-name()='emit']/*[local-name()='enderEmit']/*[local-name()='UF']")
            If ufNode IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(ufNode.InnerText) Then
                Return ufNode.InnerText.Trim().ToUpper()
            End If
            Dim cufNode As System.Xml.XmlNode = doc.SelectSingleNode("//nfe:ide/nfe:cUF", nsmgr)
            If cufNode Is Nothing Then cufNode = doc.SelectSingleNode("//*[local-name()='ide']/*[local-name()='cUF']")
            If cufNode IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cufNode.InnerText) Then
                Dim cod As Integer
                If Integer.TryParse(cufNode.InnerText.Trim(), cod) Then
                    Select Case cod
                        Case 11 : Return "RO"
                        Case 12 : Return "AC"
                        Case 13 : Return "AM"
                        Case 14 : Return "RR"
                        Case 15 : Return "PA"
                        Case 16 : Return "AP"
                        Case 17 : Return "TO"
                        Case 21 : Return "MA"
                        Case 22 : Return "PI"
                        Case 23 : Return "CE"
                        Case 24 : Return "RN"
                        Case 25 : Return "PB"
                        Case 26 : Return "PE"
                        Case 27 : Return "AL"
                        Case 28 : Return "SE"
                        Case 29 : Return "BA"
                        Case 31 : Return "MG"
                        Case 32 : Return "ES"
                        Case 33 : Return "RJ"
                        Case 35 : Return "SP"
                        Case 41 : Return "PR"
                        Case 42 : Return "SC"
                        Case 43 : Return "RS"
                        Case 50 : Return "MS"
                        Case 51 : Return "MT"
                        Case 52 : Return "GO"
                        Case 53 : Return "DF"
                    End Select
                End If
            End If
            Return strEmitente_estado
        Catch
            Return strEmitente_estado
        End Try
    End Function

    Public Async Function ConsultarReciboAsync(numeroRecibo As String) As Task(Of ResultadoConsultaRecibo)
        Try
            _logger.LogInfo($"Consultando recibo: {numeroRecibo}")

            ' 1. Criar XML de consulta
            Dim xmlConsultaString As String = CriarXmlConsultaRecibo(numeroRecibo)

            ' ✅ Converter string para XmlDocument
            Dim xmlConsultaDoc As New XmlDocument()
            xmlConsultaDoc.PreserveWhitespace = True
            xmlConsultaDoc.LoadXml(xmlConsultaString)

            File.WriteAllText("consulta_recibo_antes_assinatura.xml", xmlConsultaString, System.Text.Encoding.UTF8)

            ' 2. Carregar certificado
            Dim certificado As X509Certificate2 = CarregarCertificado()
            If certificado Is Nothing Then
                Throw New Exception("Certificado não carregado")
            End If

            ' 3. Assinar XML (agora passando o XmlDocument ou sua string)
            Dim xmlAssinado As String = ""

            ' Verificar qual assinatura você está usando:
            If _assinatura Is Nothing Then
                ' Se _assinatura não foi inicializado
                Dim assinaturaTemp As New AssinaturaDigitalNFCE()
                xmlAssinado = assinaturaTemp.AssinarXmlNFCe(xmlConsultaDoc.OuterXml, certificado)
            Else
                ' Usar _assinatura existente
                xmlAssinado = _assinatura.AssinarXmlNFCe(xmlConsultaDoc.OuterXml, certificado)
            End If

            ' Verificar se a assinatura foi aplicada
            If String.IsNullOrEmpty(xmlAssinado) Then
                Throw New Exception("Assinatura retornou string vazia")
            End If

            File.WriteAllText("consulta_recibo_assinado.xml", xmlAssinado, System.Text.Encoding.UTF8)
            _logger.LogInfo($"XML assinado criado com {xmlAssinado.Length} caracteres")

            ' 4. Enviar para SEFAZ
            Dim resultado As String = Await EnviarParaSefazAsync(xmlAssinado, "NfeRetAutorizacao", True)

            File.WriteAllText("consulta_recibo_resposta.xml", resultado, System.Text.Encoding.UTF8)

            ' 5. Processar resposta
            Return ProcessarRespostaConsultaRecibo(resultado)

        Catch ex As Exception
            _logger.LogError($"Erro ao consultar recibo {numeroRecibo}", ex)
            Throw New SefazException($"Erro na consulta: {ex.Message}", "", ex.Message)
        End Try
    End Function

    Public Async Function ConsultarSituacaoAsync(chaveAcesso As String) As Task(Of ResultadoConsultaSituacao)
        Try
            _logger.LogInfo($"Consultando situação NFCe: {chaveAcesso}")

            ' 1. Criar XML de consulta
            Dim xmlConsulta As String = CriarXmlConsultaSituacao(chaveAcesso)

            ' ✅ DEBUG: Salvar XML de consulta
            File.WriteAllText("consulta_situacao_xml.xml", xmlConsulta, System.Text.Encoding.UTF8)
            _logger.LogInfo($"XML consulta criado: {xmlConsulta.Length} caracteres")
            _logger.LogInfo($"Primeiros 200 chars: {xmlConsulta.Substring(0, Math.Min(200, xmlConsulta.Length))}")

            ' 2. Carregar certificado (apenas para TLS/SSL)
            Dim certificado As X509Certificate2 = CarregarCertificado()
            If certificado Is Nothing Then
                _logger.LogWarning("Certificado não carregado - continuando sem assinatura")
            End If

            ' 3. ✅ CORREÇÃO: Consultas de situação NÃO exigem assinatura pelo padrão SEFAZ
            ' Apenas enviamos o XML sem assinatura
            Dim xmlParaEnviar As String = xmlConsulta

            ' Se você realmente precisa assinar (alguns estados exigem), use:
            ' Dim assinatura As New AssinaturaDigitalNFCE()
            ' xmlParaEnviar = assinatura.AssinarXmlNFCe(xmlConsulta, certificado)

            _logger.LogInfo($"Enviando {xmlParaEnviar.Length} caracteres para SEFAZ (sem assinatura)")

            ' 4. Enviar para SEFAZ
            Dim resultado As String = Await EnviarParaSefazAsync(xmlParaEnviar, "NfeConsultaProtocolo", True)

            ' ✅ DEBUG: Salvar resposta
            File.WriteAllText("consulta_situacao_resposta.xml", resultado, System.Text.Encoding.UTF8)
            _logger.LogInfo($"Resposta SEFAZ: {resultado.Length} caracteres")

            ' 5. Processar resposta
            Return ProcessarRespostaConsultaSituacao(resultado)

        Catch ex As Exception
            _logger.LogError($"Erro ao consultar situação {chaveAcesso}", ex)
            Throw New SefazException($"Erro na consulta: {ex.Message}", "", ex.Message)
        End Try
    End Function

    Public Async Function CancelarNFCeAsync(dadosCancelamento As DadosCancelamentoNFCe) As Task(Of ResultadoCancelamento)
        Try
            _logger.LogInfo($"Cancelando NFCe: {dadosCancelamento.ChaveAcesso}")

            ' Criar XML de cancelamento
            Dim xmlCancelamento = CriarXmlCancelamento(dadosCancelamento)

            ' Assinar XML
            '  Dim xmlAssinado = _assinatura.AssinarXml(xmlCancelamento, "infEvento")

            ' Criar envelope de evento
            '  Dim xmlEvento = CriarEnvelopeEvento(xmlAssinado)

            ' Enviar para SEFAZ
            '  Dim resultado = Await EnviarParaSefazAsync(xmlEvento, "RecepcaoEvento")

            ' Processar resposta
            '  Return ProcessarRespostaCancelamento(resultado)

        Catch ex As Exception
            _logger.LogError($"Erro ao cancelar NFCe {dadosCancelamento.ChaveAcesso}", ex)
            Throw New SefazException($"Erro no cancelamento: {ex.Message}", "", ex.Message)
        End Try
    End Function
    Private Function Sha1Hex(texto As String) As String
        Using sha1 As System.Security.Cryptography.SHA1 = System.Security.Cryptography.SHA1.Create()
            Dim bytes = System.Text.Encoding.UTF8.GetBytes(texto)
            Dim hashBytes = sha1.ComputeHash(bytes)
            Dim sb As New System.Text.StringBuilder()
            For Each b In hashBytes
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    ' Valida o conteúdo da tag <qrCode> conforme pattern NFC-e e reconstrói se necessário
    Private Function GarantirQrCodeValido(xml As String) As String
        Try
            If String.IsNullOrEmpty(xml) Then Return xml

            Dim doc As New System.Xml.XmlDocument()
            doc.PreserveWhitespace = True
            doc.LoadXml(xml)

            Dim nsmgr As New System.Xml.XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            Dim qrNode As System.Xml.XmlNode = doc.SelectSingleNode("//*[local-name()='qrCode']", nsmgr)
            If qrNode Is Nothing OrElse String.IsNullOrWhiteSpace(qrNode.InnerText) Then
                Return xml
            End If

            Dim valor = qrNode.InnerText
            valor = System.Text.RegularExpressions.Regex.Replace(valor, "[\u0000-\u001F]", "")
            valor = System.Text.RegularExpressions.Regex.Replace(valor, "[`\u2018\u2019\u201C\u201D\u00B4]", "")
            valor = System.Text.RegularExpressions.Regex.Replace(valor, "[\u00A0\u2000-\u200B\u202F\u205F\u3000]", "")
            valor = System.Text.RegularExpressions.Regex.Replace(valor, "[\""']", "") ' remove aspas simples e duplas
            valor = valor.Trim()

            ' Remover espaços/crases remanescentes para garantir que o conteúdo seja uma URI válida
            valor = valor.Replace(" ", "")

            Dim rgxValido As New System.Text.RegularExpressions.Regex("^https?://[^\\s]+\\?p=\\d{44}\\|\\d{1,2}\\|[12]\\|\\d{1,6}\\|[0-9A-Fa-f]{40}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            If Not rgxValido.IsMatch(valor) Then
                Dim chave As String = Nothing
                Dim versao As String = "2"
                Dim tpAmb As String = Nothing
                Dim idToken As String = stremitenteIDCSC
                idToken = If(idToken, "").Trim()
                ' Se idToken estiver zero-padded mas o ID real for de 1 dígito, normaliza (ex.: "000002" -> "2")
                If idToken.Length > 1 Then
                    Dim idSemZeros = idToken.TrimStart("0"c)
                    If idSemZeros.Length = 1 AndAlso System.Text.RegularExpressions.Regex.IsMatch(idSemZeros, "^\d$") Then
                        idToken = idSemZeros
                    End If
                End If
                Dim csc As String = strEmitenteCodigoCSC
                Dim baseUrl As String = Nothing

                ' Tentar extrair p=... existente para obter parâmetros
                Dim rgxP As New System.Text.RegularExpressions.Regex("\\?p=(\\d{44})\\|([0-9]+)\\|([12])\\|(\\d+)\\|([0-9A-Fa-f]{40})")
                Dim mp = rgxP.Match(valor)
                If mp.Success Then
                    chave = mp.Groups(1).Value
                    versao = mp.Groups(2).Value
                    tpAmb = mp.Groups(3).Value
                    If String.IsNullOrEmpty(idToken) Then idToken = mp.Groups(4).Value
                End If

                ' Base URL a partir do urlChave
                Dim urlChaveNode = doc.SelectSingleNode("//*[local-name()='urlChave']", nsmgr)
                If urlChaveNode IsNot Nothing Then
                    Dim rawUrl = urlChaveNode.InnerText.Trim()
                    Try
                        Dim u As New Uri(rawUrl)
                        baseUrl = $"{u.Scheme}://{u.Host}{u.AbsolutePath}"
                    Catch
                    End Try
                End If

                If String.IsNullOrWhiteSpace(baseUrl) Then
                    Dim ufXml = ExtrairUfDoXml(xml)
                    Dim tpAmbUrl = If((strEmitente_nfce_ambiente & "").ToLower().Contains("prod"), "1", "2")
                    Select Case ufXml
                        Case "MT"
                            baseUrl = If(tpAmbUrl = "1", "https://www.sefaz.mt.gov.br/nfce/consultanfce", "https://homologacao.sefaz.mt.gov.br/nfce/consultanfce")
                        Case "SP"
                            baseUrl = If(tpAmbUrl = "1", "https://www.fazenda.sp.gov.br/nfce/qrcode", "https://homologacao.nfce.fazenda.sp.gov.br/qrcode")
                        Case "PR"
                            baseUrl = If(tpAmbUrl = "1", "https://www.fazenda.pr.gov.br/nfce/qrcode", "https://homologacao.fazenda.pr.gov.br/nfce/qrcode")
                        Case "RS"
                            baseUrl = If(tpAmbUrl = "1", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx")
                        Case Else
                            baseUrl = If(tpAmbUrl = "1", "https://www.sefaz.mt.gov.br/nfce/consultanfce", "https://homologacao.sefaz.mt.gov.br/nfce/consultanfce")
                    End Select
                End If

                If String.IsNullOrEmpty(tpAmb) Then
                    tpAmb = If((strEmitente_nfce_ambiente & "").ToLower().Contains("prod"), "1", "2")
                End If

                If String.IsNullOrEmpty(chave) Then
                    Dim infNFe = doc.SelectSingleNode("//*[local-name()='infNFe']")
                    If infNFe IsNot Nothing AndAlso infNFe.Attributes IsNot Nothing AndAlso infNFe.Attributes("Id") IsNot Nothing Then
                        Dim idVal = infNFe.Attributes("Id").Value
                        If idVal.StartsWith("NFe") AndAlso idVal.Length >= 48 Then
                            chave = idVal.Substring(3)
                        End If
                    End If
                End If

                If String.IsNullOrEmpty(chave) OrElse String.IsNullOrEmpty(idToken) OrElse String.IsNullOrEmpty(csc) Then
                    _logger.LogWarning("[QrCode] Parâmetros insuficientes para reconstrução. Mantendo valor saneado.")
                Else
                    Dim textoHash = $"{chave}|{versao}|{tpAmb}|{idToken}|{csc}"
                    Using sha1 As System.Security.Cryptography.SHA1 = System.Security.Cryptography.SHA1.Create()
                        Dim hb = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(textoHash))
                        Dim hs As New System.Text.StringBuilder()
                        For Each b In hb
                            hs.Append(b.ToString("X2"))
                        Next
                        Dim hashHex = hs.ToString()
                        valor = $"{baseUrl}?p={chave}|{versao}|{tpAmb}|{idToken}|{hashHex}"
                    End Using
                    _logger.LogInfo($"[QrCode] QR Code reconstruído: '{valor}'")
                End If
            End If

            qrNode.InnerText = valor

            ' Sanitizar urlChave: remover controle/crases/aspas curvas e percent-encodar espaços/crases
            Dim urlChaveNode2 = doc.SelectSingleNode("//*[local-name()='urlChave']", nsmgr)
            If urlChaveNode2 IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(urlChaveNode2.InnerText) Then
                Dim uval = urlChaveNode2.InnerText
                uval = System.Text.RegularExpressions.Regex.Replace(uval, "[\u0000-\u001F]", "")
                uval = System.Text.RegularExpressions.Regex.Replace(uval, "[`\u2018\u2019\u201C\u201D\u00B4]", "")
                uval = System.Text.RegularExpressions.Regex.Replace(uval, "[\u00A0\u2000-\u200B\u202F\u205F\u3000]", "")
                uval = System.Text.RegularExpressions.Regex.Replace(uval, "[\""']", "")
                uval = uval.Trim()
                uval = uval.Replace(" ", "")
                urlChaveNode2.InnerText = uval
            End If
            Return doc.OuterXml
        Catch ex As Exception
            _logger.LogWarning($"[QrCode] Falha ao validar/reconstruir QR Code: {ex.Message}")
            Return xml
        End Try
    End Function

    ' Sanitização global: remove crases/backticks e espaços invisíveis em todo o XML
    Private Function SanearXmlConteudo(xml As String) As String
        If String.IsNullOrEmpty(xml) Then Return xml
        Try
            Dim s = xml
            ' Remover BOM em qualquer posição
            s = s.Replace(ChrW(&HFEFF), "")
            ' Remover caracteres de controle (exceto \t, \n, \r)
            s = System.Text.RegularExpressions.Regex.Replace(s, "[\u0000-\u0008\u000B\u000C\u000E-\u001F]", "")
            ' Remover crases/backticks e aspas curvas
            s = System.Text.RegularExpressions.Regex.Replace(s, "[`\u2018\u2019\u201C\u201D\u00B4]", "")
            ' Remover espaços invisíveis
            s = System.Text.RegularExpressions.Regex.Replace(s, "[\u00A0\u2000-\u200B\u202F\u205F\u3000]", "")
            Return s
        Catch
            Return xml
        End Try
    End Function

    Private Function CriarLoteEnvio(xmlNFCe As String) As String
        Dim numeroLote = DateTime.Now.ToString("yyyyMMddHHmmss")
        _logger.LogInfo("=== CRIAR LOTE ENVIO (DOM) PRESERVANDO ASSINATURA ===")

        ' Construir enviNFe por concatenação preservando EXATAMENTE o NFe assinado
        Dim xmlEntrada = xmlNFCe.Replace(ChrW(&HFEFF), "").Trim()
        Dim inner As String = xmlEntrada
        inner = System.Text.RegularExpressions.Regex.Replace(inner, "^\s*<\?xml[^>]*\?>", "", System.Text.RegularExpressions.RegexOptions.Singleline)
        Dim posNFe = inner.IndexOf("<NFe", StringComparison.OrdinalIgnoreCase)
        If posNFe > 0 Then inner = inner.Substring(posNFe)
        If Not inner.StartsWith("<NFe") Then
            Throw New SefazException("Conteúdo interno não inicia com <NFe", "LOTE-INNER", "")
        End If
        Dim sb As New StringBuilder()
        sb.Append("<?xml version=""1.0"" encoding=""UTF-8""?>")
        sb.Append("<enviNFe xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""4.00"">")
        sb.Append("<idLote>").Append(numeroLote).Append("</idLote>")
        sb.Append("<indSinc>1</indSinc>")
        sb.Append(inner)
        sb.Append("</enviNFe>")
        Dim xmlFinal As String = sb.ToString()

        ' Verificar digest do infNFe no XML final
        Try
            Dim docVer As New XmlDocument()
            docVer.PreserveWhitespace = True
            docVer.LoadXml(xmlFinal)
            Dim infNFeNode = docVer.GetElementsByTagName("infNFe")(0)
            If infNFeNode IsNot Nothing Then
                Dim tempDoc As New XmlDocument()
                tempDoc.PreserveWhitespace = True
                Dim cloned = tempDoc.ImportNode(infNFeNode, True)
                tempDoc.AppendChild(cloned)
                Dim c14n As New System.Security.Cryptography.Xml.XmlDsigExcC14NTransform()
                c14n.LoadInput(tempDoc)
                Dim canonicalBytes As Byte()
                Using s = CType(c14n.GetOutput(GetType(Stream)), Stream)
                    Using ms2 As New MemoryStream()
                        s.CopyTo(ms2)
                        canonicalBytes = ms2.ToArray()
                    End Using
                End Using
                Dim digestCalc As String
                Using sha1 = System.Security.Cryptography.SHA1.Create()
                    digestCalc = Convert.ToBase64String(sha1.ComputeHash(canonicalBytes))
                End Using
                Dim nsmgr As New XmlNamespaceManager(docVer.NameTable)
                nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")
                Dim digestNode = docVer.SelectSingleNode("//ds:DigestValue", nsmgr)
                Dim digestSigned = If(digestNode?.InnerText, "")
                If Not String.IsNullOrEmpty(digestSigned) AndAlso digestSigned <> digestCalc Then
                    _logger.LogWarning($"⚠️ Digest mismatch após montar enviNFe. Assinado={digestSigned} Calculado={digestCalc}.")
                Else
                    _logger.LogInfo("✅ Digest confere após montagem do enviNFe (concatenação)")
                End If
            End If
        Catch ex As Exception
            _logger.LogWarning("Falha na verificação de digest do lote: " & ex.Message)
        End Try

        ' Log de diagnóstico
        _logger.LogInfo($"XML final em UTF-8: {System.Text.Encoding.UTF8.GetByteCount(xmlFinal)} bytes")

        ' Salvar XML de envio para diagnóstico
        Try
            Dim caminhoDebug As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug_xml_envio_enviNFe.xml")
            System.IO.File.WriteAllText(caminhoDebug, xmlFinal, System.Text.Encoding.UTF8)
            _logger.LogInfo($"XML de envio salvo para debug: {caminhoDebug}")
        Catch ex As Exception
            _logger.LogWarning("Falha ao salvar XML de envio para debug: " & ex.Message)
        End Try

        Return xmlFinal
    End Function


    Private Function CriarXmlConsultaRecibo(numeroRecibo As String) As String
        Return $"<?xml version=""1.0"" encoding=""UTF-8""?>
<consReciNFe xmlns = ""http://www.portalfiscal.inf.br/nfe"" versao=""4.00"">
    <tpAmb>{strEmitente_nfce_ambiente}</tpAmb>
    <nRec>{numeroRecibo}</nRec>
</consReciNFe>"
    End Function

    Private Function CriarXmlConsultaSituacao(chaveAcesso As String) As String
        Return $"<?xml version=""1.0"" encoding=""UTF-8""?>
<consSitNFe xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""4.00"">
    <tpAmb>{strEmitente_nfce_ambiente}</tpAmb>
    <xServ>CONSULTAR</xServ>
    <chNFe>{chaveAcesso}</chNFe>
</consSitNFe>"
    End Function

    Private Function CriarXmlCancelamento(dados As DadosCancelamentoNFCe) As String
        Dim idEvento = $"ID110111{dados.ChaveAcesso}{dados.SequencialEvento.ToString("D2")}"

        Return $"<?xml version=""1.0"" encoding=""UTF-8""?>
<evento xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""1.00"">
    <infEvento Id=""{idEvento}"">
        <cOrgao>{dados.ChaveAcesso.Substring(0, 2)}</cOrgao>
        <tpAmb>{strEmitente_nfce_ambiente}</tpAmb>
        <CNPJ>{strEmitente_CNPJ}</CNPJ>
        <chNFe>{dados.ChaveAcesso}</chNFe>
        <dhEvento>{dados.DataCancelamento:yyyy-MM-ddTHH:mm:sszzz}</dhEvento>
        <tpEvento>110111</tpEvento>
        <nSeqEvento>{dados.SequencialEvento}</nSeqEvento>
        <verEvento>1.00</verEvento>
        <detEvento versao=""1.00"">
            <descEvento>Cancelamento</descEvento>
            <nProt>{dados.ProtocoloAutorizacao}</nProt>
            <xJust>{dados.Justificativa}</xJust>
        </detEvento>
    </infEvento>
</evento>"
    End Function

    Private Function CarregarCertificado() As X509Certificate2
        Try
            _logger.LogInfo($"Tentando carregar certificado (tamanho: {strEmitenteCertificado?.Length} caracteres)")

            ' Verificar se é caminho de arquivo
            If System.IO.File.Exists(strEmitenteCertificado) Then
                _logger.LogInfo("Carregando certificado de arquivo")
                Return New X509Certificate2(strEmitenteCertificado, strEmitenteSenhaCertificado,
                                      X509KeyStorageFlags.MachineKeySet Or
                                      X509KeyStorageFlags.PersistKeySet)
                ' Verificar se é certificado em Base64 (string muito longa)
            ElseIf Not String.IsNullOrEmpty(strEmitenteCertificado) AndAlso strEmitenteCertificado.Length > 100 Then
                Try
                    _logger.LogInfo("Tentando carregar certificado de Base64")
                    ' Limpar possíveis caracteres de quebra de linha e espaços
                    Dim base64Limpo = strEmitenteCertificado.Replace(vbCrLf, "").Replace(vbCr, "").Replace(vbLf, "").Replace(" ", "")

                    ' Verificar se é um Base64 válido
                    If base64Limpo.Length Mod 4 = 0 Then
                        Dim certificadoBytes As Byte() = Convert.FromBase64String(base64Limpo)
                        _logger.LogInfo($"Certificado Base64 decodificado com sucesso ({certificadoBytes.Length} bytes)")
                        Return New X509Certificate2(certificadoBytes, strEmitenteSenhaCertificado,
                                              X509KeyStorageFlags.MachineKeySet Or
                                              X509KeyStorageFlags.PersistKeySet)
                    Else
                        _logger.LogWarning("String Base64 com tamanho inválido")
                    End If
                Catch base64Ex As Exception
                    _logger.LogWarning($"Erro ao decodificar Base64: {base64Ex.Message}")
                    ' Continuar para busca no repositório
                End Try
            End If

            ' Buscar no repositório do Windows
            If Not String.IsNullOrEmpty(strEmitenteCertificado) Then
                _logger.LogInfo("Procurando certificado no repositório do Windows")

                ' Tentar diferentes repositórios
                Dim stores() As X509Store = {
                    New X509Store(StoreName.My, StoreLocation.CurrentUser),
                    New X509Store(StoreName.My, StoreLocation.LocalMachine)
                }

                For Each store In stores
                    Try
                        store.Open(OpenFlags.ReadOnly)
                        _logger.LogInfo($"Procurando em: {store.Location}/{store.Name}")

                        ' Buscar por thumbprint primeiro (mais confiável)
                        If strEmitenteCertificado.Length >= 20 Then
                            Try
                                Dim certificados = store.Certificates.Find(
                                    X509FindType.FindByThumbprint,
                                    strEmitenteCertificado.Replace(" ", "").Replace(":", ""),
                                    False)

                                If certificados.Count > 0 Then
                                    _logger.LogInfo($"Certificado encontrado por thumbprint: {certificados(0).Subject}")
                                    Return certificados(0)
                                End If
                            Catch ex As Exception
                                _logger.LogWarning($"Erro ao buscar por thumbprint: {ex.Message}")
                            End Try
                        End If

                        ' Buscar por subject name
                        Try
                            Dim certificados = store.Certificates.Find(
                                X509FindType.FindBySubjectName,
                                strEmitenteCertificado,
                                False)

                            If certificados.Count > 0 Then
                                _logger.LogInfo($"Certificado encontrado por subject: {certificados(0).Subject}")
                                Return certificados(0)
                            End If
                        Catch ex As Exception
                            _logger.LogWarning($"Erro ao buscar por subject: {ex.Message}")
                        End Try

                        ' Buscar por número de série (com tratamento de erro melhorado)
                        If strEmitenteCertificado.Length <= 40 Then ' Tamanho razoável para serial
                            Try
                                Dim serialLimpo = strEmitenteCertificado.Replace(" ", "").Replace(":", "").ToUpper()
                                Dim certificados = store.Certificates.Find(
                                    X509FindType.FindBySerialNumber,
                                    serialLimpo,
                                    False)

                                If certificados.Count > 0 Then
                                    _logger.LogInfo($"Certificado encontrado por serial: {certificados(0).Subject}")
                                    Return certificados(0)
                                End If
                            Catch ex As Exception
                                _logger.LogWarning($"Erro ao buscar por serial: {ex.Message}")
                            End Try
                        End If

                    Finally
                        store.Close()
                    End Try
                Next

                ' Listar certificados disponíveis para debug
                _logger.LogWarning("Certificado não encontrado. Certificados disponíveis:")
                Try
                    Using debugStore As New X509Store(StoreName.My, StoreLocation.CurrentUser)
                        debugStore.Open(OpenFlags.ReadOnly)
                        For Each cert In debugStore.Certificates
                            _logger.LogInfo($"- Subject: {cert.Subject}, Serial: {cert.SerialNumber}, Thumbprint: {cert.Thumbprint}")
                        Next
                    End Using
                Catch
                    ' Ignorar erros de debug
                End Try

                Throw New Exception($"Certificado não encontrado: {strEmitenteCertificado}")
            Else
                ' Caso strEmitenteCertificado esteja vazio ou nulo
                Throw New Exception("Certificado não configurado")
            End If

        Catch ex As Exception
            _logger.LogError("Erro ao carregar certificado", ex)
            Throw New SefazException($"Erro no certificado: {ex.Message}", "CERT-001", ex.Message)
        End Try
    End Function

    Private Function ObterConfiguracaoTimeout() As Integer
        Try
            ' Tentar obter do banco
            Dim timeoutConfig As Integer = 0
            If Integer.TryParse(strEmitenteTimeOut, timeoutConfig) Then
                ' Se for muito baixo, usar valor mínimo seguro
                If timeoutConfig < 30000 Then ' Menos de 30 segundos
                    _logger.LogWarning($"Timeout muito baixo ({timeoutConfig}ms). Usando mínimo de 60 segundos.")
                    Return 60000
                ElseIf timeoutConfig > 300000 Then ' Mais de 5 minutos
                    _logger.LogWarning($"Timeout muito alto ({timeoutConfig}ms). Usando máximo de 5 minutos.")
                    Return 300000
                End If
                Return timeoutConfig
            Else
                _logger.LogWarning($"Timeout inválido ({strEmitenteTimeOut}). Usando padrão de 60s.")
                Return 60000
            End If

        Catch ex As Exception
            _logger.LogWarning($"Erro ao obter timeout, usando padrão de 60s: {ex.Message}")
            Return 60000 ' Fallback: 60 segundos
        End Try
    End Function

    Private Async Function EnviarParaSefazAsync(xmlEnvio As String, servico As String, Optional isNFCe As Boolean = False) As Task(Of String)
        Dim tentativaAtual As Integer = 0
        Dim maxTentativas As Integer = 3
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()

        _logger.LogInfo($"Iniciando envio para SEFAZ - Serviço: {servico}, NFCe: {isNFCe}")

        ' ✅ CORREÇÃO CRÍTICA: VERIFICAR ENCODING ANTES DE QUALQUER PROCESSAMENTO
        _logger.LogInfo("=== VERIFICAÇÃO INICIAL DO ENCODING ===")

        ' Verifica encoding atual
        Dim bytesIniciais = System.Text.Encoding.UTF8.GetBytes(xmlEnvio)
        _logger.LogInfo($"Tamanho inicial em bytes UTF-8: {bytesIniciais.Length}")

        ' Verifica se tem BOM
        If bytesIniciais.Length >= 3 AndAlso bytesIniciais(0) = &HEF AndAlso bytesIniciais(1) = &HBB AndAlso bytesIniciais(2) = &HBF Then
            _logger.LogWarning("⚠️ XML tem BOM (Byte Order Mark) - removendo...")
            xmlEnvio = System.Text.Encoding.UTF8.GetString(bytesIniciais, 3, bytesIniciais.Length - 3)
        End If

        ' Garante declaração UTF-8
        xmlEnvio = GarantirDeclaracaoXmlUTF8(xmlEnvio)

        ' Não alterar conteúdo assinado de xmlEnvio

        ' Log após correção
        Dim bytesFinais = System.Text.Encoding.UTF8.GetBytes(xmlEnvio)
        _logger.LogInfo($"Após correção: {bytesFinais.Length} bytes UTF-8 (sem BOM)")

        While tentativaAtual < maxTentativas
            tentativaAtual += 1
            Try
                stopwatch.Restart()
                _logger.LogInfo($"Tentativa {tentativaAtual}/{maxTentativas}")

                ' ↓↓↓ CONFIGURAÇÃO SIMPLIFICADA DO HANDLER ↓↓↓
                Using handler As New HttpClientHandler()
                    handler.ServerCertificateCustomValidationCallback = Function(sender, cert, chain, sslPolicyErrors) True
                    handler.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
                    handler.UseProxy = False

                    ' ↓↓↓ CERTIFICADO DIGITAL ↓↓↓
                    Dim certificado = CarregarCertificado()
                    If certificado IsNot Nothing Then
                        handler.ClientCertificates.Add(certificado)
                        _logger.LogInfo($"Certificado carregado: {certificado.Subject}")
                    Else
                        Throw New SefazException("Certificado digital não encontrado", "CERT-001", "")
                    End If

                    Using client As New HttpClient(handler)
                        client.Timeout = TimeSpan.FromSeconds(60)

                        ' Não modificar xmlEnvio após assinado; apenas trim externo
                        xmlEnvio = xmlEnvio.Trim()

                        Dim url = ObterUrlServico(servico, isNFCe)
                        Dim soapEnvelop = CriarEnvelopeSOAPCorreto(xmlEnvio, servico)

                        ' Logar o QRCode final para envio (somente leitura)
                        Try
                            Dim qPosStart2 = xmlEnvio.IndexOf("<qrCode>")
                            Dim qPosEnd2 = xmlEnvio.IndexOf("</qrCode>")
                            If qPosStart2 > -1 AndAlso qPosEnd2 > qPosStart2 Then
                                Dim finalQr2 = xmlEnvio.Substring(qPosStart2 + 8, qPosEnd2 - qPosStart2 - 8)
                                _logger.LogInfo($"[QrCode] Final enviado (xmlEnvio): {finalQr2}")
                            End If
                        Catch exLog2 As Exception
                            _logger.LogWarning("Falha ao logar qrCode final (xmlEnvio): " & exLog2.Message)
                        End Try

                        ' ✅ VERIFICAÇÃO FINAL DE ENCODING ANTES DO ENVIO
                        _logger.LogInfo("=== VERIFICAÇÃO FINAL DE ENCODING ===")

                        ' Converte para bytes UTF-8 sem BOM
                        Dim utf8NoBom As New System.Text.UTF8Encoding(False)
                        Dim bytesEnvio As Byte() = utf8NoBom.GetBytes(soapEnvelop)

                        ' Verifica se tem BOM
                        If bytesEnvio.Length >= 3 AndAlso bytesEnvio(0) = &HEF AndAlso bytesEnvio(1) = &HBB AndAlso bytesEnvio(2) = &HBF Then
                            _logger.LogError("❌ ERRO: Envelope SOAP ainda tem BOM!")
                            ' Remove BOM
                            bytesEnvio = bytesEnvio.Skip(3).ToArray()
                        End If

                        ' Garantir que o primeiro byte seja '<' (0x3C). Se houver lixo antes, remover.
                        If bytesEnvio.Length = 0 Then
                            Throw New SefazException("Envelope SOAP vazio após serialização", "SOAP-EMPTY", "")
                        End If
                        Dim idxFirstLt As Integer = Array.FindIndex(bytesEnvio, Function(b) b = &H3C)
                        If idxFirstLt = -1 Then
                            _logger.LogError("❌ Nenhum caractere '<' encontrado no início do envelope SOAP")
                            Throw New SefazException("Envelope SOAP inválido: não inicia com '<'", "SOAP-INV", "")
                        End If
                        If idxFirstLt > 0 Then
                            _logger.LogWarning($"⚠️ Removendo {idxFirstLt} bytes antes do primeiro '<' no envelope SOAP")
                            bytesEnvio = bytesEnvio.Skip(idxFirstLt).ToArray()
                        End If

                        _logger.LogInfo($"Bytes para envio: {bytesEnvio.Length} (UTF-8 sem BOM)")

                        ' Log de diagnóstico dos bytes para detectar BOM/encoding
                        Dim prefixHex = BitConverter.ToString(bytesEnvio.Take(Math.Min(64, bytesEnvio.Length)).ToArray())
                        Dim suffixHex = BitConverter.ToString(bytesEnvio.Skip(Math.Max(0, bytesEnvio.Length - 64)).ToArray())
                        Dim innerDeclPos = soapEnvelop.IndexOf("<?xml", 5)
                        If innerDeclPos >= 0 Then
                            _logger.LogError("❌ Envelope SOAP contém declaração XML interna em nfeDadosMsg (pos=" & innerDeclPos & ")")
                        End If
                        _logger.LogDebug("Prefixo bytes envio (64): " & prefixHex)
                        _logger.LogDebug("Sufixo bytes envio (64): " & suffixHex)

                        Dim content As ByteArrayContent = Nothing
                        Dim usarSoap12 As Boolean = True
                        Dim soapAction As String = ObterSoapActionCorreto(servico, strEmitente_estado)

                        ' Conteúdo SOAP 1.2 por padrão
                        content = New ByteArrayContent(bytesEnvio)
                        content.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("application/soap+xml")
                        content.Headers.ContentType.CharSet = "utf-8"
                        If Not String.IsNullOrEmpty(soapAction) Then
                            content.Headers.ContentType.Parameters.Add(New System.Net.Http.Headers.NameValueHeaderValue("action", """" & soapAction & """"))
                            _logger.LogDebug($"Content-Type action (SOAP 1.2): {soapAction}")
                        End If

                        _logger.LogInfo($"Enviando para: {url}")
                        _logger.LogDebug($"Content-Type: application/soap+xml; charset=UTF-8")
                        _logger.LogDebug($"Tamanho: {bytesEnvio.Length} bytes")

                        ' ↓↓↓ ENVIO PRINCIPAL ↓↓↓
                        Dim response = Await client.PostAsync(url, content)
                        Dim resultadoBytes = Await response.Content.ReadAsByteArrayAsync()
                        Dim resultado = System.Text.Encoding.UTF8.GetString(resultadoBytes)

                        stopwatch.Stop()
                        _logger.LogInfo($"Resposta recebida em {stopwatch.ElapsedMilliseconds}ms - Status: {CInt(response.StatusCode)}")

                        ' Log da resposta
                        _logger.LogInfo($"Resposta SEFAZ recebida: {resultado.Substring(0, Math.Min(200, resultado.Length))}")

                        ' ↓↓↓ ANÁLISE DA RESPOSTA ↓↓↓
                        If response.IsSuccessStatusCode Then
                            If resultado.Contains("nfeResultMsg") OrElse resultado.Contains("retEnviNFe") Then
                                _logger.LogSuccess("Resposta válida da SEFAZ")
                                Return resultado
                            Else
                                _logger.LogError($"Resposta inesperada: {resultado}")
                                Throw New SefazException("Resposta inesperada da SEFAZ", "UNEXP-RESP", resultado)
                            End If
                        Else
                            _logger.LogError($"Erro HTTP {CInt(response.StatusCode)}: {resultado}")

                            ' Fallback SOAP 1.1 quando parser/charset indica problemas
                            If usarSoap12 AndAlso CInt(response.StatusCode) = 500 AndAlso
                               (resultado.IndexOf("Unexpected character", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                resultado.IndexOf("Character Set Encoding", StringComparison.OrdinalIgnoreCase) >= 0) Then

                                _logger.LogWarning("Ativando fallback para SOAP 1.1 (text/xml + SOAPAction)...")

                                ' Recriar envelope SOAP 1.1
                                Dim soap11Envelope As String = CriarEnvelopeSOAP11(xmlEnvio, servico)

                                ' Converter para bytes UTF-8 sem BOM
                                Dim bytesEnvio11 As Byte() = (New System.Text.UTF8Encoding(False)).GetBytes(soap11Envelope)
                                Dim content11 As New ByteArrayContent(bytesEnvio11)
                                content11.Headers.ContentType = New System.Net.Http.Headers.MediaTypeHeaderValue("text/xml")
                                content11.Headers.ContentType.CharSet = "utf-8"
                                If Not String.IsNullOrEmpty(soapAction) Then
                                    content11.Headers.Add("SOAPAction", """" & soapAction & """")
                                    _logger.LogDebug($"SOAPAction (SOAP 1.1): {soapAction}")
                                End If

                                ' Garantir que o primeiro byte seja '<' também no fallback
                                If bytesEnvio11.Length = 0 Then
                                    Throw New SefazException("Envelope SOAP 1.1 vazio após serialização", "SOAP11-EMPTY", "")
                                End If
                                Dim idxFirstLt11 As Integer = Array.FindIndex(bytesEnvio11, Function(b) b = &H3C)
                                If idxFirstLt11 = -1 Then
                                    _logger.LogError("❌ Nenhum caractere '<' encontrado no início do envelope SOAP 1.1")
                                    Throw New SefazException("Envelope SOAP 1.1 inválido: não inicia com '<'", "SOAP11-INV", "")
                                End If
                                If idxFirstLt11 > 0 Then
                                    _logger.LogWarning($"⚠️ Removendo {idxFirstLt11} bytes antes do primeiro '<' no envelope SOAP 1.1")
                                    bytesEnvio11 = bytesEnvio11.Skip(idxFirstLt11).ToArray()
                                End If

                                ' Reenvio com SOAP 1.1
                                usarSoap12 = False
                                stopwatch.Restart()
                                response = Await client.PostAsync(url, content11)
                                resultadoBytes = Await response.Content.ReadAsByteArrayAsync()
                                resultado = System.Text.Encoding.UTF8.GetString(resultadoBytes)
                                stopwatch.Stop()

                                _logger.LogInfo($"Resposta (fallback SOAP 1.1) em {stopwatch.ElapsedMilliseconds}ms - Status: {CInt(response.StatusCode)}")
                                _logger.LogInfo($"Resposta SEFAZ (SOAP 1.1): {resultado.Substring(0, Math.Min(200, resultado.Length))}")

                                If response.IsSuccessStatusCode AndAlso
                                   (resultado.Contains("nfeResultMsg") OrElse resultado.Contains("retEnviNFe")) Then
                                    _logger.LogSuccess("Resposta válida da SEFAZ via SOAP 1.1")
                                    Return resultado
                                End If
                            End If

                            Throw New SefazException($"SEFAZ retornou erro {response.StatusCode}", "HTTP-ERR", resultado)
                        End If
                    End Using
                End Using


            Catch ex As TaskCanceledException
                stopwatch.Stop()
                _logger.LogError($"Timeout na tentativa {tentativaAtual} ({stopwatch.ElapsedMilliseconds}ms)")
                If tentativaAtual >= maxTentativas Then
                    Throw New SefazException($"Timeout após {maxTentativas} tentativas", "TIMEOUT-001", "")
                End If

            Catch ex As HttpRequestException
                stopwatch.Stop()
                _logger.LogError($"Erro de rede na tentativa {tentativaAtual}: {ex.Message}")
                If tentativaAtual >= maxTentativas Then
                    Throw New SefazException($"Erro de rede após {maxTentativas} tentativas: {ex.Message}", "NET-ERR", ex.Message)
                End If

            Catch ex As SefazException
                ' ↓↓↓ ERROS DA SEFAZ - NÃO RETENTAR ↓↓↓
                _logger.LogError($"Erro SEFAZ na tentativa {tentativaAtual}: {ex.Message}")
                Throw

            Catch ex As Exception
                stopwatch.Stop()
                ' ↓↓↓ CORREÇÃO: IDENTIFICAR E LOGAR O ERRO ESPECÍFICO ↓↓↓
                _logger.LogError($"Erro geral na tentativa {tentativaAtual}: {ex.GetType().Name} - {ex.Message}")
                _logger.LogDebug($"Stack trace: {ex.StackTrace}")

                If tentativaAtual >= maxTentativas Then
                    Throw New SefazException($"Erro geral após {maxTentativas} tentativas: {ex.Message}", "GEN-ERR", ex.Message)
                End If
            End Try

            ' ↓↓↓ BACKOFF ENTRE TENTATIVAS ↓↓↓
            If tentativaAtual < maxTentativas Then
                Dim delayMs = 2000 * tentativaAtual
                _logger.LogInfo($"Aguardando {delayMs}ms antes da próxima tentativa...")
                Await Task.Delay(delayMs)
            End If
        End While

        Throw New SefazException($"Número máximo de tentativas ({maxTentativas}) excedido", "MAX-RETRY", "")
    End Function

    ' ✅ ADICIONE ESTA FUNÇÃO PARA GARANTIR A DECLARAÇÃO XML
    Private Function GarantirDeclaracaoXmlUTF8(xml As String) As String
        Try
            _logger.LogInfo("=== GARANTIR DECLARAÇÃO XML UTF-8 ===")

            Dim xmlResult = xml.Trim()

            ' 1. Verifica se já tem declaração XML
            If xmlResult.StartsWith("<?xml") Then
                ' Verifica se já tem encoding UTF-8
                If xmlResult.Contains("encoding=") Then
                    ' Corrige para UTF-8 se não estiver
                    If Not xmlResult.Contains("encoding=""UTF-8""") AndAlso
                   Not xmlResult.Contains("encoding='UTF-8'") Then

                        xmlResult = System.Text.RegularExpressions.Regex.Replace(
                        xmlResult,
                        "encoding\s*=\s*[""'][^""']*[""']",
                        "encoding=""UTF-8""",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    )
                        _logger.LogInfo("✅ Encoding corrigido para UTF-8")
                    Else
                        _logger.LogInfo("✅ Já tem encoding UTF-8")
                    End If
                Else
                    ' Adiciona encoding UTF-8
                    xmlResult = xmlResult.Replace("<?xml", "<?xml encoding=""UTF-8""")
                    _logger.LogInfo("✅ Adicionado encoding UTF-8")
                End If
            Else
                ' Adiciona declaração completa
                xmlResult = "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & xmlResult
                _logger.LogInfo("✅ Adicionada declaração XML completa")
            End If

            ' 2. Remove BOM (Byte Order Mark) se existir
            If xmlResult.StartsWith(ChrW(&HFEFF)) Then
                xmlResult = xmlResult.Substring(1)
                _logger.LogInfo("✅ Removido BOM")
            End If

            ' 3. Garante que está realmente em UTF-8
            Dim bytes = System.Text.Encoding.UTF8.GetBytes(xmlResult)
            xmlResult = System.Text.Encoding.UTF8.GetString(bytes)

            _logger.LogInfo($"Tamanho final: {xmlResult.Length} caracteres")
            _logger.LogInfo($"Primeiros 120 chars: {xmlResult.Substring(0, Math.Min(120, xmlResult.Length))}")

            Return xmlResult

        Catch ex As Exception
            _logger.LogError($"Erro em GarantirDeclaracaoXmlUTF8: {ex.Message}")
            ' Fallback simples
            Return "<?xml version=""1.0"" encoding=""UTF-8""?>" & vbCrLf & xml.Trim()
        End Try
    End Function
    ' ↓↓↓ MÉTODO DE TESTE REAL DE CONEXÃO MELHORADO ↓↓↓
    Private Async Function TestarConexaoRealSefaz() As Task(Of Boolean)
        Try
            _logger.LogInfo("Iniciando teste de conectividade SEFAZ...")

            ' Configurar ignore SSL para homologação
            ServicePointManager.ServerCertificateValidationCallback = Function(sender, certificate, chain, sslPolicyErrors) True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Using handler As New HttpClientHandler()
                handler.ServerCertificateCustomValidationCallback = Function(sender, cert, chain, sslPolicyErrors) True
                handler.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
                handler.UseProxy = False

                ' Configurar certificado
                Dim certificado = CarregarCertificado()
                If certificado IsNot Nothing Then
                    handler.ClientCertificates.Add(certificado)
                    _logger.LogInfo($"Certificado carregado para teste: {certificado.Subject}")
                Else
                    _logger.LogWarning("Teste de conectividade sem certificado digital")
                End If

                handler.ServerCertificateCustomValidationCallback = Function(sender, cert, chain, sslPolicyErrors) True
                handler.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate

                Using client As New HttpClient(handler)
                    ' Usar timeout mais conservador para teste de conectividade
                    Dim timeoutTest As Integer = Math.Min(ObterConfiguracaoTimeout(), 30000) ' Máximo 30s para teste
                    client.Timeout = TimeSpan.FromMilliseconds(timeoutTest)

                    _logger.LogInfo($"Timeout para teste: {timeoutTest}ms")

                    ' ↑↑↑ TESTE REAL: Enviar consulta de status real ↑↑↑
                    Dim xmlConsulta = $"<?xml version=""1.0"" encoding=""UTF-8""?>
<consStatServ xmlns=""http://www.portalfiscal.inf.br/nfe"" versao=""4.00"">
    <tpAmb>{strEmitente_nfce_ambiente}</tpAmb>
    <cUF>51</cUF>
    <xServ>STATUS</xServ>
</consStatServ>"

                    Dim soapEnvelope = CriarEnvelopeSOAP(xmlConsulta, "NFeStatusServico4")
                    Dim url = ObterUrlServico("NFeStatusServico4", True, ExtrairUfDoXml(xmlConsulta)) ' True para NFCe
                    Dim soapAction = ObterSoapActionCorreto("NFeStatusServico4", strEmitente_estado)

                    _logger.LogInfo($"Testando conectividade com: {url}")

                    ' LOGS DE DIAGNÓSTICO
                    _logger.LogInfo($"[Diagnóstico] URL utilizada para status: {url}")
                    _logger.LogInfo($"[Diagnóstico] SOAPAction: {soapAction}")
                    _logger.LogDebug($"[Diagnóstico] XML enviado: {soapEnvelope}")

                    ' Usar UTF8 sem BOM para evitar erro 402
                    Dim utf8NoBom As New UTF8Encoding(False)
                    Using content = New StringContent(soapEnvelope, utf8NoBom, "application/soap+xml")
                        content.Headers.Add("SOAPAction", soapAction)

                        Dim response = Await client.PostAsync(url, content)

                        If response.IsSuccessStatusCode Then
                            Dim resposta = Await response.Content.ReadAsStringAsync()

                            If resposta.Contains("107") Or resposta.Contains("servico em operacao") Then
                                _logger.LogInfo("SEFAZ ONLINE - Serviço em operação")
                                Return True
                            ElseIf resposta.Contains("soap:Fault") Or resposta.Contains("faultstring") Then
                                _logger.LogWarning("SEFAZ respondeu mas com erro SOAP - considerado online")
                                Return True ' A SEFAZ está online, mas retornou erro
                            ElseIf resposta.Contains("<nfeResultMsg") Then
                                _logger.LogInfo("SEFAZ ONLINE - Resposta padrão do webservice recebida")
                                Return True
                            Else
                                _logger.LogWarning($"SEFAZ respondeu mas com conteúdo inesperado: {resposta.Substring(0, Math.Min(200, resposta.Length))}...")
                                Return True ' Considera online pois respondeu
                            End If
                        Else
                            _logger.LogWarning($"SEFAZ retornou status HTTP {CInt(response.StatusCode)} - considerado offline")
                            Return False
                        End If
                    End Using
                End Using
            End Using

        Catch ex As TaskCanceledException
            _logger.LogWarning($"Timeout na verificação SEFAZ: {ex.Message} - considerado offline")
            Return False

        Catch ex As HttpRequestException
            _logger.LogWarning($"Erro HTTP na verificação: {ex.Message} - considerado offline")
            If ex.Message.Contains("SSL") OrElse ex.Message.Contains("certificate") Then
                _logger.LogError("Problema com certificado SSL detectado no teste de conectividade")
            ElseIf ex.Message.Contains("refused") OrElse ex.Message.Contains("unreachable") Then
                _logger.LogError("SEFAZ inacessível - verifique conectividade de rede")
            ElseIf ex.Message.Contains("timeout") OrElse ex.Message.Contains("timed out") Then
                _logger.LogError("Timeout de conexão com SEFAZ - rede lenta ou instável")
            End If
            Return False

        Catch ex As Exception
            _logger.LogWarning($"Erro geral na verificação: {ex.Message} - considerado offline")
            _logger.LogDebug($"Stack trace: {ex.StackTrace}")
            Return False
        End Try
    End Function


    ' ✅ Auxiliar: converte string de ambiente em código tpAmb
    Private Function MapearAmbienteParaTpAmb(ambiente As String) As String
        Dim a = If(ambiente, "").Trim().ToLowerInvariant()
        If a = "1" OrElse a.Contains("prod") OrElse a = "producao" Then
            Return "1" ' produção
        End If
        Return "2" ' homologação
    End Function


    Private Function CriarEnvelopeSOAPCorreto(xml As String, servico As String) As String
        Try
            _logger.LogInfo("=== CRIAR ENVELOPE SOAP COM UTF-8 ===")

            ' 1. Remove declaração XML se existir (o SOAP terá sua própria)
            Dim xmlSemDeclaracao = xml
            ' Remover BOM em qualquer posição antes de processar
            xmlSemDeclaracao = xmlSemDeclaracao.Replace(ChrW(&HFEFF), "")
            xmlSemDeclaracao = xmlSemDeclaracao.Trim()
            ' Remover declaração XML mesmo se houver espaços antes
            xmlSemDeclaracao = System.Text.RegularExpressions.Regex.Replace(
                xmlSemDeclaracao,
                "^\s*<\?xml[^>]*\?>",
                "",
                System.Text.RegularExpressions.RegexOptions.Singleline
            ).Trim()

            ' Garantir que inicia exatamente no primeiro '<'
            Dim firstLt = xmlSemDeclaracao.IndexOf("<"c)
            If firstLt > 0 Then
                xmlSemDeclaracao = xmlSemDeclaracao.Substring(firstLt)
            End If
            If firstLt = -1 Then
                Throw New XmlException("Conteúdo de NFe/enviNFe não contém elemento raiz")
            End If

            ' 2. Criar envelope via XmlDocument para evitar texto entre tags
            Dim envDoc As New XmlDocument()
            envDoc.PreserveWhitespace = True

            ' Declaração XML
            Dim decl = envDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            envDoc.AppendChild(decl)

            ' Envelope
            Dim envelope = envDoc.CreateElement("soap12", "Envelope", "http://www.w3.org/2003/05/soap-envelope")
            envelope.SetAttribute("xmlns:nfe", "http://www.portalfiscal.inf.br/nfe/wsdl/" & servico)
            envDoc.AppendChild(envelope)

            ' Body
            Dim body = envDoc.CreateElement("soap12", "Body", "http://www.w3.org/2003/05/soap-envelope")
            envelope.AppendChild(body)

            ' nfeDadosMsg
            Dim dadosMsg = envDoc.CreateElement("nfe", "nfeDadosMsg", "http://www.portalfiscal.inf.br/nfe/wsdl/" & servico)
            body.AppendChild(dadosMsg)

            ' Carregar conteúdo como XmlElement
            Dim conteudoDoc As New XmlDocument()
            conteudoDoc.PreserveWhitespace = True
            conteudoDoc.LoadXml(xmlSemDeclaracao)

            Dim nodeToImport As XmlNode = conteudoDoc.DocumentElement
            Dim importedNode As XmlNode = envDoc.ImportNode(nodeToImport, True)
            dadosMsg.AppendChild(importedNode)

            ' Serializar para bytes UTF-8 sem BOM usando MemoryStream
            Dim settings As New XmlWriterSettings() With {
                .Encoding = New UTF8Encoding(False),
                .Indent = False,
                .OmitXmlDeclaration = False,
                .NewLineHandling = NewLineHandling.None
            }
            Dim soapBytes As Byte()
            Using ms As New MemoryStream()
                Using writer As XmlWriter = XmlWriter.Create(ms, settings)
                    envDoc.Save(writer)
                End Using
                soapBytes = ms.ToArray()
            End Using

            ' Garantir string UTF-8 sem BOM
            Dim soapEnvelope = System.Text.Encoding.UTF8.GetString(soapBytes)
            If soapEnvelope.StartsWith(ChrW(&HFEFF)) Then
                soapEnvelope = soapEnvelope.Substring(1)
            End If

            Dim tamanhoBytes = System.Text.Encoding.UTF8.GetByteCount(soapEnvelope)
            _logger.LogInfo($"Envelope SOAP criado: {tamanhoBytes} bytes em UTF-8 (sem texto solto)")
            _logger.LogDebug($"Head XML decl: {soapEnvelope.Substring(0, Math.Min(60, soapEnvelope.Length))}")

            Return soapEnvelope

        Catch ex As Exception
            _logger.LogError($"Erro ao criar envelope SOAP: {ex.Message}")
            Throw
        End Try
    End Function

    Private Function ObterSoapActionCorreto(servico As String, uf As String) As String
        Dim servicoNormalizado = servico.ToUpper().Trim()
        Dim ufSigla = If(uf.Length = 2, uf.ToUpper(), uf.ToUpper().Replace("MATO GROSSO", "MT"))

        ' ↑↑↑ SOAP ACTIONS ESPECÍFICOS PARA MT ↑↑↑ - CORRIGIDOS
        Dim soapActionsMT As New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO4", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote"},
        {"NFEAUTORIZACAO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote"},
        {"AUTORIZACAO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote"},
        {"NFERETAUTORIZACAO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote"},
        {"RETAUTORIZACAO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote"},
        {"NFESTATUSSERVICO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4/nfeStatusServicoNF"},
        {"STATUSSERVICO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4/nfeStatusServicoNF"},
        {"NFECONSULTAPROTOCOLO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4/nfeConsultaNF"},
        {"CONSULTAPROTOCOLO", "http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4/nfeConsultaNF"},
        {"RECEPCAOEVENTO", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento4/nfeRecepcaoEvento"},
        {"EVENTO", "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento4/nfeRecepcaoEvento"}
    }

        If ufSigla = "MT" AndAlso soapActionsMT.ContainsKey(servicoNormalizado) Then
            Return soapActionsMT(servicoNormalizado)
        End If

        ' ↑↑↑ FALLBACK PARA SEVAZ VIRTUAL ↑↑↑
        If soapActionsMT.ContainsKey(servicoNormalizado) Then
            Return soapActionsMT(servicoNormalizado)
        End If

        Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4/nfeStatusServicoNF"
    End Function
    ' Utilitário: limpeza robusta de XML para UTF-8 sem BOM e sem caracteres inválidos (XML 1.0)
    Private Function LimparXmlParaUTF8(xml As String) As String
        Try
            _logger.LogInfo("=== LIMPAR XML PARA UTF-8 ===")

            ' 1. Remove BOM se existir
            Dim resultado = xml
            If resultado.StartsWith(ChrW(&HFEFF)) Then
                resultado = resultado.Substring(1)
                _logger.LogInfo("Removido BOM inicial")
            End If

            ' 2. Remove caracteres de controle inválidos
            resultado = System.Text.RegularExpressions.Regex.Replace(
            resultado,
            "[\u0000-\u0008\u000B\u000C\u000E-\u001F]",
            ""
        )

            ' 3. Garante que não há múltiplas declarações XML
            Dim countDeclaracoes = System.Text.RegularExpressions.Regex.Matches(resultado, "<\?xml").Count
            If countDeclaracoes > 1 Then
                _logger.LogWarning($"Encontradas {countDeclaracoes} declarações XML. Removendo extras...")

                ' Mantém apenas a primeira
                Dim primeiraPos = resultado.IndexOf("<?xml")
                Dim primeiraFim = resultado.IndexOf("?>", primeiraPos) + 2

                If primeiraFim > primeiraPos Then
                    Dim primeiraDeclaracao = resultado.Substring(primeiraPos, primeiraFim - primeiraPos)
                    Dim resto = resultado.Substring(primeiraFim)

                    ' Remove outras declarações do resto
                    resto = System.Text.RegularExpressions.Regex.Replace(resto, "<\?xml[^>]*\?>", "")
                    resultado = primeiraDeclaracao & resto
                End If
            End If

            ' 4. Converte para UTF-8 e volta para garantir encoding
            Dim bytesUtf8 = System.Text.Encoding.UTF8.GetBytes(resultado)
            resultado = System.Text.Encoding.UTF8.GetString(bytesUtf8)

            _logger.LogInfo($"XML limpo: {bytesUtf8.Length} bytes em UTF-8")

            Return resultado

        Catch ex As Exception
            _logger.LogError($"Erro em LimparXmlParaUTF8: {ex.Message}")
            Return xml
        End Try
    End Function

    Private Function ObterNamespaceWsdl(servico As String) As String
        Dim servicoNormalizado = servico.ToUpper().Trim()

        Select Case servicoNormalizado
            Case "NFEAUTORIZACAO", "AUTORIZACAO"
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4"
            Case "NFERETAUTORIZACAO", "RETAUTORIZACAO"
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4"
            Case "NFESTATUSSERVICO", "STATUSSERVICO"
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4"
            Case "NFECONSULTAPROTOCOLO", "CONSULTAPROTOCOLO"
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4"
            Case "RECEPCAOEVENTO", "EVENTO"
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento4"
            Case Else
                Return "http://www.portalfiscal.inf.br/nfe/wsdl/NFeStatusServico4"
        End Select
    End Function
    Private Function CriarEnvelopeSOAP(xmlNFe As String, servico As String) As String
        Return CriarEnvelopeSOAPCorreto(xmlNFe, servico)
    End Function

    Private Function CriarEnvelopeSOAP11(xml As String, servico As String) As String
        Try
            Dim xmlSemDeclaracao = xml
            xmlSemDeclaracao = xmlSemDeclaracao.Replace(ChrW(&HFEFF), "")
            xmlSemDeclaracao = System.Text.RegularExpressions.Regex.Replace(
                xmlSemDeclaracao, "^\s*<\?xml[^>]*\?>", "", System.Text.RegularExpressions.RegexOptions.Singleline
            ).Trim()
            Dim firstLt = xmlSemDeclaracao.IndexOf("<"c)
            If firstLt > 0 Then xmlSemDeclaracao = xmlSemDeclaracao.Substring(firstLt)
            If firstLt = -1 Then Throw New XmlException("Conteúdo de NFe/enviNFe não contém elemento raiz")

            Dim envDoc As New XmlDocument()
            envDoc.PreserveWhitespace = True
            Dim decl = envDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            envDoc.AppendChild(decl)

            Dim envelope = envDoc.CreateElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/")
            envDoc.AppendChild(envelope)
            Dim body = envDoc.CreateElement("soapenv", "Body", "http://schemas.xmlsoap.org/soap/envelope/")
            envelope.AppendChild(body)

            Dim dadosMsg = envDoc.CreateElement("nfe", "nfeDadosMsg", "http://www.portalfiscal.inf.br/nfe/wsdl/" & servico)
            body.AppendChild(dadosMsg)

            Dim conteudoDoc As New XmlDocument()
            conteudoDoc.PreserveWhitespace = True
            conteudoDoc.LoadXml(xmlSemDeclaracao)
            Dim importedNode As XmlNode = envDoc.ImportNode(conteudoDoc.DocumentElement, True)
            dadosMsg.AppendChild(importedNode)

            Dim settings As New XmlWriterSettings() With {
                .Encoding = New UTF8Encoding(False),
                .Indent = False,
                .OmitXmlDeclaration = False,
                .NewLineHandling = NewLineHandling.None
            }
            Dim soapBytes As Byte()
            Using ms As New MemoryStream()
                Using writer As XmlWriter = XmlWriter.Create(ms, settings)
                    envDoc.Save(writer)
                End Using
                soapBytes = ms.ToArray()
            End Using

            Dim soapEnvelope = System.Text.Encoding.UTF8.GetString(soapBytes)
            If soapEnvelope.StartsWith(ChrW(&HFEFF)) Then
                soapEnvelope = soapEnvelope.Substring(1)
            End If
            Return soapEnvelope

        Catch ex As Exception
            _logger.LogError($"Erro ao criar envelope SOAP 1.1: {ex.Message}")
            Throw
        End Try
    End Function
    Private Function ObterUrlServico(servico As String, Optional isNFCe As Boolean = False, Optional ufParam As String = Nothing) As String
        Try
            Dim uf = If(ufParam, strEmitente_estado).ToUpper().Trim()
            Dim ambienteLower = If(strEmitente_nfce_ambiente IsNot Nothing, strEmitente_nfce_ambiente.ToLower().Trim(), "")
            Dim ambiente As String

            ' === LOGS CRÍTICOS ADICIONADOS ===
            _logger.LogInfo($"=== INÍCIO ObterUrlServico ===")
            _logger.LogInfo($"Serviço solicitado (ORIGINAL): '{servico}'")
            _logger.LogInfo($"UF original: '{strEmitente_estado}', isNFCe: {isNFCe}")
            _logger.LogInfo($"Ambiente configurado: '{strEmitente_nfce_ambiente}'")

            Select Case ambienteLower
                Case "1", "producao", "produção", "prod"
                    ambiente = "producao"
                Case "2", "homologacao", "homologação", "hom", "teste"
                    ambiente = "homologacao"
                Case Else
                    _logger.LogError($"Ambiente inválido: {strEmitente_nfce_ambiente}")
                    Throw New SefazException($"Ambiente inválido: {strEmitente_nfce_ambiente}", "AMB-001", "")
            End Select

            ' Validar UF
            If String.IsNullOrWhiteSpace(uf) OrElse uf.Length <> 2 Then
                _logger.LogError($"UF inválida ou não reconhecida: '{uf}'")
                Throw New SefazException($"UF inválida: {uf}", "UF-001", "")
            End If

            ' Normalizar UF
            If uf.Contains("MATO GROSSO") Then uf = "MT"
            If uf.Contains("MATO GROSSO DO SUL") Then uf = "MS"
            If uf.Contains("SÃO PAULO") OrElse uf.Contains("SAO PAULO") Then uf = "SP"
            If uf.Contains("PARANÁ") OrElse uf.Contains("PARANA") Then uf = "PR"
            If uf.Contains("RIO GRANDE DO SUL") Then uf = "RS"

            ' Normalizar nome do serviço
            Dim servicoNormalizado = servico.ToUpper().Trim()

            ' === LOG ADICIONADO ===
            _logger.LogInfo($"Serviço após normalização: '{servicoNormalizado}'")

            If servicoNormalizado = "NFESTATUSSERVICO4" Then
                servicoNormalizado = "NfeStatusServico"
            End If

            _logger.LogInfo($"Configurando URL para UF: {uf}, Ambiente: {ambiente}, Serviço: {servicoNormalizado}")

            ' URLs para MT separadas por ambiente - NFe
            Dim urlsMTProducao As New Dictionary(Of String, String) From {
            {"NFeAutorizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4"},
            {"NFeRetAutorizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4"},
            {"NfeInutilizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4"},
            {"NfeConsultaProtocolo", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeConsulta4"},
            {"NfeStatusServico", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4"},
            {"RecepcaoEvento", "https://nfce.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4"}
        }

            Dim urlsMTHomologacao As New Dictionary(Of String, String) From {
            {"NFeAutorizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4"},
            {"NFeRetAutorizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4"},
            {"NfeInutilizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4"},
            {"NfeConsultaProtocolo", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeConsulta4"},
            {"NfeStatusServico", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4"},
            {"RecepcaoEvento", "https://homologacao.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4"}
        }

            ' URLs para MT separadas por ambiente - NFCe (VERSÃO CORRIGIDA)
            Dim urlsMTNFCeProducao As New Dictionary(Of String, String) From {
            {"NFeAutorizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4"},
            {"NFeRetAutorizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4"},
            {"NfeInutilizacao", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4"},
            {"NfeConsultaProtocolo", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeConsulta4"},
            {"NfeStatusServico", "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4"},
            {"RecepcaoEvento", "https://nfce.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4"}
        }

            Dim urlsMTNFCeHomologacao As New Dictionary(Of String, String) From {
            {"NFeAutorizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4"},
            {"NFeRetAutorizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4"},
            {"NfeInutilizacao", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4"},
            {"NfeConsultaProtocolo", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeConsulta4"},
            {"NfeStatusServico", "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4"},
            {"RecepcaoEvento", "https://homologacao.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4"}
        }

            ' === LOGS ADICIONADOS para verificar as chaves disponíveis ===
            If uf = "MT" Then
                Dim urlsSelecionadas As Dictionary(Of String, String)

                If isNFCe Then
                    ' Usar URLs específicas para NFCe
                    urlsSelecionadas = If(ambiente = "producao", urlsMTNFCeProducao, urlsMTNFCeHomologacao)
                    _logger.LogInfo($"Usando URLs de NFCe para MT - Ambiente: {ambiente}")
                    _logger.LogInfo($"Chaves disponíveis NFCe: {String.Join(", ", urlsSelecionadas.Keys)}")
                Else
                    ' Usar URLs padrão para NFe
                    urlsSelecionadas = If(ambiente = "producao", urlsMTProducao, urlsMTHomologacao)
                    _logger.LogInfo($"Usando URLs de NFe para MT - Ambiente: {ambiente}")
                    _logger.LogInfo($"Chaves disponíveis NFe: {String.Join(", ", urlsSelecionadas.Keys)}")
                End If

                ' === LOG ADICIONADO antes da verificação ===
                _logger.LogInfo($"Buscando serviço: '{servicoNormalizado}' no dicionário...")

                If urlsSelecionadas.ContainsKey(servicoNormalizado) Then
                    Dim urlFinal = urlsSelecionadas(servicoNormalizado)
                    _logger.LogInfo($"URL final selecionada: {urlFinal}")
                    Return urlFinal
                Else
                    ' === LOG ADICIONADO para serviço não encontrado ===
                    _logger.LogError($"Serviço '{servicoNormalizado}' NÃO ENCONTRADO no dicionário!")
                    _logger.LogError($"Chaves disponíveis: {String.Join(", ", urlsSelecionadas.Keys)}")
                End If
            End If

            ' Fallback para Sefaz Virtual (SVRS)
            Dim urlsSVRS As New Dictionary(Of String, String) From {
            {"NFeAutorizacao", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeAutorizacao4/NFeAutorizacao4.asmx"},
            {"NFeRetAutorizacao", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeRetAutorizacao4/NFeRetAutorizacao4.asmx"},
            {"NfeStatusServico", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeStatusServico4/NFeStatusServico4.asmx"},
            {"NfeConsultaProtocolo", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeConsultaProtocolo4/NFeConsultaProtocolo4.asmx"},
            {"RecepcaoEvento", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx"}
        }

            ' === LOG ADICIONADO para fallback ===
            _logger.LogInfo($"Tentando fallback para Sefaz Virtual...")

            If urlsSVRS.ContainsKey(servicoNormalizado) Then
                Dim urlFinal = urlsSVRS(servicoNormalizado)
                _logger.LogInfo($"UF {uf} não específica, usando Sefaz Virtual: {urlFinal}")
                Return urlFinal
            Else
                _logger.LogError($"Serviço '{servicoNormalizado}' também não encontrado no SVRS!")
            End If

            _logger.LogError($"Serviço {servico} não configurado para UF {uf}")
            Throw New SefazException($"Serviço {servico} não configurado para UF {uf}", "URL-004", "")

        Catch ex As SefazException
            _logger.LogError($"SEFAZ Exception em ObterUrlServico: {ex.Message}")
            Throw
        Catch ex As Exception
            _logger.LogError("Erro crítico ao obter URL do serviço", ex)
            Throw New SefazException($"Erro na configuração de URL: {ex.Message}", "URL-003", ex.Message)
        End Try
    End Function

    ' FUNÇÃO PRINCIPAL QUE USA A UF CONFIGURADA
    Private Function ObterUrlPorUF(uf As String, servico As String, ambiente As String, isNFCe As Boolean) As String
        ' Dicionário com URLs por UF
        Dim urlsProducao As New Dictionary(Of String, Dictionary(Of String, String))
        Dim urlsHomologacao As New Dictionary(Of String, Dictionary(Of String, String))

        'CORREÇÃO: ADICIONAR "NFEAUTORIZACAO4" como chave válida
        urlsProducao("MT") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeAutorizacao4")},
        {"NFEAUTORIZACAO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeAutorizacao4")}, ' ✅ NOVA CHAVE
        {"NFERETAUTORIZACAO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeRetAutorizacao4")},
        {"NFERETAUTORIZACAO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeRetAutorizacao4")}, ' ✅ NOVA CHAVE
        {"NFESTATUSSERVICO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeStatusServico4")},
        {"NFESTATUSSERVICO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeStatusServico4")}, ' ✅ NOVA CHAVE
        {"NFECONSULTAPROTOCOLO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeConsulta4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeConsulta4")},
        {"NFECONSULTAPROTOCOLO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeConsulta4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeConsulta4")}, ' ✅ NOVA CHAVE
        {"RECEPCAOEVENTO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4", "https://nfe.sefaz.mt.gov.br/nfews/services/RecepcaoEvento4")},
        {"RECEPCAOEVENTO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4", "https://nfe.sefaz.mt.gov.br/nfews/services/RecepcaoEvento4")}, ' ✅ NOVA CHAVE
        {"NFEINUTILIZACAO", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeInutilizacao4")},
        {"NFEINUTILIZACAO4", If(isNFCe, "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4", "https://nfe.sefaz.mt.gov.br/nfews/services/NFeInutilizacao4")} ' ✅ NOVA CHAVE
    }

        urlsHomologacao("MT") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeAutorizacao4")},
        {"NFEAUTORIZACAO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeAutorizacao4")}, ' ✅ NOVA CHAVE
        {"NFERETAUTORIZACAO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeRetAutorizacao4")},
        {"NFERETAUTORIZACAO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeRetAutorizacao4")}, ' ✅ NOVA CHAVE
        {"NFESTATUSSERVICO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeStatusServico4")},
        {"NFESTATUSSERVICO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeStatusServico4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeStatusServico4")}, ' ✅ NOVA CHAVE
        {"NFECONSULTAPROTOCOLO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeConsulta4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeConsulta4")},
        {"NFECONSULTAPROTOCOLO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeConsulta4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeConsulta4")}, ' ✅ NOVA CHAVE
        {"RECEPCAOEVENTO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4", "https://homologacao.sefaz.mt.gov.br/nfews/services/RecepcaoEvento4")},
        {"RECEPCAOEVENTO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4", "https://homologacao.sefaz.mt.gov.br/nfews/services/RecepcaoEvento4")}, ' ✅ NOVA CHAVE
        {"NFEINUTILIZACAO", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeInutilizacao4")},
        {"NFEINUTILIZACAO4", If(isNFCe, "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4", "https://homologacao.sefaz.mt.gov.br/nfews/services/NFeInutilizacao4")} ' ✅ NOVA CHAVE
    }

        ' CONFIGURAÇÃO PARA SP (São Paulo)
        urlsProducao("SP") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://nfce.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx", "https://nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx")},
        {"NFERETAUTORIZACAO", If(isNFCe, "https://nfce.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx", "https://nfe.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx")},
        {"NFESTATUSSERVICO", If(isNFCe, "https://nfce.fazenda.sp.gov.br/ws/nfestatusservico4.asmx", "https://nfe.fazenda.sp.gov.br/ws/nfestatusservico4.asmx")}
    }

        urlsHomologacao("SP") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx", "https://homologacao.nfe.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx")},
        {"NFERETAUTORIZACAO", If(isNFCe, "https://homologacao.nfce.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx", "https://homologacao.nfe.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx")},
        {"NFESTATUSSERVICO", If(isNFCe, "https://homologacao.nfce.fazenda.sp.gov.br/ws/nfestatusservico4.asmx", "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfestatusservico4.asmx")}
    }

        ' CONFIGURAÇÃO PARA PR (Paraná)
        urlsProducao("PR") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://nfce.sefa.pr.gov.br/nfce/NFeAutorizacao4?wsdl", "https://nfe.sefa.pr.gov.br/nfe/NFeAutorizacao4?wsdl")},
        {"NFERETAUTORIZACAO", If(isNFCe, "https://nfce.sefa.pr.gov.br/nfce/NFeRetAutorizacao4?wsdl", "https://nfe.sefa.pr.gov.br/nfe/NFeRetAutorizacao4?wsdl")},
        {"NFESTATUSSERVICO", If(isNFCe, "https://nfce.sefa.pr.gov.br/nfce/NFeStatusServico4?wsdl", "https://nfe.sefa.pr.gov.br/nfe/NFeStatusServico4?wsdl")}
    }

        ' CONFIGURAÇÃO PARA RS (Rio Grande do Sul)
        urlsProducao("RS") = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", If(isNFCe, "https://nfce.sefazrs.rs.gov.br/ws/nfeautorizacao/NFeAutorizacao4.asmx", "https://nfe.sefazrs.rs.gov.br/ws/nfeautorizacao/NFeAutorizacao4.asmx")},
        {"NFERETAUTORIZACAO", If(isNFCe, "https://nfce.sefazrs.rs.gov.br/ws/nferetautorizacao/nferetautorizacao4.asmx", "https://nfe.sefazrs.rs.gov.br/ws/nferetautorizacao/nferetautorizacao4.asmx")},
        {"NFESTATUSSERVICO", If(isNFCe, "https://nfce.sefazrs.rs.gov.br/ws/nfestatusservico/nfestatusservico4.asmx", "https://nfe.sefazrs.rs.gov.br/ws/nfestatusservico/nfestatusservico4.asmx")}
    }

        ' SEFAZ VIRTUAL (SVRS) - Para UFs que usam SVRS
        Dim urlsSVRS = New Dictionary(Of String, String) From {
        {"NFEAUTORIZACAO", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeAutorizacao4/NFeAutorizacao4.asmx"},
        {"NFERETAUTORIZACAO", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeRetAutorizacao4/NFeRetAutorizacao4.asmx"},
        {"NFESTATUSSERVICO", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeStatusServico4/NFeStatusServico4.asmx"},
        {"NFECONSULTAPROTOCOLO", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeConsultaProtocolo4/NFeConsultaProtocolo4.asmx"},
        {"RECEPCAOEVENTO", $"https://{If(ambiente = "producao", "www", "hom")}.sefazvirtual.fazenda.gov.br/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx"}
    }

        ' UFs que usam SVRS (Sefaz Virtual)
        Dim ufsSVRS As String() = {"AC", "AL", "AP", "DF", "ES", "PA", "PB", "PI", "RJ", "RN", "RO", "RR", "SC", "SE", "TO"}

        ' ✅ BUSCAR URL ESPECÍFICA DA UF
        Dim urlsSelecionadas As Dictionary(Of String, Dictionary(Of String, String)) = If(ambiente = "producao", urlsProducao, urlsHomologacao)

        If urlsSelecionadas.ContainsKey(uf) Then
            ' UF tem configuração específica
            Dim urlDict = urlsSelecionadas(uf)
            If urlDict.ContainsKey(servico) Then
                Return urlDict(servico)
            End If
        ElseIf ufsSVRS.Contains(uf) Then
            ' UF usa Sefaz Virtual
            If urlsSVRS.ContainsKey(servico) Then
                Return urlsSVRS(servico)
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Valida a integridade básica do XML antes do parsing
    ''' </summary>
    Private Function ValidarIntegridadeXML(xml As String) As Boolean
        Try
            If String.IsNullOrEmpty(xml) Then
                Return False
            End If

            ' Verificar se contém tags básicas de envelope SOAP
            Dim temEnvelopeInicio = xml.Contains("<soap:Envelope") OrElse xml.Contains("<soapenv:Envelope") OrElse xml.Contains("<Envelope")
            Dim temEnvelopeFim = xml.Contains("</soap:Envelope>") OrElse xml.Contains("</soapenv:Envelope>") OrElse xml.Contains("</Envelope>")

            ' Se tem início mas não tem fim, provavelmente está truncado
            If temEnvelopeInicio AndAlso Not temEnvelopeFim Then
                _logger.LogWarning("XML parece estar truncado - envelope SOAP não fechado")
                Return False
            End If

            ' Verificar se tem pelo menos uma estrutura XML básica
            If Not xml.Trim().StartsWith("<") OrElse Not xml.Trim().EndsWith(">") Then
                _logger.LogWarning("XML não tem estrutura básica válida")
                Return False
            End If

            ' Contar tags de abertura e fechamento básicas
            Dim contadorAbertura = 0
            Dim contadorFechamento = 0
            Dim i = 0

            While i < xml.Length - 1
                If xml(i) = "<" Then
                    If i + 1 < xml.Length AndAlso xml(i + 1) = "/" Then
                        contadorFechamento += 1
                    ElseIf i + 1 < xml.Length AndAlso xml(i + 1) <> "?" AndAlso xml(i + 1) <> "!" Then
                        ' Verificar se não é uma tag auto-fechada
                        Dim fimTag = xml.IndexOf(">", i)
                        If fimTag > i AndAlso fimTag > 0 AndAlso xml(fimTag - 1) <> "/" Then
                            contadorAbertura += 1
                        End If
                    End If
                End If
                i += 1
            End While

            ' Se há muita diferença entre abertura e fechamento, pode estar truncado
            If Math.Abs(contadorAbertura - contadorFechamento) > 2 Then
                _logger.LogWarning($"Desequilíbrio de tags XML detectado. Abertura: {contadorAbertura}, Fechamento: {contadorFechamento}")
                Return False
            End If

            Return True

        Catch ex As Exception
            _logger.LogError($"Erro ao validar integridade do XML: {ex.Message}", ex)
            Return False
        End Try
    End Function

    Private Function ProcessarRespostaAutorizacao(xmlResposta As String) As ResultadoAutorizacao
        Try
            If String.IsNullOrEmpty(xmlResposta) Then
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Resposta vazia da SEFAZ",
                .CodigoStatus = "ERRO",
                .Autorizada = False
            }
            End If

            ' ✅ CORREÇÃO: Verificar tamanho do XML antes de processar
            If xmlResposta.Length > 5 * 1024 * 1024 Then ' 5MB máximo
                _logger.LogError($"XML muito grande: {xmlResposta.Length / 1024 / 1024:F2}MB")
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Resposta da SEFAZ muito grande",
                .CodigoStatus = "XML_MUITO_GRANDE",
                .Autorizada = False
            }
            End If

            ' ✅ CORREÇÃO: Log apenas parte inicial para economizar memória
            _logger.LogInfo($"Resposta SEFAZ recebida: {xmlResposta.Substring(0, Math.Min(300, xmlResposta.Length))}...")

            ' Verificar se contém erro conhecido
            If xmlResposta.Contains("E_FAL") OrElse xmlResposta.Contains("0x80004005") Then
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Falha de comunicação com SEFAZ",
                .CodigoStatus = "E_FAL",
                .Autorizada = False
            }
            End If

            ' Verificar se é uma mensagem de erro simples
            If xmlResposta.Contains("Parser XML") OrElse xmlResposta.Contains("DESCONHECIDO") Then
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Erro de formato XML na comunicação com SEFAZ",
                .CodigoStatus = "XML_INVALIDO",
                .Autorizada = False
            }
            End If

            ' ✅ CORREÇÃO: Validar XML de forma mais eficiente
            If Not xmlResposta.Contains("<?xml") AndAlso Not xmlResposta.Contains("<soap") Then
                _logger.LogError($"XML não inicia com declaração válida. Tamanho: {xmlResposta.Length}")
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Resposta não é um XML válido",
                .CodigoStatus = "NAO_XML",
                .Autorizada = False
            }
            End If

            ' ✅ CORREÇÃO: Usar XmlReader em vez de XmlDocument para economizar memória
            Dim cStat As String = Nothing
            Dim xMotivo As String = Nothing
            Dim nRec As String = Nothing
            Dim hasSoapFault As Boolean = False
            Dim faultMessage As String = Nothing

            Try
                Using reader As XmlReader = XmlReader.Create(New StringReader(xmlResposta),
                New XmlReaderSettings() With {
                    .IgnoreWhitespace = True,
                    .IgnoreComments = True,
                    .DtdProcessing = DtdProcessing.Ignore
                })

                    While reader.Read()
                        If reader.NodeType = XmlNodeType.Element Then
                            Select Case reader.Name
                                Case "cStat"
                                    reader.Read()
                                    cStat = reader.Value.Trim()

                                Case "xMotivo"
                                    reader.Read()
                                    xMotivo = reader.Value.Trim()

                                Case "nRec"
                                    reader.Read()
                                    nRec = reader.Value.Trim()

                                Case "faultstring", "faultcode"
                                    hasSoapFault = True
                                    reader.Read()
                                    If faultMessage Is Nothing Then
                                        faultMessage = reader.Value.Trim()
                                    End If

                                Case "Text" ' Para SOAP 1.2
                                    If reader.GetAttribute("xml:lang") IsNot Nothing Then
                                        hasSoapFault = True
                                        reader.Read()
                                        faultMessage = reader.Value.Trim()
                                    End If
                            End Select
                        End If
                    End While
                End Using
            Catch ex As OutOfMemoryException
                _logger.LogError($"OutOfMemory no XmlReader: {ex.Message}")
                ' Tentar método alternativo com regex
                Return ProcessarComRegex(xmlResposta)
            Catch ex As XmlException
                _logger.LogWarning($"XmlException no reader: {ex.Message}")
                ' Tentar método alternativo
                Return ProcessarComRegex(xmlResposta)
            End Try

            ' ✅ CORREÇÃO: Processar resultados obtidos
            If hasSoapFault Then
                _logger.LogError($"SOAP Fault detectado: {faultMessage}")
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = If(faultMessage, "Erro SOAP não especificado"),
                .CodigoStatus = "SOAP_FAULT",
                .Autorizada = False
            }
            End If

            If cStat IsNot Nothing Then
                _logger.LogInfo($"Status SEFAZ: cStat={cStat}, xMotivo={xMotivo}")

                ' Tratar códigos específicos conhecidos
                If cStat = "999" Then
                    xMotivo = "Erro não catalogado pela SEFAZ"
                    _logger.LogWarning($"SEFAZ retornou erro 999 - Erro não catalogado: {xMotivo}")
                End If

                Dim resultado As New ResultadoAutorizacao() With {
                .Sucesso = (cStat = "100" OrElse cStat = "150"),
                .CodigoStatus = cStat,
                .Mensagem = If(xMotivo, "Status sem descrição"),
                .NumeroRecibo = If(nRec, ""),
                .XmlCompleto = xmlResposta,
                .Autorizada = (cStat = "100" OrElse cStat = "150")
            }
                Return resultado
            End If

            ' ✅ CORREÇÃO: Tentar método regex se não encontrou pelos elementos
            Return ProcessarComRegex(xmlResposta)

        Catch ex As OutOfMemoryException
            ' ✅ CORREÇÃO: Tratamento específico para OutOfMemory
            _logger.LogError($"OutOfMemoryException ao processar resposta: {ex.Message}")
            _logger.LogError($"Tamanho do XML: {If(xmlResposta IsNot Nothing, xmlResposta.Length, 0)} caracteres")

            ' Tentar método mínimo com regex
            Try
                Return ProcessarComRegex(xmlResposta)
            Catch ex2 As Exception
                _logger.LogError($"Falha no método regex: {ex2.Message}")
                Return New ResultadoAutorizacao() With {
                .Sucesso = False,
                .Mensagem = "Erro de memória ao processar resposta",
                .CodigoStatus = "OUT_OF_MEMORY",
                .Autorizada = False
            }
            End Try

        Catch ex As Exception
            _logger.LogError($"Erro ao processar resposta da SEFAZ: {ex.Message}")
            Return New ResultadoAutorizacao() With {
            .Sucesso = False,
            .Mensagem = $"Erro no processamento: {ex.Message}",
            .CodigoStatus = "PROCESSAMENTO_ERRO",
            .Autorizada = False
        }
        End Try
    End Function

    ' ✅ CORREÇÃO: Método auxiliar com regex (mais leve em memória)
    Private Function ProcessarComRegex(xmlResposta As String) As ResultadoAutorizacao
        Try
            _logger.LogInfo("Usando método regex para processar resposta...")

            Dim cStat As String = Nothing
            Dim xMotivo As String = Nothing
            Dim nRec As String = Nothing

            ' ✅ CORREÇÃO: Removido Text.RegularExpressions.
            ' Extrair cStat
            Dim cStatMatch = System.Text.RegularExpressions.Regex.Match(xmlResposta, "<cStat>(\d+)</cStat>")
            If cStatMatch.Success Then
                cStat = cStatMatch.Groups(1).Value
            End If

            ' Extrair xMotivo
            Dim xMotivoMatch = System.Text.RegularExpressions.Regex.Match(xmlResposta, "<xMotivo>([^<]+)</xMotivo>")
            If xMotivoMatch.Success Then
                xMotivo = xMotivoMatch.Groups(1).Value
            End If

            ' Extrair nRec
            Dim nRecMatch = System.Text.RegularExpressions.Regex.Match(xmlResposta, "<nRec>([^<]+)</nRec>")
            If nRecMatch.Success Then
                nRec = nRecMatch.Groups(1).Value
            End If

            ' Verificar SOAP Fault
            If xmlResposta.Contains("soap:Fault") OrElse xmlResposta.Contains("faultcode") Then
                Dim faultMatch = System.Text.RegularExpressions.Regex.Match(xmlResposta, "<faultstring>([^<]+)</faultstring>")
                If faultMatch.Success Then
                    Return New ResultadoAutorizacao() With {
                    .Sucesso = False,
                    .Mensagem = faultMatch.Groups(1).Value,
                    .CodigoStatus = "SOAP_FAULT",
                    .Autorizada = False
                }
                End If
            End If

            If cStat IsNot Nothing Then
                _logger.LogInfo($"Status SEFAZ (regex): cStat={cStat}, xMotivo={xMotivo}")

                ' ✅ CORREÇÃO: Limitar tamanho do XML armazenado para economizar memória
                Dim xmlParaArmazenar As String = xmlResposta
                If xmlResposta.Length > 10000 Then
                    xmlParaArmazenar = xmlResposta.Substring(0, 10000) & "... [TRUNCADO]"
                End If

                Return New ResultadoAutorizacao() With {
                .Sucesso = (cStat = "100" OrElse cStat = "150"),
                .CodigoStatus = cStat,
                .Mensagem = If(xMotivo, "Status sem descrição"),
                .NumeroRecibo = If(nRec, ""),
                .XmlCompleto = xmlParaArmazenar,
                .Autorizada = (cStat = "100" OrElse cStat = "150")
            }
            End If

            ' Se não encontrou nada útil
            Return New ResultadoAutorizacao() With {
            .Sucesso = False,
            .Mensagem = "Não foi possível extrair informações da resposta",
            .CodigoStatus = "NAO_RECONHECIDO",
            .Autorizada = False
        }

        Catch ex As Exception
            _logger.LogError($"Erro no método regex: {ex.Message}")
            Return New ResultadoAutorizacao() With {
            .Sucesso = False,
            .Mensagem = "Falha no processamento da resposta",
            .CodigoStatus = "ERRO_PROCESSAMENTO",
            .Autorizada = False
        }
        End Try
    End Function

    ''' <summary>
    ''' Processa resposta de consulta de recibo
    ''' </summary>
    Private Function ProcessarRespostaConsultaRecibo(xmlResposta As String) As ResultadoConsultaRecibo
        Try
            If String.IsNullOrEmpty(xmlResposta) Then
                Return New ResultadoConsultaRecibo() With {
                    .Sucesso = False
                }
            End If

            _logger.LogInfo($"Processando consulta recibo: {xmlResposta.Substring(0, Math.Min(300, xmlResposta.Length))}...")

            Dim doc As New XmlDocument()
            doc.LoadXml(xmlResposta)

            Dim nsmgr As New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope")
            nsmgr.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe")
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            ' Verificar status da consulta
            Dim cStatNode = doc.SelectSingleNode("//ns:cStat | //nfe:cStat | //cStat", nsmgr)
            Dim xMotivoNode = doc.SelectSingleNode("//ns:xMotivo | //nfe:xMotivo | //xMotivo", nsmgr)
            Dim protNode = doc.SelectSingleNode("//ns:nProt | //nfe:nProt | //nProt", nsmgr)
            Dim chaveNode = doc.SelectSingleNode("//ns:chNFe | //nfe:chNFe | //chNFe", nsmgr)

            Dim sucesso = False
            Dim autorizada = False

            If cStatNode IsNot Nothing Then
                Dim status = cStatNode.InnerText
                sucesso = (status = "100" OrElse status = "150")
                autorizada = sucesso

                _logger.LogInfo($"Status consulta recibo: {status} - {If(xMotivoNode IsNot Nothing, xMotivoNode.InnerText, "Sem descrição")}")
            End If

            Return New ResultadoConsultaRecibo() With {
                .Sucesso = sucesso,
                .NFCeAutorizada = autorizada,
                .ProtocoloAutorizacao = If(protNode IsNot Nothing, protNode.InnerText, ""),
                .ChaveAcesso = If(chaveNode IsNot Nothing, chaveNode.InnerText, ""),
                .XmlNFCe = xmlResposta
            }

        Catch ex As Exception
            _logger.LogError($"Erro ao processar consulta de recibo: {ex.Message}", ex)
            Return New ResultadoConsultaRecibo() With {
                .Sucesso = False
            }
        End Try
    End Function

    ''' <summary>
    ''' Processa resposta de consulta de situação
    ''' </summary>
    Private Function ProcessarRespostaConsultaSituacao(xmlResposta As String) As ResultadoConsultaSituacao
        Try
            If String.IsNullOrEmpty(xmlResposta) Then
                Return New ResultadoConsultaSituacao() With {
                    .Autorizada = False,
                    .CodigoSituacao = "ERRO",
                    .DescricaoSituacao = "Resposta vazia da SEFAZ"
                }
            End If

            _logger.LogInfo($"Processando consulta situação: {xmlResposta.Substring(0, Math.Min(300, xmlResposta.Length))}...")

            Dim doc As New XmlDocument()
            doc.LoadXml(xmlResposta)

            Dim nsmgr As New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope")
            nsmgr.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe")
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            ' Verificar SOAP Fault
            Dim faultNode = doc.SelectSingleNode("//soap:Fault", nsmgr)
            If faultNode IsNot Nothing Then
                Dim faultString = faultNode.SelectSingleNode(".//faultstring", nsmgr)
                Return New ResultadoConsultaSituacao() With {
                    .Autorizada = False,
                    .CodigoSituacao = "SOAP_FAULT",
                    .DescricaoSituacao = If(faultString IsNot Nothing, faultString.InnerText, "Erro SOAP na consulta")
                }
            End If

            ' Verificar status da consulta
            Dim cStatNode = doc.SelectSingleNode("//ns:cStat | //nfe:cStat | //cStat", nsmgr)
            Dim xMotivoNode = doc.SelectSingleNode("//ns:xMotivo | //nfe:xMotivo | //xMotivo", nsmgr)

            ' Verificar se há informações de cancelamento
            Dim canceladaNode = doc.SelectSingleNode("//ns:cSitNFe | //nfe:cSitNFe | //cSitNFe", nsmgr)

            Dim autorizada = False
            Dim cancelada = False
            Dim codigoSituacao = "DESCONHECIDO"
            Dim descricaoSituacao = "Situação não identificada"

            If cStatNode IsNot Nothing Then
                codigoSituacao = cStatNode.InnerText
                descricaoSituacao = If(xMotivoNode IsNot Nothing, xMotivoNode.InnerText, $"Status: {codigoSituacao}")

                ' Códigos de situação conhecidos
                Select Case codigoSituacao
                    Case "100", "150" ' Autorizada
                        autorizada = True
                    Case "101", "135" ' Cancelada
                        cancelada = True
                        autorizada = False
                    Case "110" ' Uso Denegado
                        autorizada = False
                    Case Else
                        autorizada = False
                End Select

                _logger.LogInfo($"Status consulta situação: {codigoSituacao} - {descricaoSituacao}")
            End If

            ' Verificar situação específica da NFCe se disponível
            If canceladaNode IsNot Nothing Then
                Dim sitNFe = canceladaNode.InnerText
                If sitNFe = "2" Then ' NFCe cancelada
                    cancelada = True
                    autorizada = False
                End If
            End If

            Return New ResultadoConsultaSituacao() With {
                .Autorizada = autorizada,
                .Cancelada = cancelada,
                .CodigoSituacao = codigoSituacao,
                .DescricaoSituacao = descricaoSituacao
            }

        Catch ex As Exception
            _logger.LogError($"Erro ao processar consulta de situação: {ex.Message}", ex)
            Return New ResultadoConsultaSituacao() With {
                .Autorizada = False,
                .Cancelada = False,
                .CodigoSituacao = "ERRO_PROCESSAMENTO",
                .DescricaoSituacao = $"Erro no processamento: {ex.Message}"
            }
        End Try
    End Function

    ''' <summary>
    ''' Processa resposta de cancelamento
    ''' </summary>
    Private Function ProcessarRespostaCancelamento(xmlResposta As String) As ResultadoCancelamento
        Try
            If String.IsNullOrEmpty(xmlResposta) Then
                Return New ResultadoCancelamento() With {
                    .Sucesso = False,
                    .Mensagem = "Resposta vazia da SEFAZ"
                }
            End If

            _logger.LogInfo($"Processando cancelamento: {xmlResposta.Substring(0, Math.Min(300, xmlResposta.Length))}...")

            Dim doc As New XmlDocument()
            doc.LoadXml(xmlResposta)

            Dim nsmgr As New XmlNamespaceManager(doc.NameTable)
            nsmgr.AddNamespace("soap", "http://www.w3.org/2003/05/soap-envelope")
            nsmgr.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe")
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            ' Verificar SOAP Fault
            Dim faultNode = doc.SelectSingleNode("//soap:Fault", nsmgr)
            If faultNode IsNot Nothing Then
                Dim faultString = faultNode.SelectSingleNode(".//faultstring", nsmgr)
                Return New ResultadoCancelamento() With {
                    .Sucesso = False,
                    .Mensagem = If(faultString IsNot Nothing, faultString.InnerText, "Erro SOAP no cancelamento")
                }
            End If

            ' Verificar status do cancelamento
            Dim cStatNode = doc.SelectSingleNode("//ns:cStat | //nfe:cStat | //cStat", nsmgr)
            Dim xMotivoNode = doc.SelectSingleNode("//ns:xMotivo | //nfe:xMotivo | //xMotivo", nsmgr)
            Dim protNode = doc.SelectSingleNode("//ns:nProt | //nfe:nProt | //nProt", nsmgr)

            Dim sucesso = False
            Dim mensagem = "Status de cancelamento desconhecido"

            If cStatNode IsNot Nothing Then
                Dim status = cStatNode.InnerText
                sucesso = (status = "101" OrElse status = "135") ' Códigos de cancelamento aceito
                mensagem = If(xMotivoNode IsNot Nothing, xMotivoNode.InnerText, $"Status: {status}")

                _logger.LogInfo($"Status cancelamento: {status} - {mensagem}")
            End If

            Return New ResultadoCancelamento() With {
                .Sucesso = sucesso,
                .Mensagem = mensagem,
                .ProtocoloCancelamento = If(protNode IsNot Nothing, protNode.InnerText, "")
            }

        Catch ex As Exception
            _logger.LogError($"Erro ao processar cancelamento: {ex.Message}", ex)
            Return New ResultadoCancelamento() With {
                .Sucesso = False,
                .Mensagem = $"Erro no processamento do cancelamento: {ex.Message}"
            }
        End Try
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Chama o Dispose protegido passando True
        Dispose(True)
        ' Impede que o garbage collector chame o finalizador
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                ' Liberar recursos gerenciados
                If _assinatura IsNot Nothing Then

                End If
            End If
            ' Liberar recursos não gerenciados (se houver)
            _disposed = True
        End If
    End Sub

    ' Destrutor (apenas para segurança)
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


End Class

' Classes de resultado
Public Class ResultadoAutorizacao
    Public Property Sucesso As Boolean
    Public Property NumeroRecibo As String
    Public Property Mensagem As String
    Public Property CodigoStatus As String
    Public Property XmlCompleto As String
    Public Property Autorizada As Boolean
End Class

Public Class ResultadoConsultaRecibo
    Public Property Sucesso As Boolean
    Public Property NFCeAutorizada As Boolean
    Public Property ProtocoloAutorizacao As String
    Public Property ChaveAcesso As String
    Public Property XmlNFCe As String
    Public Property QRCode As String
    Public Property UrlConsulta As String
    Public Property XmlCompleto As String
    Public Property Mensagem As String
    Public Property Protocolo As String
End Class

Public Class ResultadoConsultaSituacao
    Public Property Autorizada As Boolean
    Public Property Cancelada As Boolean
    Public Property CodigoSituacao As String
    Public Property DescricaoSituacao As String
End Class

Public Class ResultadoCancelamento
    Public Property Sucesso As Boolean
    Public Property ProtocoloCancelamento As String
    Public Property Mensagem As String
End Class
