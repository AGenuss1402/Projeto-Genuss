Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsVendasMarcadas

    Public Function FCarregaClientes() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT id_cliente,nome,apelido,Telefone, '0' AS [Pagar] FROM tbl_clientes ORDER BY nome"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar a lista de clientes. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaProdutosCompradosClientes(ByVal id_cliente As Integer) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT v.id, v.id_cliente, v.id_produto, e.codigo_barras, e.produto, e.valor_venda,v.data_compra,v.data_pagamento,v.obs,v.status "
                sql += "FROM (tbl_vendas_marcadas v INNER JOIN "
                sql += "tbl_estoque e ON v.id_produto = e.id) "
                sql += "WHERE (v.id_cliente = " & id_cliente & ")"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar a relação das compras do cliente. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

End Class
