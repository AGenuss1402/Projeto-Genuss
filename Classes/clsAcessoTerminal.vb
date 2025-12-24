Imports System.Configuration
Imports System.Data.OleDb
Public Class clsAcessoTerminal
    ' Sua função existente - mantida exatamente como está
    Public Function GetConnection() As OleDbConnection
        Dim conn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & My.Settings.ConexaoBanco & ";Persist Security Info=True;Jet OLEDB:Database Password=14023600"
        Return New OleDbConnection(conn)
    End Function

    ' Método para executar consultas e retornar DataTable
    Public Function ExecuteQuery(sql As String, Optional parameters As List(Of OleDbParameter) = Nothing) As DataTable
        Dim dt As New DataTable()

        Using connection As OleDbConnection = GetConnection()
            Using command As New OleDbCommand(sql, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters.ToArray())
                End If

                connection.Open()
                Using adapter As New OleDbDataAdapter(command)
                    adapter.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    ' Método para executar comandos (INSERT, UPDATE, DELETE)
    Public Function ExecuteNonQuery(sql As String, Optional parameters As List(Of OleDbParameter) = Nothing) As Integer
        Using connection As OleDbConnection = GetConnection()
            Using command As New OleDbCommand(sql, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters.ToArray())
                End If

                connection.Open()
                Return command.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ' Método para obter um único valor
    Public Function ExecuteScalar(sql As String, Optional parameters As List(Of OleDbParameter) = Nothing) As Object
        Using connection As OleDbConnection = GetConnection()
            Using command As New OleDbCommand(sql, connection)
                If parameters IsNot Nothing Then
                    command.Parameters.AddRange(parameters.ToArray())
                End If

                connection.Open()
                Return command.ExecuteScalar()
            End Using
        End Using
    End Function
End Class
