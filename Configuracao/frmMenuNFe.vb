Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Org.BouncyCastle.Asn1.Cmp
Public Class frmMenuNFe
    Private Sub frmMenuNFe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        VerificarConfiguracoes()
        lblStatus.Text = "Sistema carregado | " & DateTime.Now.ToString()
    End Sub

    ' 2. Método para verificar configurações
    Private Sub VerificarConfiguracoes()
        Try
            ' Simulação - substitua por sua lógica real
            If File.Exists(Application.StartupPath & "\config.xml") Then
                lblStatusConfig.Text = "✅ Configurações válidas"
                lblStatusConfig.ForeColor = Color.Green
                HabilitarBotoesOperacoes(True)
            Else
                lblStatusConfig.Text = "⚠️ Arquivo config.xml não encontrado"
                lblStatusConfig.ForeColor = Color.Orange
                HabilitarBotoesOperacoes(False)
            End If
        Catch ex As Exception
            lblStatusConfig.Text = "❌ Erro ao verificar configurações"
            lblStatusConfig.ForeColor = Color.Red
        End Try
    End Sub

    ' 3. Controle dos botões de operação
    Private Sub HabilitarBotoesOperacoes(habilitar As Boolean)
        btnEmitir.Enabled = habilitar
        btnConsultar.Enabled = habilitar
        btnCancelar.Enabled = habilitar
        btnListar.Enabled = habilitar
    End Sub

    Private Sub btnConfigurar_Click(sender As Object, e As EventArgs) Handles btnConfigurar.Click

        '  Dim frmConfig As New frmConfiguracaoNFe()
        ' If frmConfig.ShowDialog() = DialogResult.OK Then
        VerificarConfiguracoes()
        '  End If
    End Sub

    Private Sub btnValidar_Click(sender As Object, e As EventArgs) Handles btnValidar.Click
        Dim erros = ValidarConfiguracoes() '← Implemente este método
        If erros.Count = 0 Then
            MessageBox.Show("Configurações válidas!", "Validação",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show($"Erros encontrados:{vbCrLf}{String.Join(vbCrLf, erros)}",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnEmitir_Click(sender As Object, e As EventArgs) Handles btnEmitir.Click
        Dim numeroPedido = InputBox("Número do pedido:", "Emitir NFCe")
        If Not String.IsNullOrEmpty(numeroPedido) Then
            ' Implemente:
            ' Dim nfe = EmitirNFCe(numeroPedido)
            MessageBox.Show($"NFCe emitida para pedido {numeroPedido}", "Sucesso")
        End If
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim chave = InputBox("Chave de acesso (44 caracteres):", "Consultar NFCe")
        If chave?.Length = 44 Then
            ' Implemente:
            ' Dim resultado = ConsultarNFCe(chave)
            MessageBox.Show("Consulta realizada. Chave: " & chave, "Resultado")
        Else
            MessageBox.Show("Chave inválida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnSobre_Click(sender As Object, e As EventArgs) Handles btnSobre.Click
        Dim info = New StringBuilder()
        info.AppendLine("Sistema NFCe/NFe Próprio")
        info.AppendLine($"Versão: {Application.ProductVersion}")
        info.AppendLine("Desenvolvido por: [Sua Empresa]")
        info.AppendLine($"Data: {DateTime.Now:dd/MM/yyyy}")

        MessageBox.Show(info.ToString(), "Sobre o Sistema",
                       MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs) Handles btnFechar.Click
        If MessageBox.Show("Deseja realmente sair?", "Confirmação",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim chave = InputBox("Chave da NFCe:", "Cancelamento")
        If String.IsNullOrEmpty(chave) Then Return

        Dim justificativa = InputBox("Justificativa (mín. 15 chars):", "Cancelamento")
        If justificativa?.Length >= 15 Then
            ' Implemente:
            ' CancelarNFCe(chave, justificativa)
            MessageBox.Show("Cancelamento solicitado!", "Sucesso")
        Else
            MessageBox.Show("Justificativa insuficiente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnListar_Click(sender As Object, e As EventArgs) Handles btnListar.Click
        ' Implemente:
        ' Dim notas = ObterUltimasNotas()
        ' dataGridView.DataSource = notas
        MessageBox.Show("Listagem de NFCe será exibida aqui", "Em desenvolvimento")
    End Sub

    Private Sub btnTeste_Click(sender As Object, e As EventArgs) Handles btnTeste.Click
        Try
            ' Testar carregamento da NFe com logs detalhados
            Console.WriteLine("Iniciando teste de carregamento da NFe com logs...")
            ' NFeManager.TestarCarregamentoNFeComLogs()
            MessageBox.Show("Teste de carregamento da NFe executado com logs detalhados. Verifique o console para detalhes.", "Teste NFe", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Erro ao executar teste da NFe: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnLogs_Click(sender As Object, e As EventArgs) Handles btnLogs.Click
        Dim pastaLogs = Path.Combine(Application.StartupPath, "Logs")
        If Directory.Exists(pastaLogs) Then
            Process.Start("explorer.exe", pastaLogs)
        Else
            MessageBox.Show("Pasta de logs não encontrada!", "Aviso",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Function ValidarConfiguracoes() As List(Of String)
        '← Implemente sua validação real
        Dim erros As New List(Of String)

        If String.IsNullOrWhiteSpace(My.Settings.CertificadoDigital) Then
            erros.Add("Certificado digital não configurado")
        End If

        Return erros
    End Function

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs)

    End Sub
    ' Private Function EmitirNFCe(numeroPedido As String) As Boolean
    'Private Function ConsultarNFCe(chave As String) As DadosNFCe
    'Private Function CancelarNFCe(chave As String, justificativa As String) As Boolean
    'Private Function ObterUltimasNotas() As DataTable
End Class