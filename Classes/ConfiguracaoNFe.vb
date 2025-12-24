Imports System.Configuration
Imports System.IO

''' <summary>
''' Classe centralizada para configurações de NFe/NFCe
''' </summary>
Public Class ConfiguracaoNFe
    
#Region "Propriedades de Configuração"
    
    ''' <summary>
    ''' Razão Social da empresa
    ''' </summary>
    Public Shared ReadOnly Property RazaoSocial As String
        Get
            Return ConfigurationManager.AppSettings("RazaoSocial") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Nome Fantasia da empresa
    ''' </summary>
    Public Shared ReadOnly Property NomeFantasia As String
        Get
            Return ConfigurationManager.AppSettings("NomeFantasia") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Inscrição Estadual
    ''' </summary>
    Public Shared ReadOnly Property InscricaoEstadual As String
        Get
            Return ConfigurationManager.AppSettings("InscricaoEstadual") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Código do município
    ''' </summary>
    Public Shared ReadOnly Property CodigoMunicipio As String
        Get
            Return ConfigurationManager.AppSettings("CodigoMunicipio") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Endereço da empresa
    ''' </summary>
    Public Shared ReadOnly Property Endereco As String
        Get
            Return ConfigurationManager.AppSettings("Endereco") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Número do endereço
    ''' </summary>
    Public Shared ReadOnly Property Numero As String
        Get
            Return ConfigurationManager.AppSettings("Numero") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Bairro
    ''' </summary>
    Public Shared ReadOnly Property Bairro As String
        Get
            Return ConfigurationManager.AppSettings("Bairro") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Cidade
    ''' </summary>
    Public Shared ReadOnly Property Cidade As String
        Get
            Return ConfigurationManager.AppSettings("Cidade") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' CEP
    ''' </summary>
    Public Shared ReadOnly Property CEP As String
        Get
            Return ConfigurationManager.AppSettings("CEP") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' CNPJ do emitente
    ''' </summary>
    Public Shared ReadOnly Property EmitenteCnpj As String
        Get
            Return ConfigurationManager.AppSettings("EmitenteCnpj") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' UF do emitente
    ''' </summary>
    Public Shared ReadOnly Property UF As String
        Get
            Return ConfigurationManager.AppSettings("UF") ?? "MT"
        End Get
    End Property
    
    ''' <summary>
    ''' Ambiente: 1=Produção, 2=Homologação
    ''' </summary>
    Public Shared ReadOnly Property Ambiente As Integer
        Get
            Dim valor As String = ConfigurationManager.AppSettings("Ambiente") ?? "2"
            Return Integer.Parse(valor)
        End Get
    End Property
    
    ''' <summary>
    ''' Caminho do certificado digital
    ''' </summary>
    Public Shared ReadOnly Property CaminhoCertificado As String
        Get
            Dim caminho = ConfigurationManager.AppSettings("CaminhoCertificado") ?? ""
            If String.IsNullOrEmpty(caminho) Then
                caminho = Path.Combine(Application.StartupPath, "Certificado", "certificado.pfx")
            End If
            Return caminho
        End Get
    End Property
    
    ''' <summary>
    ''' Senha do certificado digital
    ''' </summary>
    Public Shared ReadOnly Property SenhaCertificado As String
        Get
            Return ConfigurationManager.AppSettings("SenhaCertificado") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Diretório para salvar XMLs
    ''' </summary>
    Public Shared ReadOnly Property DiretorioXml As String
        Get
            Dim diretorio = ConfigurationManager.AppSettings("DiretorioXml") ?? ""
            If String.IsNullOrEmpty(diretorio) Then
                diretorio = Path.Combine(Application.StartupPath, "XMLs")
            End If
            
            ' Criar diretório se não existir
            If Not Directory.Exists(diretorio) Then
                Directory.CreateDirectory(diretorio)
            End If
            
            Return diretorio
        End Get
    End Property
    
    ''' <summary>
    ''' Nome da impressora térmica
    ''' </summary>
    Public Shared ReadOnly Property ImpressoraTermica As String
        Get
            Return ConfigurationManager.AppSettings("ImpressoraTermica") ?? ""
        End Get
    End Property
    
    ''' <summary>
    ''' Timeout para requisições em milissegundos
    ''' </summary>
    Public Shared ReadOnly Property TimeoutRequisicao As Integer
        Get
            Dim valor As String = ConfigurationManager.AppSettings("TimeoutRequisicao") ?? "30000"
            Return Integer.Parse(valor)
        End Get
    End Property
    
#End Region
    
#Region "Métodos de Validação"
    
    ''' <summary>
    ''' Valida se todas as configurações obrigatórias estão preenchidas
    ''' </summary>
    ''' <returns>Lista de erros encontrados</returns>
    Public Shared Function ValidarConfiguracoes() As List(Of String)
        Dim erros As New List(Of String)
        
        If String.IsNullOrEmpty(EmitenteCnpj) Then
            erros.Add("CNPJ do emitente não configurado")
        End If
        
        If String.IsNullOrEmpty(RazaoSocial) Then
            erros.Add("Razão Social não configurada")
        End If
        
        If String.IsNullOrEmpty(InscricaoEstadual) Then
            erros.Add("Inscrição Estadual não configurada")
        End If
        
        If String.IsNullOrEmpty(CaminhoCertificado) OrElse Not File.Exists(CaminhoCertificado) Then
            erros.Add("Certificado digital não encontrado")
        End If
        
        If String.IsNullOrEmpty(SenhaCertificado) Then
            erros.Add("Senha do certificado não configurada")
        End If
        
        If String.IsNullOrEmpty(CodigoMunicipio) Then
            erros.Add("Código do município não configurado")
        End If
        
        Return erros
    End Function
    
    ''' <summary>
    ''' Verifica se o certificado é válido
    ''' </summary>
    ''' <returns>True se válido</returns>
    Public Shared Function ValidarCertificado() As Boolean
        Try
            If Not File.Exists(CaminhoCertificado) Then
                Return False
            End If
            
            Dim cert As New Security.Cryptography.X509Certificates.X509Certificate2(CaminhoCertificado, SenhaCertificado)
            Return cert.NotAfter > DateTime.Now
        Catch
            Return False
        End Try
    End Function
    
#End Region
    
#Region "URLs dos WebServices"
    
    ''' <summary>
    ''' Obtém a URL do WebService baseada na UF e ambiente
    ''' </summary>
    Public Shared Function ObterUrlWebService() As String
        Dim isProducao = (Ambiente = 1)
        
        Select Case UF.ToUpper()
            Case "MT"
                Return If(isProducao, 
                    "https://nfe.sefaz.mt.gov.br/nfews/services/NfeRecepcao2",
                "https://homologacao.sefaz.mt.gov.br/nfews/services/NfeRecepcao2")
            Case "SP"
                Return If(isProducao,
                    "https://nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx",
                    "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx")
            Case Else
                ' SVAN - Ambiente Nacional
                Return If(isProducao,
                    "https://www.sefazvirtual.fazenda.gov.br/NfeAutorizacao/NfeAutorizacao.asmx",
                    "https://hom.sefazvirtual.fazenda.gov.br/NfeAutorizacao/NfeAutorizacao.asmx")
        End Select
    End Function
    
#End Region
    
End Class