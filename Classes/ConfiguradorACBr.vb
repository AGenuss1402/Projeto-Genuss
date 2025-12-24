Imports System.IO
Imports System.Text

Public Class ConfiguradorACBr
    Private Shared ReadOnly _logger As LoggerNFe = LoggerNFe.Instance
    
    ''' <summary>
    ''' Atualiza os arquivos ACBrNFe.ini com a configuração correta do ambiente
    ''' </summary>
    Public Shared Sub AtualizarConfiguracaoAmbiente()
        Try
            ' Determinar o valor do ambiente baseado na configuração
            Dim ambienteTexto = If(strEmitente_nfce_ambiente, "").ToLower().Trim()
            Dim ambienteValue As String
            
            If ambienteTexto = "1" OrElse ambienteTexto.Contains("producao") OrElse ambienteTexto.Contains("produção") OrElse ambienteTexto = "prod" Then
                ambienteValue = "1" ' Produção
            Else
                ambienteValue = "2" ' Homologação
            End If
            
            _logger.LogInfo($"Configurando ACBr para ambiente: {ambienteTexto} -> {ambienteValue}")
            
            ' Lista de caminhos dos arquivos ACBrNFe.ini
            Dim caminhos As String() = {
                Path.Combine(Application.StartupPath, "ACBrNFe.ini"),
                Path.Combine(Application.StartupPath, "bin\Debug\ACBrNFe.ini"),
                Path.Combine(Application.StartupPath, "bin\x64\Debug\ACBrNFe.ini"),
                Path.Combine(Application.StartupPath, "bin\Debug\ACBrLib\x64\ACBrNFe.ini"),
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\Debug\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\bin\x64\Debug\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\Controle de Vendas\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\Controle de Vendas\bin\Debug\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\Controle de Vendas\bin\x64\Debug\ACBrNFe.ini",
                "f:\pdv\pdv\Controle de Vendas\Controle de Vendas\Controle de Vendas\bin\Debug\ACBrLib\x64\ACBrNFe.ini"
            }
            
            ' Atualizar cada arquivo encontrado
            For Each caminho In caminhos
                AtualizarArquivoIni(caminho, ambienteValue)
            Next
            
        Catch ex As Exception
            _logger.LogError($"Erro ao atualizar configuração ACBr: {ex.Message}", ex)
        End Try
    End Sub
    
    ''' <summary>
    ''' Atualiza um arquivo INI específico
    ''' </summary>
    Private Shared Sub AtualizarArquivoIni(caminhoArquivo As String, ambienteValue As String)
        Try
            If Not File.Exists(caminhoArquivo) Then
                Return
            End If
            
            Dim linhas = File.ReadAllLines(caminhoArquivo, Encoding.UTF8).ToList()
            Dim alterado = False
            
            ' Procurar e atualizar a linha Ambiente=
            For i = 0 To linhas.Count - 1
                If linhas(i).StartsWith("Ambiente=", StringComparison.OrdinalIgnoreCase) Then
                    Dim novaLinha = $"Ambiente={ambienteValue}"
                    If linhas(i) <> novaLinha Then
                        linhas(i) = novaLinha
                        alterado = True
                        _logger.LogInfo($"Arquivo {Path.GetFileName(caminhoArquivo)} atualizado: {novaLinha}")
                    End If
                    Exit For
                End If
            Next
            
            ' Salvar apenas se houve alteração
            If alterado Then
                File.WriteAllLines(caminhoArquivo, linhas, Encoding.UTF8)
            End If
            
        Catch ex As Exception
            _logger.LogError($"Erro ao atualizar arquivo {caminhoArquivo}: {ex.Message}", ex)
        End Try
    End Sub
End Class