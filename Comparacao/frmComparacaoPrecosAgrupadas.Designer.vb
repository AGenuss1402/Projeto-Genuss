<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmComparacaoPrecosAgrupadas
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmComparacaoPrecosAgrupadas))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.dgvProdutosPorCategorias = New System.Windows.Forms.DataGridView()
        Me.id_fornecedor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fornecedor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cidade = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.uf = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.codigo_barras = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.produto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.valor_unitario = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.porcentagem = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.valor_venda = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.data_compra = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTotalItensVendidos = New System.Windows.Forms.Label()
        Me.lblQtdaItensVendidos = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnConsultarVendas = New System.Windows.Forms.Button()
        Me.dtpData = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.menuNavegacao = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.btnConsultar = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvProdutosPorCategorias, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.menuNavegacao, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.menuNavegacao.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.dgvProdutosPorCategorias)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 136)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(858, 418)
        Me.GroupBox1.TabIndex = 51
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Relação das Vendas Agrupadas"
        '
        'dgvProdutosPorCategorias
        '
        Me.dgvProdutosPorCategorias.AllowUserToAddRows = False
        Me.dgvProdutosPorCategorias.AllowUserToDeleteRows = False
        Me.dgvProdutosPorCategorias.AllowUserToResizeColumns = False
        Me.dgvProdutosPorCategorias.AllowUserToResizeRows = False
        Me.dgvProdutosPorCategorias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvProdutosPorCategorias.BackgroundColor = System.Drawing.Color.White
        Me.dgvProdutosPorCategorias.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvProdutosPorCategorias.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvProdutosPorCategorias.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvProdutosPorCategorias.ColumnHeadersHeight = 32
        Me.dgvProdutosPorCategorias.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.id_fornecedor, Me.fornecedor, Me.cidade, Me.uf, Me.codigo_barras, Me.produto, Me.valor_unitario, Me.porcentagem, Me.valor_venda, Me.data_compra})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvProdutosPorCategorias.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvProdutosPorCategorias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvProdutosPorCategorias.EnableHeadersVisualStyles = False
        Me.dgvProdutosPorCategorias.GridColor = System.Drawing.SystemColors.Control
        Me.dgvProdutosPorCategorias.Location = New System.Drawing.Point(3, 19)
        Me.dgvProdutosPorCategorias.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dgvProdutosPorCategorias.Name = "dgvProdutosPorCategorias"
        Me.dgvProdutosPorCategorias.ReadOnly = True
        Me.dgvProdutosPorCategorias.RowHeadersVisible = False
        Me.dgvProdutosPorCategorias.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.dgvProdutosPorCategorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProdutosPorCategorias.Size = New System.Drawing.Size(852, 396)
        Me.dgvProdutosPorCategorias.TabIndex = 10
        '
        'id_fornecedor
        '
        Me.id_fornecedor.HeaderText = "ID F."
        Me.id_fornecedor.Name = "id_fornecedor"
        Me.id_fornecedor.ReadOnly = True
        Me.id_fornecedor.Visible = False
        Me.id_fornecedor.Width = 43
        '
        'fornecedor
        '
        Me.fornecedor.HeaderText = "Fornecedor"
        Me.fornecedor.Name = "fornecedor"
        Me.fornecedor.ReadOnly = True
        Me.fornecedor.Width = 90
        '
        'cidade
        '
        Me.cidade.HeaderText = "Cidade"
        Me.cidade.Name = "cidade"
        Me.cidade.ReadOnly = True
        Me.cidade.Width = 67
        '
        'uf
        '
        Me.uf.HeaderText = "UF"
        Me.uf.Name = "uf"
        Me.uf.ReadOnly = True
        Me.uf.Width = 44
        '
        'codigo_barras
        '
        Me.codigo_barras.HeaderText = "Cód. Barras"
        Me.codigo_barras.Name = "codigo_barras"
        Me.codigo_barras.ReadOnly = True
        Me.codigo_barras.Width = 83
        '
        'produto
        '
        Me.produto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.produto.HeaderText = "Produto"
        Me.produto.Name = "produto"
        Me.produto.ReadOnly = True
        '
        'valor_unitario
        '
        DataGridViewCellStyle2.Format = "c"
        Me.valor_unitario.DefaultCellStyle = DataGridViewCellStyle2
        Me.valor_unitario.HeaderText = "Vlr. Unitário"
        Me.valor_unitario.Name = "valor_unitario"
        Me.valor_unitario.ReadOnly = True
        Me.valor_unitario.Width = 85
        '
        'porcentagem
        '
        Me.porcentagem.HeaderText = "%"
        Me.porcentagem.Name = "porcentagem"
        Me.porcentagem.ReadOnly = True
        Me.porcentagem.Width = 40
        '
        'valor_venda
        '
        DataGridViewCellStyle3.Format = "c"
        Me.valor_venda.DefaultCellStyle = DataGridViewCellStyle3
        Me.valor_venda.HeaderText = "Vlr. Venda"
        Me.valor_venda.Name = "valor_venda"
        Me.valor_venda.ReadOnly = True
        Me.valor_venda.Width = 76
        '
        'data_compra
        '
        DataGridViewCellStyle4.Format = "d"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.data_compra.DefaultCellStyle = DataGridViewCellStyle4
        Me.data_compra.HeaderText = "Dt. Compra"
        Me.data_compra.Name = "data_compra"
        Me.data_compra.ReadOnly = True
        Me.data_compra.Width = 84
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 15)
        Me.Label3.TabIndex = 59
        Me.Label3.Text = "Total R$ Itens Vendidos:"
        '
        'lblTotalItensVendidos
        '
        Me.lblTotalItensVendidos.AutoSize = True
        Me.lblTotalItensVendidos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalItensVendidos.Location = New System.Drawing.Point(151, 108)
        Me.lblTotalItensVendidos.Name = "lblTotalItensVendidos"
        Me.lblTotalItensVendidos.Size = New System.Drawing.Size(49, 15)
        Me.lblTotalItensVendidos.TabIndex = 60
        Me.lblTotalItensVendidos.Text = "R$ 0,00"
        '
        'lblQtdaItensVendidos
        '
        Me.lblQtdaItensVendidos.AutoSize = True
        Me.lblQtdaItensVendidos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQtdaItensVendidos.Location = New System.Drawing.Point(151, 79)
        Me.lblQtdaItensVendidos.Name = "lblQtdaItensVendidos"
        Me.lblQtdaItensVendidos.Size = New System.Drawing.Size(14, 15)
        Me.lblQtdaItensVendidos.TabIndex = 61
        Me.lblQtdaItensVendidos.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 15)
        Me.Label2.TabIndex = 62
        Me.Label2.Text = "Qtda. Itens Vendidos:"
        '
        'btnConsultarVendas
        '
        Me.btnConsultarVendas.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConsultarVendas.Image = CType(resources.GetObject("btnConsultarVendas.Image"), System.Drawing.Image)
        Me.btnConsultarVendas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConsultarVendas.Location = New System.Drawing.Point(740, 100)
        Me.btnConsultarVendas.Name = "btnConsultarVendas"
        Me.btnConsultarVendas.Size = New System.Drawing.Size(127, 30)
        Me.btnConsultarVendas.TabIndex = 58
        Me.btnConsultarVendas.Text = "Consultar"
        Me.btnConsultarVendas.UseVisualStyleBackColor = True
        '
        'dtpData
        '
        Me.dtpData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpData.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpData.Location = New System.Drawing.Point(619, 101)
        Me.dtpData.Name = "dtpData"
        Me.dtpData.Size = New System.Drawing.Size(115, 27)
        Me.dtpData.TabIndex = 57
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(616, 82)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 19)
        Me.Label1.TabIndex = 56
        Me.Label1.Text = "Data"
        '
        'menuNavegacao
        '
        Me.menuNavegacao.AddNewItem = Nothing
        Me.menuNavegacao.BackColor = System.Drawing.Color.White
        Me.menuNavegacao.CountItem = Nothing
        Me.menuNavegacao.CountItemFormat = "Total Orçamentos {0}"
        Me.menuNavegacao.DeleteItem = Nothing
        Me.menuNavegacao.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.menuNavegacao.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.menuNavegacao.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnConsultar, Me.ToolStripSeparator1})
        Me.menuNavegacao.Location = New System.Drawing.Point(0, 32)
        Me.menuNavegacao.MoveFirstItem = Nothing
        Me.menuNavegacao.MoveLastItem = Nothing
        Me.menuNavegacao.MoveNextItem = Nothing
        Me.menuNavegacao.MovePreviousItem = Nothing
        Me.menuNavegacao.Name = "menuNavegacao"
        Me.menuNavegacao.PositionItem = Nothing
        Me.menuNavegacao.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.menuNavegacao.Size = New System.Drawing.Size(882, 39)
        Me.menuNavegacao.TabIndex = 55
        Me.menuNavegacao.Text = "BindingNavigator1"
        '
        'btnConsultar
        '
        Me.btnConsultar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnConsultar.Image = CType(resources.GetObject("btnConsultar.Image"), System.Drawing.Image)
        Me.btnConsultar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnConsultar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnConsultar.Name = "btnConsultar"
        Me.btnConsultar.Size = New System.Drawing.Size(157, 36)
        Me.btnConsultar.Text = "Listar todas as vendas"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gray
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(882, 32)
        Me.Panel1.TabIndex = 63
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label5.Font = New System.Drawing.Font("Candara", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(855, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(24, 26)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "X"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Candara", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(3, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(419, 18)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Comparação de preços agrupados ( Genuss Automação Comercial )"
        '
        'frmComparacaoPrecosAgrupadas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 566)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblTotalItensVendidos)
        Me.Controls.Add(Me.lblQtdaItensVendidos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnConsultarVendas)
        Me.Controls.Add(Me.dtpData)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.menuNavegacao)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmComparacaoPrecosAgrupadas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Comparação de Preços Agrupadas"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgvProdutosPorCategorias, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.menuNavegacao, System.ComponentModel.ISupportInitialize).EndInit()
        Me.menuNavegacao.ResumeLayout(False)
        Me.menuNavegacao.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dgvProdutosPorCategorias As System.Windows.Forms.DataGridView
    Friend WithEvents id_fornecedor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fornecedor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cidade As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents uf As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents codigo_barras As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents produto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents valor_unitario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents porcentagem As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents valor_venda As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents data_compra As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTotalItensVendidos As System.Windows.Forms.Label
    Friend WithEvents lblQtdaItensVendidos As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnConsultarVendas As System.Windows.Forms.Button
    Friend WithEvents dtpData As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents menuNavegacao As System.Windows.Forms.BindingNavigator
    Private WithEvents btnConsultar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
