Imports System.Net.Http
Imports System.Text
Imports System.Data.OleDb
Imports Newtonsoft.Json
Imports System.Threading.Tasks

Public Class AuthenticationService
    Private ReadOnly connectionString As String
    Private ReadOnly httpClient As HttpClient

    Public Sub New(accessDbPath As String)
        ' String de conexão para Access
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & My.Settings.ConexaoBanco & ";Persist Security Info=True;Jet OLEDB:Database Password=14023600"
        httpClient = New HttpClient()
    End Sub

    ' Classe para deserializar a resposta da API
    Public Class LoginResponse
        Public Property token As String
        Public Property expires_in As Integer
        Public Property refresh_token As String
        Public Property message As String
        Public Property success As Boolean
        Public Property Credenciado_Cliente_id As String
    End Class

    ' Classe para os dados de login
    Public Class LoginRequest
        Public Property email As String
        Public Property password As String
    End Class

    ' Classe para armazenar configurações da empresa
    Public Class EmpresaConfig
        Public Property token_criar_url As String
        Public Property token_usuario As String
        Public Property token_password As String
    End Class

    ' Método principal para obter e salvar o token
    Public Async Function GetAndSaveTokenAsync() As Task(Of String)
        Try
            ' 1. Verificar se já existe um token válido no banco
            Dim existingToken As String = GetValidTokenFromDatabase()
            If Not String.IsNullOrEmpty(existingToken) Then
                Return existingToken
            End If

            ' 2. Se não há token válido, fazer login na API
            Dim newToken As String = Await LoginToAPIAsync()
            If Not String.IsNullOrEmpty(newToken) Then
                Return newToken
            End If

            Return Nothing

        Catch ex As Exception
            Console.WriteLine($"Erro ao obter token: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' Método para fazer login na API
    Private Async Function LoginToAPIAsync() As Task(Of String)
        Try
            ' Obter configurações da empresa
            Dim empresaConfig As EmpresaConfig = GetEmpresaConfig()
            If empresaConfig Is Nothing Then
                Console.WriteLine("Erro: Não foi possível obter configurações da empresa.")
                Return Nothing
            End If

            ' Preparar dados do login
            Dim loginData As New LoginRequest With {
                .email = empresaConfig.token_usuario,
                .password = empresaConfig.token_password
            }

            Dim json As String = JsonConvert.SerializeObject(loginData)
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")

            ' Adicionar headers
            httpClient.DefaultRequestHeaders.Clear()
            httpClient.DefaultRequestHeaders.Add("Cookie", "PHPSESSID_serviceuppi=fdcdton432sddriuv305rcp49o")

            ' Fazer a requisição
            Dim response As HttpResponseMessage = Await httpClient.PostAsync(empresaConfig.token_criar_url, content)

            If response.IsSuccessStatusCode Then
                Dim responseContent As String = Await response.Content.ReadAsStringAsync()
                Dim loginResponse As LoginResponse = JsonConvert.DeserializeObject(Of LoginResponse)(responseContent)

                If loginResponse.success AndAlso Not String.IsNullOrEmpty(loginResponse.token) Then
                    ' Salvar token no banco de dados
                    SaveTokenToDatabase(loginResponse.token, loginResponse.expires_in, loginResponse.refresh_token, loginResponse.Credenciado_Cliente_id)
                    Console.WriteLine("Token obtido e salvo com sucesso!")
                    Return loginResponse.token
                Else
                    Console.WriteLine($"Erro no login: {loginResponse.message}")
                    Return Nothing
                End If
            Else
                Console.WriteLine($"Erro HTTP: {response.StatusCode} - {response.ReasonPhrase}")
                Return Nothing
            End If

        Catch ex As Exception
            Console.WriteLine($"Erro na requisição: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' Método para obter configurações da empresa
    Private Function GetEmpresaConfig() As EmpresaConfig
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                Dim sql As String = "SELECT token_criar_url, token_usuário, token_password FROM tbl_empresa"
                Using cmd As New OleDbCommand(sql, conn)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim config As New EmpresaConfig With {
                                .token_criar_url = If(reader("token_criar_url")?.ToString(), ""),
                                .token_usuario = If(reader("token_usuário")?.ToString(), ""),
                                .token_password = If(reader("token_password")?.ToString(), "")
                            }

                            ' Validar se todos os campos necessários estão preenchidos
                            If String.IsNullOrEmpty(config.token_criar_url) OrElse
                               String.IsNullOrEmpty(config.token_usuario) OrElse
                               String.IsNullOrEmpty(config.token_password) Then
                                Console.WriteLine("Erro: Campos obrigatórios não preenchidos na tabela tbl_empresa")
                                Return Nothing
                            End If

                            Console.WriteLine($"Configurações carregadas: URL={config.token_criar_url}, Usuário={config.token_usuario}")
                            Return config
                        Else
                            Console.WriteLine("Erro: Nenhum registro encontrado na tabela tbl_empresa")
                            Return Nothing
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao obter configurações da empresa: {ex.Message}")
            Return Nothing
        End Try
    End Function

    ' Método para verificar se existe token válido no banco
    Private Function GetValidTokenFromDatabase() As String
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                Dim sql As String = "SELECT token_api FROM tbl_empresa WHERE token_expiracao > ? ORDER BY token_create DESC"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@token_expiracao", DateTime.Now)

                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not DBNull.Value.Equals(result) Then
                        Console.WriteLine("Token válido encontrado no banco!")
                        Return result.ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao verificar token no banco: {ex.Message}")
        End Try

        Return Nothing
    End Function

    ' Método para salvar token no banco de dados
    Private Sub SaveTokenToDatabase(token As String, expiresIn As Integer, refreshToken As String, Credenciado_Cliente_id As String)
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                ' Calcular data de expiração (assumindo que expires_in está em segundos)
                Dim expiryDate As DateTime = DateTime.Now.AddSeconds(expiresIn)

                Dim sql As String = "UPDATE tbl_empresa SET token_api=?, token_create=?, token_expiracao=?, credenciado_client_id=?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@token_api", token)
                    cmd.Parameters.AddWithValue("@token_Atualizado", If(refreshToken, ""))
                    cmd.Parameters.AddWithValue("@token_create", DateTime.Now)
                    cmd.Parameters.AddWithValue("@token_expiracao", expiryDate)
                    cmd.Parameters.AddWithValue("@credenciado_client_id", Credenciado_Cliente_id)

                    cmd.ExecuteNonQuery()
                    Console.WriteLine("Token salvo no banco de dados!")
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao salvar token: {ex.Message}")
        End Try
    End Sub

    ' Método para limpar tokens expirados
    Public Sub CleanExpiredTokens()
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                Dim sql As String = "DELETE FROM tbl_empresa WHERE token_expiracao < ?"
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@now", DateTime.Now)
                    Dim deletedRows As Integer = cmd.ExecuteNonQuery()
                    Console.WriteLine($"{deletedRows} tokens expirados removidos.")
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao limpar tokens expirados: {ex.Message}")
        End Try
    End Sub



    ' Método para verificar/criar estrutura da tabela tbl_empresa se necessário
    Public Sub VerifyEmpresaTable()
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()

                ' Verificar se a tabela tbl_empresa existe e tem os campos necessários
                Dim sql As String = "SELECT token_criar_url, token_usuario, token_password FROM tbl_empresa"
                Using cmd As New OleDbCommand(sql, conn)
                    Try
                        Using reader As OleDbDataReader = cmd.ExecuteReader()
                            ' Se chegou até aqui, a tabela existe com os campos necessários
                            Console.WriteLine("Tabela tbl_empresa verificada com sucesso!")
                        End Using
                    Catch ex As Exception
                        Console.WriteLine($"Aviso: Verifique se a tabela tbl_empresa existe e possui os campos: token_criar_url, token_usuario, token_password")
                        Console.WriteLine($"Erro: {ex.Message}")
                    End Try
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao verificar tabela tbl_empresa: {ex.Message}")
        End Try
    End Sub
    ' Liberar recursos
    Public Sub Dispose()
        httpClient?.Dispose()
    End Sub
End Class

' ===== EXEMPLO DE USO =====
Public Class Program
    Public Shared Async Function Main() As Task
        ' Caminho para o banco Access
        Dim dbPath As String = "\Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & My.Settings.ConexaoBanco & ";Persist Security Info=True;Jet OLEDB:Database Password=14023600"

        Dim authService As New AuthenticationService(dbPath)

        Try
            ' Verificar tabela tbl_empresa
            authService.VerifyEmpresaTable()

            ' Limpar tokens expirados
            authService.CleanExpiredTokens()

            ' Obter token (do banco ou da API)
            Dim token As String = Await authService.GetAndSaveTokenAsync()

            If Not String.IsNullOrEmpty(token) Then
                Console.WriteLine($"Token disponível: {token.Substring(0, Math.Min(20, token.Length))}...")

                ' Usar o token em suas requisições
                ' Exemplo: fazer outras chamadas para API usando o token
                ' Await UseTokenForAPICall(token)
            Else
                Console.WriteLine("Falha ao obter token de acesso.")
            End If

        Finally
            authService.Dispose()
        End Try

        Console.WriteLine("Pressione qualquer tecla para sair...")
        Console.ReadKey()
    End Function

    ' Exemplo de como usar o token em outras requisições
    Private Shared Async Function UseTokenForAPICall(token As String) As Task
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}")

            ' Fazer outras chamadas para API aqui
            ' Dim response = Await client.GetAsync("https://service.uppi.app.br/service/algum-endpoint")
        End Using
    End Function
End Class