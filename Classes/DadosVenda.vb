Public Class DadosVenda
    Public Property VendaId As Integer
    Public Property DataVenda As DateTime
    Public Property ValorTotal As Decimal
    Public Property ClienteNome As String
    Public Property ClienteDocumento As String
    Public Property FormaPagamento As String
    Public Property Desconto As Decimal
    Public Property OutrasDespesas As Decimal
    Public Property ValorTroco As Decimal
    Public Property QuantidadeItens As Integer
    Public Property NumeroCupom As String
    Public Property SerieCupom As String
    
    Public Sub New()
        VendaId = 0
        DataVenda = DateTime.Now
        ValorTotal = 0
        ClienteNome = "CONSUMIDOR"
        ClienteDocumento = ""
        FormaPagamento = "Dinheiro"
        Desconto = 0
        OutrasDespesas = 0
        ValorTroco = 0
        QuantidadeItens = 0
        NumeroCupom = ""
        SerieCupom = ""
    End Sub
End Class