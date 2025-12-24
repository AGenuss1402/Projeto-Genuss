Public Class DadosEmpresa
    Public Property RazaoSocial As String
    Public Property CNPJ As String
    Public Property InscricaoEstadual As String
    Public Property Endereco As String
    Public Property Numero As String
    Public Property Bairro As String
    Public Property Cidade As String
    Public Property Telefone As String
    Public Property CEP As String
    Public Property UF As String
    Public Property NomeFantasia As String
    
    Public Sub New()
        RazaoSocial = ""
        CNPJ = ""
        InscricaoEstadual = ""
        Endereco = ""
        Numero = ""
        Bairro = ""
        Cidade = ""
        Telefone = ""
        CEP = ""
        UF = ""
        NomeFantasia = ""
    End Sub
End Class