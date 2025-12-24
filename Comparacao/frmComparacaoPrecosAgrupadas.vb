Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class frmComparacaoPrecosAgrupadas

    Dim classVendas As New clsVendas

    Dim dtproducto As New DataTable
    Dim dtcategoria As New DataTable
    Dim cn As OleDbConnection = GetConnection()

    Dim QtdaProdutosVendidos As Integer = 0
    Dim totalProdutosVendidos As Double

    Private Sub PCarregaInformacaoesIniciais()
        Try
            Me.Cursor = Cursors.WaitCursor
            PMostrarProdutosAgrupados()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub frmComparacaoPrecosAgrupadas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PCarregaInformacaoesIniciais()
    End Sub

    Private Sub PMostrarProdutosAgrupados()
        dgvProdutosPorCategorias.Rows.Clear()
        Dim IntFila As Integer = 0
        Dim dbtotal As Double = 0
        Dim dbtotalVenda As Double = 0
        Dim totalComDesconto As Double = 0
        Dim IntQtdaVendidos As Integer = 0
        Dim IntFilaS As Integer = 0
        dtcategoria.Clear()

        Dim query As String
        query = "SELECT id_fornecedor, NomeFantasia, Cidade, Estado FROM tbl_fornecedores"

        Dim sql As New OleDbDataAdapter(query, cn)
        sql.Fill(dtcategoria)
        For i As Integer = 0 To dtcategoria.Rows.Count - 1
            'Lembrete (No parametro -> dtcategoria.Rows(i)(0) <- o (0) significa a posição da coluna, como (0),(1),(2))
            dgvProdutosPorCategorias.Rows.Add()
            dgvProdutosPorCategorias.Item("id_fornecedor", IntFila).Value = dtcategoria.Rows(i)(0)
            dgvProdutosPorCategorias.Item("NomeFantasia", IntFila).Value = dtcategoria.Rows(i)(1)
            dgvProdutosPorCategorias.Item("Cidade", IntFila).Value = dtcategoria.Rows(i)(2)
            dgvProdutosPorCategorias.Item("Estado", IntFila).Value = dtcategoria.Rows(i)(3)

            IntFilaS = IntFila

            dgvProdutosPorCategorias.Rows(IntFila).DefaultCellStyle.BackColor = Color.Lavender
            dgvProdutosPorCategorias.Rows(IntFila).DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            dtproducto.Clear()

            'DETALLE DE CATEGORIA , MOSTRANDOS TODOS SUS PRODUCTOS
            Dim sqlpro As New OleDbDataAdapter("select codigo_barras,produto,valor_unitario,Porcentagem,valor_venda,data_compra from tbl_estoque where id_fornecedor=" & dtcategoria.Rows(i)(0), cn)
            sqlpro.Fill(dtproducto)
            IntFila += 1

            For ii As Integer = 0 To dtproducto.Rows.Count - 1
                dgvProdutosPorCategorias.Rows.Add()

                dgvProdutosPorCategorias.Item("codigo_barras", IntFila).Value = dtproducto.Rows(ii)(0)
                dgvProdutosPorCategorias.Item("produto", IntFila).Value = dtproducto.Rows(ii)(1)
                dgvProdutosPorCategorias.Item("valor_unitario", IntFila).Value = FormatCurrency(dtproducto.Rows(ii)(2))
                dgvProdutosPorCategorias.Item("Porcentagem", IntFila).Value = dtproducto.Rows(ii)(3)
                dgvProdutosPorCategorias.Item("valor_venda", IntFila).Value = FormatCurrency(dtproducto.Rows(ii)(4))
                dgvProdutosPorCategorias.Item("data_compra", IntFila).Value = Format(dtproducto.Rows(ii)(5), "dd/MM/yyyy")

                dbtotal += dtproducto.Rows(ii)(2)
                dbtotalVenda += dtproducto.Rows(ii)(4)
                IntFila += 1

                IntQtdaVendidos += 1
            Next

            dgvProdutosPorCategorias.Rows.Add()
            dgvProdutosPorCategorias.Item("codigo_barras", IntFila).Value = "Qtda. -> " & IntQtdaVendidos
            dgvProdutosPorCategorias.Item("produto", IntFila).Value = "Total em R$ Produtos"
            dgvProdutosPorCategorias.Item("valor_unitario", IntFila).Value = FormatCurrency(dbtotal)
            ' dgvProdutosPorCategorias.Item("produto", IntFila).Value = "Total em R$ Produtos"
            dgvProdutosPorCategorias.Item("valor_venda", IntFila).Value = FormatCurrency(dbtotalVenda)

            dgvProdutosPorCategorias.Item("data_compra", IntFila).Value = FormatCurrency(dbtotalVenda - dbtotal)


            'colore os valores com desconto           
            Dim estilo_produto As New DataGridViewCellStyle()
            estilo_produto.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvProdutosPorCategorias.Item("produto", IntFila).Style = estilo_produto

            Dim estilo_valor As New DataGridViewCellStyle()
            estilo_valor.ForeColor = Color.Red
            dgvProdutosPorCategorias.Item("valor_unitario", IntFila).Style = estilo_valor

            Dim estilo_diferenca As New DataGridViewCellStyle()
            estilo_diferenca.ForeColor = Color.Green
            dgvProdutosPorCategorias.Item("data_compra", IntFila).Style = estilo_diferenca

            dgvProdutosPorCategorias.Rows(IntFila).DefaultCellStyle.ForeColor = Color.Blue
            dgvProdutosPorCategorias.Rows(IntFila).DefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)

            dbtotal = 0
            IntQtdaVendidos = 0
            IntFila += 1
        Next
    End Sub

    Private Sub dgvProdutosPorCategorias_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProdutosPorCategorias.CellEnter
        If IsNothing(dgvProdutosPorCategorias.CurrentRow().Cells("id_fornecedor").Value) Then Exit Sub
        dgvProdutosPorCategorias.CurrentRow().DefaultCellStyle.SelectionForeColor = Color.Black
        dgvProdutosPorCategorias.CurrentRow().DefaultCellStyle.SelectionBackColor = Color.Lavender
    End Sub

    Private Sub dgvProdutosPorCategorias_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvProdutosPorCategorias.CellFormatting
        Try
            If e.Value IsNot DBNull.Value And e.ColumnIndex = 5 And e.RowIndex.Equals("Total Produtos Vendidos") Then
                If Not e.Value.Equals("") Then
                    e.CellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                    e.CellStyle.ForeColor = Color.Red
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click
        frmMetroPrincipal.AbrirFormEnPanel(New frmComparacaoPrecos())
    End Sub
End Class