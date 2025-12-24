Imports System.Data
Imports System.Data.OleDb

Public Class frmMovimentoCaixa

    Dim _valorVendasRealizadasNoDia As Double
    Dim _totalGastosRealizadosNoDia As Double
    Dim _totalValesRealizadosNoDia As Double
    Dim _totalComprasMarcadasNoDia As Double
    Dim _totalComprasPagasNoDia As Double
    Dim _TotalInicioCaixa As Double
    Dim _ValorEntradasCaixa As Double
    Dim _TotalSaidaCaixa As Double
    Dim _TotalDinheiro As Double
    Dim _TotalCartDebito As Double
    Dim _TotCartCredito As Double
    Dim _TotCheque As Double
    Dim _TotPrazo As Double
    Dim _TotPix As Double

    Private Sub CarregarMovimento()
        ' Abertura
        lblDataAbertura.Text = DateTime.Now.ToShortDateString
        lblHoraAbertura.Text = DateTime.Now.ToShortTimeString

        'Entrada
        lblDataEntrada.Text = DateTime.Now.ToShortDateString
        lblHoraEntrada.Text = DateTime.Now.ToShortTimeString

        'Sangria
        lblDataSangria.Text = DateTime.Now.ToShortDateString
        lblHoraSangria.Text = DateTime.Now.ToShortTimeString

        'Receita
        lblDataReceita.Text = DateTime.Now.ToShortDateString
        lblHoraReceita.Text = DateTime.Now.ToShortTimeString

    End Sub

    Public Sub PTotalVendasRealizadas()
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim valor As Double
                Dim sql_sum As String = "SELECT SUM(valor_unitario)  FROM pedido_venda_itens WHERE (Data  =#" & Format(Date.Today, "MM/dd/yyyy") & "#)"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                valor = cmd_sum.ExecuteScalar
                lblValorReceira.Text = FormatCurrency(valor)
            Catch ex As Exception

            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub frmMovimentoCaixa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        frmPDV_POS.txtCodigoBarrasVenda.Focus()
    End Sub

    Private Sub frmMovimentoCaixa_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        ' Abre a tela de sangria
        If e.KeyCode = Keys.S Then
            frmSangria.ShowDialog()
        End If

        ' Abre a tela de Suprimento
        If e.KeyCode = Keys.E Then
            frmSuprimmento.ShowDialog()
        End If

        ' Abre a tela de detalhes
        If e.KeyCode = Keys.F Then
            frmFechamnetoCaixa.ShowDialog()
        End If

    End Sub

    Private Sub lblSair_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Public Sub SomatorioDodia()
        PTotalGastosRealizadasNoDia()
        PTotalVendasRealizadas()

        PTotalInicialCaixa()
        PTotalEntradasCaica()
        PTotalSaidaCaixa()
    End Sub

    Public Sub PTotalGastosRealizadasNoDia()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql As String = "SELECT SUM(valor) FROM tbl_gastos WHERE DAY(data) = DAY(NOW())"
                Dim cmd As New OleDbCommand(sql, con)
                _totalGastosRealizadosNoDia = cmd.ExecuteScalar

                If Not IsDBNull(_totalGastosRealizadosNoDia) Then
                    lblDebito.Text = FormatCurrency(_totalGastosRealizadosNoDia)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Public Sub PTotalValesRealizadasNoDia()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql As String = "SELECT SUM(vale) FROM tbl_salario_funcionarios WHERE DAY(data) = DAY(NOW())"
                Dim cmd As New OleDbCommand(sql, con)
                _totalValesRealizadosNoDia = cmd.ExecuteScalar

                If Not IsDBNull(_totalValesRealizadosNoDia) Then
                    lblCredito.Text = FormatCurrency(_totalValesRealizadosNoDia)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalInicialCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql As String = "SELECT SUM(VlrInicial) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd As New OleDbCommand(sql, con)
                _TotalInicioCaixa = cmd.ExecuteScalar

                If Not IsDBNull(_TotalInicioCaixa) Then
                    lblValorAbertura.Text = FormatCurrency(_TotalInicioCaixa)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Public Sub PTotalEntradasCaica()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql As String = "SELECT SUM(Entrada) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd As New OleDbCommand(sql, con)
                _ValorEntradasCaixa = cmd.ExecuteScalar

                If Not IsDBNull(_ValorEntradasCaixa) Then
                    lblEntradas.Text = FormatCurrency(_ValorEntradasCaixa)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Public Sub PTotalSaidaCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql As String = "SELECT SUM(Saida) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd As New OleDbCommand(sql, con)
                _TotalSaidaCaixa = cmd.ExecuteScalar

                If Not IsDBNull(_TotalSaidaCaixa) Then
                    lblValorSangria.Text = FormatCurrency(_TotalSaidaCaixa)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Public Sub CalcularTotaisPorFormaPagamento()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                'Consulta modificada para garantir que está buscando os dados corretos
                Dim sql As String = "SELECT tipo_pagamento, SUM(valor) as total " &
                               "FROM pedido_venda_condicaopagamento " &
                               "WHERE data =#" & Format(Date.Today, "MM/dd/yyyy") & "# " &
                               "GROUP BY tipo_pagamento"

                Dim cmd As New OleDbCommand(sql, con)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                'Zerar todas as labels
                lblDinheiro.Text = "R$ 0,00"
                lblDebito.Text = "R$ 0,00"
                lblCredito.Text = "R$ 0,00"
                lblCheque.Text = "R$ 0,00"
                lblPix.Text = "R$ 0,00"
                lblTotalGeral.Text = "R$ 0,00"

                'Variável para debug (opcional)
                Dim debugInfo As String = ""
                Dim totalGeral As Decimal = 0
                'Percorre todos os registros
                While reader.Read()
                    If Not IsDBNull(reader("total")) Then
                        Dim tipoPagamento As String = reader("tipo_pagamento").ToString().Trim().ToUpper()
                        Dim valor As Decimal = Convert.ToDecimal(reader("total"))

                        'Adiciona informações para debug
                        debugInfo += $"Tipo: {tipoPagamento}, Valor: {valor}" & vbCrLf

                        'Acumula no total geral
                        totalGeral += valor

                        Select Case tipoPagamento
                            Case "DINHEIRO"
                                lblDinheiro.Text = FormatCurrency(valor)
                            Case "DÉBITO", "DEBITO"
                                lblDebito.Text = FormatCurrency(valor)
                            Case "CRÉDITO", "CREDITO"
                                lblCredito.Text = FormatCurrency(valor)
                            Case "CHEQUE"
                                lblCheque.Text = FormatCurrency(valor)
                            Case "PIX"
                                lblPix.Text = FormatCurrency(valor)
                        End Select
                    End If
                End While

                'Exibe o total geral
                lblTotalGeral.Text = FormatCurrency(totalGeral)

            Catch ex As Exception
                MessageBox.Show("Erro ao calcular totais: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub Panel10_Click(sender As Object, e As EventArgs) Handles Panel10.Click
        frmSangria.ShowDialog()
    End Sub

    Private Sub Panel11_Click(sender As Object, e As EventArgs) Handles Panel11.Click
        frmSuprimmento.ShowDialog()
    End Sub
    Private Sub frmMovimentoCaixa_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = Keys.Escape Then
            frmPDV_POS.txtCodigoBarrasVenda.Focus()
            Me.Dispose()
        End If
    End Sub
    Private Sub frmMovimentoCaixa_Load(sender As Object, e As EventArgs) Handles Me.Load
        CalcularTotaisPorFormaPagamento()
        CarregarMovimento()
        SomatorioDodia()
    End Sub
    Private Sub Panel9_Click(sender As Object, e As EventArgs) Handles Panel9.Click
        frmPDV_POS.txtCodigoBarrasVenda.Focus()
        Me.Dispose()
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        frmPDV_POS.txtCodigoBarrasVenda.Focus()
        Me.Dispose()
    End Sub
    Private Sub Panel14_Click(sender As Object, e As EventArgs) Handles Panel14.Click
        frmContagemCaixa.ShowDialog()
    End Sub
    Private Sub PictureBox7_Click(sender As Object, e As EventArgs) Handles PictureBox7.Click
        ' frmContagemCaixa.Show()
    End Sub

    Private Sub Panel13_Click(sender As Object, e As EventArgs) Handles Panel13.Click
        frmFechamnetoCaixa.ShowDialog()
    End Sub
    Private Sub Panel12_Paint(sender As Object, e As PaintEventArgs)
        'frmContadorMoedas.ShowDialog()
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)
        'frmContadorMoedas.ShowDialog()
    End Sub

    Private Sub pSair_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class