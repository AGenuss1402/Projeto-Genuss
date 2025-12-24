Imports System.Data
Imports System.Data.OleDb

Public Class frmSuprimmento

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub frmSuprimmento_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If Asc(e.KeyChar) = Keys.Escape Then
            Me.Dispose()
        End If
    End Sub

    Private Sub PGravaCaixa()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "INSERT INTO pedido_venda (data_venda, suprimento) VALUES (?,?)"
                Dim cmd As New OleDbCommand(sql, con)

                cmd.Parameters.Add(New OleDbParameter("data_inicio", Date.Today))
                cmd.Parameters.Add(New OleDbParameter("suprimento", txtValorEntrada.Text))

                cmd.ExecuteNonQuery()
                MsgBox("Suprimento para o dia: " & Date.Today & " foi efetuado com sucesso", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnAdiciona_Click(sender As Object, e As EventArgs)
        PGravaCaixa()
        frmMovimentoCaixa.SomatorioDodia()
        Me.Dispose()
    End Sub

    Private Sub txtValorEntrada_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValorEntrada.KeyPress

        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))
        KeyAscii = CShort(SoNumeros(KeyAscii))
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        If Asc(e.KeyChar) = Keys.Enter Then
            PGravaCaixa()
            frmMovimentoCaixa.SomatorioDodia()
            Me.Close()
        End If

    End Sub

    Private Sub txtValorEntrada_TextChanged(sender As Object, e As EventArgs) Handles txtValorEntrada.TextChanged
        Utils.TextBoxMoeda(txtValorEntrada)
    End Sub

    Private Sub btnEntrada_Click(sender As Object, e As EventArgs) Handles btnSangria.Click
        PGravaCaixa()
        frmMovimentoCaixa.SomatorioDodia()
        Me.Dispose()
    End Sub
End Class