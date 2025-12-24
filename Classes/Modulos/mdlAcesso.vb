Imports System.Data.SQLite
Imports System.IO
Module mdlAcesso

    Public intPergunta As Integer
    Public Function GetConnection() As OleDbConnection
        Dim conn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & My.Settings.ConexaoBanco & ";Persist Security Info=True;Jet OLEDB:Database Password=14023600"
        '  Dim conn As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\bin\Debug\DB\vendas.accdb"
        Return New OleDbConnection(conn)
    End Function

    Public Function VerificaDiaNaoUtil(ByVal dataParcela As Date) As Date
        'se for sabado ou domingo transfere para dia util 
        If dataParcela.DayOfWeek = DayOfWeek.Sunday Then
            dataParcela = dataParcela.AddDays(1)
        ElseIf dataParcela.DayOfWeek = DayOfWeek.Saturday Then
            dataParcela = dataParcela.AddDays(2)
        End If
        Return dataParcela
    End Function

End Module
