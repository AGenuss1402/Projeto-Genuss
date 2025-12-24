<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLembreteMenssalidade
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLembreteMenssalidade))
        Me.main_pan_colour = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lbl_title = New System.Windows.Forms.Label()
        Me.lbl_text = New System.Windows.Forms.Label()
        Me.painelEsquerdo = New System.Windows.Forms.Panel()
        Me.tmAparecer = New System.Windows.Forms.Timer(Me.components)
        Me.tmFechar = New System.Windows.Forms.Timer(Me.components)
        Me.tmEspera = New System.Windows.Forms.Timer(Me.components)
        Me.main_pan_colour.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'main_pan_colour
        '
        Me.main_pan_colour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.main_pan_colour.Controls.Add(Me.PictureBox1)
        Me.main_pan_colour.Controls.Add(Me.lbl_title)
        Me.main_pan_colour.Controls.Add(Me.lbl_text)
        Me.main_pan_colour.Controls.Add(Me.painelEsquerdo)
        Me.main_pan_colour.Dock = System.Windows.Forms.DockStyle.Fill
        Me.main_pan_colour.Location = New System.Drawing.Point(0, 0)
        Me.main_pan_colour.Name = "main_pan_colour"
        Me.main_pan_colour.Size = New System.Drawing.Size(371, 114)
        Me.main_pan_colour.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(340, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(26, 26)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'lbl_title
        '
        Me.lbl_title.BackColor = System.Drawing.Color.Transparent
        Me.lbl_title.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_title.Location = New System.Drawing.Point(17, 24)
        Me.lbl_title.Name = "lbl_title"
        Me.lbl_title.Size = New System.Drawing.Size(282, 23)
        Me.lbl_title.TabIndex = 4
        Me.lbl_title.Text = "MsgPrincipal"
        '
        'lbl_text
        '
        Me.lbl_text.BackColor = System.Drawing.Color.Transparent
        Me.lbl_text.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_text.Location = New System.Drawing.Point(17, 47)
        Me.lbl_text.Name = "lbl_text"
        Me.lbl_text.Size = New System.Drawing.Size(341, 57)
        Me.lbl_text.TabIndex = 3
        Me.lbl_text.Text = "Complementares"
        '
        'painelEsquerdo
        '
        Me.painelEsquerdo.BackColor = System.Drawing.Color.SlateGray
        Me.painelEsquerdo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.painelEsquerdo.Dock = System.Windows.Forms.DockStyle.Left
        Me.painelEsquerdo.Location = New System.Drawing.Point(0, 0)
        Me.painelEsquerdo.Name = "painelEsquerdo"
        Me.painelEsquerdo.Size = New System.Drawing.Size(11, 112)
        Me.painelEsquerdo.TabIndex = 1
        '
        'tmAparecer
        '
        Me.tmAparecer.Interval = 8
        '
        'tmFechar
        '
        Me.tmFechar.Interval = 5
        '
        'tmEspera
        '
        Me.tmEspera.Interval = 10000
        '
        'frmLembreteMenssalidade
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(371, 114)
        Me.Controls.Add(Me.main_pan_colour)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmLembreteMenssalidade"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmLembreteMenssalidade"
        Me.TopMost = True
        Me.main_pan_colour.ResumeLayout(False)
        Me.main_pan_colour.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents main_pan_colour As System.Windows.Forms.Panel
    Friend WithEvents tmAparecer As System.Windows.Forms.Timer
    Friend WithEvents tmFechar As System.Windows.Forms.Timer
    Friend WithEvents tmEspera As System.Windows.Forms.Timer
    Friend WithEvents painelEsquerdo As System.Windows.Forms.Panel
    Friend WithEvents lbl_text As System.Windows.Forms.Label
    Friend WithEvents lbl_title As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
