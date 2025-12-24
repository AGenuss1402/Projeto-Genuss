Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports QRCoder
Public Class GerarQRCodePix
    Public Shared Function GerarQRCodePix(chavePix As String,
                                   beneficiario As String,
                                   cidade As String,
                                   valor As Decimal,
                                   identificador As String) As Bitmap
        ' Monta a payload do Pix
        Dim payloadPix As String = GerarPayloadPix(chavePix, beneficiario, cidade, valor, identificador)

        ' Gera o QR Code
        Dim qrGenerator As New QRCodeGenerator()
        Dim qrCodeData As QRCodeData = qrGenerator.CreateQrCode(payloadPix, QRCodeGenerator.ECCLevel.Q)
        Dim qrCode As New QRCode(qrCodeData)
        Dim qrCodeImage As Bitmap = qrCode.GetGraphic(20)

        Return qrCodeImage
    End Function

    Private Shared Function GerarPayloadPix(chavePix As String,
                                          beneficiario As String,
                                          cidade As String,
                                          valor As Decimal,
                                          identificador As String) As String
            ' Formata o valor para 2 casas decimais
            Dim valorFormatado As String = valor.ToString("0.00", CultureInfo.InvariantCulture)

            ' Monta a payload conforme padrão do BCB
            Dim payload As New System.Text.StringBuilder()
            payload.Append("000201") ' Payload Format Indicator
            payload.Append("26") ' Merchant Account Information Length
            payload.Append("0014br.gov.bcb.pix01") ' GUI
            payload.Append(ChrW(4) & chavePix.Length.ToString("D2") & chavePix) ' Chave Pix
        payload.Append("52040000") ' Código de Categoria do Comerciante
        payload.Append("BRL") ' Transaction Currency (BRL)
        payload.Append(intValorTRansacao & ChrW(3) & valorFormatado) ' Valor da tranzação
        payload.Append("+55") ' Codigo Pais
        payload.Append(strRasaoSocial & ChrW(3) & beneficiario.Length.ToString("D2") & beneficiario) ' Nome do beneficiario
        payload.Append(strCidade & ChrW(3) & cidade.Length.ToString("D2") & cidade) ' Cidade do comerciante
        payload.Append(strBairro & ChrW(5) & strNumero & identificador) ' Additional Data Field
        payload.Append("6304") ' CRC16

            ' Calcula o CRC16
            Dim crc As String = CalcularCRC16(payload.ToString())
            payload.Append(crc)

            Return payload.ToString()
        End Function

        Private Shared Function CalcularCRC16(texto As String) As String
            ' Implementação do cálculo CRC16 (CCITT-FALSE)
            Dim crc As UShort = &HFFFF

            For Each c As Char In texto
                Dim byteVal As Byte = AscW(c)
                crc = crc Xor (CUShort(byteVal) << 8)

                For i As Integer = 0 To 7
                    If (crc And &H8000) <> 0 Then
                        crc = (crc << 1) Xor &H1021
                    Else
                        crc = crc << 1
                    End If
                Next
            Next

            Return (crc And &HFFFF).ToString("X4")
        End Function

        Public Shared Sub SalvarQRCodeComoImagem(qrCode As Bitmap, caminho As String)
            qrCode.Save(caminho, ImageFormat.Png)
        End Sub
    End Class

