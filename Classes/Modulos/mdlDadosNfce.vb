Module mdlDadosNfce

    '//Dados de emissao cupom fiscal
    Public emitente_cnpj As String '// CNPJ responsável pela notafiscal 
    Public caixa As String '/ caixa Do pdv 
    Public cupom As String '/ // numero Do pedido gerado pdv 
    Public operador As String ' //usuário Do pdv 
    Public nome As String '// vou consumir final ou nome da pessoa que esta comprando 
    Public cpf As String '// cpf da pessoa que esta comprando ou 00000000000 11 zeros
    Public natop As String ' /// desconsidera  
    Public uf_origem As String 'sigla  estado emitente Do pdv  exemplo MT
    Public uf_destino As String ' sigla  estado consumir Do pdv  exemplo MT
    Public valor_total As Double 'valor total Do pedido
    Public vfrete As Double ' frete Do pedido
    Public vseg As Double 'seguro Do pedi…

    Public codigo As String ' código Do produto
    Public descricao_Produto As String 'descrição Do produto
    Public quantidade As String 'quantidado produto Do pedido
    Public valor_unitario As Double ' valor unitário Do produto
    Public ucom As String ' unidade de medida UN
    Public cfop_saida As String ' 5101
    Public ncm As String '62052000

    'Condição de pagamento
    Public tPag As String '  01 
    Public vPag As Double ' valor  da condição
    Public descricao As String ' descrição da condição

End Module
