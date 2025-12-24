<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCaixa
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCaixa))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtValor = New System.Windows.Forms.TextBox()
        Me.btnConfirmarCaixa = New System.Windows.Forms.Button()
        Me.txtConsultarProduto = New ns1.BunifuMetroTextbox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtValor)
        Me.Panel1.Controls.Add(Me.btnConfirmarCaixa)
        Me.Panel1.Controls.Add(Me.txtConsultarProduto)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(657, 470)
        Me.Panel1.TabIndex = 4
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(270, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(109, 95)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 85
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Menu
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Gray
        Me.Label5.Location = New System.Drawing.Point(255, 296)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 15)
        Me.Label5.TabIndex = 84
        Me.Label5.Text = "DISPONIVEM EM CAIXA"
        '
        'txtValor
        '
        Me.txtValor.BackColor = System.Drawing.SystemColors.Menu
        Me.txtValor.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtValor.Font = New System.Drawing.Font("Segoe UI", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValor.ForeColor = System.Drawing.Color.DimGray
        Me.txtValor.Location = New System.Drawing.Point(150, 318)
        Me.txtValor.Multiline = True
        Me.txtValor.Name = "txtValor"
        Me.txtValor.Size = New System.Drawing.Size(361, 40)
        Me.txtValor.TabIndex = 2
        Me.txtValor.Text = "R$ 0,00"
        Me.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnConfirmarCaixa
        '
        Me.btnConfirmarCaixa.BackColor = System.Drawing.Color.LimeGreen
        Me.btnConfirmarCaixa.FlatAppearance.BorderColor = System.Drawing.Color.LawnGreen
        Me.btnConfirmarCaixa.FlatAppearance.BorderSize = 0
        Me.btnConfirmarCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfirmarCaixa.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConfirmarCaixa.ForeColor = System.Drawing.Color.White
        Me.btnConfirmarCaixa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConfirmarCaixa.Location = New System.Drawing.Point(121, 361)
        Me.btnConfirmarCaixa.Name = "btnConfirmarCaixa"
        Me.btnConfirmarCaixa.Size = New System.Drawing.Size(421, 51)
        Me.btnConfirmarCaixa.TabIndex = 3
        Me.btnConfirmarCaixa.Text = "ABRIR CAIXA"
        Me.btnConfirmarCaixa.UseVisualStyleBackColor = False
        '
        'txtConsultarProduto
        '
        Me.txtConsultarProduto.BackColor = System.Drawing.SystemColors.Menu
        Me.txtConsultarProduto.BorderColorFocused = System.Drawing.Color.LimeGreen
        Me.txtConsultarProduto.BorderColorIdle = System.Drawing.SystemColors.InactiveBorder
        Me.txtConsultarProduto.BorderColorMouseHover = System.Drawing.Color.LimeGreen
        Me.txtConsultarProduto.BorderThickness = 1
        Me.txtConsultarProduto.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtConsultarProduto.Enabled = False
        Me.txtConsultarProduto.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConsultarProduto.ForeColor = System.Drawing.Color.Black
        Me.txtConsultarProduto.isPassword = False
        Me.txtConsultarProduto.Location = New System.Drawing.Point(121, 286)
        Me.txtConsultarProduto.Margin = New System.Windows.Forms.Padding(4)
        Me.txtConsultarProduto.Name = "txtConsultarProduto"
        Me.txtConsultarProduto.Size = New System.Drawing.Size(421, 77)
        Me.txtConsultarProduto.TabIndex = 83
        Me.txtConsultarProduto.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Gray
        Me.Label3.Location = New System.Drawing.Point(118, 220)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(424, 16)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "INFORME OS VALORES DISPONIVEIS PARA INICIAR A ABERTURA"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Silver
        Me.Label4.Location = New System.Drawing.Point(624, 1)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(25, 30)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "X"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(6, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 15)
        Me.Label2.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.LimeGreen
        Me.Label1.Location = New System.Drawing.Point(113, 168)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(437, 46)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "ABERTURA DE CAIXA"
        '
        'frmCaixa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(657, 470)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmCaixa"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Caixa"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents MetroLabel7 As Global.MetroFramework.Controls.MetroLabel
    Friend WithEvents btnConfirmarCaixa As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtValor As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtConsultarProduto As ns1.BunifuMetroTextbox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
