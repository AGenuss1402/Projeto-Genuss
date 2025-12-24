Imports System.Net.Http
Imports System.Threading.Tasks

Module BaixarNFeSefaz

    Async Function DownloadXmlNfceAsync(url As String) As Task(Of String)
        Using httpClient As New HttpClient()
            Try
                ' Faz a requisição GET para a URL do Web Service
                Dim response As HttpResponseMessage = Await httpClient.GetAsync(url)

                ' Verifica se a requisição foi bem-sucedida
                If response.IsSuccessStatusCode Then
                    ' Lê o conteúdo da resposta como uma string (XML)
                    Dim xmlContent As String = Await response.Content.ReadAsStringAsync()
                    Return xmlContent
                Else
                    ' Trate o erro conforme necessário
                    Console.WriteLine("Erro ao baixar o XML: " & response.StatusCode.ToString())
                    Return Nothing
                End If
            Catch ex As Exception
                ' Trate exceções
                Console.WriteLine("Erro: " & ex.Message)
                Return Nothing
            End Try
        End Using
    End Function

    Sub Main()
        ' URL de exemplo (substitua pela URL correta do Web Service)
        Dim url As String = "https://www.sefaz.mt.gov.br/nfce/consultanfce?p={chaveAcesso}|2|1|51|{codigoSeguranca}"

        ' Chama a função assíncrona para baixar o XML
        Dim xmlTask As Task(Of String) = DownloadXmlNfceAsync(url)
        xmlTask.Wait()

        ' Obtém o resultado da tarefa
        Dim xmlContent As String = xmlTask.Result

        If Not String.IsNullOrEmpty(xmlContent) Then
            ' Salva o XML em um arquivo ou processa conforme necessário
            Console.WriteLine("XML baixado com sucesso!")
            Console.WriteLine(xmlContent)
        Else
            Console.WriteLine("Falha ao baixar o XML.")
        End If
    End Sub

End Module
