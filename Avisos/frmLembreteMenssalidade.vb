Public Class frmLembreteMenssalidade
    Dim message_que As New List(Of String)
    Private Sub size_vaildation()
        Me.Location = New Point((My.Computer.Screen.Bounds.Width / 2) - 200, -155)
        tmAparecer.Enabled = True
        tmAparecer.Start()
    End Sub

    Private Sub tmAparecer_Tick(sender As Object, e As EventArgs) Handles tmAparecer.Tick
        If Me.Location.Y = 0 Then
            tmAparecer.Stop()
            tmEspera.Enabled = True
            tmEspera.Start()
        Else
            Me.Location = New Point((My.Computer.Screen.Bounds.Width / 2) - 200, Me.Location.Y + 1)
        End If
    End Sub

    Private Sub tmFechar_Tick(sender As Object, e As EventArgs) Handles tmFechar.Tick
        If Me.Location.Y = -155 Then
            If intAviso = 4 Then
                Me.Hide()
            Else
                Me.Dispose()
            End If
        Else
            Me.Location = New Point((My.Computer.Screen.Bounds.Width / 2) - 200, Me.Location.Y - 1)
        End If
    End Sub

    Private Sub tmEspera_Tick(sender As Object, e As EventArgs) Handles tmEspera.Tick
        tmEspera.Stop()
        tmFechar.Enabled = True
        tmFechar.Start()
    End Sub

    Private Sub frmLembreteMenssalidade_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Location = New Point((My.Computer.Screen.Bounds.Width / 2) - 200, -155)
        Me.ShowInTaskbar = False

        If intAviso = 1 Then
            Show_msg("Notificação - Atualização de Senha", _
                     "Novas atualizações do sistema já estão disponivéis para download. " & _
                     "Aconselhamos que realize o processo de atualização o mais rápido possível.", MessageType.Information)
        ElseIf intAviso = 2 Then
            Show_msg("Mensagem - Bloqueio do Sistema", "Não deixe que o sistema seja bloqueado peça sua senha o quanto antes para evitar transtornos.", _
                     MessageType.Warning)
        ElseIf intAviso = 3 Then
            Show_msg("Mensagem - Bloqueio do Sistema", "Não deixe que o sistema seja bloqueado peça sua senha o quanto antes para evitar transtornos.", _
                     MessageType.YellowTheme)
        ElseIf intAviso = 4 Then
            Show_msg("Sistema Bloqueado", "Para desbloquear o sistema favor entrar em contato com o administrador do sistema.", _
                     MessageType.RedAlert)
        End If
    End Sub

    Public Enum MessageType
        Warning
        Information
        Notice
        RedAlert
        YellowTheme
    End Enum

    Public Sub Show_msg(ByVal title As String, ByVal text As String, ByVal typeM As MessageType)

        size_vaildation()

        Select Case (typeM)
            Case MessageType.Warning
                painelEsquerdo.BackColor = Color.DarkOrange
                main_pan_colour.BackColor = Color.Bisque
                lbl_text.ForeColor = Color.Black
                lbl_title.ForeColor = Color.Black
            Case MessageType.Information
                painelEsquerdo.BackColor = Color.YellowGreen
                main_pan_colour.BackColor = Color.Ivory
                lbl_text.ForeColor = Color.Black
                lbl_title.ForeColor = Color.Black
            Case MessageType.Notice
                painelEsquerdo.BackColor = Color.SkyBlue
                main_pan_colour.BackColor = Color.Azure
                lbl_text.ForeColor = Color.Black
                lbl_title.ForeColor = Color.Black
            Case MessageType.RedAlert
                painelEsquerdo.BackColor = Color.Black
                main_pan_colour.BackColor = Color.Red
                lbl_text.ForeColor = Color.Black
                lbl_title.ForeColor = Color.Black
            Case MessageType.YellowTheme
                painelEsquerdo.BackColor = Color.Yellow
                main_pan_colour.BackColor = Color.LightYellow
                lbl_text.ForeColor = Color.Black
                lbl_title.ForeColor = Color.Black
        End Select

        lbl_title.Text = title
        lbl_text.Text = text
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Me.Location = New Point((My.Computer.Screen.Bounds.Width / 2) - 200, -155)
        Me.Dispose()
    End Sub
End Class