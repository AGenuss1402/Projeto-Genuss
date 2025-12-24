Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Xml
Imports System.Security.Cryptography.X509Certificates

Public Class AssinaturaDigital

    Public Function AssinarXML(xmlDoc As XmlDocument, certificado As X509Certificate2) As XmlDocument
        ' Converte a chave para RSACryptoServiceProvider
        Dim rsaCng = TryCast(certificado.GetRSAPrivateKey(), RSA)
        If rsaCng Is Nothing Then
            Throw New Exception("A chave privada não é RSA ou não está acessível.")
        End If

        ' Exporta a chave para compatibilidade com SignedXml
        Dim rsaParams = rsaCng.ExportParameters(True)
        Dim rsaProvider = New RSACryptoServiceProvider()
        rsaProvider.ImportParameters(rsaParams)

        Dim signedXml As New SignedXml(xmlDoc)
        signedXml.SigningKey = rsaProvider

        ' Localiza o elemento infNFe e extrai o ID
        Dim infNFeNode = xmlDoc.GetElementsByTagName("infNFe")(0)
        Dim reference As New Reference()
        reference.Uri = "#" & infNFeNode.Attributes("Id").Value

        reference.AddTransform(New XmlDsigEnvelopedSignatureTransform())
        reference.AddTransform(New XmlDsigC14NTransform())

        ' Para homologação MT o schema exige SHA-1
        reference.DigestMethod = SignedXml.XmlDsigSHA1Url

        ' Configurar algoritmos exigidos pela SEFAZ (RSA-SHA1 + C14N)
        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url
        signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl

        signedXml.AddReference(reference)

        Dim keyInfo As New KeyInfo()
        keyInfo.AddClause(New KeyInfoX509Data(certificado))
        signedXml.KeyInfo = keyInfo

        signedXml.ComputeSignature()

        Dim xmlDigitalSignature As XmlElement = signedXml.GetXml()
        xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, True))

        Return xmlDoc
    End Function

End Class
