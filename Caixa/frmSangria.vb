Imports System.Data
Imports System.Data.OleDb

Public Class frmSangria

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub frmSangria_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = Keys.Escape Then
            Me.Dispose()
        End If
    End Sub

    Private Sub PGravaCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "INSERT INTO pedido_venda (data_venda, retirada) VALUES (?,?)"
                Dim cmd As New OleDbCommand(sql, con)

                cmd.Parameters.Add(New OleDbParameter("data_venda", Date.Today))
                cmd.Parameters.Add(New OleDbParameter("retirada", txtValorSangria.Text))

                cmd.ExecuteNonQuery()
                MsgBox("Retirada feita no dia: " & Date.Today & " com sucesso", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub txtValorSangria_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValorSangria.KeyPress

        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))
        KeyAscii = CShort(SoNumeros(KeyAscii))
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = Keys.Enter Then
            PGravaCaixa()
            frmMovimentoCaixa.SomatorioDodia()
            Me.Dispose()
        End If
    End Sub

    Private Sub txtValorSangria_TextChanged(sender As Object, e As EventArgs) Handles txtValorSangria.TextChanged
        Utils.TextBoxMoeda(txtValorSangria)
    End Sub

    Private Sub btnEntrada_Click(sender As Object, e As EventArgs) Handles btnEntrada.Click
        PGravaCaixa()
        frmMovimentoCaixa.SomatorioDodia()
        Me.Close()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class