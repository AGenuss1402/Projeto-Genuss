Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing.Text
Public Class frmFechamnetoCaixa
    Dim classVendas As New clsVendas

    Dim _SomarSaldoDodia As Double
    Dim _SomarValorDinheiro As Double
    Dim _SomarVendasCartaoCredito As Double
    Dim _SOmarVendasCartaoDebito As Double
    Dim _SomarVendasPrazo As Double
    Dim _SomarSuprimentoDodia As Double
    Dim _SomarValorRetiradas As Double
    Dim _SomarValorVendasPix As Double
    Dim _SomarVendaNoCehque As Double

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub PTotalVendasCheque()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Cheque) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarVendaNoCehque = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarVendaNoCehque) Then
                    txtVendaCheque.Text = FormatCurrency(_SomarVendaNoCehque)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalVendasPix()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Pix) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarValorVendasPix = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarValorVendasPix) Then
                    txtVlrPix.Text = FormatCurrency(_SomarValorVendasPix)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalRetiradasdoDia()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Saida) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarValorRetiradas = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarValorRetiradas) Then
                    txtSangria.Text = FormatCurrency(_SomarValorRetiradas)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub


    Public Sub PTotalSuprimento()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Entrada) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarSuprimentoDodia = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarSuprimentoDodia) Then
                    txtSuprimento.Text = FormatCurrency(_SomarSuprimentoDodia)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalVendasPrazo()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Prazo) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarVendasPrazo = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarVendasPrazo) Then
                    txtVendaPrazo.Text = FormatCurrency(_SomarVendasPrazo)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub


    Public Sub PTotalVendasCartaoDebito()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(CartaoDebito) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SOmarVendasCartaoDebito = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SOmarVendasCartaoDebito) Then
                    txtCrtaoDebito.Text = FormatCurrency(_SOmarVendasCartaoDebito)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalVendasRealizadasCartaoCredito()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(CartaoCredito) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarVendasCartaoCredito = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarVendasCartaoCredito) Then
                    txtCartaoCredito.Text = FormatCurrency(_SomarVendasCartaoCredito)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalVendasRealizadasDinheiro()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(Dinheiro) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarValorDinheiro = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarValorDinheiro) Then
                    txtVlrDinheiro.Text = FormatCurrency(_SomarValorDinheiro)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Public Sub PTotalVendasRealizadasNoDia()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim sql_sum As String = "SELECT SUM(VlrInicial) FROM tbl_vendas WHERE DAY(data_venda) = DAY(NOW())"
                Dim cmd_sum As New OleDbCommand(sql_sum, con)
                _SomarSaldoDodia = cmd_sum.ExecuteScalar

                If Not IsDBNull(_SomarSaldoDodia) Then
                    txtSaldoInicial.Text = FormatCurrency(_SomarSaldoDodia)
                End If
            Catch ex As Exception
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub CarregarSimatoriodoDia()
        PTotalVendasRealizadasNoDia()
        PTotalVendasRealizadasDinheiro()
        PTotalVendasRealizadasCartaoCredito()
        PTotalVendasCartaoDebito()
        PTotalVendasPrazo()
        PTotalSuprimento()
        PTotalRetiradasdoDia()
        PTotalVendasPix()
        PTotalVendasCheque()

        txtSomar1.Text = FormatCurrency(_SomarSaldoDodia + _SomarValorDinheiro + _SomarSuprimentoDodia - _SomarValorRetiradas)
        txtSomar2.Text = FormatCurrency(_SomarVendasCartaoCredito + _SOmarVendasCartaoDebito + _SomarValorVendasPix + _SomarVendasPrazo + _SomarVendaNoCehque)
        Dim VlrSoma1, VlrSoma2 As Double
        VlrSoma1 = txtSomar1.Text
        VlrSoma2 = txtSomar2.Text
        txtTotalGeral.Text = FormatCurrency(VlrSoma1 + VlrSoma2)
        dgvMovimento.DataSource = classVendas.CarregaFluxoCaixaEntreDatas(txtDtIncio.Text, txtDataFim.Text)
        FormatGridMovimento()
        SomarColunasFlux()
        SomarFluxoEntreDatas()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Me.Close()
    End Sub

    Private Sub frmFechamnetoCaixa_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtDtIncio.Text = Date.Today
        txtDataFim.Text = Date.Today
        txtUsuario.Text = strLogim
        CarregarSimatoriodoDia()

    End Sub

    Private Sub FormatGridMovimento()

        With Me.dgvMovimento

            .Columns("data_venda").HeaderText = "Data"
            .Columns("Cod_Venda").HeaderText = "ID."
            .Columns("VlrInicial").HeaderText = "Vlr. Inicial"
            .Columns("Dinheiro").HeaderText = "Vlr. Dinheiro"
            .Columns("CartaoDebito").HeaderText = "Vlr. Debito"
            .Columns("CartaoCredito").HeaderText = "Vlr. Credito"
            .Columns("Pix").HeaderText = "Vlr. Pix"
            .Columns("Prazo").HeaderText = "Vlr. Prazo"
            .Columns("Entrada").HeaderText = "Vlr. Entradas"
            .Columns("Saida").HeaderText = "Vlr. Saidas"

            .Columns("data_venda").DefaultCellStyle.Format = ("d")
            .Columns("VlrInicial").DefaultCellStyle.Format = ("c")
            .Columns("Dinheiro").DefaultCellStyle.Format = ("c")
            .Columns("CartaoDebito").DefaultCellStyle.Format = ("c")
            .Columns("CartaoCredito").DefaultCellStyle.Format = ("c")
            .Columns("Pix").DefaultCellStyle.Format = ("c")
            .Columns("Prazo").DefaultCellStyle.Format = ("c")
            .Columns("Entrada").DefaultCellStyle.Format = ("c")
            .Columns("Saida").DefaultCellStyle.Format = ("c")

            .Columns("data_venda").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Cod_Venda").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("VlrInicial").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Dinheiro").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("CartaoDebito").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("CartaoCredito").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Pix").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Prazo").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Entrada").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns("Saida").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End With
    End Sub

    Private Sub btnConsulta_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        dgvMovimento.DataSource = classVendas.CarregaFluxoCaixaEntreDatas(txtDtIncio.Text, txtDataFim.Text)
        SomarColunasFlux()
        SomarFluxoEntreDatas()
    End Sub
    Private Sub SomarColunasFlux()
        SomarDinheiro()
        SomarCredito()
        SomarDebito()
        SomarPix()
        SomarPrazo()
        SomarCheque()
    End Sub
    Public Sub SomarDinheiro()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("Dinheiro").Value
        Next
        txtSomarDinheiro.Text = FormatCurrency(valor)
    End Sub
    Public Sub SomarCredito()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("CartaoCredito").Value
        Next
        txtSomarCartaoCredito.Text = FormatCurrency(valor)
    End Sub
    Public Sub SomarDebito()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("CartaoDebito").Value
        Next
        txtSomarCartaoDebito.Text = FormatCurrency(valor)
    End Sub
    Public Sub SomarPix()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("Pix").Value
        Next
        txtSomarPix.Text = FormatCurrency(valor)
    End Sub
    Public Sub SomarPrazo()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("Prazo").Value
        Next
        txtSomarPrazo.Text = FormatCurrency(valor)
    End Sub
    Public Sub SomarCheque()
        Dim valor As Decimal
        For Each col As DataGridViewRow In dgvMovimento.Rows
            valor = valor + col.Cells("Cheque").Value
        Next
        txtSomarCheque.Text = FormatCurrency(valor)
    End Sub
    Private Sub SomarFluxoEntreDatas()
        Dim dinheiro, credito, debito, pix, prazo, cheque As Double
        dinheiro = txtSomarDinheiro.Text
        credito = txtSomarCartaoCredito.Text
        debito = txtSomarCartaoDebito.Text
        pix = txtSomarPix.Text
        prazo = txtSomarPrazo.Text
        cheque = txtSomarCheque.Text
        txtVlrGeral.Text = FormatCurrency(dinheiro + credito + debito + pix + prazo + cheque)
    End Sub

    Private Sub imprimir()
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub
End Class