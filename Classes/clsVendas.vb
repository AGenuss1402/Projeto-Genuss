Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsVendas

    Public Function fCarregarOrcamentos() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_Orcamento"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os orçamrntos cadastrados. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaProdutosVendidos() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM pedido_venda_itens ORDER BY Data"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    Public Function FCarregaItensVendidos() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Data,descricao,codigo_produto,quantidade,valor_total,venda_id FROM pedido_venda_itens ORDER BY Data"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaEntreDatas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT * FROM pedido_venda_itens "
                sql += "WHERE (Data BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaEntreDatasProdutos(ByVal dataInicial As Date, ByVal dataFinal As Date, ByVal produto As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT * FROM pedido_venda_itens "
                sql += "WHERE (Data BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "# AND descricao LIKE '%" & produto & "%') "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaProdutos(ByVal produto As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT * FROM pedido_venda_itens "
                sql += "WHERE descricao LIKE '%" & produto & "%' "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function


    '/################## CAIXA #####################/'
    Public Function FCarregaCaixas() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_caixa ORDER BY data_inicio"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os caixas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaEntreDatasCaixas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT * FROM tbl_vendas "
                sql += "WHERE (data_venda BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY data_venda"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os caixas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaItensVendidos(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT Data, Cod_Venda, CodigoBarra, Descricao, Quant, Valor  FROM tbl_Intens_Vendidos "
                sql += "WHERE (Data BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os caixas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function


    '############ CÓDIGO UNICO DAS VENDAS ###############'
    Public Function FCarregaCodigoUnicosProdutosVendidos() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_cod_vendas_realizadas"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaCodigoUnicoEntreDatas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT tbl_cod_vendas_realizadas.id, tbl_cod_vendas_realizadas.unique_id_venda, tbl_cod_vendas_realizadas.desconto "
                sql += "FROM (tbl_cod_vendas_realizadas INNER JOIN "
                sql += "tbl_vendas ON tbl_cod_vendas_realizadas.unique_id_venda = tbl_vendas.unique_id_venda )"
                sql += "WHERE (tbl_vendas.data_venda BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY data_venda"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function fSelecionaVendaPorIDvenda(ByVal Id_venda As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT Data, numeroPedido, codigo_produto, descricao_produto, quantidade, valor_unitario FROM tbl_Itens_Pedidos WHERE numeroPedido LIKE '%" & Id_venda & "%' ORDER BY numeroPedido"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os pedidos de vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function CarregaFluxoCaixaEntreDatas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT data_venda, Cod_Venda,VlrInicial,Dinheiro, CartaoDebito, CartaoCredito, Cheque,Pix, Prazo,Entrada,Saida FROM tbl_vendas "
                sql += "WHERE (data_venda BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY data_venda"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaPedidosVendas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT Data, numeroPedido, codigo_produto, descricao_produto, quantidade, valor_unitario FROM tbl_Itens_Pedidos "
                sql += "WHERE (Data BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os pedidos de vendas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    ' Consultas as formas de pagamentos por data
    Public Function FConsultaPedidosPagamentos(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT Data, venda_id, tipo_pagamento, valor FROM tbl_formas_pagamento "
                sql += "WHERE (Data BETWEEN #" & Format(dataInicial, "MM/dd/yyyy") & "# AND #" & Format(dataFinal, "MM/dd/yyyy") & "#) "
                sql += "ORDER BY Data"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as formas de pagamento. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    ' Consulta as formas de pagamento pelo ID da vendas
    Public Function fSelecionaPagamentoPorIDvenda(ByVal Id_venda As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT Data, venda_id, tipo_pagamento, valor FROM tbl_formas_pagamento WHERE venda_id LIKE '%" & Id_venda & "%' ORDER BY venda_id"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar as formas de pagamentos pelo ID. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function Top10ProdutosMaisVendidos(ByVal dataDia As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                ' Calcula datas para o intervalo de 5 dias
                Dim dataFinal As Date = Date.Today
                Dim dataInicial As Date = dataFinal.AddDays(-5)
                Dim sql As String

                sql = "SELECT TOP 10 Data, codigo_produto, descricao, valor_unitario, SUM(quantidade)" &
                  "FROM pedido_venda_itens " &
                  "WHERE Data BETWEEN @DataInicial AND @DataFinal " &
                  "GROUP BY Data, codigo_produto, descricao, valor_unitario " &
                  "ORDER BY SUM(quantidade) DESC"

                Dim cmd As New OleDbCommand(sql, con)
                cmd.Parameters.AddWithValue("@DataInicial", dataInicial)
                cmd.Parameters.AddWithValue("@DataFinal", dataFinal)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)

                ' Adiciona uma coluna para a posição/ranking
                If tabela.Columns.Contains("Posicao") = False Then
                    tabela.Columns.Add("Posicao", GetType(Integer))
                    For i As Integer = 0 To tabela.Rows.Count - 1
                        tabela.Rows(i)("Posicao") = i + 1
                    Next
                End If
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos mais vendidos. " &
                  Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
End Class
