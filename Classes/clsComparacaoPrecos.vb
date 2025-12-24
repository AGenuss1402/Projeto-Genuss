Imports System.Data
Imports System.Data.OleDb

Public Class clsComparacaoPrecos

    Public Function FCarregaPrecos() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT f.id_fornecedor, f.NomeFantasia, f.Cidade, f.Estado, e.codigo_barras, e.produto, e.valor_unitario, e.porcentagem, e.valor_venda, e.data_compra "
                sql += "FROM (tbl_fornecedores f INNER JOIN "
                sql += "tbl_estoque e ON f.id_fornecedor = e.id_fornecedor) "
                ' sql += "WHERE (f.id_fornecedor = ?)"
                sql += "ORDER BY f.NomeFantasia, e.produto"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar a relação dos preços. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsulta(ByVal fornecedor As String, ByVal produto As String, ByVal codigo_barras As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT f.id_fornecedor, f.NomeFantasia, f.Cidade, f.Estado, e.codigo_barras, e.produto, e.valor_unitario, e.porcentagem, e.valor_venda, e.data_compra "
                sql += "FROM (tbl_fornecedores f INNER JOIN "
                sql += "tbl_estoque e ON f.id_fornecedor = e.id_fornecedor) "

                If fornecedor <> "" Then
                    sql += "WHERE (f.NomeFantasia LIKE '" & fornecedor & "%') "
                End If

                If codigo_barras <> "" Then
                    sql += "WHERE (e.codigo_barras LIKE '" & codigo_barras & "%') "
                End If

                If produto <> "" Then
                    sql += "WHERE (e.produto LIKE '" & produto & "%') "
                End If

                sql += "ORDER BY f.NomeFantasia, e.produto"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar a relação dos preços. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

End Class
