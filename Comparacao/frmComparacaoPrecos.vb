Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class frmComparacaoPrecos

    Dim classComparacaoPrecos As New clsComparacaoPrecos

    Private Sub PFormataGrid()
        With dgvComparacaoPrecos
            .Columns(0).HeaderText = "ID Forcedor."
            .Columns(1).HeaderText = "Fornecedor"
            .Columns(2).HeaderText = "Cidade"
            .Columns(3).HeaderText = "UF"
            .Columns(4).HeaderText = "Cód. Barras"
            .Columns(5).HeaderText = "Produto"
            .Columns(6).HeaderText = "Vlr. Unitario."
            .Columns(7).HeaderText = "% Venda"
            .Columns(8).HeaderText = "Vlr. Venda"
            .Columns(9).HeaderText = "Dt. Compra"

            .Columns(6).DefaultCellStyle.Format = "c"
            .Columns(7).DefaultCellStyle.Format = FormatPercent("00")
            .Columns(8).DefaultCellStyle.Format = "c"

            .Columns(6).DefaultCellStyle.ForeColor = Color.Red
            .Columns(7).DefaultCellStyle.ForeColor = Color.Green
            .Columns(8).DefaultCellStyle.ForeColor = Color.Blue
            .Columns(6).DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            .Columns(7).DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            .Columns(8).DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

            .Columns(9).DefaultCellStyle.Format = "d"
            .Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End With
    End Sub

    Private Sub frmComparacaoPrecos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' dgvComparacaoPrecos.DataSource = classComparacaoPrecos.FCarregaPrecos
        ' PFormataGrid()
    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs)
        Me.Dispose()
    End Sub

    Private Sub PMedia(ByVal id_fornecedor As Integer)
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim _media As Double
                Dim sql As String = "Select AVG(valor_unitario) from tbl_estoque WHERE id_fornecedor = " & id_fornecedor
                Dim cmd As New OleDbCommand(sql, con)

                _media = cmd.ExecuteScalar
                txtMedia.Text = FormatCurrency(_media)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar obter a média dos produtos. Mensagem original: " &
                       ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub dgvComparacaoPrecos_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvComparacaoPrecos.CellEnter
        PMedia(CInt(dgvComparacaoPrecos.CurrentRow().Cells("id_fornecedor").Value))
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        Me.Close()
    End Sub

    Private Sub txtProduto_OnValueChanged(sender As Object, e As EventArgs) Handles txtProduto.OnValueChanged
        If cmbTipoConsulta.SelectedIndex = -1 Then
            MsgBox("Por favor, selecione um tipo de consulta.", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            cmbTipoConsulta.DroppedDown = True
            Exit Sub
        End If
        If cmbTipoConsulta.SelectedIndex = 0 Then
            dgvComparacaoPrecos.DataSource = classComparacaoPrecos.FConsulta(txtProduto.Text, String.Empty, String.Empty)
        ElseIf cmbTipoConsulta.SelectedIndex = 1 Then
            dgvComparacaoPrecos.DataSource = classComparacaoPrecos.FConsulta(String.Empty, txtProduto.Text, String.Empty)
        ElseIf cmbTipoConsulta.SelectedIndex = 2 Then
            dgvComparacaoPrecos.DataSource = classComparacaoPrecos.FConsulta(String.Empty, String.Empty, txtProduto.Text)
        End If

        PFormataGrid()
    End Sub

    Private Sub btnVendas_Click(sender As Object, e As EventArgs) Handles btnVendas.Click
        frmMetroPrincipal.AbrirFormEnPanel(New frmComparacaoPrecosAgrupadas())
    End Sub
    Private Sub CalcularPorcentagem()
        Try
            For i As Integer = 0 To dgvComparacaoPrecos.RowCount - 1

                Dim VlrVenda As Double = dgvComparacaoPrecos.Rows(i).Cells(8).Value
                Dim VlrCompra As Double = dgvComparacaoPrecos.Rows(i).Cells(6).Value

                Dim valorPorc As Double = Val((100 / VlrVenda) + VlrCompra)
                dgvComparacaoPrecos.Rows(i).Cells(7).Value = FormatPercent(valorPorc)
            Next
        Catch ex As Exception

        End Try
    End Sub
End Class