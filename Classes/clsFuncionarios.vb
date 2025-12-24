Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsFuncionarios

    Public Function FCarregaFuncionarios() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT id_funcionario,nome,endereco,cidade,telefone,salario FROM tbl_funcionarios ORDER BY nome"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os funcionários. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaValesFuncionarios(ByVal idFuncionario As Integer) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT tbl_funcionarios.id_funcionario, tbl_funcionarios.nome, 'R$ 0,00' AS [Salário], tbl_salario_funcionarios.vale, tbl_salario_funcionarios.data "
                sql += "FROM (tbl_funcionarios INNER JOIN "
                sql += "tbl_salario_funcionarios ON tbl_funcionarios.id_funcionario = tbl_salario_funcionarios.id_funcionario) "
                sql += "WHERE (tbl_funcionarios.id_funcionario = " & idFuncionario & ") "
                sql += "ORDER BY data"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os vales dos funcionários. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

End Class
