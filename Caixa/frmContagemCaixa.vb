Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Text
Public Class frmContagemCaixa
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub BtnNAO_Click(sender As Object, e As EventArgs) Handles BtnNAO.Click
        LimpaCampos()
        Me.Close()
    End Sub

    Public Sub CalcularTotaisVendasCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                ' Consulta para formas de pagamento
                Dim sql As String = "SELECT tipo_pagamento, SUM(valor) as total " &
                               "FROM pedido_venda_condicaopagamento " &
                               "WHERE FORMAT(data, 'yyyy-MM-dd') = FORMAT(@dataHoje, 'yyyy-MM-dd') " &
                               "GROUP BY tipo_pagamento"

                Dim cmd As New OleDbCommand(sql, con)
                cmd.Parameters.AddWithValue("@dataHoje", Date.Today)

                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                ' Inicializa totais
                Dim totalGeral As Decimal = 0
                Dim totalDinheiro As Decimal = 0
                Dim totalDebito As Decimal = 0
                Dim totalCredito As Decimal = 0
                Dim totalCheque As Decimal = 0
                Dim totalPix As Decimal = 0

                ' Processa resultados das formas de pagamento
                While reader.Read()
                    If Not IsDBNull(reader("total")) Then
                        Dim tipoPagamento As String = reader("tipo_pagamento").ToString().Trim().ToUpper()
                        Dim valor As Decimal = Convert.ToDecimal(reader("total"))
                        totalGeral += valor

                        Select Case tipoPagamento
                            Case "DINHEIRO"
                                totalDinheiro = valor
                            Case "DÉBITO", "DEBITO"
                                totalDebito = valor
                            Case "CRÉDITO", "CREDITO"
                                totalCredito = valor
                            Case "CHEQUE"
                                totalCheque = valor
                            Case "PIX"
                                totalPix = valor
                        End Select
                    End If
                End While
                reader.Close()

                ' Calcula totais de movimentação de caixa
                Dim abertura As Decimal = 0
                Dim suprimento As Decimal = 0
                Dim retirada As Decimal = 0

                Decimal.TryParse(txtAbertura.Text.Replace("R$", "").Trim(), abertura)
                Decimal.TryParse(txtSuprimento.Text.Replace("R$", "").Trim(), suprimento)
                Decimal.TryParse(txtRetirada.Text.Replace("R$", "").Trim(), retirada)

                Dim totalMovimentacao As Decimal = abertura + suprimento - retirada
                Dim totalFinal As Decimal = totalGeral + totalMovimentacao

                ' Atualiza a interface
                txtDinheiro.Text = totalDinheiro.ToString("C2")
                txtDebito.Text = totalDebito.ToString("C2")
                txtCredito.Text = totalCredito.ToString("C2")
                txtCheque.Text = totalCheque.ToString("C2")
                txtPIX.Text = totalPix.ToString("C2")
                txtTotal.Text = totalFinal.ToString("C2")

            Catch ex As Exception
                MessageBox.Show("Erro ao calcular totais: " & ex.Message, "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub frmContagemCaixa_Load(sender As Object, e As EventArgs) Handles Me.Load
        CalcularTotaisVendasCaixa()
        PTotalRecebidosTroco()
        PTotalEntradasSaidas()
        btnImprimir.Focus()
    End Sub

    Private Sub txtcDinheito_TextChanged(sender As Object, e As EventArgs) Handles txtcDinheito.TextChanged
        If Not IsNumeric(txtcDinheito.Text) Then txtcDinheito.Text = vbEmpty
        Utils.TextBoxMoeda(txtcDinheito)
    End Sub

    Private Sub txtcDinheito_LostFocus(sender As Object, e As EventArgs) Handles txtcDinheito.LostFocus
        txtcDinheito.Text = FormatCurrency(txtcDinheito.Text)
    End Sub

    Private Sub txtcDinheito_GotFocus(sender As Object, e As EventArgs) Handles txtcDinheito.GotFocus
        txtcDinheito.Text = txtcDinheito.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcDebito_TextChanged(sender As Object, e As EventArgs) Handles txtcDebito.TextChanged
        If Not IsNumeric(txtcDebito.Text) Then txtcDebito.Text = vbEmpty
        Utils.TextBoxMoeda(txtcDebito)
    End Sub

    Private Sub txtcDebito_LostFocus(sender As Object, e As EventArgs) Handles txtcDebito.LostFocus
        txtcDebito.Text = FormatCurrency(txtcDebito.Text)
    End Sub

    Private Sub txtcDebito_GotFocus(sender As Object, e As EventArgs) Handles txtcDebito.GotFocus
        txtcDebito.Text = txtcDebito.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcCredito_TextChanged(sender As Object, e As EventArgs) Handles txtcCredito.TextChanged
        If Not IsNumeric(txtcCredito.Text) Then txtcCredito.Text = vbEmpty
        Utils.TextBoxMoeda(txtcCredito)
    End Sub

    Private Sub txtcCredito_LostFocus(sender As Object, e As EventArgs) Handles txtcCredito.LostFocus
        txtcCredito.Text = FormatCurrency(txtcCredito.Text)
    End Sub

    Private Sub txtcCredito_GotFocus(sender As Object, e As EventArgs) Handles txtcCredito.GotFocus
        txtcCredito.Text = txtcCredito.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcPix_TextChanged(sender As Object, e As EventArgs) Handles txtcPix.TextChanged
        If Not IsNumeric(txtcPix.Text) Then txtcPix.Text = vbEmpty
        Utils.TextBoxMoeda(txtcPix)
    End Sub

    Private Sub txtcPix_LostFocus(sender As Object, e As EventArgs) Handles txtcPix.LostFocus
        txtcPix.Text = FormatCurrency(txtcPix.Text)
    End Sub

    Private Sub txtcPix_GotFocus(sender As Object, e As EventArgs) Handles txtcPix.GotFocus
        txtcPix.Text = txtcPix.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcCheque_TextChanged(sender As Object, e As EventArgs) Handles txtcCheque.TextChanged
        If Not IsNumeric(txtcCheque.Text) Then txtcCheque.Text = vbEmpty
        Utils.TextBoxMoeda(txtcCheque)
    End Sub

    Private Sub txtcCheque_LostFocus(sender As Object, e As EventArgs) Handles txtcCheque.LostFocus
        txtcCheque.Text = FormatCurrency(txtcCheque.Text)
    End Sub

    Private Sub txtcCheque_GotFocus(sender As Object, e As EventArgs) Handles txtcCheque.GotFocus
        txtcCheque.Text = txtcCheque.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcOutros_TextChanged(sender As Object, e As EventArgs) Handles txtcOutros.TextChanged
        If Not IsNumeric(txtcOutros.Text) Then txtcOutros.Text = vbEmpty
        Utils.TextBoxMoeda(txtcOutros)
    End Sub

    Private Sub txtcOutros_LostFocus(sender As Object, e As EventArgs) Handles txtcOutros.LostFocus
        txtcOutros.Text = FormatCurrency(txtcOutros.Text)
    End Sub

    Private Sub txtcOutros_GotFocus(sender As Object, e As EventArgs) Handles txtcOutros.GotFocus
        txtcOutros.Text = txtcOutros.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtcDinheito_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcDinheito.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorDInheiro As Double
            valorDInheiro = txtDinheiro.Text - txtcDinheito.Text
            txtrDinheiro.Text = FormatCurrency(valorDInheiro)
            somarTotalRestante()
            txtcDebito.Focus()
        End If
    End Sub

    Private Sub txtcDebito_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcDebito.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorDebito As Double
            valorDebito = txtDebito.Text - txtcDebito.Text
            txtrDebito.Text = FormatCurrency(valorDebito)
            somarTotalRestante()
            txtcCredito.Focus()
        End If
    End Sub

    Private Sub txtcCredito_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcCredito.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorCredito As Double
            valorCredito = txtCredito.Text - txtcCredito.Text
            txtrCredito.Text = FormatCurrency(valorCredito)
            somarTotalRestante()
            txtcPix.Focus()
        End If
    End Sub

    Private Sub txtcPix_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcPix.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorPix As Double
            valorPix = txtPIX.Text - txtcPix.Text
            txtrPix.Text = FormatCurrency(valorPix)
            somarTotalRestante()
            txtcCheque.Focus()
        End If
    End Sub

    Private Sub txtcCheque_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcCheque.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorCeque As Double
            valorCeque = txtCheque.Text - txtcCheque.Text
            txtrCheque.Text = FormatCurrency(valorCeque)
            somarTotalRestante()
            txtcOutros.Focus()
        End If
    End Sub

    Private Sub txtcOutros_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcOutros.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim valorOutros As Double
            valorOutros = txtOutros.Text - txtcOutros.Text
            txtrOutros.Text = FormatCurrency(valorOutros)
            somarTotalRestante()
            btnImprimir.Focus()
        End If
    End Sub
    Private Sub somarTotalRestante()
        Dim total As Double = 0

        'Lista de todas as TextBox a serem somadas
        Dim textBoxes() As TextBox = {txtrDinheiro, txtrDebito, txtrCredito, txtrPix, txtrCheque, txtrOutros}

        For Each txt As TextBox In textBoxes
            If Not String.IsNullOrEmpty(txt.Text) Then
                'Remove apenas o R$ e espaços, mantendo a vírgula decimal
                Dim valorLimpo As String = txt.Text.Replace("R$", "").Trim()

                'Converte considerando o formato monetário brasileiro
                Dim valorAtual As Double
                If Double.TryParse(valorLimpo, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"), valorAtual) Then
                    total += valorAtual
                Else
                    'Para debug - mostra qual valor não está convertendo corretamente
                    Console.WriteLine($"Valor não convertido: {txt.Text}")
                End If
            End If
        Next

        txtValorRestante.Text = FormatCurrency(total)
    End Sub
    Public Sub PTotalEntradasSaidas()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                'Consulta corrigida para buscar todos os valores
                Dim sql As String = "SELECT " &
                               "SUM(IIF(ISNULL(vlr_inicial), 0, vlr_inicial)) as vlEntrada, " &
                               "SUM(IIF(ISNULL(suprimento), 0, suprimento)) as vlSuprimento, " &
                               "SUM(IIF(ISNULL(retirada), 0, retirada)) as vlRetirada " &
                               "FROM pedido_venda " &
                               "WHERE FORMAT(data_venda, 'yyyy-MM-dd') = FORMAT(NOW(), 'yyyy-MM-dd')"

                Dim cmd As New OleDbCommand(sql, con)

                'Para debug - mostra a query que será executada
                Debug.WriteLine("Executando query: " & sql)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    'Valores padrão (caso não encontre dados)
                    Dim totalEntrada As Decimal = 0
                    Dim totalSuprimento As Decimal = 0
                    Dim totalRetirada As Decimal = 0

                    If reader.Read() Then
                        'Obtém os valores com tratamento de nulos
                        totalEntrada = If(Not IsDBNull(reader("vlEntrada")), Convert.ToDecimal(reader("vlEntrada")), 0)
                        totalSuprimento = If(Not IsDBNull(reader("vlSuprimento")), Convert.ToDecimal(reader("vlSuprimento")), 0)
                        totalRetirada = If(Not IsDBNull(reader("vlRetirada")), Convert.ToDecimal(reader("vlRetirada")), 0)
                    End If

                    'Atualiza as TextBox na thread da UI (se necessário)
                    Me.Invoke(Sub()
                                  txtAbertura.Text = totalEntrada.ToString("C2")
                                  txtSuprimento.Text = totalSuprimento.ToString("C2")
                                  txtRetirada.Text = totalRetirada.ToString("C2")

                                  'Para debug - mostra os valores obtidos
                                  Debug.WriteLine($"Totais - Entrada: {totalEntrada}, Suprimento: {totalSuprimento}, Retirada: {totalRetirada}")
                              End Sub)
                End Using

            Catch ex As Exception
                'Em caso de erro, define zeros e mostra mensagem
                Me.Invoke(Sub()
                              txtAbertura.Text = "R$ 0,00"
                              txtSuprimento.Text = "R$ 0,00"
                              txtRetirada.Text = "R$ 0,00"
                              MessageBox.Show($"Erro ao carregar totais: {ex.Message}", "Erro",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                          End Sub)
                Debug.WriteLine($"ERRO: {ex.ToString()}")
            End Try
        End Using
    End Sub
    Public Sub PTotalRecebidosTroco()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                'Consulta corrigida para buscar ambos os valores
                Dim sql As String = "SELECT SUM(total_recebido) AS T_RECEBIDO, SUM(troco) AS T_TROCO " &
                               "FROM pedido_venda WHERE data_venda =#" & Format(Date.Today, "MM/dd/yyyy") & "# "

                Dim cmd As New OleDbCommand(sql, con)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    'Total Recebido
                    If Not IsDBNull(reader("T_RECEBIDO")) Then
                        txtRecebido.Text = FormatCurrency(reader("T_RECEBIDO"))
                    Else
                        txtRecebido.Text = "R$ 0,00"
                    End If

                    'Total de Trocos
                    If Not IsDBNull(reader("T_TROCO")) Then
                        txtTroco.Text = FormatCurrency(reader("T_TROCO"))
                    Else
                        txtTroco.Text = "R$ 0,00"
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show("Erro ao calcular totais: " & ex.Message, "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Const LARGURA_CUPOM As Integer = 48 ' Caracteres por linha para 80mm
    Private Const ESPACOS_CORTE As Integer = 5 ' Linhas em branco para cortar

    Private Function SafeDBValue(dr As OleDbDataReader, campo As String) As String
        Try
            Return If(dr(campo) Is DBNull.Value, String.Empty, dr(campo).ToString())
        Catch
            Return String.Empty
        End Try
    End Function

    Private Function SafeCellValue(row As DataGridViewRow, index As Integer) As String
        Try
            Return If(row.Cells(index).Value Is Nothing, String.Empty, row.Cells(index).Value.ToString())
        Catch
            Return String.Empty
        End Try
    End Function

    Private Function Centralizar(texto As String, largura As Integer) As String
        Dim espacos = (largura - texto.Length) \ 2
        Return New String(" "c, espacos) & texto
    End Function

    ' Constantes para impressora 80mm
    Private Const TAMANHO_FONTE_NORMAL As Single = 9.0F
    Private Const TAMANHO_FONTE_PEQUENA As Single = 9.0F
    Private Const TAMANHO_FONTE_GRANDE As Single = 13
    Private Const LARGURA_CARACTERES As Integer = 80 ' Para impressora de 80mm
    Private Const MARGEM_ESQUERDA As Integer = 3

    Public Sub ImprimirFluxoCaixa()
        Dim conteudoCupom As New StringBuilder()

        ' Configurar fontes
        Dim fonteGrande As New Font("Arial", TAMANHO_FONTE_GRANDE, FontStyle.Bold)
        Dim fonteNegrito As New Font("Arial", TAMANHO_FONTE_NORMAL, FontStyle.Bold)
        Dim fonteNormal As New Font("Arial", TAMANHO_FONTE_NORMAL)
        Dim fontePequena As New Font("Arial", TAMANHO_FONTE_PEQUENA)

        Try
            ' ========== CABEÇALHO ==========
            conteudoCupom.AppendLine(Centralizar("RESUMO DA SESSÃO", LARGURA_CUPOM))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))

            conteudoCupom.AppendLine($"DATA: {DateTime.Now.ToString("dd/MM/yyyy HH:mm").PadLeft(LARGURA_CUPOM - 6)}")
            conteudoCupom.AppendLine($"OPERADOR: {(strLogim).PadLeft(LARGURA_CUPOM - 12)}")
            ' conteudoCupom.AppendLine($"TERMINAL: {(evir).PadLeft(LARGURA_CUPOM - 10)}")

            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))


            conteudoCupom.AppendLine(Centralizar("RESUMO DO CAIXA", LARGURA_CUPOM))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))
            conteudoCupom.AppendLine($"Total Geral R$:".PadRight(LARGURA_CUPOM - 11) & txtTotal.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Recebimentos R$:".PadRight(LARGURA_CUPOM - 11) & txtRecebido.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Troco R$:".PadRight(LARGURA_CUPOM - 11) & txtTroco.Text.PadLeft(11))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))
            conteudoCupom.AppendLine()

            conteudoCupom.AppendLine(Centralizar("TOTAIS EM PAGAMENTOS", LARGURA_CUPOM))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))
            conteudoCupom.AppendLine($"Dinheiro R$:".PadRight(LARGURA_CUPOM - 11) & txtDinheiro.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Debito R$:".PadRight(LARGURA_CUPOM - 11) & txtDebito.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Credito R$:".PadRight(LARGURA_CUPOM - 11) & txtCredito.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Pix R$:".PadRight(LARGURA_CUPOM - 11) & txtPIX.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Cheques R$:".PadRight(LARGURA_CUPOM - 11) & txtPIX.Text.PadLeft(11))
            conteudoCupom.AppendLine($"Outros R$:".PadRight(LARGURA_CUPOM - 11) & txtOutros.Text.PadLeft(11))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))

            conteudoCupom.AppendLine($"Diferença R$:".PadRight(LARGURA_CUPOM - 10) & txtValorRestante.Text.PadLeft(10))
            conteudoCupom.AppendLine(New String("-", LARGURA_CUPOM))
            conteudoCupom.AppendLine()
            conteudoCupom.AppendLine(Centralizar("O sistema calcula as informações com base", LARGURA_CUPOM))
            conteudoCupom.AppendLine(Centralizar("nas operações realizadas, como faturamento", LARGURA_CUPOM))
            conteudoCupom.AppendLine(Centralizar("e valores de cada forma de pagamento. ", LARGURA_CUPOM))
            conteudoCupom.AppendLine()
            conteudoCupom.AppendLine()
            conteudoCupom.AppendLine()
            conteudoCupom.AppendLine(Centralizar("__________________________________ ", LARGURA_CUPOM))
            conteudoCupom.AppendLine(Centralizar("Ass.Responssavel (Caixa). ", LARGURA_CUPOM))


            ' ========== PREPARAÇÃO PARA CORTE ==========
            For i As Integer = 1 To ESPACOS_CORTE
                conteudoCupom.AppendLine()
            Next
            conteudoCupom.AppendLine(Chr(29) & "V" & Chr(1)) ' Comando para cortar papel

            ' ========== IMPRESSÃO REMOVIDA ==========
            ' Funcionalidade de impressão foi removida - implementar nova solução

        Catch ex As Exception
            MsgBox($"ERRO: {ex.Message}", MsgBoxStyle.Critical, "Erro na Impressão")
        End Try
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        ImprimirFluxoCaixa()
        LimpaCampos()
        Me.Close()
    End Sub
    Private Sub LimpaCampos()
        txtcDinheito.Text = "0,00"
        txtcDebito.Text = "0,00"
        txtcCredito.Text = "0,00"
        txtcPix.Text = "0,00"
        txtcCheque.Text = "0,00"
        txtcOutros.Text = "0,00"

        txtrDinheiro.Text = "0,00"
        txtrDebito.Text = "0,00"
        txtrCredito.Text = "0,00"
        txtrPix.Text = "0,00"
        txtrCheque.Text = "0,00"
        txtrOutros.Text = "0,00"

        txtValorRestante.Text = "0,00"

    End Sub

    Private Sub frmContagemCaixa_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        btnImprimir.Focus()
    End Sub
End Class