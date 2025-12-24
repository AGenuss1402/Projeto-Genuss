<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClientes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClientes))
        Me.menuNavegacao = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.btnCadastrarProdutos = New System.Windows.Forms.ToolStripButton()
        Me.btnGravarAlterar = New System.Windows.Forms.ToolStripButton()
        Me.btnExcluir = New System.Windows.Forms.ToolStripButton()
        Me.btnFechar = New System.Windows.Forms.ToolStripButton()
        Me.cmbUF = New System.Windows.Forms.ComboBox()
        Me.txtTelefone = New System.Windows.Forms.TextBox()
        Me.txtEndereco = New System.Windows.Forms.TextBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBairro = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCep = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCelular = New System.Windows.Forms.TextBox()
        Me.cmbSexo = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dtpNascimento = New System.Windows.Forms.DateTimePicker()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.dtpCadastro = New System.Windows.Forms.DateTimePicker()
        Me.txtCidade = New System.Windows.Forms.TextBox()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtApelido = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblFechar = New ns1.BunifuCustomLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblImage = New System.Windows.Forms.LinkLabel()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtObsF = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.cmbUFfiliacao = New System.Windows.Forms.ComboBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtCidadeF = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtBairroF = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtNumero = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtEnderecoFiliacao = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.cmbOcupacaoMae = New System.Windows.Forms.ComboBox()
        Me.cmbOcupacaoPai = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtMae = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtPai = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.imgCliente = New System.Windows.Forms.PictureBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnAcoes = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.menuNavegacao, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.menuNavegacao.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.imgCliente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'menuNavegacao
        '
        Me.menuNavegacao.AddNewItem = Nothing
        Me.menuNavegacao.BackColor = System.Drawing.Color.White
        Me.menuNavegacao.CountItem = Nothing
        Me.menuNavegacao.CountItemFormat = "Total Orçamentos {0}"
        Me.menuNavegacao.DeleteItem = Nothing
        Me.menuNavegacao.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.menuNavegacao.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCadastrarProdutos, Me.btnGravarAlterar, Me.btnExcluir, Me.btnFechar})
        Me.menuNavegacao.Location = New System.Drawing.Point(0, 41)
        Me.menuNavegacao.MoveFirstItem = Nothing
        Me.menuNavegacao.MoveLastItem = Nothing
        Me.menuNavegacao.MoveNextItem = Nothing
        Me.menuNavegacao.MovePreviousItem = Nothing
        Me.menuNavegacao.Name = "menuNavegacao"
        Me.menuNavegacao.PositionItem = Nothing
        Me.menuNavegacao.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.menuNavegacao.Size = New System.Drawing.Size(1068, 54)
        Me.menuNavegacao.TabIndex = 0
        Me.menuNavegacao.Text = "BindingNavigator1"
        '
        'btnCadastrarProdutos
        '
        Me.btnCadastrarProdutos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnCadastrarProdutos.Image = CType(resources.GetObject("btnCadastrarProdutos.Image"), System.Drawing.Image)
        Me.btnCadastrarProdutos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnCadastrarProdutos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCadastrarProdutos.Name = "btnCadastrarProdutos"
        Me.btnCadastrarProdutos.Size = New System.Drawing.Size(40, 51)
        Me.btnCadastrarProdutos.Text = "Novo"
        Me.btnCadastrarProdutos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnGravarAlterar
        '
        Me.btnGravarAlterar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnGravarAlterar.Image = CType(resources.GetObject("btnGravarAlterar.Image"), System.Drawing.Image)
        Me.btnGravarAlterar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnGravarAlterar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnGravarAlterar.Name = "btnGravarAlterar"
        Me.btnGravarAlterar.Size = New System.Drawing.Size(85, 51)
        Me.btnGravarAlterar.Text = "Gravar/Alterar"
        Me.btnGravarAlterar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnExcluir
        '
        Me.btnExcluir.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnExcluir.Image = CType(resources.GetObject("btnExcluir.Image"), System.Drawing.Image)
        Me.btnExcluir.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnExcluir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnExcluir.Name = "btnExcluir"
        Me.btnExcluir.Size = New System.Drawing.Size(45, 51)
        Me.btnExcluir.Text = "Excluir"
        Me.btnExcluir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnFechar
        '
        Me.btnFechar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnFechar.Image = CType(resources.GetObject("btnFechar.Image"), System.Drawing.Image)
        Me.btnFechar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnFechar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFechar.Name = "btnFechar"
        Me.btnFechar.Size = New System.Drawing.Size(36, 51)
        Me.btnFechar.Text = "Sair"
        Me.btnFechar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'cmbUF
        '
        Me.cmbUF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUF.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUF.FormattingEnabled = True
        Me.cmbUF.Items.AddRange(New Object() {"Acre" & Global.Microsoft.VisualBasic.ChrW(9), "Alagoas", "Amazonas" & Global.Microsoft.VisualBasic.ChrW(9), "Bahia" & Global.Microsoft.VisualBasic.ChrW(9), "Ceará" & Global.Microsoft.VisualBasic.ChrW(9), "Espírito Santo" & Global.Microsoft.VisualBasic.ChrW(9), "Goiás" & Global.Microsoft.VisualBasic.ChrW(9), "Maranhão", "Mato Grosso" & Global.Microsoft.VisualBasic.ChrW(9), "Mato Grosso do Sul" & Global.Microsoft.VisualBasic.ChrW(9), "Minas Gerais" & Global.Microsoft.VisualBasic.ChrW(9), "Pará" & Global.Microsoft.VisualBasic.ChrW(9), "Paraíba", "Paraná" & Global.Microsoft.VisualBasic.ChrW(9), "Pernambuco" & Global.Microsoft.VisualBasic.ChrW(9), "Piauí" & Global.Microsoft.VisualBasic.ChrW(9), "Rio de Janeiro" & Global.Microsoft.VisualBasic.ChrW(9), "Rio Grande do Norte", "Rio Grande do Sul" & Global.Microsoft.VisualBasic.ChrW(9), "Rondônia" & Global.Microsoft.VisualBasic.ChrW(9) & Global.Microsoft.VisualBasic.ChrW(9), "Roraima", "Santa Catarina" & Global.Microsoft.VisualBasic.ChrW(9), "São Paulo" & Global.Microsoft.VisualBasic.ChrW(9), "Sergipe" & Global.Microsoft.VisualBasic.ChrW(9), "Tocantins" & Global.Microsoft.VisualBasic.ChrW(9)})
        Me.cmbUF.Location = New System.Drawing.Point(12, 70)
        Me.cmbUF.Name = "cmbUF"
        Me.cmbUF.Size = New System.Drawing.Size(167, 23)
        Me.cmbUF.TabIndex = 17
        '
        'txtTelefone
        '
        Me.txtTelefone.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTelefone.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTelefone.Location = New System.Drawing.Point(185, 71)
        Me.txtTelefone.Name = "txtTelefone"
        Me.txtTelefone.Size = New System.Drawing.Size(106, 23)
        Me.txtTelefone.TabIndex = 19
        '
        'txtEndereco
        '
        Me.txtEndereco.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndereco.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEndereco.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndereco.Location = New System.Drawing.Point(12, 26)
        Me.txtEndereco.Name = "txtEndereco"
        Me.txtEndereco.Size = New System.Drawing.Size(364, 23)
        Me.txtEndereco.TabIndex = 9
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEmail.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.Location = New System.Drawing.Point(592, 69)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(277, 23)
        Me.txtEmail.TabIndex = 27
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(9, 54)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 15)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Estado"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(183, 54)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(55, 15)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Telefone"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(9, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 15)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Endereço"
        '
        'txtNome
        '
        Me.txtNome.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNome.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNome.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNome.Location = New System.Drawing.Point(105, 23)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(313, 21)
        Me.txtNome.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(675, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 15)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Cidade"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(102, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nome Completo"
        '
        'txtCodigo
        '
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(8, 23)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(90, 21)
        Me.txtCodigo.TabIndex = 0
        Me.txtCodigo.Text = "Novo"
        '
        'txtDocumento
        '
        Me.txtDocumento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocumento.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumento.Location = New System.Drawing.Point(655, 23)
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(131, 21)
        Me.txtDocumento.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(652, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 15)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Documento"
        '
        'txtBairro
        '
        Me.txtBairro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBairro.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBairro.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBairro.Location = New System.Drawing.Point(383, 26)
        Me.txtBairro.Name = "txtBairro"
        Me.txtBairro.Size = New System.Drawing.Size(192, 23)
        Me.txtBairro.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(384, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 15)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Bairro"
        '
        'txtCep
        '
        Me.txtCep.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCep.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCep.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCep.Location = New System.Drawing.Point(580, 26)
        Me.txtCep.Name = "txtCep"
        Me.txtCep.Size = New System.Drawing.Size(91, 23)
        Me.txtCep.TabIndex = 13
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(577, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(29, 15)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Cep"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(295, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 15)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Celular"
        '
        'txtCelular
        '
        Me.txtCelular.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCelular.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCelular.Location = New System.Drawing.Point(295, 71)
        Me.txtCelular.Name = "txtCelular"
        Me.txtCelular.Size = New System.Drawing.Size(97, 23)
        Me.txtCelular.TabIndex = 21
        '
        'cmbSexo
        '
        Me.cmbSexo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSexo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSexo.FormattingEnabled = True
        Me.cmbSexo.Items.AddRange(New Object() {"Masculino", "Feminino"})
        Me.cmbSexo.Location = New System.Drawing.Point(396, 71)
        Me.cmbSexo.Name = "cmbSexo"
        Me.cmbSexo.Size = New System.Drawing.Size(94, 23)
        Me.cmbSexo.TabIndex = 23
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(394, 54)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 15)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Sexo"
        '
        'dtpNascimento
        '
        Me.dtpNascimento.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpNascimento.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpNascimento.Location = New System.Drawing.Point(495, 71)
        Me.dtpNascimento.Name = "dtpNascimento"
        Me.dtpNascimento.Size = New System.Drawing.Size(91, 22)
        Me.dtpNascimento.TabIndex = 25
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(492, 54)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(73, 15)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Nascimento"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(589, 54)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(43, 15)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "E-mail"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(787, 5)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 15)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Cadastro"
        '
        'dtpCadastro
        '
        Me.dtpCadastro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpCadastro.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.dtpCadastro.Enabled = False
        Me.dtpCadastro.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpCadastro.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpCadastro.Location = New System.Drawing.Point(792, 23)
        Me.dtpCadastro.Name = "dtpCadastro"
        Me.dtpCadastro.Size = New System.Drawing.Size(91, 21)
        Me.dtpCadastro.TabIndex = 29
        '
        'txtCidade
        '
        Me.txtCidade.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCidade.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCidade.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCidade.Location = New System.Drawing.Point(678, 26)
        Me.txtCidade.Name = "txtCidade"
        Me.txtCidade.Size = New System.Drawing.Size(191, 23)
        Me.txtCidade.TabIndex = 15
        '
        'txtObs
        '
        Me.txtObs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObs.BackColor = System.Drawing.Color.White
        Me.txtObs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtObs.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtObs.Location = New System.Drawing.Point(12, 115)
        Me.txtObs.Multiline = True
        Me.txtObs.Name = "txtObs"
        Me.txtObs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObs.Size = New System.Drawing.Size(857, 39)
        Me.txtObs.TabIndex = 31
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(9, 98)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 30
        Me.Label14.Text = "Anotações"
        '
        'txtApelido
        '
        Me.txtApelido.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtApelido.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtApelido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtApelido.Location = New System.Drawing.Point(424, 23)
        Me.txtApelido.Name = "txtApelido"
        Me.txtApelido.Size = New System.Drawing.Size(129, 21)
        Me.txtApelido.TabIndex = 4
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(421, 5)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 15)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Apelido"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.DarkSlateBlue
        Me.Panel3.Controls.Add(Me.Label27)
        Me.Panel3.Controls.Add(Me.PictureBox1)
        Me.Panel3.Controls.Add(Me.lblFechar)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1068, 41)
        Me.Panel3.TabIndex = 54
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ForeColor = System.Drawing.Color.White
        Me.Label27.Location = New System.Drawing.Point(43, 12)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(283, 18)
        Me.Label27.TabIndex = 154
        Me.Label27.Text = "CADASTRO DE CLIENTES E AFILIADOS"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(11, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 153
        Me.PictureBox1.TabStop = False
        '
        'lblFechar
        '
        Me.lblFechar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFechar.AutoSize = True
        Me.lblFechar.BackColor = System.Drawing.Color.Transparent
        Me.lblFechar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblFechar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFechar.ForeColor = System.Drawing.Color.White
        Me.lblFechar.Location = New System.Drawing.Point(1045, 7)
        Me.lblFechar.Name = "lblFechar"
        Me.lblFechar.Size = New System.Drawing.Size(20, 20)
        Me.lblFechar.TabIndex = 74
        Me.lblFechar.Text = "X"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Font = New System.Drawing.Font("Candara", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(7, 98)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1055, 422)
        Me.TabControl1.TabIndex = 55
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.White
        Me.TabPage1.Controls.Add(Me.lblImage)
        Me.TabPage1.Controls.Add(Me.Label29)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.Label18)
        Me.TabPage1.Controls.Add(Me.ComboBox1)
        Me.TabPage1.Controls.Add(Me.Label17)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.dtpCadastro)
        Me.TabPage1.Controls.Add(Me.imgCliente)
        Me.TabPage1.Controls.Add(Me.txtNome)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtCodigo)
        Me.TabPage1.Controls.Add(Me.txtApelido)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.txtDocumento)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Font = New System.Drawing.Font("Candara", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1047, 394)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Informaçoes de cadastro de clientes"
        '
        'lblImage
        '
        Me.lblImage.AutoSize = True
        Me.lblImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblImage.Location = New System.Drawing.Point(926, 188)
        Me.lblImage.Name = "lblImage"
        Me.lblImage.Size = New System.Drawing.Size(114, 16)
        Me.lblImage.TabIndex = 100
        Me.lblImage.TabStop = True
        Me.lblImage.Text = "Selecionar Image"
        '
        'Label29
        '
        Me.Label29.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label29.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label29.Font = New System.Drawing.Font("Candara", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ForeColor = System.Drawing.Color.White
        Me.Label29.Location = New System.Drawing.Point(10, 213)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(1028, 22)
        Me.Label29.TabIndex = 98
        Me.Label29.Text = "Informações e dados de afiliados do cliente"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtObsF)
        Me.GroupBox2.Controls.Add(Me.Label28)
        Me.GroupBox2.Controls.Add(Me.cmbUFfiliacao)
        Me.GroupBox2.Controls.Add(Me.Label26)
        Me.GroupBox2.Controls.Add(Me.txtCidadeF)
        Me.GroupBox2.Controls.Add(Me.Label25)
        Me.GroupBox2.Controls.Add(Me.txtBairroF)
        Me.GroupBox2.Controls.Add(Me.Label24)
        Me.GroupBox2.Controls.Add(Me.txtNumero)
        Me.GroupBox2.Controls.Add(Me.Label23)
        Me.GroupBox2.Controls.Add(Me.txtEnderecoFiliacao)
        Me.GroupBox2.Controls.Add(Me.Label22)
        Me.GroupBox2.Controls.Add(Me.Label21)
        Me.GroupBox2.Controls.Add(Me.cmbOcupacaoMae)
        Me.GroupBox2.Controls.Add(Me.cmbOcupacaoPai)
        Me.GroupBox2.Controls.Add(Me.Label20)
        Me.GroupBox2.Controls.Add(Me.txtMae)
        Me.GroupBox2.Controls.Add(Me.Label19)
        Me.GroupBox2.Controls.Add(Me.txtPai)
        Me.GroupBox2.Font = New System.Drawing.Font("Candara", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GroupBox2.Location = New System.Drawing.Point(9, 239)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1027, 150)
        Me.GroupBox2.TabIndex = 97
        Me.GroupBox2.TabStop = False
        '
        'txtObsF
        '
        Me.txtObsF.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObsF.BackColor = System.Drawing.Color.White
        Me.txtObsF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtObsF.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtObsF.Location = New System.Drawing.Point(613, 20)
        Me.txtObsF.Multiline = True
        Me.txtObsF.Name = "txtObsF"
        Me.txtObsF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObsF.Size = New System.Drawing.Size(408, 76)
        Me.txtObsF.TabIndex = 32
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label28.Location = New System.Drawing.Point(796, 103)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(45, 15)
        Me.Label28.TabIndex = 46
        Me.Label28.Text = "Estado"
        '
        'cmbUFfiliacao
        '
        Me.cmbUFfiliacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUFfiliacao.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbUFfiliacao.FormattingEnabled = True
        Me.cmbUFfiliacao.Items.AddRange(New Object() {"Acre" & Global.Microsoft.VisualBasic.ChrW(9), "Alagoas", "Amazonas" & Global.Microsoft.VisualBasic.ChrW(9), "Bahia" & Global.Microsoft.VisualBasic.ChrW(9), "Ceará" & Global.Microsoft.VisualBasic.ChrW(9), "Espírito Santo" & Global.Microsoft.VisualBasic.ChrW(9), "Goiás" & Global.Microsoft.VisualBasic.ChrW(9), "Maranhão", "Mato Grosso" & Global.Microsoft.VisualBasic.ChrW(9), "Mato Grosso do Sul" & Global.Microsoft.VisualBasic.ChrW(9), "Minas Gerais" & Global.Microsoft.VisualBasic.ChrW(9), "Pará" & Global.Microsoft.VisualBasic.ChrW(9), "Paraíba", "Paraná" & Global.Microsoft.VisualBasic.ChrW(9), "Pernambuco" & Global.Microsoft.VisualBasic.ChrW(9), "Piauí" & Global.Microsoft.VisualBasic.ChrW(9), "Rio de Janeiro" & Global.Microsoft.VisualBasic.ChrW(9), "Rio Grande do Norte", "Rio Grande do Sul" & Global.Microsoft.VisualBasic.ChrW(9), "Rondônia" & Global.Microsoft.VisualBasic.ChrW(9) & Global.Microsoft.VisualBasic.ChrW(9), "Roraima", "Santa Catarina" & Global.Microsoft.VisualBasic.ChrW(9), "São Paulo" & Global.Microsoft.VisualBasic.ChrW(9), "Sergipe" & Global.Microsoft.VisualBasic.ChrW(9), "Tocantins" & Global.Microsoft.VisualBasic.ChrW(9)})
        Me.cmbUFfiliacao.Location = New System.Drawing.Point(799, 119)
        Me.cmbUFfiliacao.Name = "cmbUFfiliacao"
        Me.cmbUFfiliacao.Size = New System.Drawing.Size(168, 23)
        Me.cmbUFfiliacao.TabIndex = 47
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label26.Location = New System.Drawing.Point(603, 101)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(54, 15)
        Me.Label26.TabIndex = 45
        Me.Label26.Text = "Cidade *"
        '
        'txtCidadeF
        '
        Me.txtCidadeF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCidadeF.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCidadeF.Location = New System.Drawing.Point(606, 119)
        Me.txtCidadeF.Name = "txtCidadeF"
        Me.txtCidadeF.Size = New System.Drawing.Size(187, 23)
        Me.txtCidadeF.TabIndex = 44
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.BackColor = System.Drawing.Color.Transparent
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label25.Location = New System.Drawing.Point(414, 101)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(48, 15)
        Me.Label25.TabIndex = 43
        Me.Label25.Text = "Bairro *"
        '
        'txtBairroF
        '
        Me.txtBairroF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBairroF.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBairroF.Location = New System.Drawing.Point(413, 119)
        Me.txtBairroF.Name = "txtBairroF"
        Me.txtBairroF.Size = New System.Drawing.Size(187, 23)
        Me.txtBairroF.TabIndex = 42
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.BackColor = System.Drawing.Color.Transparent
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label24.Location = New System.Drawing.Point(331, 101)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(60, 15)
        Me.Label24.TabIndex = 41
        Me.Label24.Text = "Numero *"
        '
        'txtNumero
        '
        Me.txtNumero.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNumero.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumero.Location = New System.Drawing.Point(331, 119)
        Me.txtNumero.Name = "txtNumero"
        Me.txtNumero.Size = New System.Drawing.Size(76, 23)
        Me.txtNumero.TabIndex = 40
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.Color.Transparent
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label23.Location = New System.Drawing.Point(9, 101)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(166, 15)
        Me.Label23.TabIndex = 39
        Me.Label23.Text = "Endereço de afiliação ( Rua )"
        '
        'txtEnderecoFiliacao
        '
        Me.txtEnderecoFiliacao.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEnderecoFiliacao.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEnderecoFiliacao.Location = New System.Drawing.Point(12, 119)
        Me.txtEnderecoFiliacao.Name = "txtEnderecoFiliacao"
        Me.txtEnderecoFiliacao.Size = New System.Drawing.Size(313, 23)
        Me.txtEnderecoFiliacao.TabIndex = 38
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.BackColor = System.Drawing.Color.Transparent
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label22.Location = New System.Drawing.Point(331, 55)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(116, 15)
        Me.Label22.TabIndex = 37
        Me.Label22.Text = "Ocupação da mãe *"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label21.Location = New System.Drawing.Point(331, 11)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(108, 15)
        Me.Label21.TabIndex = 36
        Me.Label21.Text = "Ocupação do pai *"
        '
        'cmbOcupacaoMae
        '
        Me.cmbOcupacaoMae.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOcupacaoMae.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOcupacaoMae.FormattingEnabled = True
        Me.cmbOcupacaoMae.Items.AddRange(New Object() {"Analista de sistema", "Tecnico em informatica", "Secretaria do lar", "Secretaria", "Médicos", "Médicos veterinários", "Advogados", "Jornalistas", "Contadores", "Corretores de imóveis", "Economistas", "Engenheiros (a)", "Arquitetos (a)", "Psicólogos (a)", "Dentistas (a)", "Professor (a)", "Pedreiro", "Montador de moveis", "Comerciante", "Autonomo", "Bombeiro", "Atendente", "Cantor", "Humorista", "Gari", "Jardineiro", "Juiz", "Nadados", "Operador de maquinas pesadas", "Quimico", "Radialista", "Sapateiro", "Superintendente", "Torneiro Mecânico", "Tratorista", "Ufólogo", "Vigia", "Vendedor", "Xerocador", "Zelador", "Eletricista", "Desenhista"})
        Me.cmbOcupacaoMae.Location = New System.Drawing.Point(331, 73)
        Me.cmbOcupacaoMae.Name = "cmbOcupacaoMae"
        Me.cmbOcupacaoMae.Size = New System.Drawing.Size(274, 23)
        Me.cmbOcupacaoMae.TabIndex = 35
        '
        'cmbOcupacaoPai
        '
        Me.cmbOcupacaoPai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOcupacaoPai.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOcupacaoPai.FormattingEnabled = True
        Me.cmbOcupacaoPai.Items.AddRange(New Object() {"Analista de sistema", "Tecnico em informatica", "Secretaria do lar", "Secretaria", "Médicos", "Médicos veterinários", "Advogados", "Jornalistas", "Contadores", "Corretores de imóveis", "Economistas", "Engenheiros (a)", "Arquitetos (a)", "Psicólogos (a)", "Dentistas (a)", "Professor (a)", "Pedreiro", "Montador de moveis", "Comerciante", "Autonomo", "Bombeiro", "Atendente", "Cantor", "Humorista", "Gari", "Jardineiro", "Juiz", "Nadados", "Operador de maquinas pesadas", "Quimico", "Radialista", "Sapateiro", "Superintendente", "Torneiro Mecânico", "Tratorista", "Ufólogo", "Vigia", "Vendedor", "Xerocador", "Zelador", "Eletricista", "Desenhista"})
        Me.cmbOcupacaoPai.Location = New System.Drawing.Point(331, 29)
        Me.cmbOcupacaoPai.Name = "cmbOcupacaoPai"
        Me.cmbOcupacaoPai.Size = New System.Drawing.Size(274, 23)
        Me.cmbOcupacaoPai.TabIndex = 34
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label20.Location = New System.Drawing.Point(9, 55)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(94, 15)
        Me.Label20.TabIndex = 33
        Me.Label20.Text = "Nome da mãe *"
        '
        'txtMae
        '
        Me.txtMae.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMae.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMae.Location = New System.Drawing.Point(12, 73)
        Me.txtMae.Name = "txtMae"
        Me.txtMae.Size = New System.Drawing.Size(313, 23)
        Me.txtMae.TabIndex = 32
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label19.Location = New System.Drawing.Point(9, 11)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(86, 15)
        Me.Label19.TabIndex = 31
        Me.Label19.Text = "Nome do pai *"
        '
        'txtPai
        '
        Me.txtPai.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPai.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPai.Location = New System.Drawing.Point(12, 29)
        Me.txtPai.Name = "txtPai"
        Me.txtPai.Size = New System.Drawing.Size(313, 23)
        Me.txtPai.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.dtpNascimento)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtEndereco)
        Me.GroupBox1.Controls.Add(Me.txtBairro)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtObs)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtCep)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtCidade)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtEmail)
        Me.GroupBox1.Controls.Add(Me.txtTelefone)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbUF)
        Me.GroupBox1.Controls.Add(Me.txtCelular)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.cmbSexo)
        Me.GroupBox1.Font = New System.Drawing.Font("Candara", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GroupBox1.Location = New System.Drawing.Point(8, 46)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(875, 165)
        Me.GroupBox1.TabIndex = 96
        Me.GroupBox1.TabStop = False
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label18.Location = New System.Drawing.Point(556, 5)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(56, 15)
        Me.Label18.TabIndex = 95
        Me.Label18.Text = "Tipo Doc"
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"CPF", "RG", "CNH"})
        Me.ComboBox1.Location = New System.Drawing.Point(559, 23)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(90, 23)
        Me.ComboBox1.TabIndex = 94
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label17.Location = New System.Drawing.Point(5, 5)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(46, 15)
        Me.Label17.TabIndex = 93
        Me.Label17.Text = "Codigo"
        '
        'imgCliente
        '
        Me.imgCliente.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.imgCliente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imgCliente.Image = CType(resources.GetObject("imgCliente.Image"), System.Drawing.Image)
        Me.imgCliente.Location = New System.Drawing.Point(897, 22)
        Me.imgCliente.Name = "imgCliente"
        Me.imgCliente.Size = New System.Drawing.Size(141, 165)
        Me.imgCliente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgCliente.TabIndex = 50
        Me.imgCliente.TabStop = False
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1047, 394)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Dados financeiros e estatisticas do cliente"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel2.Controls.Add(Me.btnAcoes)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 522)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1068, 31)
        Me.Panel2.TabIndex = 58
        '
        'btnAcoes
        '
        Me.btnAcoes.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnAcoes.FlatAppearance.BorderSize = 0
        Me.btnAcoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAcoes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAcoes.ForeColor = System.Drawing.Color.White
        Me.btnAcoes.Image = CType(resources.GetObject("btnAcoes.Image"), System.Drawing.Image)
        Me.btnAcoes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAcoes.Location = New System.Drawing.Point(997, 0)
        Me.btnAcoes.Name = "btnAcoes"
        Me.btnAcoes.Size = New System.Drawing.Size(71, 31)
        Me.btnAcoes.TabIndex = 109
        Me.btnAcoes.Text = "Ações"
        Me.btnAcoes.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAcoes.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'frmClientes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1068, 553)
        Me.ControlBox = False
        Me.Controls.Add(Me.menuNavegacao)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel3)
        Me.Font = New System.Drawing.Font("Candara", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        CType(Me.menuNavegacao, System.ComponentModel.ISupportInitialize).EndInit()
        Me.menuNavegacao.ResumeLayout(False)
        Me.menuNavegacao.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.imgCliente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents menuNavegacao As System.Windows.Forms.BindingNavigator
    Private WithEvents btnCadastrarProdutos As System.Windows.Forms.ToolStripButton
    Private WithEvents btnGravarAlterar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnFechar As System.Windows.Forms.ToolStripButton
    Friend WithEvents cmbUF As System.Windows.Forms.ComboBox
    Friend WithEvents txtTelefone As System.Windows.Forms.TextBox
    Friend WithEvents txtEndereco As System.Windows.Forms.TextBox
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNome As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents txtDocumento As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtBairro As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtCep As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCelular As System.Windows.Forms.TextBox
    Friend WithEvents cmbSexo As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dtpNascimento As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dtpCadastro As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtCidade As System.Windows.Forms.TextBox
    Friend WithEvents txtObs As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtApelido As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnExcluir As System.Windows.Forms.ToolStripButton
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents imgCliente As System.Windows.Forms.PictureBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtPai As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtMae As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtObsF As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents cmbUFfiliacao As System.Windows.Forms.ComboBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtCidadeF As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtBairroF As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtEnderecoFiliacao As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cmbOcupacaoMae As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOcupacaoPai As System.Windows.Forms.ComboBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblFechar As ns1.BunifuCustomLabel
    Friend WithEvents lblImage As System.Windows.Forms.LinkLabel
    Friend WithEvents btnAcoes As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
End Class
