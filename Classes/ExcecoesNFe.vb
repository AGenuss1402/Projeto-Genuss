''' <summary>
''' Exceções personalizadas para operações NFe/NFCe
''' </summary>

''' <summary>
''' Exceção base para operações NFe/NFCe
''' </summary>
Public Class NFeException
    Inherits Exception
    
    Public Property CodigoErro As String
    Public Property DetalhesErro As String
    Public Property TipoOperacao As String
    
    Public Sub New(mensagem As String)
        MyBase.New(mensagem)
    End Sub
    
    Public Sub New(mensagem As String, innerException As Exception)
        MyBase.New(mensagem, innerException)
    End Sub
    
    Public Sub New(mensagem As String, codigoErro As String, detalhes As String)
        MyBase.New(mensagem)
        Me.CodigoErro = codigoErro
        Me.DetalhesErro = detalhes
    End Sub
    
    Public Overrides Function ToString() As String
        Dim sb As New Text.StringBuilder()
        sb.AppendLine($"Erro NFe: {Message}")
        
        If Not String.IsNullOrEmpty(CodigoErro) Then
            sb.AppendLine($"Código: {CodigoErro}")
        End If
        
        If Not String.IsNullOrEmpty(DetalhesErro) Then
            sb.AppendLine($"Detalhes: {DetalhesErro}")
        End If
        
        If Not String.IsNullOrEmpty(TipoOperacao) Then
            sb.AppendLine($"Operação: {TipoOperacao}")
        End If
        
        Return sb.ToString()
    End Function
End Class

''' <summary>
''' Exceção específica para NFCe
''' </summary>
Public Class NFCeException
    Inherits NFeException
    
    Public Property NumeroPedido As Integer
    
    Public Sub New(mensagem As String, numeroPedido As Integer)
        MyBase.New(mensagem)
        Me.NumeroPedido = numeroPedido
        Me.TipoOperacao = "NFCe"
    End Sub
    
    Public Sub New(mensagem As String, numeroPedido As Integer, innerException As Exception)
        MyBase.New(mensagem, innerException)
        Me.NumeroPedido = numeroPedido
        Me.TipoOperacao = "NFCe"
    End Sub
End Class

''' <summary>
''' Exceção específica para NFe
''' </summary>
Public Class NFeConsultaException
    Inherits NFeException
    
    Public Property ChaveAcesso As String
    
    Public Sub New(mensagem As String, chaveAcesso As String)
        MyBase.New(mensagem)
        Me.ChaveAcesso = chaveAcesso
        Me.TipoOperacao = "NFe Consulta"
    End Sub
    
    Public Sub New(mensagem As String, chaveAcesso As String, innerException As Exception)
        MyBase.New(mensagem, innerException)
        Me.ChaveAcesso = chaveAcesso
        Me.TipoOperacao = "NFe Consulta"
    End Sub
End Class

''' <summary>
''' Exceção para problemas de certificado
''' </summary>
Public Class CertificadoException
    Inherits NFeException
    
    Public Sub New(mensagem As String)
        MyBase.New(mensagem)
        Me.TipoOperacao = "Certificado Digital"
    End Sub
    
    Public Sub New(mensagem As String, innerException As Exception)
        MyBase.New(mensagem, innerException)
        Me.TipoOperacao = "Certificado Digital"
    End Sub
End Class

''' <summary>
''' Exceção para problemas de configuração
''' </summary>
Public Class ConfiguracaoException
    Inherits NFeException
    
    Public Property ParametroFaltante As String
    
    Public Sub New(mensagem As String, parametro As String)
        MyBase.New(mensagem)
        Me.ParametroFaltante = parametro
        Me.TipoOperacao = "Configuração"
    End Sub
End Class

''' <summary>
''' Exceção para problemas de comunicação com SEFAZ
''' </summary>
Public Class SefazException
    Inherits NFeException
    
    Public Property CodigoSefaz As String
    Public Property MensagemSefaz As String
    
    Public Sub New(mensagem As String, codigoSefaz As String, mensagemSefaz As String)
        MyBase.New(mensagem)
        Me.CodigoSefaz = codigoSefaz
        Me.MensagemSefaz = mensagemSefaz
        Me.TipoOperacao = "Comunicação SEFAZ"
    End Sub
End Class