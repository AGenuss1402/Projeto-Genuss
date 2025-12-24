Public Class clsFunctions

    Dim titulo As String = "Sistema de Vendas e Controle de Estoque [ 1.0.0.0 ]"
    Dim intPergunta As Integer

    Private Sub SetMessage(ByVal mensagem As String, ByVal titulo As String, ByVal tipo As Integer)

        Select Case tipo
            Case 1
                MsgBox(mensagem, MsgBoxStyle.Information, titulo)
            Case 2
                MsgBox(mensagem, MsgBoxStyle.Exclamation, titulo)
            Case 3
                MsgBox(mensagem, MsgBoxStyle.Critical, titulo)
            Case 4
                intPergunta = MsgBox(mensagem, MsgBoxStyle.Question + MsgBoxStyle.YesNo, titulo)
        End Select


    End Sub

End Class
