Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class frmClientes
    Dim classClientes As New clsClientes
    Private Sub PGrava()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim arrImage() As Byte
                Dim strImage As String
                Dim myMs As New IO.MemoryStream

                If Not IsNothing(Me.imgCliente.Image) Then
                    Me.imgCliente.Image.Save(myMs, Me.imgCliente.Image.RawFormat)
                    arrImage = myMs.GetBuffer
                    strImage = "?"
                Else
                    arrImage = Nothing
                    strImage = "NULL"
                End If

                Dim sql As String
                sql = "INSERT INTO tbl_clientes (nome,apelido,Documento,Endereco,Bairro,Cep,cidade,"
                sql += "Estado,Telefone,Celular,Sexo,data_nascimento,Email,Obs,data_cadastro,AfiliacaoPai,AfiliacaoMae,EnderecoF,"
                sql += "NumeroF,BairroF,CidadeF,EstadoF,ObsF,ProfPai,ProfMae,[Foto] )"
                sql += "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
                Dim cmd As New OleDbCommand(sql, con)
                cmd.Parameters.Add(New OleDbParameter("nome", txtNome.Text))
                cmd.Parameters.Add(New OleDbParameter("apelido", txtApelido.Text))
                cmd.Parameters.Add(New OleDbParameter("Documento", txtDocumento.Text))
                cmd.Parameters.Add(New OleDbParameter("Endereco", txtEndereco.Text))
                cmd.Parameters.Add(New OleDbParameter("Bairro", txtBairro.Text))
                cmd.Parameters.Add(New OleDbParameter("Cep", txtCep.Text))
                cmd.Parameters.Add(New OleDbParameter("cidade", txtCidade.Text))
                cmd.Parameters.Add(New OleDbParameter("Estado", cmbUF.Text))
                cmd.Parameters.Add(New OleDbParameter("Telefone", txtTelefone.Text))
                cmd.Parameters.Add(New OleDbParameter("Celular", txtCelular.Text))
                cmd.Parameters.Add(New OleDbParameter("Sexo", cmbSexo.Text))
                cmd.Parameters.Add(New OleDbParameter("data_nascimento", dtpNascimento.Text))
                cmd.Parameters.Add(New OleDbParameter("Email", txtEmail.Text))
                cmd.Parameters.Add(New OleDbParameter("Obs", txtObs.Text))
                cmd.Parameters.Add(New OleDbParameter("data_cadastro", dtpCadastro.Text))
                cmd.Parameters.Add(New OleDbParameter("AfiliacaoPai", txtPai.Text))
                cmd.Parameters.Add(New OleDbParameter("AfiliacaoMae", txtMae.Text))
                cmd.Parameters.Add(New OleDbParameter("EnderecoF", txtEnderecoFiliacao.Text))
                cmd.Parameters.Add(New OleDbParameter("NumeroF", txtNumero.Text))
                cmd.Parameters.Add(New OleDbParameter("BairroF", txtBairroF.Text))
                cmd.Parameters.Add(New OleDbParameter("CidadeF", txtCidadeF.Text))
                cmd.Parameters.Add(New OleDbParameter("EstadoF", cmbUFfiliacao.Text))
                cmd.Parameters.Add(New OleDbParameter("ObsF", txtObsF.Text))
                cmd.Parameters.Add(New OleDbParameter("ProfPai", cmbOcupacaoPai.Text))
                cmd.Parameters.Add(New OleDbParameter("ProfMae", cmbOcupacaoMae.Text))
                cmd.Parameters.Add(New OleDbParameter("Foto", arrImage))

                If strImage = "?" Then
                    cmd.Parameters.Add(strImage, OleDb.OleDbType.Binary).Value = arrImage
                End If

                cmd.ExecuteNonQuery()
                MsgBox("O cliente " & txtNome.Text & " foi adicionado com sucesso ", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub PAlteraDadosCliente()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()

                Dim arrImage() As Byte
                Dim strImage As String
                Dim myMs As New IO.MemoryStream

                If Not IsNothing(Me.imgCliente.Image) Then
                    Me.imgCliente.Image.Save(myMs, Me.imgCliente.Image.RawFormat)
                    arrImage = myMs.GetBuffer
                    strImage = "?"
                Else
                    arrImage = Nothing
                    strImage = "NULL"
                End If

                Dim sql As String
                sql = "UPDATE tbl_clientes SET nome=?,apelido=?,Documento=?,Endereco=?,Bairro=?,Cep=?,cidade=?,"
                sql += "Estado=?,Telefone=?,Celular=?,Sexo=?,data_nascimento=?,Email=?,Obs=?,data_cadastro=?, "
                sql += "AfiliacaoPai=?,AfiliacaoMae=?,EnderecoF=?,NumeroF=?,BairroF=?,CidadeF=?,EstadoF=?,ObsF=?,ProfPai=?,ProfMae=?,Foto=? "
                sql += "WHERE id_cliente = " & txtCodigo.Text
                Dim cmd As New OleDbCommand(sql, con)
                cmd.Parameters.Add(New OleDbParameter("nome", txtNome.Text))
                cmd.Parameters.Add(New OleDbParameter("apelido", txtApelido.Text))
                cmd.Parameters.Add(New OleDbParameter("Documento", txtDocumento.Text))
                cmd.Parameters.Add(New OleDbParameter("Endereco", txtEndereco.Text))
                cmd.Parameters.Add(New OleDbParameter("Bairro", txtBairro.Text))
                cmd.Parameters.Add(New OleDbParameter("Cep", txtCep.Text))
                cmd.Parameters.Add(New OleDbParameter("cidade", txtCidade.Text))
                cmd.Parameters.Add(New OleDbParameter("Estado", cmbUF.Text))
                cmd.Parameters.Add(New OleDbParameter("Telefone", txtTelefone.Text))
                cmd.Parameters.Add(New OleDbParameter("Celular", txtCelular.Text))
                cmd.Parameters.Add(New OleDbParameter("Sexo", cmbSexo.Text))
                cmd.Parameters.Add(New OleDbParameter("data_nascimento", dtpNascimento.Text))
                cmd.Parameters.Add(New OleDbParameter("Email", txtEmail.Text))
                cmd.Parameters.Add(New OleDbParameter("Obs", txtObs.Text))
                cmd.Parameters.Add(New OleDbParameter("data_cadastro", dtpCadastro.Text))
                cmd.Parameters.Add(New OleDbParameter("AfiliacaoPai", txtPai.Text))
                cmd.Parameters.Add(New OleDbParameter("AfiliacaoMae", txtMae.Text))
                cmd.Parameters.Add(New OleDbParameter("EnderecoF", txtEnderecoFiliacao.Text))
                cmd.Parameters.Add(New OleDbParameter("NumeroF", txtNumero.Text))
                cmd.Parameters.Add(New OleDbParameter("BairroF", txtBairroF.Text))
                cmd.Parameters.Add(New OleDbParameter("CidadeF", txtCidadeF.Text))
                cmd.Parameters.Add(New OleDbParameter("EstadoF", cmbUFfiliacao.Text))
                cmd.Parameters.Add(New OleDbParameter("ObsF", txtObsF.Text))
                cmd.Parameters.Add(New OleDbParameter("ProfPai", cmbOcupacaoPai.Text))
                cmd.Parameters.Add(New OleDbParameter("ProfMae", cmbOcupacaoMae.Text))
                cmd.Parameters.Add(New OleDbParameter("Foto", arrImage))

                If strImage = "?" Then
                    cmd.Parameters.Add(strImage, OleDb.OleDbType.Binary).Value = arrImage
                End If

                cmd.ExecuteNonQuery()
                MsgBox("O cliente " & txtNome.Text & " foi alterado com sucesso ", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub PExclui(ByVal id_cliente As Integer)
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "DELETE FROM tbl_clientes WHERE id_cliente = " & id_cliente
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()
                MsgBox("Cliente " & txtNome.Text & " foi excluído com sucesso.", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub PLimpaCampos()
        txtCodigo.Text = "Novo"
        txtNome.Text = ""
        txtApelido.Text = ""
        txtDocumento.Text = ""
        txtEndereco.Text = ""
        txtBairro.Text = ""
        txtCep.Text = ""
        txtCidade.Text = ""
        cmbUF.Text = ""
        txtTelefone.Text = ""
        txtCelular.Text = ""
        cmbSexo.SelectedIndex = 0
        dtpNascimento.Value = Date.Today
        txtEmail.Text = ""
        txtObs.Text = ""
        dtpCadastro.Value = Date.Today
        txtPai.Text = ""
        txtMae.Text = ""
        txtEnderecoFiliacao.Text = ""
        txtNumero.Text = ""
        txtBairroF.Text = ""
        txtCidadeF.Text = ""
        cmbUFfiliacao.Text = Nothing
        cmbOcupacaoPai.Text = Nothing
        cmbOcupacaoMae.Text = Nothing
        txtNome.Focus()
    End Sub

    Private Function FValidaCampos() As Boolean
        If txtNome.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Nome' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        If txtDocumento.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Documento' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        If txtEndereco.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Endereço' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        If txtBairro.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Bairro' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        If txtCep.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Cep' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        If txtCidade.Text = "" Then
            MsgBox("Preencha corretamente o campo 'Cidade' ", MsgBoxStyle.Exclamation, My.Application.Info.CompanyName)
            Return False
        End If
        Return True
    End Function

    Private Sub btnCadastrarProdutos_Click(sender As Object, e As EventArgs) Handles btnCadastrarProdutos.Click
        PLimpaCampos()
    End Sub

    Private Sub btnGravarAlterar_Click(sender As Object, e As EventArgs) Handles btnGravarAlterar.Click

        If FValidaCampos() = False Then Exit Sub

        If txtCodigo.Text = "Novo" Then
            PGrava()
            frmListaClientes.dgvClientes.DataSource = classClientes.FCarregaClientesCadastrados
            Me.Close()
        Else
            PAlteraDadosCliente()
            frmListaClientes.dgvClientes.DataSource = classClientes.FCarregaClientesCadastrados
            Me.Close()
        End If
        PLimpaCampos()
    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs) Handles btnFechar.Click
        Me.Dispose()
    End Sub
    Public Sub PLerDadosCliente(ByVal codigo As Integer)
        Dim dr As OleDbDataReader = Nothing
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT id_cliente,nome,apelido,Documento,Endereco,Bairro,Cep,"
                sql += "Cidade,Estado,Telefone,Celular,Sexo,data_nascimento,Email,Obs,"
                sql += "data_cadastro,AfiliacaoPai,AfiliacaoMae,EnderecoF,NumeroF,BairroF,CidadeF,"
                sql += "EstadoF,ObsF,ProfPai,ProfMae FROM tbl_clientes WHERE id_cliente = " & CInt(codigo)
                Dim cmd As New OleDbCommand(sql, con)
                dr = cmd.ExecuteReader(CommandBehavior.SingleRow)
                If dr.HasRows Then
                    dr.Read()
                    txtNome.Text = dr.Item("nome")

                    If IsDBNull(dr.Item("apelido")) Then
                        txtApelido.Text = ""
                    Else
                        txtApelido.Text = (dr.Item("apelido"))
                    End If

                    txtDocumento.Text = (dr.Item("Documento"))
                    txtEndereco.Text = dr.Item("Endereco")
                    txtBairro.Text = dr.Item("Bairro")
                    txtCep.Text = dr.Item("Cep")
                    txtCidade.Text = dr.Item("Cidade")
                    cmbUF.Text = dr.Item("Estado")

                    If IsDBNull(dr.Item("Telefone")) Then
                        txtTelefone.Text = ""
                    Else
                        txtTelefone.Text = (dr.Item("Telefone"))
                    End If

                    If IsDBNull(dr.Item("Celular")) Then
                        txtCelular.Text = ""
                    Else
                        txtCelular.Text = (dr.Item("Celular"))
                    End If

                    cmbSexo.Text = dr.Item("Sexo")
                    dtpNascimento.Text = dr.Item("data_nascimento")

                    If IsDBNull(dr.Item("Email")) Then
                        txtEmail.Text = ""
                    Else
                        txtEmail.Text = (dr.Item("Email"))
                    End If

                    If IsDBNull(dr.Item("Obs")) Then
                        txtObs.Text = ""
                    Else
                        txtObs.Text = (dr.Item("Obs"))
                    End If
                    dtpCadastro.Text = dr.Item("data_cadastro")

                    If IsDBNull(dr.Item("AfiliacaoPai")) Then
                        txtPai.Text = ""
                    Else
                        txtPai.Text = (dr.Item("AfiliacaoPai"))
                    End If

                    If IsDBNull(dr.Item("AfiliacaoMae")) Then
                        txtMae.Text = ""
                    Else
                        txtMae.Text = (dr.Item("AfiliacaoMae"))
                    End If

                    If IsDBNull(dr.Item("EnderecoF")) Then
                        txtEnderecoFiliacao.Text = ""
                    Else
                        txtEnderecoFiliacao.Text = (dr.Item("EnderecoF"))
                    End If

                    If IsDBNull(dr.Item("NumeroF")) Then
                        txtNumero.Text = ""
                    Else
                        txtNumero.Text = (dr.Item("NumeroF"))
                    End If

                    If IsDBNull(dr.Item("BairroF")) Then
                        txtBairroF.Text = ""
                    Else
                        txtBairroF.Text = (dr.Item("BairroF"))
                    End If

                    If IsDBNull(dr.Item("CidadeF")) Then
                        txtCidadeF.Text = ""
                    Else
                        txtCidadeF.Text = (dr.Item("CidadeF"))
                    End If

                    If IsDBNull(dr.Item("EstadoF")) Then
                        cmbUFfiliacao.Text = ""
                    Else
                        cmbUFfiliacao.Text = (dr.Item("EstadoF"))
                    End If

                    If IsDBNull(dr.Item("ObsF")) Then
                        txtObsF.Text = ""
                    Else
                        txtObsF.Text = (dr.Item("ObsF"))
                    End If

                    If IsDBNull(dr.Item("ProfPai")) Then
                        cmbOcupacaoPai.Text = ""
                    Else
                        cmbOcupacaoPai.Text = (dr.Item("ProfPai"))
                    End If

                    If IsDBNull(dr.Item("ProfMae")) Then
                        cmbOcupacaoMae.Text = ""
                    Else
                        cmbOcupacaoMae.Text = (dr.Item("ProfMae"))
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                dr.Close()
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        PExclui(txtCodigo.Text)
        frmListaClientes.dgvClientes.DataSource = classClientes.FCarregaClientesCadastrados
        Me.Close()
    End Sub

    Private Sub lblFechar_Click_1(sender As Object, e As EventArgs) Handles lblFechar.Click
        Me.Close()
    End Sub

    Private Sub lblImage_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblImage.LinkClicked
        If Me.OpenFileDialog1.ShowDialog = 1 Then
            Me.imgCliente.Image = System.Drawing.Image.FromFile(Me.OpenFileDialog1.FileName)
        End If
    End Sub
End Class