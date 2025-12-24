Imports System.Data.OleDb

Public Class DuplicacaoHelper
    
    ''' <summary>
    ''' Verifica se um item já existe na tabela pedido_venda_itens
    ''' </summary>
    ''' <param name="vendaId">ID da venda</param>
    ''' <param name="codigoProduto">Código do produto</param>
    ''' <returns>True se o item já existe</returns>
    Public Shared Function ItemJaExiste(vendaId As Integer, codigoProduto As String) As Boolean
        Try
            Using conn As OleDbConnection = GetConnection()
                conn.Open()
                
                Dim sql As String = "SELECT COUNT(*) FROM pedido_venda_itens WHERE venda_id = ? AND codigo_produto = ?"
                
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", vendaId)
                    cmd.Parameters.AddWithValue("?", codigoProduto)
                    
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao verificar duplicação de item: {ex.Message}")
            Return False
        End Try
    End Function
    
    ''' <summary>
    ''' Verifica se um pagamento já existe na tabela pedido_venda_condicaopagamento
    ''' </summary>
    ''' <param name="vendaId">ID da venda</param>
    ''' <param name="tipoPagamento">Tipo de pagamento</param>
    ''' <param name="valor">Valor do pagamento</param>
    ''' <returns>True se o pagamento já existe</returns>
    Public Shared Function PagamentoJaExiste(vendaId As Integer, formaPagamento As String, valor As Decimal) As Boolean
        Try
            Using con As OleDbConnection = GetConnection()
                con.Open()
                
                Dim sql As String = "SELECT COUNT(*) FROM pedido_venda_condicaopagamento WHERE venda_id = ? AND forma_pagamento = ? AND valor = ?"
                
                Using cmd As New OleDbCommand(sql, con)
                    cmd.Parameters.AddWithValue("@venda_id", vendaId)
                    cmd.Parameters.AddWithValue("@forma_pagamento", formaPagamento)
                    cmd.Parameters.AddWithValue("@valor", valor)
                    
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count > 0
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao verificar duplicação de pagamento: {ex.Message}")
            Return False
        End Try
    End Function
    

    
    ''' <summary>
    ''' Remove itens duplicados de uma venda específica
    ''' </summary>
    ''' <param name="vendaId">ID da venda</param>
    ''' <returns>Número de itens removidos</returns>
    Public Shared Function RemoverItensDuplicados(vendaId As Integer) As Integer
        Try
            Using conn As OleDbConnection = GetConnection()
                conn.Open()
                
                ' Remove duplicatas mantendo apenas o primeiro registro de cada produto
                Dim sql As String = "
                    DELETE FROM pedido_venda_itens 
                    WHERE venda_id = ? 
                    AND id NOT IN (
                        SELECT MIN(id) 
                        FROM pedido_venda_itens 
                        WHERE venda_id = ? 
                        GROUP BY codigo_produto
                    )"
                
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", vendaId)
                    cmd.Parameters.AddWithValue("?", vendaId)
                    
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao remover itens duplicados: {ex.Message}")
            Return 0
        End Try
    End Function
    
    ''' <summary>
    ''' Remove pagamentos duplicados de uma venda específica
    ''' </summary>
    ''' <param name="vendaId">ID da venda</param>
    ''' <returns>Número de pagamentos removidos</returns>
    Public Shared Function RemoverPagamentosDuplicados(vendaId As Integer) As Integer
        Try
            Using conn As OleDbConnection = GetConnection()
                conn.Open()
                
                ' Remove duplicatas mantendo apenas o primeiro registro de cada tipo de pagamento
                Dim sql As String = "
                    DELETE FROM pedido_venda_condicaopagamento 
                    WHERE venda_id = ? 
                    AND id NOT IN (
                        SELECT MIN(id) 
                        FROM pedido_venda_condicaopagamento 
                        WHERE venda_id = ? 
                        GROUP BY tipo_pagamento, valor
                    )"
                
                Using cmd As New OleDbCommand(sql, conn)
                    cmd.Parameters.AddWithValue("?", vendaId)
                    cmd.Parameters.AddWithValue("?", vendaId)
                    
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"Erro ao remover pagamentos duplicados: {ex.Message}")
            Return 0
        End Try
    End Function
    
    ''' <summary>
    ''' Limpa todas as duplicações de uma venda
    ''' </summary>
    ''' <param name="vendaId">ID da venda</param>
    ''' <returns>Relatório da limpeza</returns>
    Public Shared Function LimparDuplicacoes(vendaId As Integer) As String
        Dim itensRemovidos As Integer = RemoverItensDuplicados(vendaId)
        Dim pagamentosRemovidos As Integer = RemoverPagamentosDuplicados(vendaId)
        
        Return $"Venda {vendaId}: {itensRemovidos} itens duplicados removidos, {pagamentosRemovidos} pagamentos duplicados removidos"
    End Function
    
End Class