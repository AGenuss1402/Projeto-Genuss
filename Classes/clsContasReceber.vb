Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsContasReceber
    Dim ClassReceber As New clsContasReceber
    Public Function FCarregaContasReceber() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_receber "
                sql += "WHERE MONTH(vencimentoParcela) = MONTH(NOW()) AND YEAR(NOW())"
                sql += "ORDER BY vencimentoParcela"

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

    Public Function FConsultaContasReceber(ByVal mes As Integer, ByVal ano As Integer, ByVal tipoConsulta As Boolean) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_receber "
                sql += "WHERE MONTH(vencimentoParcela) = " & mes & " AND YEAR(vencimentoParcela) = " & ano & " AND status = " & tipoConsulta & " "
                sql += "ORDER BY vencimentoParcela"

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

    Public Function FCarregaTodasContasReceber() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT * FROM tbl_contas_receber "
                'sql += "WHERE MONTH(data_vencimento) = MONTH(NOW()) AND YEAR(NOW())"
                sql += "ORDER BY vencimentoParcela"

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

    Public Function FPesquisaContaCliente(ByVal Cliente As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_contas_receber WHERE Cliente LIKE '%" & Cliente & "%' ORDER BY Cliente"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os Cliente selecionado. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaClientesContasReceber(ByVal Cliente As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT NomeCliente FROM tbl_contas_receber WHERE NomeCliente LIKE '%" & Cliente & "%' ORDER BY NomeCliente"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                    & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, strEmpresaParceiro)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
End Class
