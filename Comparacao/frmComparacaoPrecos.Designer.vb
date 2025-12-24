<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmComparacaoPrecos
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmComparacaoPrecos))
        Me.dgvComparacaoPrecos = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbTipoConsulta = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtProduto = New ns1.BunifuMetroTextbox()
        Me.txtMedia = New ns1.BunifuMetroTextbox()
        Me.lupaClente = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BunifuSeparator1 = New ns1.BunifuSeparator()
        Me.btnVendas = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.dgvComparacaoPrecos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.lupaClente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvComparacaoPrecos
        '
        Me.dgvComparacaoPrecos.AllowUserToAddRows = False
        Me.dgvComparacaoPrecos.AllowUserToDeleteRows = False
        Me.dgvComparacaoPrecos.AllowUserToResizeColumns = False
        Me.dgvComparacaoPrecos.AllowUserToResizeRows = False
        Me.dgvComparacaoPrecos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvComparacaoPrecos.BackgroundColor = System.Drawing.Color.White
        Me.dgvComparacaoPrecos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvComparacaoPrecos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvComparacaoPrecos.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvComparacaoPrecos.ColumnHeadersHeight = 32
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvComparacaoPrecos.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvComparacaoPrecos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvComparacaoPrecos.EnableHeadersVisualStyles = False
        Me.dgvComparacaoPrecos.GridColor = System.Drawing.SystemColors.Control
        Me.dgvComparacaoPrecos.Location = New System.Drawing.Point(3, 19)
        Me.dgvComparacaoPrecos.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dgvComparacaoPrecos.Name = "dgvComparacaoPrecos"
        Me.dgvComparacaoPrecos.ReadOnly = True
        Me.dgvComparacaoPrecos.RowHeadersVisible = False
        Me.dgvComparacaoPrecos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.dgvComparacaoPrecos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvComparacaoPrecos.Size = New System.Drawing.Size(1065, 318)
        Me.dgvComparacaoPrecos.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.dgvComparacaoPrecos)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 132)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1071, 340)
        Me.GroupBox1.TabIndex = 52
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Relação dos Preços dos Fornecedores"
        '
        'cmbTipoConsulta
        '
        Me.cmbTipoConsulta.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmbTipoConsulta.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipoConsulta.FormattingEnabled = True
        Me.cmbTipoConsulta.Items.AddRange(New Object() {"Fornecedor", "Produtos", "Código Barras"})
        Me.cmbTipoConsulta.Location = New System.Drawing.Point(13, 85)
        Me.cmbTipoConsulta.Name = "cmbTipoConsulta"
        Me.cmbTipoConsulta.Size = New System.Drawing.Size(169, 33)
        Me.cmbTipoConsulta.TabIndex = 54
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(850, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(233, 15)
        Me.Label1.TabIndex = 61
        Me.Label1.Text = "Média em R$ dos Produtos por Fornecedor"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label5.Font = New System.Drawing.Font("Candara", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(1067, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(24, 26)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "X"
        '
        'txtProduto
        '
        Me.txtProduto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProduto.BackColor = System.Drawing.Color.White
        Me.txtProduto.BorderColorFocused = System.Drawing.Color.SteelBlue
        Me.txtProduto.BorderColorIdle = System.Drawing.Color.Silver
        Me.txtProduto.BorderColorMouseHover = System.Drawing.Color.SteelBlue
        Me.txtProduto.BorderThickness = 1
        Me.txtProduto.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtProduto.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProduto.ForeColor = System.Drawing.Color.Black
        Me.txtProduto.isPassword = False
        Me.txtProduto.Location = New System.Drawing.Point(186, 85)
        Me.txtProduto.Margin = New System.Windows.Forms.Padding(4)
        Me.txtProduto.Name = "txtProduto"
        Me.txtProduto.Size = New System.Drawing.Size(630, 33)
        Me.txtProduto.TabIndex = 83
        Me.txtProduto.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'txtMedia
        '
        Me.txtMedia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMedia.BackColor = System.Drawing.Color.White
        Me.txtMedia.BorderColorFocused = System.Drawing.Color.SteelBlue
        Me.txtMedia.BorderColorIdle = System.Drawing.Color.Silver
        Me.txtMedia.BorderColorMouseHover = System.Drawing.Color.SteelBlue
        Me.txtMedia.BorderThickness = 1
        Me.txtMedia.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMedia.Font = New System.Drawing.Font("Microsoft YaHei UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMedia.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.txtMedia.isPassword = False
        Me.txtMedia.Location = New System.Drawing.Point(825, 85)
        Me.txtMedia.Margin = New System.Windows.Forms.Padding(5)
        Me.txtMedia.Name = "txtMedia"
        Me.txtMedia.Size = New System.Drawing.Size(257, 33)
        Me.txtMedia.TabIndex = 84
        Me.txtMedia.Text = "0,00"
        Me.txtMedia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lupaClente
        '
        Me.lupaClente.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lupaClente.BackColor = System.Drawing.Color.White
        Me.lupaClente.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lupaClente.Image = CType(resources.GetObject("lupaClente.Image"), System.Drawing.Image)
        Me.lupaClente.Location = New System.Drawing.Point(786, 89)
        Me.lupaClente.Name = "lupaClente"
        Me.lupaClente.Size = New System.Drawing.Size(28, 26)
        Me.lupaClente.TabIndex = 149
        Me.lupaClente.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(183, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(117, 15)
        Me.Label6.TabIndex = 150
        Me.Label6.Text = "Digite sua consulta"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(8, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 15)
        Me.Label3.TabIndex = 151
        Me.Label3.Text = "Tipo de consulta"
        '
        'BunifuSeparator1
        '
        Me.BunifuSeparator1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BunifuSeparator1.BackColor = System.Drawing.Color.Transparent
        Me.BunifuSeparator1.LineColor = System.Drawing.Color.Silver
        Me.BunifuSeparator1.LineThickness = 1
        Me.BunifuSeparator1.Location = New System.Drawing.Point(0, 41)
        Me.BunifuSeparator1.Name = "BunifuSeparator1"
        Me.BunifuSeparator1.Size = New System.Drawing.Size(1095, 14)
        Me.BunifuSeparator1.TabIndex = 153
        Me.BunifuSeparator1.Transparency = 255
        Me.BunifuSeparator1.Vertical = False
        '
        'btnVendas
        '
        Me.btnVendas.BackColor = System.Drawing.Color.White
        Me.btnVendas.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro
        Me.btnVendas.FlatAppearance.BorderSize = 0
        Me.btnVendas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVendas.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVendas.ForeColor = System.Drawing.Color.Black
        Me.btnVendas.Image = CType(resources.GetObject("btnVendas.Image"), System.Drawing.Image)
        Me.btnVendas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnVendas.Location = New System.Drawing.Point(7, 5)
        Me.btnVendas.Name = "btnVendas"
        Me.btnVendas.Size = New System.Drawing.Size(326, 38)
        Me.btnVendas.TabIndex = 152
        Me.btnVendas.Text = "Consulta vendas agrupadas por fornecedores"
        Me.btnVendas.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnVendas.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(12, 475)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(252, 15)
        Me.Label2.TabIndex = 154
        Me.Label2.Text = "Comparação de preços por fornecedores"
        '
        'frmComparacaoPrecos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1096, 497)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BunifuSeparator1)
        Me.Controls.Add(Me.btnVendas)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lupaClente)
        Me.Controls.Add(Me.txtMedia)
        Me.Controls.Add(Me.txtProduto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbTipoConsulta)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmComparacaoPrecos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Comparação de Preços"
        CType(Me.dgvComparacaoPrecos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.lupaClente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvComparacaoPrecos As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbTipoConsulta As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtProduto As ns1.BunifuMetroTextbox
    Friend WithEvents txtMedia As ns1.BunifuMetroTextbox
    Friend WithEvents lupaClente As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents BunifuSeparator1 As ns1.BunifuSeparator
    Friend WithEvents btnVendas As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
