Imports System
Imports System.Data
Imports System.Data.OleDb
Public Class frmCaixaBalcao

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Me.Close()
    End Sub

    Private Sub frmCaixaBalcao_Load(sender As Object, e As EventArgs) Handles Me.Load
        PVerificaCaixaAberto()
    End Sub
    Private Sub PVerificaCaixaAberto()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim result As Integer
                Dim sql As String = "SELECT COUNT(*) AS Expr1 FROM tbl_vendas WHERE (data_venda = #" & Format(Date.Today, "MM/dd/yyyy") & "#)"
                Dim cmd As New OleDbCommand(sql, con)

                result = cmd.ExecuteScalar

                If result <> 0 Then
                    txtValor.Enabled = False
                    btnConfirmarCaixa.Enabled = False
                    PLeValorCaixa()
                Else
                    txtValor.Enabled = True
                    btnConfirmarCaixa.Enabled = True
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub PLeValorCaixa()
        Dim dr As OleDbDataReader = Nothing
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT VlrInicial FROM tbl_vendas WHERE (data_venda = #" & Format(Date.Today, "MM/dd/yyyy") & "#)"
                Dim cmd As New OleDbCommand(sql, con)

                dr = cmd.ExecuteReader(CommandBehavior.SingleRow)

                If dr.HasRows Then
                    dr.Read()
                    txtValor.Text = FormatCurrency(dr.Item("VlrInicial"))
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Private Sub PGravaCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "INSERT INTO tbl_vendas (data_venda,VlrInicial) VALUES (?,?)"
                Dim cmd As New OleDbCommand(sql, con)

                cmd.Parameters.Add(New OleDbParameter("data_venda", Date.Today))
                cmd.Parameters.Add(New OleDbParameter("VlrInicial", txtValor.Text))

                cmd.ExecuteNonQuery()
                MsgBox("Caixa definido para o dia: " & Date.Today & " com sucesso", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub


    Private Sub btnConfirmarCaixa_Click_1(sender As Object, e As EventArgs) Handles btnConfirmarCaixa.Click
        If txtValor.Text = "" Then
            MsgBox("Por favor, informe o valor incial do caixa.", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Exit Sub
        End If

        PGravaCaixa()
        Me.Dispose()
        frmVendaMarcada.ShowDialog()
    End Sub

    Private Sub txtValor_GotFocus(sender As Object, e As EventArgs) Handles txtValor.GotFocus
        txtValor.Text = txtValor.Text.Replace("R$ 0,00", "")
    End Sub

    Private Sub txtValor_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValor.KeyPress
        If Char.IsLetter(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtValor_LostFocus(sender As Object, e As EventArgs) Handles txtValor.LostFocus
        txtValor.Text = FormatCurrency(txtValor.Text)
    End Sub


    Private Sub txtValor_TextChanged_1(sender As Object, e As EventArgs) Handles txtValor.TextChanged
        Utils.TextBoxMoeda(txtValor)
    End Sub
End Class