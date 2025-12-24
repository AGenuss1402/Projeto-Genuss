Imports System.Data.SQLite
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports ADODB

''' <summary>
''' Classe para operações com banco SQLite - Sistema completo
''' </summary>
Public Class DatabaseSQLite
    Implements IDisposable

    Private ReadOnly _logger As LoggerNFe
    Public Sub New()
        _logger = LoggerNFe.Instance
        ' InicializarBanco()
    End Sub
    Public Sub SalvarConfiguracaoAsync(Confing As Configuracao)
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            Dim trans As OleDbTransaction = conn.BeginTransaction()

            Try

                ' Inserir pedido
                Dim sqlConfigurcao As New OleDbCommand("
                    INSERT INTO configuracoes
                    (emitente_cnpj, Emitetente_razao, emitente_fantazia, emitente_InscEstadual, 
                    emitente_insMunicipal, emitente_endereco, emitente_numero, emitente_bairro, emitente_cidade, emitente_estado, emitente_cep, emitente_email,
                    emitente_fone, emitente_celular, emitente_certificado, emitente_senha_certificado, emitente_serie_nfce, emitente_proximo_numero_nfce)
                    emitente_diretorio_xml, emitente_timeout_sefaz, emitente_ambiente) 
                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", conn, trans)
                With sqlConfigurcao.Parameters
                    .AddWithValue("?", Confing.cnpj)
                    .AddWithValue("?", Confing.razao)
                    .AddWithValue("?", Confing.fantasia)
                    .AddWithValue("?", Confing.InsEstadua)
                    .AddWithValue("?", Confing.InstMunicipal)
                    .AddWithValue("?", Confing.endreco)
                    .AddWithValue("?", Confing.numero)
                    .AddWithValue("?", Confing.bairro)
                    .AddWithValue("?", Confing.cidade)
                    .AddWithValue("?", Confing.estado)
                    .AddWithValue("?", Confing.cep)
                    .AddWithValue("?", Confing.email)
                    .AddWithValue("?", Confing.fone)
                    .AddWithValue("?", Confing.celular)
                    .AddWithValue("?", Confing.certificado)
                    .AddWithValue("?", Confing.senha_certificado)
                    .AddWithValue("?", Confing.serie_nfce)
                    .AddWithValue("?", Confing.prox_nu_nfce)
                    .AddWithValue("?", Confing.diretorio_xml)
                    .AddWithValue("?", Confing.timerout_sefaz)
                    .AddWithValue("?", Confing.ambiente)
                End With

                ' Executa o comando
                sqlConfigurcao.ExecuteNonQuery()
                ' Confirma transação
                trans.Commit()
                MsgBox("As configurações foram adicionadas com sucesso", MsgBoxStyle.Information, "Sucesso")
            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Erro ao salvar dados: " & ex.Message)
            End Try
        End Using
    End Sub

    ''' <summary>
    ''' Obtém configuração do banco
    ''' </summary>
    Public Async Function ObterConfiguracaoAsync(chave As String, valorPadrao As String) As Task(Of String)
        Using con As OleDbConnection = GetConnection()
            con.Open()
            Dim sql = "SELECT valor FROM configuracoes WHERE chave = @chave"
            Using cmd As New OleDbCommand(sql, con)
                cmd.Parameters.AddWithValue("@chave", chave)

                Dim resultado = Await cmd.ExecuteScalarAsync()
                Return If(resultado IsNot Nothing, resultado.ToString(), valorPadrao)
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Obtém próximo número de NFCe
    ''' </summary>
    Public Async Function ObterProximoNumeroNFCeAsync(serie As Integer) As Task(Of Integer)
        Using con As OleDbConnection = GetConnection()
            con.Open()
            Dim sql = "SELECT IIF(ISNULL(MAX(numero_nfce)), 0, MAX(numero_nfce)) + 1 FROM tbl_nfce WHERE serie = @serie"
            Using cmd As New OleDbCommand(sql, con)
                cmd.Parameters.AddWithValue("@serie", serie)

                Dim resultado = Await cmd.ExecuteScalarAsync()
                Return Convert.ToInt32(resultado)
            End Using
        End Using
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        GC.SuppressFinalize(Me)
    End Sub

End Class

''' <summary>
''' Exceção específica para operações de banco
''' </summary>
Public Class DatabaseException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class


Public Class Configuracao
    Public Property _Id As Integer
    Public Property cnpj As String
    Public Property razao As String
    Public Property fantasia As String
    Public Property InsEstadua As String
    Public Property InstMunicipal As String
    Public Property endreco As String
    Public Property numero As String
    Public Property bairro As String
    Public Property cidade As String
    Public Property estado As String
    Public Property cep As String
    Public Property email As String
    Public Property fone As String
    Public Property celular As String
    Public Property certificado As String
    Public Property senha_certificado As String
    Public Property serie_nfce As String
    Public Property prox_nu_nfce As String
    Public Property diretorio_xml As String
    Public Property timerout_sefaz As String
    Public Property ambiente As String



End Class


