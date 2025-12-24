Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsContasPagar

    Public Function FCarregaContasPagar() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_pagar "
                sql += "WHERE MONTH(data_vencimento) = MONTH(NOW()) AND YEAR(NOW())"
                sql += "ORDER BY data_vencimento"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as contas à pagar. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaContasPagar(ByVal mes As Integer, ByVal ano As Integer) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_pagar "
                sql += "WHERE MONTH(data_vencimento) = " & mes & " AND YEAR(data_vencimento) = " & ano & " "
                sql += "ORDER BY data_vencimento"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as contas à pagar. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaContasPagar(ByVal mes As Integer, ByVal ano As Integer, ByVal status As Boolean) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_pagar "
                sql += "WHERE MONTH(data_vencimento) = " & mes & " AND YEAR(data_vencimento) = " & ano & " AND status = " & status & " "
                sql += "ORDER BY data_vencimento"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as contas à receber. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaTodasContasPagar() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_pagar "
                'sql += "WHERE MONTH(data_vencimento) = MONTH(NOW()) AND YEAR(NOW())"
                sql += "ORDER BY data_vencimento"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as contas à pagar. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

End Class
