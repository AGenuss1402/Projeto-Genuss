Imports System.Data
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices.ComTypes

Module mdlUltilitarios

    Public dadosArray() As String = {"111.111.111-11", "222.222.222-22", "333.333.333-33", "444.444.444-44",
                                  "555.555.555-55", "666.666.666-66", "777.777.777-77", "888.888.888-88",
                                  "999.999.999-99"}

    Public strSerialDefinitivo As String = "#@!B9D00PBLMZJABS01FGP1GYJV!@#"
    Private Const msgErro As String = "Dados Inválidos"

    Public intAviso As Integer
    Public intCodigoSerial As Integer
    Public intDiasSerial As Integer
    Public strSerial As String

    ' Variaveis da tela do PDV
    Public strValorTotal As String
    Public strDocumento As String
    Public strClienteVendas As String
    Public strLogim As String
    Public strPerfil As String
    Public strImgUsuario As Byte
    Public strValorCartao As Double
    Public strPagamento As String
    Public IntSenhaAdmin As String

    '/// Variaveis para contas a receber
    Public intCodCliente As Integer
    Public strNomeCliente As String
    Public intCodigoParcelaPagar As Integer
    Public codParcela As Integer
    Public strValorParcela As String
    Public intDataVencimento As Date

    ' /// variaveis para configuração do sistema
    Public strRasaoSocial As String
    Public strEmpresa As String
    Public strFantasia As String
    Public strCNPJ As String
    Public strEndereco As String
    Public strNumero As String
    Public strBairro As String
    Public strCidade As String
    Public strEstado As String
    Public strCep As String
    Public strFone As String
    Public strCelular As String
    Public strEmail As String

    ' Emitentes
    Public strEmitente_rasaosocial As String
    Public strEmitente_empresa As String
    Public strEmitente_fantasia As String
    Public strEmitente_CNPJ As String
    Public strEmitente_IE As String
    Public strEmitente_IM As String
    Public strEmitente_endereco As String
    Public strEmitente_numero As String
    Public strEmitente_bairro As String
    Public strEmitente_cidade As String
    Public strEmitente_estado As String
    Public strEmitente_cep As String
    Public strEmitente_fone As String
    Public strEmitente_celular As String
    Public strEmitente_email As String
    Public strEmitente_token_criar_url As String
    Public strEmitente_token_usuario As String
    Public strEmitente_token_password As String
    Public strEmitente_url_emissao_nfce As String
    Public strEmitente_token_api As String
    Public strEmitente_credenciado_client_id As String
    Public strEmitente_nfce_ambiente As String
    Public strEmitenteRegimeTributario As String
    Public strEmitenteCertificado As String
    Public strEmitenteSenhaCertificado As String
    Public strEmitenteTimeOut As String
    Public strEmitenteTimeOut_tentativas As String
    Public strEmitenteCainhoSchemas As String
    Public stremitenteIDCSC As String
    Public strEmitenteCodigoCSC As String
    Public strEmitenteCFOP As String

    Public strEmitenteProximoNumero As String
    Public strEmitenteSerieBanco As Integer



    Public vlrPgtoDinheiro As Double
    Public vlrPgtoDebito As Double
    Public vlrPgtoCredito As Double
    Public vlrPgtoCheque As Double
    Public vlrPgtoPix As Double
    Public vlrPgtoOutros As Double

    Public vlrTotalRecebido As Double
    Public vlrTotalPedido As Double
    Public vlrTrocoPedido As Double


    Public strImagemRelatorio As Byte
    Public strImage As Byte
    Public strImageCupom As Byte
    Public LogoMarca As Image

    Public StrDataRegistro As String

    ' Variaveis para Orçamentos
    Public strClienteOrcamento As String
    Public intNuOrcamento As Integer
    Public Intquant As Integer
    Public strCodProduto As String
    Public strDescricao As String
    Public intUnit As Integer
    Public intDesconto As Integer
    Public intTotal As Integer

    ' Variaveis para modulo comanda
    Public IntCliente As String
    Public IntMesas As String
    Public dbValor As Double

    Public intCodigoBarras As String
    Public strTroco As Double
    Public strJuros As Double

    '//Parametros da balanca
    Public intVerificador As Integer
    Public intCodBarrasBall As Integer

    '// Parametros Fiscais
    Public TpAmHomologacao

    '//VAriaveis de dados de parceiros de negocios
    Public strCidadeParceiro As String = ""
    Public strUfParceiro As String = ""
    Public strLogomarcaParceiro As String = ""
    Public strEmpresaParceiro As String = ""
    Public strEmailParceiro As String = ""
    Public strFoneParceiro As String = ""
    Public strCNPJParceiro As String = ""

    '// VAriaveis de Pagamento Pix

    Public strChavePix As String
    Public strToken As String
    Public intValorTRansacao As Integer

    'VAriavel que grava os valores pra imprimir cupom
    Public strFormPagamento As String
    Public strValorPAgo As String
    Public strTrocoImpresso As String

    '// Terminal do pdv

    Public intTerminal As Integer
    Public strUsuarioId As String
    Public intNumeroPedido As Integer
    Public strValorPagoPedido As String

    'variavel pra mostra o terminal no cupom do fechamento do caixa
    Public intTerminalCaxa As String

    'VAraveis para consumir api joson4
    Public intcaixa As String
    Public intpedido As String
    Public intoperado As String
    Public strcliente As String
    Public strcpf As String
    Public strfrete As String
    Public strseguro As String
    Public strdesconto As String
    Public stroutros As String
    Public strtotal As String

    Public strImpressora As String
    Public strImpressoraFiscal As String

    ' ########### Valida a caixa de testo somente para numeros ################
    Function SoNumeros(ByVal Keyascii As Short) As Short
        If InStr("1234567890", Chr(Keyascii)) = 0 Then
            SoNumeros = 0
        Else
            SoNumeros = Keyascii
        End If
        Select Case Keyascii
            Case 8
                SoNumeros = Keyascii
            Case 13
                SoNumeros = Keyascii
            Case 32
                SoNumeros = Keyascii
        End Select
    End Function

    Public Function FValidaEmail(ByVal email As String) As Boolean

        ' Pattern ou mascara de verificação
        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"

        ' Verifica se o email corresponde a pattern/mascara
        Dim emailAddressMatch As Match = Regex.Match(email, pattern)

        ' Caso corresponda
        If emailAddressMatch.Success Then
            Return True
        Else
            MsgBox("E-mail informado não é válido. Verifique se digitou corretamente.", _
               MsgBoxStyle.Information, "CS .Net Tecnologia")
            Return False
        End If
    End Function
    Public Function FValidaCPF(ByVal CPF As String) As Boolean
        Dim i, x, n1, n2 As Integer
        CPF = CPF.Trim
        For i = 0 To dadosArray.Length - 1
            If CPF.Length <> 14 Or dadosArray(i).Equals(CPF) Then
                Return False
            End If
        Next
        'remove a maskara
        CPF = CPF.Substring(0, 3) + CPF.Substring(4, 3) + CPF.Substring(8, 3) + CPF.Substring(12)
        For x = 0 To 1
            n1 = 0
            For i = 0 To 8 + x
                n1 = n1 + (Val(CPF.Substring(i, 1)) * (10 + x - i))
            Next
            n2 = 11 - (n1 - (Int(n1 / 11) * 11))
            If n2 = 10 Or n2 = 11 Then n2 = 0
            If n2 <> Val(CPF.Substring(9 + x, 1)) Then
                MsgBox("O CPF informado não é válido. Verifique se digitou corretamente.", _
                       MsgBoxStyle.Information, "Vision Plus")
                Return False
            End If
        Next
        Return True
    End Function

    Private Const Chave As String = ""
    Private TripleDES As New TripleDESCryptoServiceProvider
    Private MD5 As New MD5CryptoServiceProvider

    Public Function LimparNumeros(texto As String) As String
        Return New String(texto.Where(AddressOf Char.IsDigit).ToArray())
    End Function
    Public Function ValidarCNPJ(cnpj As String) As Boolean
        cnpj = LimparNumeros(cnpj)

        If cnpj.Length <> 14 Then Return False

        Dim multiplicador1() As Integer = {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2}
        Dim multiplicador2() As Integer = {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2}
        Dim tempCNPJ As String = cnpj.Substring(0, 12)
        Dim soma As Integer = 0

        For i As Integer = 0 To 11
            soma += Integer.Parse(tempCNPJ(i).ToString()) * multiplicador1(i)
        Next

        Dim resto As Integer = soma Mod 11
        Dim digito1 As Integer = If(resto < 2, 0, 11 - resto)
        tempCNPJ &= digito1.ToString()
        soma = 0

        For i As Integer = 0 To 12
            soma += Integer.Parse(tempCNPJ(i).ToString()) * multiplicador2(i)
        Next

        resto = soma Mod 11
        Dim digito2 As Integer = If(resto < 2, 0, 11 - resto)
        tempCNPJ &= digito2.ToString()

        Return cnpj = tempCNPJ
    End Function
End Module
