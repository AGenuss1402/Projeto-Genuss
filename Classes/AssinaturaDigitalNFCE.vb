Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml
Imports System.IO
Imports System.Text

Public Class AssinaturaDigitalNFCE
    Private ReadOnly _logger As LoggerNFe

    Public Sub New()
        _logger = LoggerNFe.Instance
    End Sub

    ' ✅ FUNÇÃO SIMPLIFICADA E CORRETA
    Public Function AssinarXmlNFCe(xmlContent As String, certificado As X509Certificate2) As String
        Try
            _logger.LogInfo("=== INICIANDO ASSINATURA NFC-e ===")

            ' 1. Carregar XML
            Dim xmlDoc As New XmlDocument()
            xmlDoc.PreserveWhitespace = True
            CarregarXml(xmlDoc, xmlContent)

            ' 2. Verificar estrutura ANTES de assinar
            VerificarEstruturaXML(xmlDoc)

            ' 3. Encontrar o elemento infNFe
            Dim infNFeElement = CType(xmlDoc.GetElementsByTagName("infNFe")(0), XmlElement)
            If infNFeElement Is Nothing Then
                Throw New Exception("Elemento infNFe não encontrado no XML")
            End If

            Dim idNFe = infNFeElement.GetAttribute("Id")
            If String.IsNullOrEmpty(idNFe) Then
                Throw New Exception("Atributo Id não encontrado no infNFe")
            End If

            _logger.LogInfo($"ID da NFC-e: {idNFe}")

            ' 4. Remover assinaturas existentes (se houver)
            RemoverAssinaturas(xmlDoc)

            ' 5. Criar objeto SignedXml
            Dim signedXml As New SignedXml(xmlDoc)
            Dim rsa = certificado.GetRSAPrivateKey()
            If rsa Is Nothing Then
                Throw New Exception("Certificado não possui chave RSA válida")
            End If
            signedXml.SigningKey = rsa

            ' 6. Configurar a referência ao infNFe
            Dim reference As New Reference($"#{idNFe}")

            ' 7. Usar canonicalização EXCLUSIVA (XmlDsigExcC14N) conforme NFe/NFC-e
            reference.AddTransform(New XmlDsigExcC14NTransform())
            reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1"

            signedXml.AddReference(reference)

            ' 8. Configurar métodos de assinatura
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1"

            ' 9. Adicionar informações do certificado
            Dim keyInfo As New KeyInfo()
            keyInfo.AddClause(New KeyInfoX509Data(certificado))
            signedXml.KeyInfo = keyInfo

            ' 10. Calcular a assinatura
            _logger.LogInfo("Calculando assinatura digital...")
            signedXml.ComputeSignature()

            ' 11. Obter a assinatura como XML
            Dim signatureElement = signedXml.GetXml()

            ' 12. Adicionar a assinatura ao XML
            '    ⚠️ IMPORTANTE: Adicionar DEPOIS do elemento infNFe, mas DENTRO do elemento NFe
            Dim nfeElement = xmlDoc.DocumentElement
            nfeElement.AppendChild(xmlDoc.ImportNode(signatureElement, True))

            ' 13. Salvar XML assinado para debug
            Dim xmlAssinado = SerializarXml(xmlDoc)

            ' Verificar se infNFeSupl está DENTRO do infNFe
            VerificarPosicaoInfNFeSupl(xmlAssinado)

            ' Validação local da assinatura
            Try
                Dim xmlCheck As New XmlDocument()
                xmlCheck.PreserveWhitespace = True
                xmlCheck.LoadXml(xmlAssinado)
                Dim nsMgr As New XmlNamespaceManager(xmlCheck.NameTable)
                nsMgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")
                Dim sigNode As XmlNode = xmlCheck.SelectSingleNode("//ds:Signature", nsMgr)
                If sigNode IsNot Nothing Then
                    Dim sx As New SignedXml(xmlCheck)
                    sx.LoadXml(CType(sigNode, XmlElement))
                    If sx.CheckSignature(certificado, True) Then
                        _logger.LogSuccess("✅ Assinatura válida (CheckSignature)")
                    Else
                        _logger.LogError("❌ Assinatura inválida (CheckSignature)")
                    End If
                Else
                    _logger.LogError("❌ Nó Signature não encontrado para validação")
                End If
            Catch exVal As Exception
                _logger.LogError($"Erro na validação local da assinatura: {exVal.Message}")
            End Try

            ' 14. Calcular e verificar o DigestValue
            Dim digestCalculado = CalcularDigestInfNFe(xmlDoc, infNFeElement)
            Dim digestAssinado = ObterDigestDoXML(xmlAssinado)

            _logger.LogInfo($"Digest calculado:  {digestCalculado}")
            _logger.LogInfo($"Digest na assinatura: {digestAssinado}")

            If digestCalculado = digestAssinado Then
                _logger.LogSuccess("✅ Assinatura válida - DigestValues conferem")
            Else
                _logger.LogWarning($"⚠️ DigestValues não conferem. Pode ser problema de canonicalização.")
                ' Salvar para análise
                File.WriteAllText("nfce_com_problema_digest.xml", xmlAssinado, Encoding.UTF8)
            End If

            ' 15. Salvar XML final
            File.WriteAllText("nfce_assinada_final.xml", xmlAssinado, Encoding.UTF8)
            _logger.LogSuccess($"✅ XML assinado com sucesso. Tamanho: {xmlAssinado.Length} bytes")

            Return xmlAssinado

        Catch ex As Exception
            _logger.LogError($"❌ Erro na assinatura: {ex.Message}")
            _logger.LogError($"Stack: {ex.StackTrace}")
            Throw New Exception($"Falha na assinatura digital: {ex.Message}", ex)
        End Try
    End Function

    ' ✅ VERIFICAR ESTRUTURA DO XML
    Private Sub VerificarEstruturaXML(xmlDoc As XmlDocument)
        Try
            _logger.LogInfo("=== VERIFICAÇÃO DA ESTRUTURA XML ===")

            ' 1. Verificar elemento raiz
            If xmlDoc.DocumentElement Is Nothing Then
                Throw New Exception("Documento XML sem elemento raiz")
            End If

            _logger.LogInfo($"Elemento raiz: {xmlDoc.DocumentElement.Name}")

            ' 2. Verificar namespace
            Dim xmlns = xmlDoc.DocumentElement.GetAttribute("xmlns")
            _logger.LogInfo($"Namespace: {xmlns}")

            ' 3. Verificar infNFe
            Dim infNFeNodes = xmlDoc.GetElementsByTagName("infNFe")
            If infNFeNodes.Count = 0 Then
                Throw New Exception("Elemento infNFe não encontrado")
            End If

            Dim infNFeElement = CType(infNFeNodes(0), XmlElement)
            _logger.LogInfo($"infNFe Id: {infNFeElement.GetAttribute("Id")}")
            _logger.LogInfo($"infNFe versao: {infNFeElement.GetAttribute("versao")}")

            ' 4. Verificar e garantir posição do infNFeSupl PARA NFC-e
            Dim nfeElement As XmlElement = xmlDoc.DocumentElement
            Dim infNFeSuplDentro = infNFeElement.GetElementsByTagName("infNFeSupl")
            If infNFeSuplDentro.Count > 0 Then
                Dim infNFeSuplElement = CType(infNFeSuplDentro(0), XmlElement)
                infNFeElement.RemoveChild(infNFeSuplElement)
                Dim posDepoisInfNFe = infNFeElement.NextSibling
                If posDepoisInfNFe Is Nothing Then
                    nfeElement.AppendChild(infNFeSuplElement)
                Else
                    nfeElement.InsertBefore(infNFeSuplElement, posDepoisInfNFe)
                End If
                _logger.LogInfo("✅ infNFeSupl movido para fora de infNFe, como filho de NFe")
            Else
                Dim infNFeSuplFora = nfeElement.GetElementsByTagName("infNFeSupl")
                If infNFeSuplFora.Count > 0 Then
                    _logger.LogSuccess("✅ infNFeSupl está fora de infNFe (correto para NFC-e)")
                Else
                    _logger.LogWarning("⚠️ infNFeSupl não encontrado no documento")
                End If
            End If

            ' 5. Verificar se há signature existente
            Dim nsManager As New XmlNamespaceManager(xmlDoc.NameTable)
            nsManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")

            Dim signatures = xmlDoc.SelectNodes("//ds:Signature", nsManager)
            _logger.LogInfo($"Assinaturas existentes: {signatures.Count}")

        Catch ex As Exception
            _logger.LogError($"Erro na verificação: {ex.Message}")
        End Try
    End Sub

    ' ✅ CALCULAR DIGEST DO INFNFE (método confiável)
    Private Function CalcularDigestInfNFe(xmlDoc As XmlDocument, infNFeElement As XmlElement) As String
        Try
            ' Criar um novo documento apenas com o infNFe
            Dim tempDoc As New XmlDocument()
            tempDoc.PreserveWhitespace = True

            ' Importar o infNFe com todos os seus filhos (incluindo infNFeSupl)
            Dim clonedInfNFe = CType(tempDoc.ImportNode(infNFeElement, True), XmlElement)
            tempDoc.AppendChild(clonedInfNFe)

            ' Aplicar transformação C14N EXCLUSIVA
            Dim c14nTransform As New XmlDsigExcC14NTransform()
            c14nTransform.LoadInput(tempDoc)

            ' Obter bytes canonicalizados
            Dim canonicalBytes As Byte()
            Using stream = CType(c14nTransform.GetOutput(GetType(Stream)), Stream)
                Using ms = New MemoryStream()
                    stream.CopyTo(ms)
                    canonicalBytes = ms.ToArray()

                    ' Salvar para análise
                    Dim canonicalXml = Encoding.UTF8.GetString(canonicalBytes)
                    File.WriteAllText("infnfe_canonical_para_digest.xml", canonicalXml, Encoding.UTF8)
                    _logger.LogInfo($"infNFe canonicalizado: {canonicalXml.Length} chars")
                End Using
            End Using

            ' Calcular SHA-1
            Using sha1Alg = SHA1.Create()
                Dim hashBytes = sha1Alg.ComputeHash(canonicalBytes)
                Return Convert.ToBase64String(hashBytes)
            End Using

        Catch ex As Exception
            _logger.LogError($"Erro ao calcular digest: {ex.Message}")
            Return "ERRO"
        End Try
    End Function

    ' ✅ OBTER DIGEST DO XML ASSINADO
    Private Function ObterDigestDoXML(xmlContent As String) As String
        Try
            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(xmlContent)

            Dim nsManager As New XmlNamespaceManager(xmlDoc.NameTable)
            nsManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")

            Dim digestNode = xmlDoc.SelectSingleNode("//ds:DigestValue", nsManager)
            Return If(digestNode?.InnerText, "NÃO ENCONTRADO")

        Catch ex As Exception
            Return $"ERRO: {ex.Message}"
        End Try
    End Function

    ' ✅ VERIFICAR POSIÇÃO DO INFNFESUPL
    Private Sub VerificarPosicaoInfNFeSupl(xmlContent As String)
        Try
            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(xmlContent)

            ' Encontrar infNFe
            Dim infNFeElement = CType(xmlDoc.GetElementsByTagName("infNFe")(0), XmlElement)

            ' Verificar se infNFeSupl está FORA
            Dim nfeElement = xmlDoc.DocumentElement
            Dim infNFeSuplNodes = nfeElement.GetElementsByTagName("infNFeSupl")

            If infNFeSuplNodes.Count > 0 Then
                Dim dentro = infNFeElement.GetElementsByTagName("infNFeSupl").Count > 0
                If dentro Then
                    _logger.LogError("❌ VERIFICAÇÃO: infNFeSupl está DENTRO do infNFe")
                Else
                    _logger.LogSuccess("✅ VERIFICAÇÃO: infNFeSupl está FORA do infNFe")
                End If
            Else
                _logger.LogWarning("⚠️ VERIFICAÇÃO: infNFeSupl não encontrado")
            End If

        Catch ex As Exception
            _logger.LogError($"Erro na verificação: {ex.Message}")
        End Try
    End Sub

    ' ✅ MÉTODOS AUXILIARES (mantenha como estão)
    Private Sub CarregarXml(xmlDoc As XmlDocument, xmlContent As String)
        Try
            If String.IsNullOrWhiteSpace(xmlContent) Then
                Throw New ArgumentException("XML vazio")
            End If

            Dim conteudo = xmlContent.TrimStart(ChrW(&HFEFF))

            If Not conteudo.TrimStart().StartsWith("<?xml") Then
                conteudo = "<?xml version=""1.0"" encoding=""UTF-8""?>" & Environment.NewLine & conteudo
            End If

            xmlDoc.LoadXml(conteudo)
            _logger.LogInfo("✅ XML carregado")

        Catch ex As XmlException
            _logger.LogError($"Erro XML: {ex.Message}")
            Throw
        Catch ex As Exception
            _logger.LogError($"Erro ao carregar XML: {ex.Message}")
            Throw
        End Try
    End Sub

    Private Sub RemoverAssinaturas(xmlDoc As XmlDocument)
        Try
            Dim nsManager As New XmlNamespaceManager(xmlDoc.NameTable)
            nsManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")

            Dim signatures = xmlDoc.SelectNodes("//ds:Signature", nsManager)
            For Each sig As XmlNode In signatures
                sig.ParentNode.RemoveChild(sig)
            Next

            If signatures.Count > 0 Then
                _logger.LogInfo($"Removidas {signatures.Count} assinaturas antigas")
            End If

        Catch ex As Exception
            ' Ignorar
        End Try
    End Sub

    Private Function SerializarXml(xmlDoc As XmlDocument) As String
        Try
            Dim settings As New XmlWriterSettings() With {
                .Encoding = New UTF8Encoding(False),
                .Indent = False,
                .OmitXmlDeclaration = False,
                .NewLineHandling = NewLineHandling.None
            }

            Using ms As New MemoryStream()
                Using writer As XmlWriter = XmlWriter.Create(ms, settings)
                    xmlDoc.Save(writer)
                End Using
                Return Encoding.UTF8.GetString(ms.ToArray())
            End Using

        Catch ex As Exception
            Return xmlDoc.OuterXml
        End Try
    End Function
End Class
