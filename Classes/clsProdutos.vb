Imports System
Imports System.Data
Imports System.Data.OleDb

Public Class clsProdutos

    Public Function CarregarProdutosComanda(ByVal Mesa As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT  Mesa,Quant,Cod_barras,Produto,valor FROM tbl_Comanda  WHERE Mesa LIKE '%" & Mesa & "%' ORDER BY Mesa"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaProdutosEmEstoque() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT TOP 15 id,data_compra,codigo_barras,produto,valor_venda,Saldo,estoque_minimo,Situacao,Status FROM tbl_estoque ORDER BY produto"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FPesquisaProdutosEmEstoque(ByVal produto As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT id,data_compra,codigo_barras,produto,valor_venda,Saldo,estoque_minimo,Situacao,Status FROM tbl_estoque WHERE produto LIKE '%" & produto & "%' ORDER BY produto"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FPesquisaFornecedores(ByVal Fornecedor As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT TOP 25 id_fornecedor, NomeFantasia, CNPJ, Status FROM tbl_Fornecedores WHERE NomeFantasia LIKE '%" & Fornecedor & "%' ORDER BY NomeFantasia"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FPesquisaFornecedoresGeral(ByVal Fornecedor As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT TOP 25 id_fornecedor, CNPJ, TipoEmpresa, Abertura, RazaoSocial, NomeFantasia, Porte,"
                sql += "NaturezaJuridica, Telefone, Estado, Status FROM tbl_Fornecedores WHERE NomeFantasia LIKE '%" & Fornecedor & "%' ORDER BY NomeFantasia"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FPesquisaProdutosCodigoBarrasEmEstoque(ByVal codigo_barras As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT id,data_compra,codigo_barras,produto,valor_venda,Saldo,estoque_minimo,Situacao,Status FROM tbl_estoque WHERE id LIKE '%" & codigo_barras & "%' ORDER BY id"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    Public Function FPesquisaProdutosCodigoBarras(ByVal codigo_barras As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT TOP 25 id,CodInterno,data_compra,codigo_barras,produto,valor_venda,Saldo,estoque_minimo,Situacao,Status FROM tbl_estoque WHERE codigo_barras LIKE '%" & codigo_barras & "%' ORDER BY codigo_barras"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    Public Function FCarregaSituacaoProdutosEmEstoque(ByVal situacao As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT id,data_compra,codigo_barras,produto,valor_venda,Saldo,estoque_minimo,Situacao,Status FROM tbl_estoque WHERE situacao = '" & situacao & "' ORDER BY produto"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                       & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaProdutosEmEstoque(ByVal produto As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Top 30 id, codigo_barras, data_compra, produto, Ncm, Cfop, Unidade, Saldo, valor_venda, Status FROM tbl_estoque WHERE produto LIKE '%" & produto & "%' ORDER BY produto"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                    & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaProdutosEntradaEstoque(ByVal produto As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String
                sql = "SELECT id,codigo_barras,produto,valor_unitario,saldo,estoque_minimo,situacao FROM tbl_estoque "

                If produto <> "" Then
                    sql += "WHERE produto LIKE '%" & produto & "%' "
                End If

                sql += "ORDER BY produto"

                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                      & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FCarregaMovimentoEstoque() As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT * FROM tbl_Movimento_Estoque ORDER BY Produto"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar carregar os produtos em estoque. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaMovimentoEntreDatas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT * FROM tbl_Movimento_Estoque "
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
    ' Pesquisar orçamentos
    Public Function FPesquisaOrcamentosCadastrados(ByVal Cliente As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Código, Data, Nu_orc, Qtd, Cliente, Codigo_produto, Descricao, Unit, Desconto, Total FROM tbl_Orcamento WHERE Cliente LIKE '%" & Cliente & "%' ORDER BY Cliente"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar o Orçamento cadastrado. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function
    ' Pesquisa serviços efetuados por loja
    Public Function FPesquisaServicosPorLojas(ByVal Loja As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Data, Loja, Montador, Codigo, Descricao, qunatidade, Valor FROM tbl_gastos WHERE Loja LIKE '%" & Loja & "%' ORDER BY Loja"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os Serviços prestados por lojas. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FPesquisaServicosPorMontador(ByVal Montador As String) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "SELECT Data, Loja, Montador, Codigo, Descricao, qunatidade, Valor FROM tbl_gastos WHERE Montador LIKE '%" & Montador & "%' ORDER BY Montador"
                Dim cmd As New OleDbCommand(sql, con)
                Dim dt As New OleDbDataAdapter(cmd)
                dt.Fill(tabela)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar consultar os Serviços prestados por Montador. " _
                                     & Environment.NewLine & "Erro: " & ex.Message, MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
        Return tabela
    End Function

    Public Function FConsultaXmlcupomEntreDatas(ByVal dataInicial As Date, ByVal dataFinal As Date) As DataTable
        Dim tabela As New DataTable
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String

                sql = "SELECT data_venda, venda_id, nota_fiscal, serie, nf_status, nfce_xml, total_itens, total_valor  FROM pedido_venda "
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


End Class
