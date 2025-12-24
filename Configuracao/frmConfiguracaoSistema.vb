Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Drawing.Text
Imports System.IO
Imports System.Net.Http
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading.Tasks
Imports iTextSharp.text.log
Imports Newtonsoft.Json
Imports System.Data.SQLite
Imports System.Transactions

Public Class frmConfiguracaoSistema

    Private Sub frmConfiguracaoSistema_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbSmtp.SelectedIndex = -1
        txtMessage.Text = My.Settings.strEmail
        txtSenha.Text = My.Settings.strSenhaEmail
        cmbSmtp.SelectedValue = My.Settings.strSmtp
        lblImpressorasPadrao.Text = GetImpressoraPadrao()
        GetImpressoras()
        txtCaminhoDados.Text = My.Settings.ConexaoBanco
        CarregarDadosEmpresa()

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs)
        frmMetroPrincipal.AbrirFormEnPanel(New frmDhastebord())
    End Sub

    Private Sub CarregarDadosEmpresa()
        Dim dr As OleDbDataReader = Nothing
        Using con As OleDbConnection = GetConnection()
            Try

                con.Open()
                Dim sql As String = "SELECT ID, Razao, Fantasia, CNPJ, IE, IM, Rua, Numero, Bairro, Cidade, Estado,"
                sql += "Telefone, Cep, Celular, Email, nfce_ambiente, nfe_certificado, nfe_senha_certificado, nfe_serie, nfe_nu_serie, nfe_diretorio, nfe_tentativas, nfe_timeout, nfe_regime, nfe_Schemas, nfe_IDCSC, nfe_Codigo_CSC, nfce_Fiscal, nfce_cfop FROM tbl_empresa"
                Dim cmd As New OleDbCommand(sql, con)
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
                If dr.HasRows Then
                    dr.Read()
                    txtID.Text = dr.Item("ID")
                    txtRazao.Text = dr.Item("Razao")
                    txtFantasia.Text = dr.Item("Fantasia")
                    txtCNPJ.Text = dr.Item("CNPJ")
                    txtIE.Text = dr.Item("IE")
                    txtIM.Text = dr.Item("IM")
                    txtEndereco.Text = dr.Item("Rua")
                    txtNumero.Text = dr.Item("Numero")
                    txtBairro.Text = dr.Item("Bairro")
                    txtCidade.Text = dr.Item("Cidade")
                    cboEstado.Text = dr.Item("Estado")
                    txtFone.Text = dr.Item("Telefone")
                    txtCep.Text = dr.Item("Cep")
                    txtCelurar.Text = dr.Item("Celular")
                    txtEmail.Text = dr.Item("Email")
                    cmbAmbienteNFCe.Text = dr.Item("nfce_ambiente")
                    ' Verificar se é certificado Base64 antigo e limpar
                    Dim certificadoValue As String = If(dr.Item("nfe_certificado")?.ToString(), "")
                    If certificadoValue.StartsWith("MII") AndAlso certificadoValue.Length > 100 Then
                        ' É um certificado Base64 antigo - limpar
                        LimparCertificadoBase64Antigo()
                        txtCaminhoCertificado.Text = ""
                        txtSenhaCertificado.Text = ""
                        MsgBox("Certificado Base64 antigo detectado e removido. Por favor, configure novamente o certificado usando o caminho do arquivo.", MsgBoxStyle.Information, "Limpeza de Dados")
                    Else
                        txtCaminhoCertificado.Text = certificadoValue
                        txtSenhaCertificado.Text = dr.Item("nfe_senha_certificado")
                    End If
                    numSerieNFCe.Text = dr.Item("nfe_serie")
                    numProximoNumeroNFCe.Text = dr.Item("nfe_nu_serie")
                    txtTimeOut.Text = dr.Item("nfe_timeout")
                    txtTentativas.Text = dr.Item("nfe_tentativas")
                    cboRegime.Text = dr.Item("nfe_regime")
                    txtCaminhoSchemas.Text = dr.Item("nfe_Schemas")
                    txtIdCSCToen.Text = dr.Item("nfe_IDCSC")
                    txtCodigoCSC.Text = dr.Item("nfe_Codigo_CSC")
                    
                    ' Carregar valor do campo nfce_Fiscal no checkbox
                    If Not IsDBNull(dr.Item("nfce_Fiscal")) Then
                        chkHabilitarNFCe.Checked = CBool(dr.Item("nfce_Fiscal"))
                    Else
                        chkHabilitarNFCe.Checked = False
                    End If
                    cboCFOP.Text = dr.Item("nfce_cfop")
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                dr.Close()
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If Me.OpenFileDialog1.ShowDialog = 1 Then
            Me.imgLogoPDV.Image = System.Drawing.Image.FromFile(Me.OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub GravarDados()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                ' Logo PDV
                Dim arrLogoPDV() As Byte = Nothing
                If imgLogoPDV.Image IsNot Nothing Then
                    Using msLogo As New IO.MemoryStream()
                        imgLogoPDV.Image.Save(msLogo, imgLogoPDV.Image.RawFormat)
                        arrLogoPDV = msLogo.ToArray()
                    End Using
                End If

                ' Logo Cupom
                Dim arrLogoCupom() As Byte = Nothing
                If imgCupom.Image IsNot Nothing Then
                    Using msCupom As New IO.MemoryStream()
                        imgCupom.Image.Save(msCupom, imgCupom.Image.RawFormat)
                        arrLogoCupom = msCupom.ToArray()
                    End Using
                End If

                ' Certificado em Base64
                Dim certificadoBytes As Byte() = File.ReadAllBytes(txtCaminhoCertificado.Text)
                Dim certificadoBase64 As String = Convert.ToBase64String(certificadoBytes)

                ' Comando SQL
                Dim sql As String = "INSERT INTO tbl_empresa (" &
                "Razao, Fantasia, CNPJ, IE, IM, Rua, Numero, Bairro, Cidade, Estado, Cep, " &
                "Telefone, Celular, Email, nfce_ambiente, nfe_certificado, nfe_senha_certificado, " &
                "nfe_serie, nfe_nu_serie, nfe_tentativas, nfe_timeout, nfe_regime, " &
                "Logo, Logo_Cupom, nfe_Schemas, nfe_IDCSC, nfe_Codigo_CSC, nfce_Fiscal, nfce_cfop) " &
                "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

                Dim cmd As New OleDbCommand(sql, con)

                ' Parâmetros
                cmd.Parameters.Add(New OleDbParameter("Razao", txtRazao.Text))
                cmd.Parameters.Add(New OleDbParameter("Fantasia", txtFantasia.Text))
                cmd.Parameters.Add(New OleDbParameter("CNPJ", txtCNPJ.Text))
                cmd.Parameters.Add(New OleDbParameter("IE", txtIE.Text))
                cmd.Parameters.Add(New OleDbParameter("IM", txtIM.Text))
                cmd.Parameters.Add(New OleDbParameter("Rua", txtEndereco.Text))
                cmd.Parameters.Add(New OleDbParameter("Numero", txtNumero.Text))
                cmd.Parameters.Add(New OleDbParameter("Bairro", txtBairro.Text))
                cmd.Parameters.Add(New OleDbParameter("Cidade", txtCidade.Text))
                cmd.Parameters.Add(New OleDbParameter("Estado", cboEstado.Text))
                cmd.Parameters.Add(New OleDbParameter("Cep", txtCep.Text))
                cmd.Parameters.Add(New OleDbParameter("Telefone", txtFone.Text))
                cmd.Parameters.Add(New OleDbParameter("Celular", txtCelurar.Text))
                cmd.Parameters.Add(New OleDbParameter("Email", txtEmail.Text))
                cmd.Parameters.Add(New OleDbParameter("nfce_ambiente", cmbAmbienteNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_certificado", txtCaminhoCertificado.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_senha_certificado", txtSenhaCertificado.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_serie", numSerieNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_nu_serie", numProximoNumeroNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_tentativas", txtTentativas.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_timeout", txtTimeOut.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_regime", cboRegime.Text))
                cmd.Parameters.Add(New OleDbParameter("Logo", arrLogoPDV))
                cmd.Parameters.Add(New OleDbParameter("Logo_Cupom", arrLogoCupom))
                cmd.Parameters.Add(New OleDbParameter("nfe_Schemas", txtCaminhoSchemas.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_IDCSC", txtIdCSCToen.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_Codigo_CSC", txtCodigoCSC.Text))
                cmd.Parameters.Add(New OleDbParameter("nfce_Fiscal", chkHabilitarNFCe.Checked))
                cmd.Parameters.Add(New OleDbParameter("nfce_cfop", cboCFOP.Text))

                ' Executar
                cmd.ExecuteNonQuery()
                MsgBox("Configuração foi efetuada com sucesso", MsgBoxStyle.Information, strEmpresaParceiro)

            Catch ex As Exception
                MsgBox("Erro ao gravar dados: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Private Sub AtualizarDados()
        Using con As OleDbConnection = GetConnection()
            Dim transaction As OleDbTransaction = Nothing
            Try
                con.Open()
                transaction = con.BeginTransaction()

                ' Validação de campos obrigatórios
                If String.IsNullOrEmpty(txtRazao.Text) Then
                    MsgBox("Razão Social é obrigatória", MsgBoxStyle.Exclamation, "Validação")
                    txtRazao.Focus()
                    Return
                End If

                If String.IsNullOrEmpty(txtCNPJ.Text) Then
                    MsgBox("CNPJ é obrigatório", MsgBoxStyle.Exclamation, "Validação")
                    txtCNPJ.Focus()
                    Return
                End If

                ' Logo PDV
                Dim arrLogoPDV() As Byte = Nothing
                If imgLogoPDV.Image IsNot Nothing Then
                    Using msLogo As New IO.MemoryStream()
                        imgLogoPDV.Image.Save(msLogo, imgLogoPDV.Image.RawFormat)
                        arrLogoPDV = msLogo.ToArray()
                    End Using
                End If

                ' Logo Cupom
                Dim arrLogoCupom() As Byte = Nothing
                If imgCupom.Image IsNot Nothing Then
                    Using msLogoCupom As New IO.MemoryStream()
                        imgCupom.Image.Save(msLogoCupom, imgCupom.Image.RawFormat)
                        arrLogoCupom = msLogoCupom.ToArray()
                    End Using
                End If

                ' Caminho do Certificado (não converter para Base64 aqui)
                Dim caminhoCertificado As String = ""
                If Not String.IsNullOrEmpty(txtCaminhoCertificado.Text) Then
                    If File.Exists(txtCaminhoCertificado.Text) Then
                        caminhoCertificado = txtCaminhoCertificado.Text
                    Else
                        MsgBox($"Arquivo de certificado não encontrado: {txtCaminhoCertificado.Text}", MsgBoxStyle.Exclamation, "Validação")
                        txtCaminhoCertificado.Focus()
                        Return
                    End If
                End If

                ' Gerar nome de arquivo XML curto e seguro
                Dim nomeEmpresa = txtRazao.Text
                Dim nomeSanitizado = nomeEmpresa.Replace(".", "").Replace(" ", "_").Replace("/", "_")
                Dim cnpj = txtCNPJ.Text
                Dim nNF = numProximoNumeroNFCe.Text
                Dim nomeArquivoXml = $"{nomeSanitizado}_{cnpj}_{nNF}.xml"

                ' Validar comprimento do nome
                If nomeArquivoXml.Length > 100 Then
                    nomeArquivoXml = $"{cnpj}_{nNF}.xml"
                End If


                ' Comando SQL
                Dim sql As String = "UPDATE tbl_empresa SET " &
                "Razao=?, Fantasia=?, CNPJ=?, IE=?, IM=?, Rua=?, Numero=?, Bairro=?, Cidade=?, Estado=?, Cep=?," &
                "Telefone=?, Celular=?, Email=?, nfce_ambiente=?, nfe_certificado=?, nfe_senha_certificado=?," &
                "nfe_serie=?, nfe_nu_serie=?, nfe_timeout=?, nfe_tentativas=?, nfe_regime=?, Logo=?, Logo_Cupom=?," &
                "nfe_Schemas=?, nfe_IDCSC=?, nfe_Codigo_CSC=?, nfce_Fiscal=?, nfce_cfop=?" &
                "WHERE ID=" & txtID.Text

                Dim cmd As New OleDbCommand(sql, con, transaction)

                ' Parâmetros
                cmd.Parameters.Add(New OleDbParameter("Razao", txtRazao.Text))
                cmd.Parameters.Add(New OleDbParameter("Fantasia", txtFantasia.Text))
                cmd.Parameters.Add(New OleDbParameter("CNPJ", txtCNPJ.Text))
                cmd.Parameters.Add(New OleDbParameter("IE", txtIE.Text))
                cmd.Parameters.Add(New OleDbParameter("IM", txtIM.Text))
                cmd.Parameters.Add(New OleDbParameter("Rua", txtEndereco.Text))
                cmd.Parameters.Add(New OleDbParameter("Numero", txtNumero.Text))
                cmd.Parameters.Add(New OleDbParameter("Bairro", txtBairro.Text))
                cmd.Parameters.Add(New OleDbParameter("Cidade", txtCidade.Text))
                cmd.Parameters.Add(New OleDbParameter("Estado", cboEstado.Text))
                cmd.Parameters.Add(New OleDbParameter("Cep", txtCep.Text))
                cmd.Parameters.Add(New OleDbParameter("Telefone", txtFone.Text))
                cmd.Parameters.Add(New OleDbParameter("Celular", txtCelurar.Text))
                cmd.Parameters.Add(New OleDbParameter("Email", txtEmail.Text))
                cmd.Parameters.Add(New OleDbParameter("nfce_ambiente", cmbAmbienteNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_certificado", txtCaminhoCertificado.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_senha_certificado", txtSenhaCertificado.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_serie", numSerieNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_nu_serie", numProximoNumeroNFCe.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_timeout", txtTimeOut.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_tentativas", txtTentativas.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_regime", cboRegime.Text))
                cmd.Parameters.Add(New OleDbParameter("Logo", arrLogoPDV))
                cmd.Parameters.Add(New OleDbParameter("Logo_Cupom", arrLogoCupom))
                cmd.Parameters.Add(New OleDbParameter("nfe_Schemas", txtCaminhoSchemas.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_IDCSC", txtIdCSCToen.Text))
                cmd.Parameters.Add(New OleDbParameter("nfe_Codigo_CSC", txtCodigoCSC.Text))
                cmd.Parameters.Add(New OleDbParameter("nfce_Fiscal", chkHabilitarNFCe.Checked))
                cmd.Parameters.Add(New OleDbParameter("nfce_cfop", cboCFOP.Text))

                ' Executar
                cmd.ExecuteNonQuery()
                transaction.Commit()
                
                ' Atualizar variável global e configuração do ACBr
                strEmitente_nfce_ambiente = cmbAmbienteNFCe.Text
                ConfiguradorACBr.AtualizarConfiguracaoAmbiente()
                
                MsgBox("Configuração atualizada com sucesso", MsgBoxStyle.Information, "Sucesso")

            Catch ex As Exception
                If transaction IsNot Nothing Then
                    transaction.Rollback()
                End If
                MsgBox("Erro ao atualizar dados: " & ex.Message, MsgBoxStyle.Critical, "Erro")
            Finally
                If transaction IsNot Nothing Then
                    transaction.Dispose()
                End If
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked

        If Me.OpenFileDialog1.ShowDialog = 1 Then
            Me.imgCupom.Image = System.Drawing.Image.FromFile(Me.OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub btnConfigurarDados_Click(sender As Object, e As EventArgs) Handles btnConfigurarDados.Click
        intPergunta = MsgBox("Tem certeza que gostaria de alterar o caminho do banco de dados ? " &
                                  " sertifique-se que sim pois a alteração pode ocasionar em problemas na Rede.",
                                MsgBoxStyle.Question + MsgBoxStyle.YesNo, strEmpresaParceiro)
        If intPergunta = vbNo Then Exit Sub

        My.Settings.ConexaoBanco = ""
        My.Settings.Save()
        My.Settings.Reload()

        frmRede.ShowDialog()
    End Sub

    Private Sub GetImpressoras()
        For Each Ímpressoras As String In PrinterSettings.InstalledPrinters
            '  ListImpressoras.Items.Add(Ímpressoras)
        Next
    End Sub

    Private Function GetImpressoraPadrao() As String
        Dim Settings = New PrinterSettings
        Return Settings.PrinterName
        strImpressora = Settings.PrinterName

    End Function

    Private Sub btnDeletProdutos_Click(sender As Object, e As EventArgs) Handles btnDeletProdutos.Click
        intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente a tabela de produto ", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
        If intPergunta <> vbYes Then Exit Sub
        PExcluiProdutos()
    End Sub
    Private Sub PExcluiProdutos()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_estoque "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de produto foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o produto.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnDeleteCliente_Click(sender As Object, e As EventArgs) Handles btnDeleteCliente.Click
        intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente a tabela de clientes ", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
        If intPergunta <> vbYes Then Exit Sub
        PExcluiClientes()
    End Sub
    Private Sub PExcluiClientes()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_clientes "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de cliente foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o produto.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnDeleteContas_Click(sender As Object, e As EventArgs) Handles btnDeleteContas.Click
        intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente a tabela de contas a receber ", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
        If intPergunta <> vbYes Then Exit Sub
        PExcluiContasReceber()
    End Sub
    Private Sub PExcluiContasReceber()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_contas_receber "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de contas a receber foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o produto.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnDeleteVendidos_Click(sender As Object, e As EventArgs) Handles btnDeleteVendidos.Click
        intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente a tabela de produtos vendidos ", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
        If intPergunta <> vbYes Then Exit Sub
        PExcluiProdutosVendidos()
    End Sub
    Private Sub PExcluiProdutosVendidos()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_vendas "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de produtos vendidos foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o produto.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnDeleteFuncionarios_Click(sender As Object, e As EventArgs) Handles btnDeleteFuncionarios.Click
        intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente a tabela de Funcionarios Cadastrados ", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
        If intPergunta <> vbYes Then Exit Sub
        PExcluiFuncionarios()
    End Sub
    Private Sub PExcluiFuncionarios()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_funcionarios "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de Funcionarios foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o produto.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    Private Sub btnExecultar_Click(sender As Object, e As EventArgs) Handles btnExecultar.Click
        If txtSenhaRestrita.Text = "14023600" Then
            pSenha.Visible = False
        Else
            MsgBox("A senha digitada é invalida, informe ao adimistrador do sistema", MsgBoxStyle.Exclamation, strEmpresaParceiro)
            txtSenhaRestrita.Text = ""
            txtSenhaRestrita.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub btnContasPagar_Click(sender As Object, e As EventArgs) Handles btnContasPagar.Click
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_contas_pagar "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de contas a pagar foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir as contas a pagar.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnVendas_Click(sender As Object, e As EventArgs) Handles btnVendas.Click
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_vendas "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de vendas foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir as vendas.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnMovimentos_Click(sender As Object, e As EventArgs) Handles btnMovimentos.Click
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_Movimento_Estoque "
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("a tabela de movimentos de estoque foi excluído com sucesso.", MsgBoxStyle.Information, strEmpresaParceiro)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir os movimentos de estoque.", MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
    End Sub
    'Função para fazer login


    Public Class Usuario
        Public Property id As Integer
        Public Property nome As String
        Public Property email As String
    End Class

    ' Valida os dados antes de salvar
    Private Function ValidarDados() As Boolean
        Dim erros As New List(Of String)

        If String.IsNullOrEmpty(txtCNPJ.Text) Then
            erros.Add("CNPJ é obrigatório")
        End If

        If String.IsNullOrEmpty(txtRazao.Text) Then
            erros.Add("Razão Social é obrigatória")
        End If

        If String.IsNullOrEmpty(txtCaminhoCertificado.Text) Then
            erros.Add("Certificado digital é obrigatório")
        ElseIf Not File.Exists(txtCaminhoCertificado.Text) Then
            erros.Add("Arquivo de certificado não encontrado")
        End If

        If erros.Count > 0 Then
            MessageBox.Show("Corrija os seguintes erros:" & vbCrLf & vbCrLf &
                          String.Join(vbCrLf, erros.Select(Function(e) "• " + e)),
                          "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function
    Private Sub AtualizarStatus(mensagem As String)
        lblStatus.Text = mensagem
        Application.DoEvents()
    End Sub

    Private Sub BtnProcurarCertificado_Click(sender As Object, e As EventArgs) Handles btnProcurarCertificado.Click
        ' Configura o OpenFileDialog
        OpenFileDialog1.Filter = "Certificados Digitais (*.pfx, *.p12)|*.pfx;*.p12|Todos os arquivos (*.*)|*.*"
        OpenFileDialog1.Title = "Selecionar Certificado Digital"

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Carrega o caminho na primeira TextBox
            txtCaminhoCertificado.Text = OpenFileDialog1.FileName

            ' Solicita a senha e carrega na segunda TextBox
            Dim senha As String = InputBox("Digite a senha do certificado:", "Senha do Certificado", "")
            txtSenhaCertificado.Text = senha

            ' Opcional: Validar o certificado
            If Not String.IsNullOrEmpty(senha) Then
                If ValidarCertificado(OpenFileDialog1.FileName, senha) Then
                    MessageBox.Show("Certificado válido e carregado com sucesso!")
                Else
                    MessageBox.Show("Certificado inválido ou senha incorreta!")
                End If
            End If
        End If

        Try
            If String.IsNullOrEmpty(txtCaminhoCertificado.Text) OrElse
               String.IsNullOrEmpty(txtSenhaCertificado.Text) Then
                MessageBox.Show("Informe o caminho e senha do certificado!", "Aviso",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim cert = New X509Certificate2(txtCaminhoCertificado.Text, txtSenhaCertificado.Text)
            ExibirInformacoesCertificado(cert)
            MessageBox.Show("Certificado testado com sucesso!", "Sucesso",
                          MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ExibirInformacoesCertificado(Nothing, ex.Message)
            MessageBox.Show($"Erro ao testar certificado: {ex.Message}", "Erro",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    ' Função para validar o certificado
    Private Function ValidarCertificado(caminho As String, senha As String) As Boolean
        Try
            Using certificado As New X509Certificate2(caminho, senha)
                ' Se chegou aqui, o certificado é válido
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub ExibirInformacoesCertificado(cert As X509Certificate2, Optional erro As String = "")
        If cert IsNot Nothing Then
            lblCertInfo.Text = $"✅ CERTIFICADO VÁLIDO" & vbCrLf &
                             $"Proprietário: {cert.SubjectName.Name}" & vbCrLf &
                             $"Emissor: {cert.IssuerName.Name}" & vbCrLf &
                             $"Válido até: {cert.NotAfter:dd/MM/yyyy}" & vbCrLf &
                             $"Chave Privada: {If(cert.HasPrivateKey, "Sim", "Não")}"
            lblCertInfo.ForeColor = Color.Green
        Else
            lblCertInfo.Text = $"❌ ERRO NO CERTIFICADO" & vbCrLf & erro
            lblCertInfo.ForeColor = Color.Red
        End If
    End Sub

    ''' <summary>
    ''' Remove certificados Base64 antigos do banco de dados
    ''' </summary>
    Private Sub LimparCertificadoBase64Antigo()
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()

                ' Limpar certificado Base64 e senha
                Dim sql As String = "UPDATE tbl_empresa SET nfe_certificado = '', nfe_senha_certificado = '' WHERE nfe_certificado LIKE 'MII%'"
                Using cmd As New OleDbCommand(sql, con)
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    Console.WriteLine($"Certificados Base64 removidos: {rowsAffected} registros")
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao limpar certificado Base64: {ex.Message}")
        End Try
    End Sub

    Private Sub btnGravarDados_Click(sender As Object, e As EventArgs) Handles btnGravarDados.Click
        If txtID.Text = "" Then
            GravarDados()
        Else
            AtualizarDados()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub txtCNPJ_TextChanged(sender As Object, e As EventArgs) Handles txtCNPJ.TextChanged
        If bloqueandoTexto Then Exit Sub

        bloqueandoTexto = True
        Dim posicao = txtCNPJ.SelectionStart
        txtCNPJ.Text = AplicarMascaraCNPJ(txtCNPJ.Text)
        txtCNPJ.SelectionStart = Math.Min(posicao, txtCNPJ.Text.Length)
        bloqueandoTexto = False
    End Sub

    Private Sub txtCNPJ_LostFocus(sender As Object, e As EventArgs) Handles txtCNPJ.LostFocus
        txtCNPJ.Text = ValidarCNPJ(txtCNPJ.Text)

    End Sub
    Private bloqueandoTexto As Boolean = False
    Private Function AplicarMascaraCNPJ(texto As String) As String
        Dim numeros = LimparNumeros(texto)

        If numeros.Length > 14 Then
            numeros = numeros.Substring(0, 14)
        End If

        Select Case numeros.Length
            Case <= 2
                Return numeros
            Case <= 5
                Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2)}"
            Case <= 8
                Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2, 3)}.{numeros.Substring(5)}"
            Case <= 12
                Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2, 3)}.{numeros.Substring(5, 3)}/{numeros.Substring(8)}"
            Case Else
                Return $"{numeros.Substring(0, 2)}.{numeros.Substring(2, 3)}.{numeros.Substring(5, 3)}/{numeros.Substring(8, 4)}-{numeros.Substring(12)}"
        End Select
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        frmMenuNFe.ShowDialog()
    End Sub

    Private Sub btnSchemas_Click(sender As Object, e As EventArgs) Handles btnSchemas.Click
        Dim dialog As New FolderBrowserDialog()
        If dialog.ShowDialog() = DialogResult.OK Then
            txtCaminhoSchemas.Text = dialog.SelectedPath
        End If

    End Sub
End Class