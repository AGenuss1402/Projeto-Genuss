<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmListaClientes
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmListaClientes))
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.dgvClientes = New System.Windows.Forms.DataGridView()
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtConsultaCliente = New ns1.BunifuTextbox()
        Me.cmbTipoConsulta = New ns1.BunifuDropdown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.btnExportaExel = New System.Windows.Forms.Button()
        Me.btnAtualizarCliente = New System.Windows.Forms.Button()
        Me.btnNovoCliente = New System.Windows.Forms.Button()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.lblFechar = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BunifuElipse1 = New Bunifu.Framework.UI.BunifuElipse(Me.components)
        CType(Me.dgvClientes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.LightGray
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1049, 1)
        Me.Panel3.TabIndex = 52
        '
        'dgvClientes
        '
        Me.dgvClientes.AllowUserToAddRows = False
        Me.dgvClientes.AllowUserToDeleteRows = False
        Me.dgvClientes.AllowUserToResizeRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.AliceBlue
        Me.dgvClientes.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        Me.dgvClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvClientes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvClientes.BackgroundColor = System.Drawing.Color.White
        Me.dgvClientes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvClientes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(208, Byte), Integer))
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(208, Byte), Integer))
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvClientes.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.dgvClientes.ColumnHeadersHeight = 30
        Me.dgvClientes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewImageColumn1, Me.Column2})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.PowderBlue
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvClientes.DefaultCellStyle = DataGridViewCellStyle11
        Me.dgvClientes.EnableHeadersVisualStyles = False
        Me.dgvClientes.GridColor = System.Drawing.Color.Gainsboro
        Me.dgvClientes.Location = New System.Drawing.Point(15, 204)
        Me.dgvClientes.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvClientes.Name = "dgvClientes"
        Me.dgvClientes.ReadOnly = True
        Me.dgvClientes.RowHeadersVisible = False
        Me.dgvClientes.RowHeadersWidth = 50
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.White
        Me.dgvClientes.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.dgvClientes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvClientes.Size = New System.Drawing.Size(1016, 330)
        Me.dgvClientes.TabIndex = 74
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DataGridViewImageColumn1.HeaderText = "#"
        Me.DataGridViewImageColumn1.Image = CType(resources.GetObject("DataGridViewImageColumn1.Image"), System.Drawing.Image)
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewImageColumn1.Width = 60
        '
        'Column2
        '
        Me.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Column2.HeaderText = "#"
        Me.Column2.Image = CType(resources.GetObject("Column2.Image"), System.Drawing.Image)
        Me.Column2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column2.Width = 60
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightGray
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 543)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1049, 21)
        Me.Panel2.TabIndex = 57
        '
        'txtConsultaCliente
        '
        Me.txtConsultaCliente.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConsultaCliente.BackColor = System.Drawing.SystemColors.Control
        Me.txtConsultaCliente.BackgroundImage = CType(resources.GetObject("txtConsultaCliente.BackgroundImage"), System.Drawing.Image)
        Me.txtConsultaCliente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.txtConsultaCliente.ForeColor = System.Drawing.Color.Gray
        Me.txtConsultaCliente.Icon = CType(resources.GetObject("txtConsultaCliente.Icon"), System.Drawing.Image)
        Me.txtConsultaCliente.Location = New System.Drawing.Point(224, 161)
        Me.txtConsultaCliente.Name = "txtConsultaCliente"
        Me.txtConsultaCliente.Size = New System.Drawing.Size(250, 35)
        Me.txtConsultaCliente.TabIndex = 156
        Me.txtConsultaCliente.text = ""
        '
        'cmbTipoConsulta
        '
        Me.cmbTipoConsulta.BackColor = System.Drawing.SystemColors.HotTrack
        Me.cmbTipoConsulta.BorderRadius = 5
        Me.cmbTipoConsulta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.cmbTipoConsulta.ForeColor = System.Drawing.Color.White
        Me.cmbTipoConsulta.Items = New String() {"Pesquisa por codigo", "Pesquisa por nome"}
        Me.cmbTipoConsulta.Location = New System.Drawing.Point(15, 161)
        Me.cmbTipoConsulta.Name = "cmbTipoConsulta"
        Me.cmbTipoConsulta.NomalColor = System.Drawing.SystemColors.HotTrack
        Me.cmbTipoConsulta.onHoverColor = System.Drawing.SystemColors.Highlight
        Me.cmbTipoConsulta.selectedIndex = -1
        Me.cmbTipoConsulta.Size = New System.Drawing.Size(203, 35)
        Me.cmbTipoConsulta.TabIndex = 157
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(143, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(281, 24)
        Me.Label4.TabIndex = 174
        Me.Label4.Text = "Relação de clientes cadastrados"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(41, 3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(106, 31)
        Me.Label10.TabIndex = 175
        Me.Label10.Text = "Cliente"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1049, 564)
        Me.Panel1.TabIndex = 167
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.Panel6)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Controls.Add(Me.txtConsultaCliente)
        Me.Panel4.Controls.Add(Me.cmbTipoConsulta)
        Me.Panel4.Controls.Add(Me.dgvClientes)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1047, 562)
        Me.Panel4.TabIndex = 183
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(11, 138)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(277, 20)
        Me.Label1.TabIndex = 191
        Me.Label1.Text = "Pesquisa por código/ Nome do Cliente"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.White
        Me.Panel6.Controls.Add(Me.btnExportaExel)
        Me.Panel6.Controls.Add(Me.btnAtualizarCliente)
        Me.Panel6.Controls.Add(Me.btnNovoCliente)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(0, 39)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1045, 80)
        Me.Panel6.TabIndex = 190
        '
        'btnExportaExel
        '
        Me.btnExportaExel.BackColor = System.Drawing.Color.White
        Me.btnExportaExel.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnExportaExel.FlatAppearance.BorderSize = 0
        Me.btnExportaExel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportaExel.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.btnExportaExel.Image = CType(resources.GetObject("btnExportaExel.Image"), System.Drawing.Image)
        Me.btnExportaExel.Location = New System.Drawing.Point(164, 0)
        Me.btnExportaExel.Name = "btnExportaExel"
        Me.btnExportaExel.Size = New System.Drawing.Size(106, 80)
        Me.btnExportaExel.TabIndex = 2
        Me.btnExportaExel.Text = "Exportar Exel"
        Me.btnExportaExel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnExportaExel.UseVisualStyleBackColor = False
        '
        'btnAtualizarCliente
        '
        Me.btnAtualizarCliente.BackColor = System.Drawing.Color.White
        Me.btnAtualizarCliente.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnAtualizarCliente.FlatAppearance.BorderSize = 0
        Me.btnAtualizarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAtualizarCliente.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.btnAtualizarCliente.Image = CType(resources.GetObject("btnAtualizarCliente.Image"), System.Drawing.Image)
        Me.btnAtualizarCliente.Location = New System.Drawing.Point(82, 0)
        Me.btnAtualizarCliente.Name = "btnAtualizarCliente"
        Me.btnAtualizarCliente.Size = New System.Drawing.Size(82, 80)
        Me.btnAtualizarCliente.TabIndex = 1
        Me.btnAtualizarCliente.Text = "Atualizar"
        Me.btnAtualizarCliente.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnAtualizarCliente.UseVisualStyleBackColor = False
        '
        'btnNovoCliente
        '
        Me.btnNovoCliente.BackColor = System.Drawing.Color.White
        Me.btnNovoCliente.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnNovoCliente.FlatAppearance.BorderSize = 0
        Me.btnNovoCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNovoCliente.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.btnNovoCliente.Image = CType(resources.GetObject("btnNovoCliente.Image"), System.Drawing.Image)
        Me.btnNovoCliente.Location = New System.Drawing.Point(0, 0)
        Me.btnNovoCliente.Name = "btnNovoCliente"
        Me.btnNovoCliente.Size = New System.Drawing.Size(82, 80)
        Me.btnNovoCliente.TabIndex = 0
        Me.btnNovoCliente.Text = "Novo"
        Me.btnNovoCliente.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnNovoCliente.UseVisualStyleBackColor = False
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(94, Byte), Integer), CType(CType(158, Byte), Integer))
        Me.Panel5.Controls.Add(Me.lblFechar)
        Me.Panel5.Controls.Add(Me.PictureBox1)
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1045, 39)
        Me.Panel5.TabIndex = 189
        '
        'lblFechar
        '
        Me.lblFechar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFechar.AutoSize = True
        Me.lblFechar.BackColor = System.Drawing.Color.Transparent
        Me.lblFechar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblFechar.Font = New System.Drawing.Font("Century Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFechar.ForeColor = System.Drawing.Color.White
        Me.lblFechar.Location = New System.Drawing.Point(1018, 5)
        Me.lblFechar.Name = "lblFechar"
        Me.lblFechar.Size = New System.Drawing.Size(23, 24)
        Me.lblFechar.TabIndex = 176
        Me.lblFechar.Text = "X"
        Me.lblFechar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(4, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 176
        Me.PictureBox1.TabStop = False
        '
        'BunifuElipse1
        '
        Me.BunifuElipse1.ElipseRadius = 15
        Me.BunifuElipse1.TargetControl = Me
        '
        'frmListaClientes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1049, 564)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmListaClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmListaClientes"
        CType(Me.dgvClientes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtConsultaCliente1 As System.Windows.Forms.TextBox
    Friend WithEvents dgvClientes As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents txtConsultaCliente As ns1.BunifuTextbox
    Friend WithEvents cmbTipoConsulta As ns1.BunifuDropdown
    Friend WithEvents Label4 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BunifuElipse1 As Bunifu.Framework.UI.BunifuElipse
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents lblFechar As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents btnExportaExel As Button
    Friend WithEvents btnAtualizarCliente As Button
    Friend WithEvents btnNovoCliente As Button
    Friend WithEvents Label1 As Label
End Class
