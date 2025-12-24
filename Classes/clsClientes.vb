Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsClientes

    Public Function FCarregaClientesCadastrados() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Top 30 id_cliente,data_cadastro,nome,apelido,Documento,Endereco,Bairro,Cep,Cidade,"
                sql += "Estado,Telefone,Celular,Sexo,data_nascimento,Email FROM tbl_clientes"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os clientes cadastrados. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    Public Function FPesquisaClientesCodigo(ByVal codigo As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Top 30 id_cliente,data_cadastro,nome,apelido,Documento,Endereco,Bairro,Cep,Cidade,"
                Sql += "Estado,Telefone,Celular,Sexo,data_nascimento,Email FROM tbl_clientes WHERE id_cliente LIKE '%" & codigo & "%' ORDER BY nome"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os cliente cadastrado. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaClientesNome(ByVal NomeCliente As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Top 30 id_cliente,data_cadastro,nome,apelido,Documento,Endereco,Bairro,Cep,Cidade,"
                sql += "Estado,Telefone,Celular,Sexo,data_nascimento,Email FROM tbl_clientes WHERE nome LIKE '%" & NomeCliente & "%' ORDER BY nome"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os clientes cadastrados. " _
                                    & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
End Class
