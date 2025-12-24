Imports System.IO
Imports System.Text

''' <summary>
''' Sistema de logging específico para operações NFe/NFCe
''' </summary>
Public Class LoggerNFe
    
    Private Shared ReadOnly _lockObject As New Object()
    Private Shared _instance As LoggerNFe
    
    ''' <summary>
    ''' Singleton instance
    ''' </summary>
    Public Shared ReadOnly Property Instance As LoggerNFe
        Get
            If _instance Is Nothing Then
                SyncLock _lockObject
                    If _instance Is Nothing Then
                        _instance = New LoggerNFe()
                    End If
                End SyncLock
            End If
            Return _instance
        End Get
    End Property
    
    Private ReadOnly Property LogDirectory As String
        Get
            Dim dir = Path.Combine(Application.StartupPath, "Logs", "NFe")
            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If
            Return dir
        End Get
    End Property
    
    ''' <summary>
    ''' Níveis de log
    ''' </summary>
    Public Enum LogLevel
        Info
        Warning
        [Error]
        Debug
        Success
    End Enum
    
    ''' <summary>
    ''' Log de informação
    ''' </summary>
    Public Sub LogInfo(mensagem As String, Optional detalhes As String = "")
        EscreverLog(LogLevel.Info, mensagem, detalhes)
    End Sub
    
    ''' <summary>
    ''' Log de aviso
    ''' </summary>
    Public Sub LogWarning(mensagem As String, Optional detalhes As String = "")
        EscreverLog(LogLevel.Warning, mensagem, detalhes)
    End Sub
    
    ''' <summary>
    ''' Log de erro
    ''' </summary>
    Public Sub LogError(mensagem As String, Optional exception As Exception = Nothing)
        Dim detalhes = ""
        If exception IsNot Nothing Then
            detalhes = $"Exception: {exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}"
        End If
        EscreverLog(LogLevel.Error, mensagem, detalhes)
    End Sub
    
    ''' <summary>
    ''' Log de debug
    ''' </summary>
    Public Sub LogDebug(mensagem As String, Optional detalhes As String = "")
        EscreverLog(LogLevel.Debug, mensagem, detalhes)
    End Sub
    
    ''' <summary>
    ''' Log de sucesso
    ''' </summary>
    Public Sub LogSuccess(mensagem As String, Optional detalhes As String = "")
        EscreverLog(LogLevel.Success, mensagem, detalhes)
    End Sub
    
    ''' <summary>
    ''' Log específico para operações NFCe
    ''' </summary>
    Public Sub LogNFCe(operacao As String, numeroPedido As Integer, resultado As String, Optional erro As String = "")
        Dim mensagem = $"NFCe - {operacao} - Pedido: {numeroPedido}"
        Dim detalhes = $"Resultado: {resultado}"
        If Not String.IsNullOrEmpty(erro) Then
            detalhes += $"{Environment.NewLine}Erro: {erro}"
        End If
        
        If String.IsNullOrEmpty(erro) Then
            LogSuccess(mensagem, detalhes)
        Else
            '  LogError(mensagem, detalhes)
        End If
    End Sub
    
    ''' <summary>
    ''' Log específico para operações NFe
    ''' </summary>
    Public Sub LogNFe(operacao As String, chaveAcesso As String, resultado As String, Optional erro As String = "")
        Dim mensagem = $"NFe - {operacao} - Chave: {chaveAcesso}"
        Dim detalhes = $"Resultado: {resultado}"
        If Not String.IsNullOrEmpty(erro) Then
            detalhes += $"{Environment.NewLine}Erro: {erro}"
        End If
        
        If String.IsNullOrEmpty(erro) Then
            LogSuccess(mensagem, detalhes)
        Else
            '  LogError(mensagem, detalhes)
        End If
    End Sub
    
    ''' <summary>
    ''' Escreve o log no arquivo
    ''' </summary>
    Private Sub EscreverLog(level As LogLevel, mensagem As String, detalhes As String)
        Try
            SyncLock _lockObject
                Dim nomeArquivo = $"NFe_{DateTime.Now:yyyyMMdd}.log"
                Dim caminhoArquivo = Path.Combine(LogDirectory, nomeArquivo)
                
                Dim sb As New StringBuilder()
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {mensagem}")
                
                If Not String.IsNullOrEmpty(detalhes) Then
                    sb.AppendLine($"Detalhes: {detalhes}")
                End If
                
                sb.AppendLine(New String("-"c, 80))
                
                File.AppendAllText(caminhoArquivo, sb.ToString(), Encoding.UTF8)
            End SyncLock
        Catch ex As Exception
            ' Em caso de erro no log, não fazer nada para evitar loop infinito
            Debug.WriteLine($"Erro ao escrever log: {ex.Message}")
        End Try
    End Sub
    
    ''' <summary>
    ''' Limpa logs antigos (mantém apenas os últimos 30 dias)
    ''' </summary>
    Public Sub LimparLogsAntigos()
        Try
            Dim arquivos = Directory.GetFiles(LogDirectory, "NFe_*.log")
            Dim dataLimite = DateTime.Now.AddDays(-30)
            
            For Each arquivo In arquivos
                Dim info As New FileInfo(arquivo)
                If info.CreationTime < dataLimite Then
                    File.Delete(arquivo)
                End If
            Next
        Catch ex As Exception
            LogError("Erro ao limpar logs antigos", ex)
        End Try
    End Sub
    
End Class
