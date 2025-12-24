Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsLembretes

    Public Function FCarregaLembretes() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_lembrete ORDER BY data"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os lembretes. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

End Class
