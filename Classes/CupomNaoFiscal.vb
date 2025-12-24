Imports System.Text
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Data.OleDb
Imports System.Linq
Imports System.Xml

Public Class CupomNaoFiscal
    Private pd As PrintDocument
    Private cupomTexto As StringBuilder
    Private dadosEmpresa As DadosEmpresa
    Private dadosVenda As DadosVenda

    ' Constantes para formatação do cupom (48mm = 48 caracteres)
    Private Const LARGURA_CUPOM As Integer = 48
    Private Const LINHA_TRACEJADA As String = "------------------------------------------------"

    Private connectionString As String
    Private vendaId As Integer

    Public Sub New(connectionString As String, vendaId As Integer)
        Me.connectionString = connectionString
        Me.vendaId = vendaId
        Me.cupomTexto = New StringBuilder()

        CarregarDadosEmpresa()
        CarregarDadosVenda()
        pd = New PrintDocument()
        AddHandler pd.PrintPage, AddressOf ImprimirPagina
    End Sub

    Public Function GerarCupomNaoFiscal() As String
        Try
            cupomTexto.Clear()
            Console.WriteLine("[CupomNaoFiscal] Iniciando geração do cupom não fiscal")

            ' Estrutura do cupom não fiscal seguindo padrão profissional
            AdicionarCabecalhoEmpresa()
            AdicionarDetalhesDocumento()
            AdicionarDetalhesVenda()
            AdicionarProdutos()
            AdicionarTotalizadores()
            AdicionarFormaPagamento()
            AdicionarRodape()

            Console.WriteLine("[CupomNaoFiscal] Cupom gerado com sucesso")
            Return cupomTexto.ToString()
        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro ao gerar cupom: {ex.Message}")
            Return "Erro ao gerar cupom não fiscal: " & ex.Message
        End Try
    End Function

    Private Sub AdicionarCabecalhoEmpresa()
        Try
            Console.WriteLine("[CupomNaoFiscal] Adicionando cabeçalho da empresa")

            ' Nome da empresa em negrito e centralizado
            cupomTexto.AppendLine(FormatarTexto(Centralizar(dadosEmpresa.RazaoSocial), "negrito"))

            ' CNPJ e IE centralizados
            Dim linhaCNPJ As String = $"CNPJ {FormatarCNPJ(dadosEmpresa.CNPJ)}"
            If Not String.IsNullOrEmpty(dadosEmpresa.InscricaoEstadual) Then
                linhaCNPJ &= $" I.E. {dadosEmpresa.InscricaoEstadual}"
            End If
            cupomTexto.AppendLine(Centralizar(linhaCNPJ))

            ' Endereço centralizado
            Dim endereco As String = dadosEmpresa.Endereco
            If Not String.IsNullOrEmpty(dadosEmpresa.Numero) Then
                endereco &= $", {dadosEmpresa.Numero}"
            End If
            If Not String.IsNullOrEmpty(dadosEmpresa.Bairro) Then
                endereco &= $" - {dadosEmpresa.Bairro}"
            End If
            cupomTexto.AppendLine(Centralizar(endereco))

            ' Cidade, UF e Telefone centralizados
            Dim cidadeUF As String = dadosEmpresa.Cidade
            If Not String.IsNullOrEmpty(dadosEmpresa.UF) Then
                cidadeUF &= $" - {dadosEmpresa.UF}"
            End If
            If Not String.IsNullOrEmpty(dadosEmpresa.Telefone) Then
                cidadeUF &= $" - Fone{dadosEmpresa.Telefone}"
            End If
            cupomTexto.AppendLine(Centralizar(cidadeUF))

            cupomTexto.AppendLine("")
            cupomTexto.AppendLine(Centralizar(New String("-", LARGURA_CUPOM)))

        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro ao adicionar cabeçalho: {ex.Message}")
            ' Cabeçalho básico em caso de erro
            cupomTexto.AppendLine(Centralizar("CUPOM NÃO FISCAL"))
            cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
        End Try
    End Sub

    Private Sub AdicionarDetalhesDocumento()
        ' Título do documento seguindo formato NFCe
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto(Centralizar("DANFE NFC-e - Documento Auxiliar"), "negrito"))
        cupomTexto.AppendLine(Centralizar("da Nota Fiscal Eletrônica para Consumidor Final"))
        cupomTexto.AppendLine(Centralizar("Não permite aproveitamento de crédito de ICMS"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
    End Sub

    Private Sub AdicionarDetalhesVenda()
        cupomTexto.AppendLine(FormatarTexto(Centralizar("DETALHE DA VENDA"), "negrito"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine("CÓDIGO   DESCRIÇÃO         QTD UN VL UN   VL TOT")
        cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
    End Sub

    Private Sub AdicionarProdutos()
        Try
            Console.WriteLine("[CupomNaoFiscal] Adicionando produtos")

            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Query para buscar os itens da venda
                Dim query As String = "SELECT codigo_produto as codigo, descricao, quantidade, valor_unitario FROM pedido_venda_itens WHERE venda_id = ?"

                Using command As New OleDbCommand(query, con)
                    command.Parameters.AddWithValue("@venda_id", vendaId)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        While reader.Read()
                            Dim codigo As String = SafeDBValue(reader("codigo"))
                            Dim descricao As String = SafeDBValue(reader("descricao"))
                            Dim quantidade As Decimal = If(reader("quantidade") IsNot DBNull.Value, Convert.ToDecimal(reader("quantidade")), 0)
                            Dim valorUnitario As Decimal = If(reader("valor_unitario") IsNot DBNull.Value, Convert.ToDecimal(reader("valor_unitario")), 0)
                            Dim valorTotal As Decimal = quantidade * valorUnitario

                            ' Formatação para 48 caracteres
                            ' CÓDIGO(8) DESCRIÇÃO(18) QTD(3) UN(2) VL UN(7) VL TOT(8)
                            Dim codigoFormatado As String = codigo.PadRight(8).Substring(0, 8)
                            Dim descricaoFormatada As String = If(descricao.Length > 18, descricao.Substring(0, 18), descricao.PadRight(18))
                            Dim qtdFormatada As String = quantidade.ToString("F0").PadLeft(3)
                            Dim unidade As String = "UN"
                            Dim valorUnitFormatado As String = valorUnitario.ToString("F2").PadLeft(7)
                            Dim valorTotalFormatado As String = valorTotal.ToString("F2").PadLeft(8)

                            cupomTexto.AppendLine($"{codigoFormatado} {descricaoFormatada} {qtdFormatada} {unidade} {valorUnitFormatado} {valorTotalFormatado}")
                        End While
                    End Using
                End Using
            End Using

            cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))

        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro ao adicionar produtos: {ex.Message}")
            cupomTexto.AppendLine("Erro ao carregar produtos")
            cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
        End Try
    End Sub

    Private Sub AdicionarTotalizadores()
        Dim valorTotal As Decimal = dadosVenda.ValorTotal

        ' Contar total de itens
        Dim totalItens As Integer = 0
        Using con As OleDbConnection = GetConnection()
            con.Open()
            Dim query As String = "SELECT COUNT(*) FROM pedido_venda_itens WHERE venda_id = ?"
            Using command As New OleDbCommand(query, con)
                command.Parameters.AddWithValue("@venda_id", vendaId)
                totalItens = Convert.ToInt32(command.ExecuteScalar())
            End Using
        End Using

        ' Totalizadores para 48 caracteres
        cupomTexto.AppendLine("QTD. TOTAL DE PRODUTOS")
        cupomTexto.AppendLine($"QTD. TOTAL DE ITENS{totalItens.ToString().PadLeft(29)}")
        cupomTexto.AppendLine($"DESCONTO{"0,00".PadLeft(40)}")
        cupomTexto.AppendLine($"OUTRAS DESPESAS{"0,00".PadLeft(33)}")
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto($"VALOR TOTAL R${valorTotal.ToString("F2").PadLeft(29)}", "negrito"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
    End Sub

    Private Sub AdicionarFormaPagamento()
        cupomTexto.AppendLine(FormatarTexto("FORMA DE PAGAMENTO".PadRight(32) & " Valor Pago", "negrito"))
        cupomTexto.AppendLine("")

        Dim formaPagamento As String = If(String.IsNullOrEmpty(dadosVenda.FormaPagamento), "Dinheiro", dadosVenda.FormaPagamento)
        Dim valorFormatado As String = dadosVenda.ValorTotal.ToString("F2")

        cupomTexto.AppendLine($"{formaPagamento.PadRight(32)}{valorFormatado.PadLeft(16)}")

        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(Centralizar(LINHA_TRACEJADA))
    End Sub

    Private Sub AdicionarRodape()
        ' Informações técnicas seguindo formato do cupom fiscal
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine($"CUPOM: {If(String.IsNullOrEmpty(dadosVenda.NumeroCupom), vendaId.ToString().PadLeft(6, "0"c), dadosVenda.NumeroCupom)}")
        cupomTexto.AppendLine($"SÉRIE: {If(String.IsNullOrEmpty(dadosVenda.SerieCupom), "1", dadosVenda.SerieCupom)}")
        cupomTexto.AppendLine($"DATA/HORA: {dadosVenda.DataVenda.ToString("dd/MM/yyyy HH:mm:ss")}")
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(LINHA_TRACEJADA)
        cupomTexto.AppendLine(Centralizar("Este documento não possui valor fiscal"))
        cupomTexto.AppendLine(Centralizar("Para reclamações, procure o PROCON"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(FormatarTexto(Centralizar("OBRIGADO PELA PREFERÊNCIA!"), "negrito"))
        cupomTexto.AppendLine("")
        cupomTexto.AppendLine(LINHA_TRACEJADA)
        cupomTexto.AppendLine(Centralizar("Genuss Automação Comercial"))
        cupomTexto.AppendLine(Centralizar("Fone: (66)9-9620-6025"))
    End Sub

    Public Sub ImprimirCupom(Optional nomeImpressora As String = "")
        Try
            ' Verificar se o PrintDocument foi inicializado
            If pd Is Nothing Then
                pd = New PrintDocument()
                AddHandler pd.PrintPage, AddressOf ImprimirPagina
                Console.WriteLine("PrintDocument foi inicializado")
            End If

            ' Configurar a impressora
            If Not String.IsNullOrEmpty(nomeImpressora) Then
                pd.PrinterSettings.PrinterName = nomeImpressora
            ElseIf Not String.IsNullOrEmpty(strImpressora) Then
                pd.PrinterSettings.PrinterName = strImpressora
            Else
                ' Se a impressora não estiver definida, usar a impressora padrão
                Console.WriteLine("Impressora não definida, usando impressora padrão")
                Dim settings As New PrinterSettings()
                pd.PrinterSettings.PrinterName = settings.PrinterName
                Console.WriteLine($"Impressora padrão: {pd.PrinterSettings.PrinterName}")
            End If

            ' Verificar se existe alguma impressora instalada
            If PrinterSettings.InstalledPrinters.Count = 0 Then
                MessageBox.Show("Nenhuma impressora instalada no sistema.", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Console.WriteLine("Nenhuma impressora instalada no sistema")
                Return
            End If

            ' Verificar se a impressora é válida
            If Not pd.PrinterSettings.IsValid Then
                MessageBox.Show($"Impressora '{pd.PrinterSettings.PrinterName}' não está disponível ou configurada corretamente", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Console.WriteLine($"Impressora '{pd.PrinterSettings.PrinterName}' não está disponível ou configurada corretamente")
                Return
            End If

            ' Configurar impressora para cupom não fiscal (80mm)
            pd.DefaultPageSettings.PaperSize = New PaperSize("Cupom", 315, 600) ' 80mm x 150mm aprox
            pd.DefaultPageSettings.Margins = New Margins(5, 5, 5, 5)

            ' Garantir que o evento PrintPage esteja associado
            RemoveHandler pd.PrintPage, AddressOf ImprimirPagina
            AddHandler pd.PrintPage, AddressOf ImprimirPagina

            Console.WriteLine($"Imprimindo cupom não fiscal na impressora: {pd.PrinterSettings.PrinterName}")
            pd.Print()
            Console.WriteLine("Impressão enviada com sucesso")
            
        Catch ex As Exception
            Console.WriteLine($"ERRO AO IMPRIMIR: {ex.Message}")
            MessageBox.Show($"Erro ao imprimir cupom não fiscal: {ex.Message}", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' Não relançar a exceção para evitar que o fluxo seja interrompido
        End Try
    End Sub

    Private Sub ImprimirPagina(sender As Object, e As PrintPageEventArgs)
        Dim fonteNormal As New Font("Courier New", 8)
        Dim fonteNegrito As New Font("Courier New", 9, FontStyle.Bold)

        Dim yPos As Single = 10
        Dim xPos As Single = 0
        Dim linhaAltura As Single = fonteNormal.GetHeight(e.Graphics)
        Dim larguraPagina As Single = e.PageBounds.Width

        Dim formatoEsquerda As New StringFormat()
        formatoEsquerda.Alignment = StringAlignment.Near

        Dim formatoCentro As New StringFormat()
        formatoCentro.Alignment = StringAlignment.Center

        Dim linhas() As String = cupomTexto.ToString().Split(vbCrLf.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

        For Each linha As String In linhas
            If linha.Contains("[NEGRITO]") Then
                linha = linha.Replace("[NEGRITO]", "").Replace("[/NEGRITO]", "")
                e.Graphics.DrawString(linha, fonteNegrito, Brushes.Black, New RectangleF(xPos, yPos, larguraPagina, linhaAltura), formatoEsquerda)
            Else
                e.Graphics.DrawString(linha, fonteNormal, Brushes.Black, New RectangleF(xPos, yPos, larguraPagina, linhaAltura), formatoEsquerda)
            End If
            yPos += linhaAltura
        Next

        e.HasMorePages = False
    End Sub

    Private Sub CarregarDadosEmpresa()
        Try
            Console.WriteLine("[CupomNaoFiscal] Carregando dados da empresa")

            Using con As OleDbConnection = GetConnection()
                con.Open()
                Dim query As String = "SELECT Fantasia, Razao, CNPJ, Rua, Cidade, Estado FROM tbl_empresa"
                Using command As New OleDbCommand(query, con)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            dadosEmpresa = New DadosEmpresa() With {
                                .RazaoSocial = SafeDBValue(reader("Fantasia")),
                                .CNPJ = FormatarCNPJ(SafeDBValue(reader("CNPJ"))),
                                .InscricaoEstadual = If(String.IsNullOrEmpty(strEmitente_IE), "000.000.000.00", strEmitente_IE),
                                .Endereco = SafeDBValue(reader("Rua")),
                                .Numero = If(String.IsNullOrEmpty(strEmitente_numero), "", strEmitente_numero),
                                .Bairro = If(String.IsNullOrEmpty(strEmitente_bairro), "Centro", strEmitente_bairro),
                                .UF = SafeDBValue(reader("Estado")),
                                .Cidade = SafeDBValue(reader("Cidade")),
                                .Telefone = If(String.IsNullOrEmpty(strEmitente_celular), "(00) 1234-5678", strEmitente_celular)
                            }
                        Else
                            ' Fallback para variáveis globais se não encontrar na tabela
                            dadosEmpresa = New DadosEmpresa() With {
                                .RazaoSocial = If(String.IsNullOrEmpty(strEmitente_rasaosocial), "SUA EMPRESA LTDA-ME", strEmitente_rasaosocial),
                                .CNPJ = If(String.IsNullOrEmpty(strEmitente_CNPJ), "00.000.000/0000-00", FormatarCNPJ(strEmitente_CNPJ)),
                                .InscricaoEstadual = If(String.IsNullOrEmpty(strEmitente_IE), "000.000.000.00", strEmitente_IE),
                                .Endereco = If(String.IsNullOrEmpty(strEmitente_endereco), "Rua Exemplo, 123", strEmitente_endereco),
                                .Numero = If(String.IsNullOrEmpty(strEmitente_numero), "", strEmitente_numero),
                                .Bairro = If(String.IsNullOrEmpty(strEmitente_bairro), "Centro", strEmitente_bairro),
                                .UF = If(String.IsNullOrEmpty(strEmitente_estado), "SP", strEmitente_estado),
                                .Cidade = If(String.IsNullOrEmpty(strEmitente_cidade), "São Paulo", strEmitente_cidade),
                                .Telefone = If(String.IsNullOrEmpty(strEmitente_celular), "(00) 1234-5678", strEmitente_celular)
                            }
                        End If
                    End Using
                End Using
            End Using

            Console.WriteLine($"[CupomNaoFiscal] Empresa carregada: {dadosEmpresa.RazaoSocial}")

        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro ao carregar dados da empresa: {ex.Message}")

            ' Em caso de erro, usar dados padrão
            dadosEmpresa = New DadosEmpresa() With {
                .RazaoSocial = "SUA EMPRESA LTDA-ME",
                .CNPJ = "00.000.000/0000-00",
                .InscricaoEstadual = "000.000.000.00",
                .Endereco = "Rua Exemplo, 123",
                .Numero = "",
                .Bairro = "Centro",
                .UF = "SP",
                .Cidade = "São Paulo",
                .Telefone = "(00) 1234-5678"
            }
        End Try
    End Sub

    Private Sub CarregarDadosVenda()
        Try
            Console.WriteLine($"[CupomNaoFiscal] Carregando dados da venda ID: {vendaId}")

            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Tentar diferentes queries para encontrar os dados da venda
                Dim queries() As String = {
                    "SELECT pv.*, c.nome, c.documento FROM pedido_venda pv LEFT JOIN cliente c ON pv.cliente_id = c.codigo WHERE pv.codigo = ?",
                    "SELECT * FROM pedido_venda WHERE codigo = ?",
                    "SELECT * FROM pedido_venda WHERE venda_id = ?"
                }

                Dim dadosEncontrados As Boolean = False

                For Each query In queries
                    Try
                        Using command As New OleDbCommand(query, con)
                            command.Parameters.AddWithValue("@codigo", vendaId)
                            Using reader As OleDbDataReader = command.ExecuteReader()
                                If reader.Read() Then
                                    dadosEncontrados = True

                                    ' Extrair dados básicos com tratamento seguro
                                    Dim numeroCupom As String = SafeDBValue(reader("nfce_numero"))
                                    Dim serieCupom As String = SafeDBValue(reader("nfce_serie"))
                                    Dim xmlNFCe As String = SafeDBValue(reader("nfe_xml"))

                                    ' Se os dados não estão no banco, extrai do XML
                                    If (String.IsNullOrEmpty(numeroCupom) Or String.IsNullOrEmpty(serieCupom)) And Not String.IsNullOrEmpty(xmlNFCe) Then
                                        Try
                                            If String.IsNullOrEmpty(numeroCupom) Then
                                                numeroCupom = ExtrairNumeroDoXml(xmlNFCe).ToString()
                                            End If
                                            If String.IsNullOrEmpty(serieCupom) Then
                                                serieCupom = ExtrairSerieDoXml(xmlNFCe).ToString()
                                            End If
                                        Catch ex As Exception
                                            Console.WriteLine($"[CupomNaoFiscal] Erro na extração XML: {ex.Message}")
                                        End Try
                                    End If

                                    dadosVenda = New DadosVenda() With {
                                        .vendaId = vendaId,
                                        .DataVenda = If(reader("data") IsNot DBNull.Value, Convert.ToDateTime(reader("data")), DateTime.Now),
                                        .ValorTotal = If(reader("total_valor") IsNot DBNull.Value, Convert.ToDecimal(reader("total_valor")), 0),
                                        .ClienteNome = SafeDBValue(reader("nome")),
                                        .ClienteDocumento = SafeDBValue(reader("documento")),
                                        .Desconto = If(reader("vdesc") IsNot DBNull.Value, Convert.ToDecimal(reader("vdesc")), 0),
                                        .NumeroCupom = If(String.IsNullOrEmpty(numeroCupom), vendaId.ToString().PadLeft(6, "0"c), numeroCupom),
                                        .SerieCupom = If(String.IsNullOrEmpty(serieCupom), "1", serieCupom)
                                    }

                                    Console.WriteLine($"[CupomNaoFiscal] Venda carregada: {dadosVenda.ClienteNome} - Total: {dadosVenda.ValorTotal}")
                                    Exit For
                                End If
                            End Using
                        End Using

                    Catch ex As Exception
                        Console.WriteLine($"[CupomNaoFiscal] Erro na query: {query} - {ex.Message}")
                        Continue For
                    End Try
                Next

                ' Carregar forma de pagamento se os dados foram encontrados
                If dadosEncontrados AndAlso dadosVenda IsNot Nothing Then
                    Try
                        Dim queryPagamento As String = "SELECT tipo_pagamento FROM pedido_venda_condicaopagamento WHERE venda_id = ?"
                        Using commandPagamento As New OleDbCommand(queryPagamento, con)
                            commandPagamento.Parameters.AddWithValue("@venda_id", vendaId)
                            Dim formaPagamento As Object = commandPagamento.ExecuteScalar()
                            dadosVenda.FormaPagamento = If(formaPagamento IsNot Nothing, formaPagamento.ToString(), "Dinheiro")
                        End Using
                    Catch ex As Exception
                        Console.WriteLine($"[CupomNaoFiscal] Erro ao carregar forma de pagamento: {ex.Message}")
                        dadosVenda.FormaPagamento = "Dinheiro"
                    End Try
                End If

                ' Se não encontrou dados, criar dados padrão
                If Not dadosEncontrados Then
                    Console.WriteLine("[CupomNaoFiscal] Nenhum dado de venda encontrado, usando dados padrão")
                    dadosVenda = New DadosVenda() With {
                        .vendaId = vendaId,
                        .DataVenda = DateTime.Now,
                        .ValorTotal = 0,
                        .ClienteNome = "CONSUMIDOR",
                        .ClienteDocumento = "",
                        .FormaPagamento = "Dinheiro",
                        .Desconto = 0,
                        .NumeroCupom = vendaId.ToString().PadLeft(6, "0"c),
                        .SerieCupom = "1"
                    }
                End If
            End Using

        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro geral ao carregar dados da venda: {ex.Message}")

            ' Dados padrão em caso de erro geral
            dadosVenda = New DadosVenda() With {
                .vendaId = vendaId,
                .DataVenda = DateTime.Now,
                .ValorTotal = 0,
                .ClienteNome = "CONSUMIDOR",
                .ClienteDocumento = "",
                .FormaPagamento = "Dinheiro",
                .Desconto = 0,
                .NumeroCupom = vendaId.ToString().PadLeft(6, "0"c),
                .SerieCupom = "1"
            }
        End Try
    End Sub

    ''' <summary>
    ''' Extrai o número da NFCe do XML
    ''' </summary>
    Private Function ExtrairNumeroDoXml(xmlContent As String) As Integer
        Try
            If String.IsNullOrEmpty(xmlContent) Then
                Return 0
            End If

            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(xmlContent)

            Dim nsmgr As New XmlNamespaceManager(xmlDoc.NameTable)
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            ' Busca o elemento nNF
            Dim numeroNode As XmlNode = xmlDoc.SelectSingleNode("//nfe:nNF", nsmgr)
            If numeroNode IsNot Nothing Then
                Dim numero As Integer
                If Integer.TryParse(numeroNode.InnerText.Trim(), numero) Then
                    Return numero
                End If
            End If

            ' Tenta buscar sem namespace
            numeroNode = xmlDoc.SelectSingleNode("//nNF")
            If numeroNode IsNot Nothing Then
                Dim numero As Integer
                If Integer.TryParse(numeroNode.InnerText.Trim(), numero) Then
                    Return numero
                End If
            End If

            Return 0

        Catch ex As Exception
            Return 0
        End Try
    End Function

    ' Extrai a série da NFCe do XML
    Private Function ExtrairSerieDoXml(xmlContent As String) As Integer
        Try
            If String.IsNullOrEmpty(xmlContent) Then
                Return 0
            End If

            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(xmlContent)

            Dim nsmgr As New XmlNamespaceManager(xmlDoc.NameTable)
            nsmgr.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe")

            ' Busca o elemento serie
            Dim serieNode As XmlNode = xmlDoc.SelectSingleNode("//nfe:serie", nsmgr)
            If serieNode IsNot Nothing Then
                Dim serie As Integer
                If Integer.TryParse(serieNode.InnerText.Trim(), serie) Then
                    Return serie
                End If
            End If

            ' Tenta buscar sem namespace
            serieNode = xmlDoc.SelectSingleNode("//serie")
            If serieNode IsNot Nothing Then
                Dim serie As Integer
                If Integer.TryParse(serieNode.InnerText.Trim(), serie) Then
                    Return serie
                End If
            End If

            Return 0

        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function SafeDBValue(value As Object) As String
        Return If(value Is DBNull.Value OrElse value Is Nothing, String.Empty, value.ToString())
    End Function

    Private Function Centralizar(texto As String) As String
        ' Centralizar texto no cupom de 32 caracteres
        If texto.Length >= LARGURA_CUPOM Then
            Return texto.Substring(0, LARGURA_CUPOM)
        End If

        Dim espacos As Integer = (LARGURA_CUPOM - texto.Length) \ 2
        Return New String(" "c, espacos) & texto
    End Function

    Private Function FormatarTexto(texto As String, ParamArray estilos() As String) As String
        Dim resultado As String = texto

        ' Ajustar largura para caber no cupom de 32 caracteres
        If resultado.Length > LARGURA_CUPOM Then
            resultado = resultado.Substring(0, LARGURA_CUPOM)
        End If

        For Each estilo In estilos
            Select Case estilo.ToLower()
                Case "negrito"
                    resultado = "[NEGRITO]" & resultado & "[/NEGRITO]"
                Case "dupla_altura"
                    resultado = "[DUPLA_ALTURA]" & resultado & "[/DUPLA_ALTURA]"
            End Select
        Next

        Return resultado
    End Function

    Private Function FormatarCNPJ(cnpj As String) As String
        If String.IsNullOrEmpty(cnpj) OrElse cnpj.Length < 14 Then
            Return cnpj
        End If

        Dim numeros As String = New String(cnpj.Where(Function(c) Char.IsDigit(c)).ToArray())
        If numeros.Length = 14 Then
            Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2, 3)}.{numeros.Substring(5, 3)}/{numeros.Substring(8, 4)}-{numeros.Substring(12, 2)}"
        End If

        Return cnpj
    End Function

    Private Function GetConnection() As OleDbConnection
        Try
            Dim caminhoCompleto As String = System.IO.Path.Combine(Application.StartupPath, "DB\vendas.accdb")

            ' Verificar se o arquivo existe
            If Not System.IO.File.Exists(caminhoCompleto) Then
                Console.WriteLine($"[CupomNaoFiscal] Arquivo de banco não encontrado: {caminhoCompleto}")
                Throw New FileNotFoundException($"Banco de dados não encontrado: {caminhoCompleto}")
            End If

            Dim conn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & caminhoCompleto & ";Persist Security Info=True;Jet OLEDB:Database Password=14023600"
            Return New OleDbConnection(conn)

        Catch ex As Exception
            Console.WriteLine($"[CupomNaoFiscal] Erro ao criar conexão: {ex.Message}")
            Throw
        End Try
    End Function
End Class
