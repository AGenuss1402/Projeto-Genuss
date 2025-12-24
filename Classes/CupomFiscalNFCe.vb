Imports System.Text
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Data.OleDb
Imports System.Linq
Imports System.Xml.Linq
Imports QRCoder

Public Class CupomFiscalNFCe
    Private pd As PrintDocument
    Private dadosNFCe As DadosNFCeCompletos
    Private qrCodeImage As Image = Nothing

    Private Const LARGURA_CUPOM As Integer = 80
    Private Const LINHA_TRACEJADA As String = "------------------------------------------------"

    Private connectionString As String
    Private vendaId As Integer
    Private chaveAcesso As String
    Private protocoloAutorizacao As String
    Private qrCodeUrl As String
    Private serieNFe As String
    Private numeroNFe As String
    Private valorTotalNFe As Decimal
    Private formaPagamentoNFe As String

    Private cupomTexto As StringBuilder
    Private dadosEmpresa As DadosEmpresa
    Private dadosVenda As DadosVenda

    ' Construtor principal que recebe dados do XML
    Public Sub New(xmlNFCe As String, connectionString As String, vendaId As Integer)
        Me.connectionString = connectionString
        Me.vendaId = vendaId
        Me.cupomTexto = New StringBuilder()

        ' Extrair dados do XML usando ExtractorXmlNFCe
        ExtrairDadosXML(xmlNFCe)

        CarregarDadosEmpresa()
        CarregarDadosVenda()
        pd = New PrintDocument()
        AddHandler pd.PrintPage, AddressOf ImprimirPagina
    End Sub

    ' Construtor alternativo para compatibilidade
    Public Sub New(dados As DadosNFCeCompletos)
        dadosNFCe = dados
        Me.vendaId = dados.NumeroNFCe
        Me.cupomTexto = New StringBuilder()
        Me.connectionString = GetConnectionString()

        CarregarDadosEmpresa()
        CarregarDadosVenda()
        pd = New PrintDocument()
        AddHandler pd.PrintPage, AddressOf ImprimirPagina
    End Sub

    ' Construtor alternativo para compatibilidade
    Public Sub New(connectionString As String, vendaId As Integer)
        Me.connectionString = connectionString
        Me.vendaId = vendaId
        Me.cupomTexto = New StringBuilder()

        ' Criar dados NFCe básicos
        dadosNFCe = New DadosNFCeCompletos()
        dadosNFCe.NumeroNFCe = vendaId

        ' Carregar dados do banco se disponível
        CarregarDadosNFCeDoBanco()
        CarregarDadosEmpresa()
        CarregarDadosVenda()
        pd = New PrintDocument()
        AddHandler pd.PrintPage, AddressOf ImprimirPagina
    End Sub

    ' Método para extrair dados do XML usando ExtractorXmlNFCe
    Private Sub ExtrairDadosXML(xmlContent As String)
        Try
            Console.WriteLine($"[ExtrairDadosXML] XML recebido: {If(String.IsNullOrEmpty(xmlContent), "VAZIO/NULO", $"len={xmlContent.Length}")}")
            If String.IsNullOrEmpty(xmlContent) Then
                ' Deixar campos vazios se não há XML
                chaveAcesso = ""
                protocoloAutorizacao = ""
                serieNFe = ""
                numeroNFe = ""
                qrCodeUrl = ""
                Console.WriteLine("[ExtrairDadosXML] XML vazio - campos zerados")
                Return
            End If

            ' Criar instância do ExtractorXmlNFCe
            Dim extractor As New ExtractorXmlNFCe(LoggerNFe.Instance)

            ' Usar ExtractorXmlNFCe para extrair dados corretamente
            chaveAcesso = extractor.ExtrairChaveAcessoDoXml(xmlContent)
            protocoloAutorizacao = extractor.ExtrairProtocoloDoXml(xmlContent)
            qrCodeUrl = extractor.ExtrairQRCodeDoXml(xmlContent)
            Console.WriteLine($"[CupomFiscalNFCe] qrCode extraído (len={If(qrCodeUrl IsNot Nothing, qrCodeUrl.Length, 0)})")

            ' Removido fallback: não substituir QRCode por urlChave

            Dim serieExtraida As Integer = extractor.ExtrairSerieDoXml(xmlContent)
            serieNFe = If(serieExtraida > 0, serieExtraida.ToString(), "")

            Dim numeroExtraido As Integer = extractor.ExtrairNumeroDoXml(xmlContent)
            numeroNFe = If(numeroExtraido > 0, numeroExtraido.ToString(), "")

            ' Fallback adicional: se a chave ainda estiver vazia mas há URL de QR, tentar extrair chNFe/chave da URL
            If String.IsNullOrEmpty(chaveAcesso) AndAlso Not String.IsNullOrEmpty(qrCodeUrl) Then
                Try
                    Dim urlLower As String = qrCodeUrl.ToLower()
                    Dim possivelChave As String = ""
                    ' Tenta padrões comuns em QR da NFC-e
                    Dim padroes As String() = {"chNFe=", "chave=", "chaveacesso=", "p=", "chave_nfe="}
                    For Each p In padroes
                        Dim idx As Integer = urlLower.IndexOf(p.ToLower())
                        If idx >= 0 Then
                            Dim inicio As Integer = idx + p.Length
                            ' captura subsequente com até 44 dígitos
                            Dim resto As String = qrCodeUrl.Substring(inicio)
                            Dim digitos As String = New String(resto.TakeWhile(Function(c) Char.IsDigit(c)).ToArray())
                            If digitos.Length = 44 Then
                                possivelChave = digitos
                                Exit For
                            End If
                        End If
                    Next
                    If Not String.IsNullOrEmpty(possivelChave) Then
                        chaveAcesso = possivelChave
                        Console.WriteLine($"[CupomFiscalNFCe] Fallback: chave extraída da URL do QR ({chaveAcesso})")
                    End If
                Catch
                    ' Ignorar erros de parsing
                End Try
            End If

        Catch ex As Exception
            ' Deixar campos vazios em caso de erro
            chaveAcesso = ""
            protocoloAutorizacao = ""
            serieNFe = ""
            numeroNFe = ""
            qrCodeUrl = ""
        End Try
    End Sub

    ' Método para converter código de forma de pagamento em descrição
    Private Function ObterDescricaoFormaPagamento(codigo As String) As String
        Dim formasPagamento As New Dictionary(Of String, String) From {
            {"01", "Dinheiro"},
            {"02", "Cheque"},
            {"03", "Cartão de Crédito"},
            {"04", "Cartão de Débito"},
            {"05", "Crédito Loja"},
            {"10", "Vale Alimentação"},
            {"11", "Vale Refeição"},
            {"12", "Vale Presente"},
            {"13", "Vale Combustível"},
            {"14", "Duplicata Mercantil"},
            {"15", "Boleto Bancário"},
            {"90", "Sem Pagamento"},
            {"99", "Outros"}
        }

        Return If(formasPagamento.ContainsKey(codigo), formasPagamento(codigo), "Outros")
    End Function

    Public Sub DefinirQRCode(image As Image)
        qrCodeImage = image
    End Sub

    Public Function GerarCupomFiscal() As String
        Try
            cupomTexto.Clear()

            ' Estrutura do cupom fiscal NFCe conforme exemplo
            AdicionarCabecalhoEmpresa()
            AdicionarDetalhesDocumento()
            AdicionarDetalhesVenda()
            AdicionarProdutos()
            AdicionarTotalizadores()
            AdicionarFormaPagamento()
            AdicionarInformacoesFiscais()
            AdicionarQRCode()
            AdicionarRodape()

            Return cupomTexto.ToString()
        Catch ex As Exception
            Return "Erro ao gerar cupom fiscal: " & ex.Message
        End Try
    End Function

    Private Sub AdicionarCabecalhoEmpresa()
        ' Cabeçalho da empresa seguindo formato da imagem
        cupomTexto.AppendLine(FormatarTexto(Centralizar(dadosEmpresa.RazaoSocial), "negrito"))
        cupomTexto.AppendLine(Centralizar($"CNPJ {FormatarCNPJ(dadosEmpresa.CNPJ)} I.E. {dadosEmpresa.InscricaoEstadual}"))
        cupomTexto.AppendLine(Centralizar($"{dadosEmpresa.Endereco}, {dadosEmpresa.Numero} - {dadosEmpresa.Bairro} - Fone{dadosEmpresa.Telefone}"))
        cupomTexto.AppendLine(Centralizar($"{dadosEmpresa.Cidade}"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(New String("-", 48))
    End Sub

    Private Sub AdicionarDetalhesDocumento()
        cupomTexto.AppendLine(FormatarTexto(Centralizar("DANFE NFC-e - Documento Auxiliar"), "negrito"))
        cupomTexto.AppendLine(Centralizar("da Nota Fiscal Eletrônica para Consumidor Final"))
        cupomTexto.AppendLine(Centralizar("Não permite aproveitamento de crédito de ICMS"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(New String("-", 48))
    End Sub

    Private Sub AdicionarDetalhesVenda()
        cupomTexto.AppendLine(FormatarTexto(Centralizar("DETALHE DA VENDA"), "negrito"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto("CÓDIGO DESCRIÇÃO            QTD UN  VL UN  VL TOT", "negrito"))
    End Sub

    Private Sub CarregarDadosEmpresa()
        Try
            ' Usar as variáveis públicas carregadas no frmLogon
            dadosEmpresa = New DadosEmpresa() With {
                .RazaoSocial = If(String.IsNullOrEmpty(strEmitente_rasaosocial), "SUA EMPRESA LTDA-ME", strEmitente_rasaosocial),
                .CNPJ = If(String.IsNullOrEmpty(strEmitente_CNPJ), "00.000.000/0000-00", FormatarCNPJ(strEmitente_CNPJ)),
                .InscricaoEstadual = If(String.IsNullOrEmpty(strEmitente_IE), "000.000.000.00", strEmitente_IE),
                .Endereco = If(String.IsNullOrEmpty(strEmitente_endereco), "Rua Afonso Arinos, 1277", strEmitente_endereco),
                .Numero = If(String.IsNullOrEmpty(strEmitente_numero), "", strEmitente_numero),
                .Bairro = If(String.IsNullOrEmpty(strEmitente_bairro), "Centro", strEmitente_bairro),
                .Cidade = If(String.IsNullOrEmpty(strEmitente_cidade), "", strEmitente_cidade),
                .Telefone = If(String.IsNullOrEmpty(strEmitente_celular), "(00) 1234-5678", strEmitente_celular)
                          }
        Catch ex As Exception
            ' Em caso de erro, usar dados padrão
            dadosEmpresa = New DadosEmpresa() With {
                .RazaoSocial = "SUA EMPRESA LTDA-ME",
                .CNPJ = "00.000.000/0000-00",
                .InscricaoEstadual = "000.000.000.00",
                .Endereco = "Rua Afonso Arinos, 1277",
                .Numero = "",
                .Bairro = "Centro",
                .Cidade = "",
                .Telefone = "(00) 1234-5678"
            }
        End Try
    End Sub

    Private Sub CarregarDadosVenda()
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Carregar dados básicos da venda
                Dim query As String = "SELECT * FROM pedido_venda WHERE venda_id = ?"
                Using command As New OleDbCommand(query, con)
                    command.Parameters.AddWithValue("@venda_id", vendaId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            dadosVenda = New DadosVenda() With {
                                .vendaId = vendaId,
                                .DataVenda = Convert.ToDateTime(reader("data")),
                                .ValorTotal = Convert.ToDecimal(reader("total_valor")),
                                .ClienteNome = SafeDBValue(reader("nome")),
                                .ClienteDocumento = SafeDBValue(reader("documento")),
                                .Desconto = If(reader("vdesc") IsNot DBNull.Value, Convert.ToDecimal(reader("vdesc")), 0)
                            }
                        End If
                    End Using
                End Using

                ' Carregar forma de pagamento da tabela específica
                If dadosVenda IsNot Nothing Then
                    Dim queryPagamento As String = "SELECT tipo_pagamento FROM pedido_venda_condicaopagamento WHERE venda_id = ?"
                    Using commandPagamento As New OleDbCommand(queryPagamento, con)
                        commandPagamento.Parameters.AddWithValue("@venda_id", vendaId)
                        Dim formaPagamento As Object = commandPagamento.ExecuteScalar()
                        dadosVenda.FormaPagamento = If(formaPagamento IsNot Nothing, formaPagamento.ToString(), "Dinheiro")
                    End Using
                End If
            End Using
        Catch ex As Exception
            ' Dados padrão em caso de erro
            dadosVenda = New DadosVenda() With {
                .vendaId = vendaId,
                .DataVenda = DateTime.Now,
                .ValorTotal = 0,
                .ClienteNome = "CONSUMIDOR",
                .ClienteDocumento = "01064627170",
                .FormaPagamento = "Dinheiro",
                .Desconto = 0
            }
        End Try
    End Sub

    Private Sub AdicionarProdutos()
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim query As String = "SELECT * FROM pedido_venda_itens WHERE venda_id = ?"
                Console.WriteLine($"DEBUG CupomFiscal - Buscando itens para venda_id: {vendaId}")
                Using command As New OleDbCommand(query, con)
                    command.Parameters.AddWithValue("@venda_id", vendaId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        Dim itemCount As Integer = 0
                        While reader.Read()
                            itemCount += 1
                            Dim codigo As String = SafeDBValue(reader("codigo_produto"))
                            Dim descricao As String = SafeDBValue(reader("descricao"))
                            Dim quantidade As Decimal = Convert.ToDecimal(SafeDBValue(reader("quantidade")))
                            Dim valorUnitario As Decimal = Convert.ToDecimal(SafeDBValue(reader("valor_unitario")))
                            Dim valorTotal As Decimal = quantidade * valorUnitario
                            Console.WriteLine($"DEBUG CupomFiscal - Item {itemCount}: {codigo} - {descricao} - Qtd: {quantidade} - Valor: {valorUnitario}")

                            ' Formatação seguindo o layout da imagem
                            Dim codigoFormatado As String = codigo.Substring(0, Math.Min(6, codigo.Length)).PadRight(6)
                            Dim descricaoFormatada As String = If(descricao.Length > 20, descricao.Substring(0, 20), descricao).PadRight(20)
                            Dim qtdFormatada As String = quantidade.ToString("F0").PadLeft(3)
                            Dim unidade As String = "UN"
                            Dim valorUnitFormatado As String = valorUnitario.ToString("F2").PadLeft(6)
                            Dim valorTotalFormatado As String = valorTotal.ToString("F2").PadLeft(7)

                            cupomTexto.AppendLine($"{codigoFormatado} {descricaoFormatada} {qtdFormatada} {unidade} {valorUnitFormatado} {valorTotalFormatado}")
                        End While
                        Console.WriteLine($"DEBUG CupomFiscal - Total de itens encontrados: {itemCount}")
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Produtos fictícios em caso de erro com formatação otimizada
            cupomTexto.AppendLine("111111 TORNEIRA PLÁSTICA AC  1UN  50,00   50,00")
            cupomTexto.AppendLine("222222 MANGUEIRA BORRACHA M  1UN  42,50   42,50")
        End Try

        cupomTexto.AppendLine(New String("-", 48))
    End Sub

    Private Sub AdicionarTotalizadores()
        Dim qtdItens As Integer = 0
        Dim total As Decimal = 0

        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Buscar quantidade total de itens
                Dim queryQtd As String = "SELECT COUNT(*) as qtd_itens FROM pedido_venda_itens WHERE venda_id = ?"
                Using commandQtd As New OleDbCommand(queryQtd, con)
                    commandQtd.Parameters.AddWithValue("@venda_id", vendaId)
                    Dim resultQtd = commandQtd.ExecuteScalar()
                    If resultQtd IsNot Nothing AndAlso Not IsDBNull(resultQtd) Then
                        qtdItens = Convert.ToInt32(resultQtd)
                    End If
                End Using

                ' Buscar valor total
                Dim queryTotal As String = "SELECT SUM(valor_total) as total FROM pedido_venda_itens WHERE venda_id = ?"
                Using commandTotal As New OleDbCommand(queryTotal, con)
                    commandTotal.Parameters.AddWithValue("@venda_id", vendaId)
                    Dim resultTotal = commandTotal.ExecuteScalar()
                    If resultTotal IsNot Nothing AndAlso Not IsDBNull(resultTotal) Then
                        total = Convert.ToDecimal(resultTotal)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao buscar totalizadores: {ex.Message}")
            ' Usar valores padrão em caso de erro
            qtdItens = 1
            total = 50D
        End Try

        ' Se não encontrou dados, usar valores padrão
        If qtdItens = 0 Then qtdItens = 1
        If total = 0 Then total = 50D

        Console.WriteLine($"DEBUG Totalizadores - Qtd: {qtdItens}, Total: {total}")

        cupomTexto.AppendLine("QTD. TOTAL DE PRODUTOS")
        cupomTexto.AppendLine($"QTD. TOTAL DE ITENS{qtdItens.ToString().PadLeft(28)}")
        cupomTexto.AppendLine($"DESCONTO{"---".PadLeft(39)}")
        cupomTexto.AppendLine($"OUTRAS DESPESAS{"---".PadLeft(32)}")
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto($"VALOR TOTAL R${total.ToString("F2").PadLeft(30)}", "negrito"))
        cupomTexto.AppendLine(New String("-", 48))
    End Sub

    Private Sub AdicionarFormaPagamento()
        Dim valorTotalVenda As Decimal = 0
        Dim formaPagamento As String = "DINHEIRO"
        Dim valorPago As Decimal = 0
        Dim troco As Decimal = 0

        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Primeiro, buscar o valor total da venda
                Dim queryTotal As String = "SELECT SUM(valor_total) as total FROM pedido_venda_itens WHERE venda_id = ?"
                Using commandTotal As New OleDbCommand(queryTotal, con)
                    commandTotal.Parameters.AddWithValue("@venda_id", vendaId)
                    Dim resultTotal = commandTotal.ExecuteScalar()
                    If resultTotal IsNot Nothing AndAlso Not IsDBNull(resultTotal) Then
                        valorTotalVenda = Convert.ToDecimal(resultTotal)
                    End If
                End Using

                ' Buscar dados de pagamento da tabela tbl_Tipo_Pagamento
                Dim queryPagamento As String = "SELECT tipo_pagamento, valor FROM pedido_venda_condicaopagamento WHERE venda_id = ?"
                Using commandPagamento As New OleDbCommand(queryPagamento, con)
                    commandPagamento.Parameters.AddWithValue("@venda_id", vendaId)
                    Using readerPagamento As OleDbDataReader = commandPagamento.ExecuteReader()
                        If readerPagamento.Read() Then
                            formaPagamento = If(IsDBNull(readerPagamento("tipo_pagamento")), "DINHEIRO", readerPagamento("tipo_pagamento").ToString().ToUpper())
                            valorPago = Convert.ToDecimal(SafeDBValue(readerPagamento, "valor"))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao buscar dados de pagamento: {ex.Message}")
        End Try

        ' Se não encontrou valor total, usar padrão
        If valorTotalVenda = 0 Then valorTotalVenda = 50D

        ' Calcular troco apenas se valor pago foi encontrado
        If valorPago > 0 Then
            troco = valorPago - valorTotalVenda
        Else
            troco = 0
        End If

        Console.WriteLine($"DEBUG Pagamento - Forma: {formaPagamento}, Valor Pago: {valorPago}, Total: {valorTotalVenda}, Troco: {troco}")

        ' Formatação seguindo o layout da imagem
        cupomTexto.AppendLine($"FORMA PGTO: {formaPagamento}")
        If valorPago > 0 Then
            cupomTexto.AppendLine($"VALOR PAGO: R$ {valorPago.ToString("F2")}")
        Else
            cupomTexto.AppendLine($"VALOR PAGO: R$ {valorTotalVenda.ToString("F2")}")
        End If

        If troco > 0 Then
            cupomTexto.AppendLine($"TROCO: R$ {troco.ToString("F2")}")
        Else
            cupomTexto.AppendLine($"TROCO: R$ 0,00")
        End If

        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(New String("-", 48))
    End Sub

    Private Sub AdicionarInformacoesFiscais()
        cupomTexto.AppendLine(FormatarTexto(Centralizar("CONSULTA PELA CHAVE DE ACESSO:"), "negrito"))
        cupomTexto.AppendLine(Centralizar("www.sefaz.gov.br/nfce/consultanfce"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto(Centralizar("CHAVE DE ACESSO"), "negrito"))
        cupomTexto.AppendLine(Centralizar(FormatarChaveAcesso(chaveAcesso)))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto(Centralizar("CONSUMIDOR"), "negrito"))
        If dadosVenda IsNot Nothing AndAlso Not String.IsNullOrEmpty(dadosVenda.ClienteNome) AndAlso dadosVenda.ClienteNome <> "CONSUMIDOR" Then
            cupomTexto.AppendLine(Centralizar($"NOME: {dadosVenda.ClienteNome.ToUpper()}"))
        Else
            cupomTexto.AppendLine(Centralizar("NOME: NÃO IDENTIFICADO"))
        End If
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(New String("-", 48))

        ' Sempre mostrar número e série quando há XML da NFCe
        Dim numeroExibir As String = If(String.IsNullOrEmpty(numeroNFe), vendaId.ToString(), numeroNFe)
        Dim serieExibir As String = If(String.IsNullOrEmpty(serieNFe), "1", serieNFe)
        cupomTexto.AppendLine(FormatarTexto(Centralizar($"Nº {numeroExibir} Série {serieExibir}"), "negrito"))
        cupomTexto.AppendLine(Centralizar($"{DateTime.Now:dd/MM/yyyy} - Via Consumidor"))
        cupomTexto.AppendLine("")

        ' Verificar se há protocolo ou se é contingência
        If Not String.IsNullOrEmpty(protocoloAutorizacao) Then
            cupomTexto.AppendLine(FormatarTexto(Centralizar("PROTOCOLO DE AUTORIZAÇÃO"), "negrito"))
            cupomTexto.AppendLine(Centralizar($"{protocoloAutorizacao} - {DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm}min"))
        ElseIf Not String.IsNullOrEmpty(chaveAcesso) Then
            cupomTexto.AppendLine(FormatarTexto(Centralizar("NFCe EM CONTINGÊNCIA"), "negrito"))
            cupomTexto.AppendLine(Centralizar($"Chave: {chaveAcesso.Substring(0, Math.Min(20, chaveAcesso.Length))}..."))
            cupomTexto.AppendLine(Centralizar($"{DateTime.Now:dd/MM/yyyy} - {DateTime.Now:HH:mm}min"))
        End If

        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(New String("-", 48))
        cupomTexto.AppendLine(Centralizar("Consulta via leitor de QR Code"))
        cupomTexto.AppendLine("")
    End Sub

    Private Sub AdicionarQRCode()
        Try
            Console.WriteLine($"[AdicionarQRCode] qrCodeUrl = '{qrCodeUrl}' (len={If(qrCodeUrl IsNot Nothing, qrCodeUrl.Length, 0)})")
            Console.WriteLine($"[AdicionarQRCode] chaveAcesso = '{chaveAcesso}' (len={If(chaveAcesso IsNot Nothing, chaveAcesso.Length, 0)})")

            ' Usar exclusivamente o valor do QR Code extraído do XML
            If Not String.IsNullOrEmpty(qrCodeUrl) Then
                ' Sanitizar conteúdo do QR antes de gerar
                Dim qrContent As String = qrCodeUrl.Trim()
                qrContent = System.Text.RegularExpressions.Regex.Replace(qrContent, "[\u0000-\u001F]", "")
                qrContent = qrContent.Replace(vbCr, "").Replace(vbLf, "").Replace(vbTab, "")
                Console.WriteLine($"DEBUG - Conteúdo QR (len={qrContent.Length}): {If(qrContent.Length > 200, qrContent.Substring(0, 200) & "...", qrContent)}")

                Try
                    Dim qrGenerator As New QRCoder.QRCodeGenerator()
                    Dim qrCodeData As QRCoder.QRCodeData = qrGenerator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q)
                    Dim qrCode As New QRCoder.QRCode(qrCodeData)
                    qrCodeImage = qrCode.GetGraphic(10)

                    cupomTexto.AppendLine("")
                    cupomTexto.AppendLine(Centralizar("[QR CODE SERÁ IMPRESSO AQUI]"))
                    cupomTexto.AppendLine("")
                    Console.WriteLine("DEBUG - QR Code imagem gerada com sucesso (XML)")
                Catch qrEx As Exception
                    Try
                        Dim writer As New ZXing.BarcodeWriter()
                        writer.Format = ZXing.BarcodeFormat.QR_CODE
                        writer.Options = New ZXing.Common.EncodingOptions() With {
                            .Width = 400,
                            .Height = 400,
                            .Margin = 0
                        }
                        qrCodeImage = writer.Write(qrContent)

                        cupomTexto.AppendLine("")
                        cupomTexto.AppendLine(Centralizar("[QR CODE SERÁ IMPRESSO AQUI]"))
                        cupomTexto.AppendLine("")
                        Console.WriteLine("DEBUG - QR Code imagem gerada com ZXing (XML)")
                    Catch zxingEx As Exception
                        cupomTexto.AppendLine("")
                        Console.WriteLine($"DEBUG - Erro ao gerar QR Code com ambas bibliotecas: {zxingEx.Message}")
                    End Try
                End Try
            Else
                ' Sem URL de QR Code no XML: apenas instruções de consulta manual
                Console.WriteLine("[AdicionarQRCode] QR Code URL vazio/nulo - adicionando instruções de consulta manual")
                cupomTexto.AppendLine("")
                cupomTexto.AppendLine(Centralizar("CONSULTE PELA CHAVE DE ACESSO EM"))
                cupomTexto.AppendLine(Centralizar("www.nfe.fazenda.gov.br"))
                cupomTexto.AppendLine(Centralizar("OU"))
                cupomTexto.AppendLine(Centralizar("www.nfce.fazenda.gov.br"))
                If Not String.IsNullOrEmpty(chaveAcesso) Then
                    cupomTexto.AppendLine("")
                    cupomTexto.AppendLine(Centralizar("CHAVE DE ACESSO:"))
                    cupomTexto.AppendLine(Centralizar(chaveAcesso))
                Else
                    cupomTexto.AppendLine("")
                    cupomTexto.AppendLine(Centralizar("CHAVE DE ACESSO NÃO DISPONÍVEL"))
                End If
                cupomTexto.AppendLine("")
                Console.WriteLine("DEBUG - QR Code URL ausente; imprimindo instruções de consulta manual")
            End If

        Catch ex As Exception
            Console.WriteLine("DEBUG - Erro ao processar QR Code: " & ex.Message)
        End Try
    End Sub



    Private Sub AdicionarRodape()
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(Centralizar("ÁREA DE MENSAGEM DE INTERESSE DO CONTRIBUINTE"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(Centralizar("Inf dos Tributos Totais Incidentes (Lei Federal"))
        cupomTexto.AppendLine(Centralizar("NFCe EMITIDO PARA TESTE DE IMPRESSÃO"))
        cupomTexto.AppendLine("")
    End Sub

    Public Sub Imprimir()
        Try
            ' Verificar se o PrintDocument foi inicializado
            If pd Is Nothing Then
                pd = New PrintDocument()
                Console.WriteLine("PrintDocument foi inicializado")
            End If

            ' Configura a impressora térmica (usa a variável global do sistema)
            If String.IsNullOrEmpty(strImpressora) Then
                ' Se a impressora não estiver definida, usar a impressora padrão
                Console.WriteLine("Impressora não definida, usando impressora padrão")
                Dim settings As New PrinterSettings()
                strImpressora = settings.PrinterName
                Console.WriteLine($"Impressora padrão: {strImpressora}")
            End If

            ' Verificar se existe alguma impressora instalada
            If PrinterSettings.InstalledPrinters.Count = 0 Then
                MessageBox.Show("Nenhuma impressora instalada no sistema.", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Console.WriteLine("Nenhuma impressora instalada no sistema")
                Return
            End If

            ' Verificar se a impressora definida existe
            Dim impressoraExiste As Boolean = False
            For i As Integer = 0 To PrinterSettings.InstalledPrinters.Count - 1
                If PrinterSettings.InstalledPrinters(i).Equals(strImpressora, StringComparison.OrdinalIgnoreCase) Then
                    impressoraExiste = True
                    Exit For
                End If
            Next

            ' Se a impressora não existir, usar a primeira disponível
            If Not impressoraExiste Then
                strImpressora = PrinterSettings.InstalledPrinters(0)
                Console.WriteLine($"Impressora definida não encontrada. Usando a primeira disponível: {strImpressora}")
            End If

            pd.PrinterSettings.PrinterName = strImpressora

            ' Verificar se a impressora é válida
            If Not pd.PrinterSettings.IsValid Then
                MessageBox.Show($"Impressora '{strImpressora}' não está disponível ou configurada corretamente", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Console.WriteLine($"Impressora '{strImpressora}' não está disponível ou configurada corretamente")
                Return
            End If

            ' Configurar impressora para cupom fiscal (80mm)
            ' Aumentar altura da página para garantir que a CHAVE e o QR sejam impressos mesmo com muitos itens
            pd.DefaultPageSettings.PaperSize = New PaperSize("Cupom", 315, 1000) ' 80mm x ~250mm aprox
            ' Aumentar margem inferior para garantir avanço de papel antes do corte
            pd.DefaultPageSettings.Margins = New Margins(5, 5, 5, 30)

            ' Garantir que o evento PrintPage esteja associado
            RemoveHandler pd.PrintPage, AddressOf ImprimirPagina
            AddHandler pd.PrintPage, AddressOf ImprimirPagina

            Console.WriteLine($"Imprimindo cupom fiscal na impressora: {strImpressora}")
            pd.Print()
            Console.WriteLine("Impressão enviada com sucesso")

        Catch ex As Exception
            Console.WriteLine($"ERRO AO IMPRIMIR: {ex.Message}")
            MessageBox.Show($"Erro ao imprimir cupom: {ex.Message}", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' Não relançar a exceção para evitar que o fluxo seja interrompido
        End Try
    End Sub

    Public Sub ImprimirCupom()
        Imprimir()
    End Sub

    Private Sub ImprimirPagina(sender As Object, e As PrintPageEventArgs)
        ' Fontes conforme layout da imagem
        Dim fonteCabecalho As New Font("Arial", 9, FontStyle.Bold)
        Dim fonteNormal As New Font("Arial", 8)
        Dim fonteItens As New Font("Courier New", 7)
        Dim fonteNegrito As New Font("Arial", 8, FontStyle.Bold)
        Dim fontePequena As New Font("Arial", 7)
        Dim fonteQRCode As New Font("Courier New", 6)

        Dim yPos As Integer = 10
        Dim xPos As Integer = 10
        Dim linhaAltura As Integer = 14
        Dim larguraPagina As Integer = 280 ' 80mm

        ' Formatação centralizada
        Dim formatoCentro As New StringFormat()
        formatoCentro.Alignment = StringAlignment.Center

        Dim formatoEsquerda As New StringFormat()
        formatoEsquerda.Alignment = StringAlignment.Near


        ' === CABEÇALHO DA EMPRESA ===
        Dim nomeEmpresa As String = If(dadosEmpresa IsNot Nothing AndAlso Not String.IsNullOrEmpty(strEmitente_fantasia), strEmitente_rasaosocial, "SUA EMPRESA LTDA-ME")
        e.Graphics.DrawString(nomeEmpresa, fonteCabecalho, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        Dim cnpjFormatado As String = If(dadosEmpresa IsNot Nothing AndAlso Not String.IsNullOrEmpty(strEmitente_CNPJ), FormatarCNPJ(strEmitente_CNPJ), "00.000.000/0000-00")
        Dim inscricaoEstadual As String = If(dadosEmpresa IsNot Nothing AndAlso Not String.IsNullOrEmpty(strEmitente_IE), strEmitente_IE, "000.000.000.00")
        e.Graphics.DrawString($"CNPJ {cnpjFormatado} I.E. {inscricaoEstadual}", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        Dim enderecoCompleto As String = If(dadosEmpresa IsNot Nothing AndAlso Not String.IsNullOrEmpty(strEmitente_endereco), strEmitente_endereco, "Rua Afonso Arinos, 1277 - Centro - Fone(00) 1234-5678")
        e.Graphics.DrawString(enderecoCompleto, fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 5

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' === TÍTULO DO DOCUMENTO ===
        e.Graphics.DrawString("DANFE NFC-e - Documento Auxiliar", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura
        e.Graphics.DrawString("da Nota Fiscal Eletrônica para Consumidor Final", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura
        e.Graphics.DrawString("Não permite aproveitamento de crédito de ICMS", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 5

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' === DETALHE DA VENDA ===
        e.Graphics.DrawString("DETALHE DA VENDA", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura
        yPos += linhaAltura

        ' Cabeçalho dos itens
        e.Graphics.DrawString("CÓDIGO DESCRIÇÃO         QTD UN  VLUN  VLTOT", fonteNormal, Brushes.Black, xPos, yPos)
        yPos += linhaAltura

        ' Itens da venda - carregando dados reais
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim query As String = "SELECT * FROM pedido_venda_itens WHERE venda_id = ?"
                Using command As New OleDbCommand(query, con)
                    command.Parameters.AddWithValue("@venda_id", vendaId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim codigo As String = SafeDBValue(reader("codigo_produto")).ToString()
                            Dim descricao As String = SafeDBValue(reader("descricao")).ToString()
                            Dim quantidade As Decimal = Convert.ToDecimal(SafeDBValue(reader("quantidade")))
                            Dim unidade As String = "UN"
                            Dim valorUnitario As Decimal = Convert.ToDecimal(SafeDBValue(reader("valor_unitario")))
                            Dim valorTotalItem As Decimal = quantidade * valorUnitario

                            If codigo.Length > 15 Then
                                ' Código muito longo - colocar em linha separada
                                e.Graphics.DrawString($"COD: {codigo}", fonteItens, Brushes.Black, xPos, yPos)
                                yPos += linhaAltura
                                If descricao.Length > 25 Then descricao = descricao.Substring(0, 25)
                                descricao = descricao.PadRight(25)
                                Dim linhaItem As String = $"{descricao} {quantidade.ToString("F0").PadLeft(2)}{unidade} {valorUnitario.ToString("F2").PadLeft(6)} {valorTotalItem.ToString("F2").PadLeft(7)}"
                                e.Graphics.DrawString(linhaItem, fonteItens, Brushes.Black, xPos, yPos)
                            Else
                                ' Código normal - formatação padrão
                                Dim codigoFormatado As String = codigo.PadRight(6)
                                If descricao.Length > 18 Then descricao = descricao.Substring(0, 18)
                                descricao = descricao.PadRight(18)
                                Dim linhaItem As String = $"{codigoFormatado} {descricao} {quantidade.ToString("F0").PadLeft(2)}{unidade} {valorUnitario.ToString("F2").PadLeft(6)} {valorTotalItem.ToString("F2").PadLeft(7)}"
                                e.Graphics.DrawString(linhaItem, fonteItens, Brushes.Black, xPos, yPos)
                            End If
                            yPos += linhaAltura
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Itens fictícios em caso de erro
            e.Graphics.DrawString("111111 TORNEIRA PLÁSTICA     1UN  50,00   50,00", fonteItens, Brushes.Black, xPos, yPos)
            yPos += linhaAltura
            e.Graphics.DrawString("222222 MANGUEIRA BORRACHA    1UN  42,50   42,50", fonteItens, Brushes.Black, xPos, yPos)
            yPos += linhaAltura
        End Try

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' === TOTAIS ===
        ' Garantir que dadosVenda está inicializado
        If dadosVenda Is Nothing Then
            dadosVenda = New DadosVenda()
        End If

        Dim quantidadeTotal As Integer = 0
        Dim valorTotalVenda As Decimal = If(dadosVenda IsNot Nothing, dadosVenda.ValorTotal, 0)
        Dim desconto As Decimal = If(dadosVenda IsNot Nothing, dadosVenda.Desconto, 0)

        ' Calcular quantidade total de produtos
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim queryQtd As String = "SELECT SUM(quantidade) FROM pedido_venda_itens WHERE venda_id = ?"
                Using command As New OleDbCommand(queryQtd, con)
                    command.Parameters.AddWithValue("?", vendaId)
                    Dim resultado = command.ExecuteScalar()
                    If resultado IsNot Nothing AndAlso Not IsDBNull(resultado) Then
                        quantidadeTotal = Convert.ToInt32(resultado)
                    End If
                End Using
            End Using
        Catch ex As Exception
            quantidadeTotal = 2 ' Valor padrão em caso de erro
        End Try

        e.Graphics.DrawString($"QTD. TOTAL DE PRODUTOS{quantidadeTotal.ToString().PadLeft(25)}", fonteNormal, Brushes.Black, xPos, yPos)
        yPos += linhaAltura

        Dim descontoTexto As String = If(desconto > 0, desconto.ToString("F2"), "---")
        e.Graphics.DrawString($"DESCONTO{descontoTexto.PadLeft(40)}", fonteNormal, Brushes.Black, xPos, yPos)
        yPos += linhaAltura

        e.Graphics.DrawString($"OUTRAS DESPESAS{"---".PadLeft(33)}", fonteNormal, Brushes.Black, xPos, yPos)
        yPos += linhaAltura

        ' Alinha o VALOR TOTAL com coluna de valores à direita
        Dim formatoDireita As New StringFormat()
        formatoDireita.Alignment = StringAlignment.Far
        formatoDireita.LineAlignment = StringAlignment.Near
        e.Graphics.DrawString("VALOR TOTAL R$", fonteNegrito, Brushes.Black, xPos, yPos)
        e.Graphics.DrawString(valorTotalVenda.ToString("F2"), fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina - 10, linhaAltura), formatoDireita)
        yPos += linhaAltura + 5

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' === FORMA DE PAGAMENTO ===
        e.Graphics.DrawString("FORMA DE PAGAMENTO", fonteNegrito, Brushes.Black, xPos, yPos)
        e.Graphics.DrawString("Valor Pago", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina - 10, linhaAltura), formatoDireita)
        yPos += linhaAltura + 3

        Dim formaPagamento As String = If(dadosVenda IsNot Nothing AndAlso Not String.IsNullOrEmpty(dadosVenda.FormaPagamento), dadosVenda.FormaPagamento, "Dinheiro")
        Dim valorPagamento As Decimal = 0

        ' Buscar valor real pago da base de dados
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim queryPagamento As String = "SELECT valor FROM pedido_venda_condicaopagamento WHERE venda_id = ?"
                Using commandPagamento As New OleDbCommand(queryPagamento, con)
                    commandPagamento.Parameters.AddWithValue("@venda_id", vendaId)
                    Dim resultPagamento = commandPagamento.ExecuteScalar()
                    If resultPagamento IsNot Nothing AndAlso Not IsDBNull(resultPagamento) Then
                        valorPagamento = Convert.ToDecimal(resultPagamento)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao buscar valor pago: {ex.Message}")
            valorPagamento = 0
        End Try

        ' Linhas de pagamento alinhadas com coluna de valores
        If formaPagamento.Length > 40 Then formaPagamento = formaPagamento.Substring(0, 40)
        e.Graphics.DrawString(formaPagamento, fonteNormal, Brushes.Black, xPos, yPos)
        e.Graphics.DrawString(valorPagamento.ToString("F2"), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina - 10, linhaAltura), formatoDireita)
        yPos += linhaAltura

        ' Troco (valor pago - total), somente se houver pagamento
        Dim troco As Decimal = 0D
        If valorPagamento > 0 AndAlso valorTotalVenda > 0 Then
            troco = Math.Max(valorPagamento - valorTotalVenda, 0D)
        End If
        e.Graphics.DrawString("TROCO", fonteNormal, Brushes.Black, xPos, yPos)
        e.Graphics.DrawString(troco.ToString("F2"), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina - 10, linhaAltura), formatoDireita)
        yPos += linhaAltura + 5

        ' === CONSULTA PELA CHAVE DE ACESSO (apenas se houver chave real) ===
        If Not String.IsNullOrEmpty(chaveAcesso) Then
            e.Graphics.DrawString("CONSULTA PELA CHAVE DE ACESSO:", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura
            e.Graphics.DrawString("www.sefaz.gov.br/nfce/consultanfce", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura + 3

            e.Graphics.DrawString("CHAVE DE ACESSO", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura

            Dim chaveFormatada As String = FormatarChaveAcesso(chaveAcesso)
            e.Graphics.DrawString(chaveFormatada, fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura + 5
        End If

        ' === CONSUMIDOR ===
        e.Graphics.DrawString("CONSUMIDOR", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura
        e.Graphics.DrawString("NOME: NÃO IDENTIFICADO", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 5

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' === NÚMERO E SÉRIE (sempre mostrar quando há XML da NFCe) ===
        Dim numeroExibir As String = If(String.IsNullOrEmpty(numeroNFe), vendaId.ToString(), numeroNFe)
        Dim serieExibir As String = If(String.IsNullOrEmpty(serieNFe), "1", serieNFe)
        e.Graphics.DrawString($"Nº {numeroExibir} Série {serieExibir}", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        Dim dataVenda As DateTime = If(dadosVenda IsNot Nothing, dadosVenda.DataVenda, DateTime.Now)
        e.Graphics.DrawString($"{dataVenda.ToString("dd/MM/yyyy")}  -  Via Consumidor", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 3

        ' === PROTOCOLO DE AUTORIZAÇÃO (apenas se houver protocolo real) ===
        If Not String.IsNullOrEmpty(protocoloAutorizacao) Then
            e.Graphics.DrawString("PROTOCOLO DE AUTORIZAÇÃO", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura

            e.Graphics.DrawString($"{protocoloAutorizacao} - {dataVenda.ToString("dd/MM/yyyy")} - {dataVenda.ToString("HH\hmm\min")}", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
            yPos += linhaAltura + 5
        End If

        ' Linha separadora
        e.Graphics.DrawString(New String("-", 80), fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        e.Graphics.DrawString("Consulta via leitor de QR Code", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 5

        ' Imprimir QR Code se disponível
        If qrCodeImage IsNot Nothing Then
            Try
                ' Calcular posição centralizada para o QR Code
                Dim qrSize As Integer = 120 ' Tamanho do QR Code em pixels
                Dim xPosQR As Single = (larguraPagina - qrSize) / 2

                ' Desenhar a imagem do QR Code
                e.Graphics.DrawImage(qrCodeImage, xPosQR, yPos, qrSize, qrSize)
                yPos += qrSize + 10

                Console.WriteLine("DEBUG - QR Code imagem impressa")
            Catch qrEx As Exception
                Console.WriteLine($"DEBUG - Erro ao imprimir QR Code: {qrEx.Message}")
                yPos += 10
            End Try
        Else
            yPos += 10
        End If

        ' === RODAPÉ ===
        e.Graphics.DrawString("Inf dos Tributos Totais Incidentes (Lei Federal", fonteNormal, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura
        e.Graphics.DrawString("Genuss Automação Comercial", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura + 3
        e.Graphics.DrawString("Fone: (66)9-9620-6025", fonteNegrito, Brushes.Black, New RectangleF(0, yPos, larguraPagina, linhaAltura), formatoCentro)
        yPos += linhaAltura

        ' Espaço de segurança após o QR Code para evitar corte da guilhotina em cima do código
        ' Desenha uma área em branco para forçar avanço de papel antes do fim da página
        Dim margemSegurancaPx As Integer = 160 ' ~20-25mm dependendo da DPI
        e.Graphics.FillRectangle(Brushes.White, 0, yPos, larguraPagina, margemSegurancaPx)
        yPos += margemSegurancaPx

        ' Garantir que não há mais páginas
        e.HasMorePages = False

        ' Dispose das fontes
        fonteCabecalho.Dispose()
        fonteNormal.Dispose()
        fonteItens.Dispose()
        fonteNegrito.Dispose()
        fontePequena.Dispose()
    End Sub

    Private Function Centralizar(texto As String) As String
        If texto.Length >= LARGURA_CUPOM Then
            Return texto.Substring(0, LARGURA_CUPOM)
        End If

        Dim espacos As Integer = (LARGURA_CUPOM - texto.Length) \ 2
        Return New String(" "c, espacos) & texto
    End Function

    Private Function SafeDBValue(reader As OleDbDataReader, fieldName As String) As String
        Try
            If reader.IsDBNull(reader.GetOrdinal(fieldName)) Then
                Return ""
            Else
                Return reader(fieldName).ToString()
            End If
        Catch
            Return ""
        End Try
    End Function

    Private Function FormatarChaveAcesso(chave As String) As String
        If String.IsNullOrEmpty(chave) OrElse chave.Length <> 44 Then
            Return chave
        End If

        ' Formatar chave de acesso em grupos de 4 dígitos
        Dim resultado As New StringBuilder()
        For i As Integer = 0 To chave.Length - 1 Step 4
            If i > 0 Then resultado.Append(" ")
            resultado.Append(chave.Substring(i, Math.Min(4, chave.Length - i)))
        Next

        Return resultado.ToString()
    End Function

    Private Function FormatarTexto(texto As String, ParamArray formatos() As String) As String
        ' Comandos ESC/POS para formatação de texto em impressoras térmicas
        Dim resultado As String = texto
        Dim inicioComandos As String = ""
        Dim fimComandos As String = ""

        For Each formato In formatos
            Select Case formato.ToLower()
                Case "negrito"
                    inicioComandos += Chr(27) & "E" & Chr(1)  ' ESC E 1 - Liga negrito
                    fimComandos = Chr(27) & "E" & Chr(0) & fimComandos  ' ESC E 0 - Desliga negrito
                Case "expandido"
                    inicioComandos += Chr(27) & "!" & Chr(16)  ' ESC ! 16 - Liga expandido
                    fimComandos = Chr(27) & "!" & Chr(0) & fimComandos  ' ESC ! 0 - Desliga expandido
                Case "sublinhado"
                    inicioComandos += Chr(27) & "-" & Chr(1)  ' ESC - 1 - Liga sublinhado
                    fimComandos = Chr(27) & "-" & Chr(0) & fimComandos  ' ESC - 0 - Desliga sublinhado
                Case "italico"
                    inicioComandos += Chr(27) & "4"  ' ESC 4 - Liga itálico
                    fimComandos = Chr(27) & "5" & fimComandos  ' ESC 5 - Desliga itálico
                Case "invertido"
                    inicioComandos += Chr(29) & "B" & Chr(1)  ' GS B 1 - Liga invertido
                    fimComandos = Chr(29) & "B" & Chr(0) & fimComandos  ' GS B 0 - Desliga invertido
                Case "condensado"
                    inicioComandos += Chr(27) & Chr(15)  ' ESC SI - Liga condensado
                    fimComandos = Chr(18) & fimComandos  ' DC2 - Desliga condensado
                Case "dupla_altura"
                    inicioComandos += Chr(27) & "!" & Chr(48)  ' ESC ! 48 - Liga dupla altura
                    fimComandos = Chr(27) & "!" & Chr(0) & fimComandos  ' ESC ! 0 - Desliga dupla altura
                Case "dupla_largura"
                    inicioComandos += Chr(27) & "!" & Chr(32)  ' ESC ! 32 - Liga dupla largura
                    fimComandos = Chr(27) & "!" & Chr(0) & fimComandos  ' ESC ! 0 - Desliga dupla largura
            End Select
        Next

        Return inicioComandos & resultado & fimComandos
    End Function

    Private Function FormatarCNPJ(cnpj As String) As String
        If String.IsNullOrEmpty(cnpj) Then Return "00.000.000/0000-00"

        ' Remove caracteres não numéricos
        Dim numeros As String = New String(cnpj.Where(Function(c) Char.IsDigit(c)).ToArray())

        If numeros.Length = 14 Then
            Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2, 3)}.{numeros.Substring(5, 3)}/{numeros.Substring(8, 4)}-{numeros.Substring(12, 2)}"
        Else
            Return cnpj
        End If
    End Function

    ' Sobrecarga para compatibilidade com código existente
    Private Function SafeDBValue(value As Object) As String
        Try
            If value Is Nothing OrElse value Is DBNull.Value Then
                Return ""
            Else
                Return value.ToString()
            End If
        Catch
            Return ""
        End Try
    End Function

    Private Function QuebrarDescricao(descricao As String, tamanhoLinha As Integer) As String()
        Dim linhas As New List(Of String)

        ' Quebra a descrição em palavras
        Dim palavras As String() = descricao.Split(" "c)
        Dim linhaAtual As String = ""

        For Each palavra In palavras
            If linhaAtual.Length + palavra.Length + 1 <= tamanhoLinha Then
                linhaAtual += If(String.IsNullOrEmpty(linhaAtual), "", " ") & palavra
            Else
                linhas.Add(linhaAtual)
                linhaAtual = palavra
            End If
        Next

        If Not String.IsNullOrEmpty(linhaAtual) Then
            linhas.Add(linhaAtual)
        End If

        ' Garante pelo menos 3 linhas
        While linhas.Count < 3
            linhas.Add("")
        End While

        Return linhas.Take(3).ToArray()
    End Function

    Private Function FormatCNPJ(cnpj As String) As String
        If cnpj.Length = 14 Then
            Return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12)}"
        End If
        Return cnpj
    End Function

    Private Function FormatCPF(cpf As String) As String
        If cpf.Length = 11 Then
            Return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9)}"
        End If
        Return cpf
    End Function

    ' Método para carregar dados da NFCe do banco de dados
    Private Sub CarregarDadosNFCeDoBanco()
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim query As String = "SELECT nfce_chave, nfce_protocolo, nfce_serie, nfce_numero, nfce_qrcode, nfe_xml FROM pedido_venda WHERE venda_id = ?"
                Using command As New OleDbCommand(query, con)
                    command.Parameters.AddWithValue("@venda_id", vendaId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        Dim encontrou As Boolean = reader.Read()
                        ' Removido fallback por Codigo: manter apenas consulta por venda_id
                        If encontrou Then
                            chaveAcesso = SafeDBValue(reader("nfce_chave"))
                            protocoloAutorizacao = SafeDBValue(reader("nfce_protocolo"))
                            serieNFe = SafeDBValue(reader("nfce_serie"))
                            numeroNFe = SafeDBValue(reader("nfce_numero"))
                            qrCodeUrl = SafeDBValue(reader("nfce_qrcode"))
                            Dim xmlContent As String = SafeDBValue(reader, "nfe_xml")

                            Console.WriteLine($"[CupomFiscalNFCe] Dados carregados do banco:")
                            Console.WriteLine($"[CupomFiscalNFCe] Chave: {chaveAcesso}")
                            Console.WriteLine($"[CupomFiscalNFCe] Protocolo: {protocoloAutorizacao}")
                            Console.WriteLine($"[CupomFiscalNFCe] Série: {serieNFe}")
                            Console.WriteLine($"[CupomFiscalNFCe] Número: {numeroNFe}")
                            ' Fallback: se informações críticas vierem vazias, tentar extrair do XML salvo
                            Dim precisaExtrair As Boolean = String.IsNullOrEmpty(chaveAcesso) OrElse String.IsNullOrEmpty(qrCodeUrl) OrElse String.IsNullOrEmpty(protocoloAutorizacao) OrElse String.IsNullOrEmpty(serieNFe) OrElse String.IsNullOrEmpty(numeroNFe)
                            If precisaExtrair AndAlso Not String.IsNullOrEmpty(xmlContent) Then
                                Console.WriteLine("[CupomFiscalNFCe] Campos NFCe vazios — tentando extrair do XML salvo no banco.")
                                ExtrairDadosXML(xmlContent)
                            End If

                        Else
                            ' Usar valores padrão se não encontrar no banco
                            chaveAcesso = "Chave não disponível"
                            protocoloAutorizacao = "Protocolo não disponível"
                            serieNFe = "1"
                            numeroNFe = vendaId.ToString()
                            qrCodeUrl = ""
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao carregar dados NFCe do banco: {ex.Message}")
            ' Valores padrão em caso de erro
            chaveAcesso = "Erro ao carregar chave"
            protocoloAutorizacao = "Erro ao carregar protocolo"
            serieNFe = "1"
            numeroNFe = vendaId.ToString()
            qrCodeUrl = ""
        End Try
    End Sub

    ' Método para obter string de conexão

    Private Function GetConnectionString() As String
        Try
            ' Usar a mesma lógica do módulo mdlAcesso
            Dim caminhoCompleto As String = System.IO.Path.Combine(Application.StartupPath, My.Settings.ConexaoBanco)
            Return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={caminhoCompleto};Persist Security Info=True;Jet OLEDB:Database Password=14023600"
        Catch ex As Exception
            ' String de conexão padrão em caso de erro
            Return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=banco.mdb;Persist Security Info=True;Jet OLEDB:Database Password=14023600"
        End Try
    End Function

    Private Function GetConnection() As OleDbConnection
        Return New OleDbConnection(connectionString)
    End Function
End Class

Public Class DadosVenda
    Public Property vendaId As Integer
    Public Property DataVenda As DateTime
    Public Property ValorTotal As Decimal
    Public Property ClienteNome As String
    Public Property ClienteDocumento As String
    Public Property FormaPagamento As String
    Public Property ValorPagamento As String
    Public Property Desconto As String
    Public Property NumeroCupom As String
    Public Property SerieCupom As String
End Class