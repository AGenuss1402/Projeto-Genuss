
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Data.SQLite

Public Class clsPedidoVenda
    Private _connString As String

    Public Function SalvarPedido(pedido As aPedido) As Integer
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            Dim trans As OleDbTransaction = conn.BeginTransaction()

            Try
                ' Início da transação


                ' Gerar próximo venda_id sequencial
                Dim cmdMaxId As New OleDbCommand("SELECT MAX(venda_id) FROM pedido_venda", conn, trans)
                Dim maxIdResult = cmdMaxId.ExecuteScalar()
                Dim novoVendaId As Integer = If(maxIdResult Is DBNull.Value OrElse maxIdResult Is Nothing, 1, Convert.ToInt32(maxIdResult) + 1)

                ' Atualizar o pedido com o novo ID
                pedido.venda_id = novoVendaId

                ' Inserir pedido
                Dim cmdPedido As New OleDbCommand("
                    INSERT INTO pedido_venda 
                    (venda_id, data, emitente_cnpj, documento, nome, caixa, operador,
                     natureza_operacao, uf_origem, uf_destino, 
                     total_valor, total_recebido, troco, obs, Status, total_itens, data_venda, vlr_inicial, suprimento, retirada) 
                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", conn, trans)
                With cmdPedido.Parameters
                    .AddWithValue("?", pedido.venda_id)
                    .AddWithValue("?", pedido.data)
                    .AddWithValue("?", Regex.Replace(pedido.emitente_cnpj, "[^\d]", ""))
                    .AddWithValue("?", pedido.documento)
                    .AddWithValue("?", pedido.nome)
                    .AddWithValue("?", pedido.caixa)
                    .AddWithValue("?", pedido.operador)
                    .AddWithValue("?", pedido.natureza_operacao)
                    .AddWithValue("?", pedido.uf_origem)
                    .AddWithValue("?", pedido.uf_destino)
                    .AddWithValue("?", pedido.total_valor)
                    .AddWithValue("?", pedido.total_recebido)
                    .AddWithValue("?", pedido.troco)
                    .AddWithValue("?", pedido.obs)
                    .AddWithValue("?", pedido.Status)
                    .AddWithValue("?", pedido.total_itens)
                    .AddWithValue("?", pedido.dat_lancamento)
                    .AddWithValue("?", pedido.vlr_ininial)
                    .AddWithValue("?", pedido.vlr_suprimento)
                    .AddWithValue("?", pedido.vlr_retirada)
                End With

                ' Executa o comando
                cmdPedido.ExecuteNonQuery()
                ' Confirma transação
                trans.Commit()
                
                ' Retorna o ID da venda que foi salva
                Return pedido.venda_id

            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Erro ao salvar dados: " & ex.Message)
            End Try
        End Using
    End Function


    Public Sub SalvarResumoPedido(pedido As aPedido)
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            Dim trans As OleDbTransaction = conn.BeginTransaction()
            Try
                ' Início da transação

                Dim cmdItem As New OleDbCommand("
                        UPDATE pedido_venda SET total_recebido=?,troco=? WHERE venda_id =" & pedido.venda_id.ToString, conn, trans)

                With cmdItem.Parameters
                    .AddWithValue("?", pedido.total_recebido)
                    .AddWithValue("?", pedido.troco)
                End With
                cmdItem.ExecuteNonQuery()
                ' Confirma transação
                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Erro ao salvar dados: " & ex.Message)
            End Try
        End Using
    End Sub



    Public Sub SalvarItensPedido(item As aItensPedido)
        ' Verifica se o item já existe para evitar duplicação
        If DuplicacaoHelper.ItemJaExiste(item.venda_id, item.codigo_produto) Then
            Console.WriteLine($"Item {item.codigo_produto} já existe na venda {item.venda_id}. Pulando inserção.")
            Return
        End If
        
        Using conn As OleDbConnection = GetConnection()
            conn.Open()
            ' Início da transação
            Dim trans As OleDbTransaction = conn.BeginTransaction()
            Try
                ' Inserir itens
                Dim cmdItem As New OleDbCommand("
                        INSERT INTO pedido_venda_itens 
                        ( venda_id, Data, codigo_produto, descricao, quantidade, valor_unitario, unidade_medida, valor_total, cfop, ncm ) 
                        VALUES (?,?,?,?,?,?,?,?,?,?)", conn, trans)
                With cmdItem.Parameters
                    .AddWithValue("?", item.venda_id)
                    .AddWithValue("?", item.dat_venda)
                    .AddWithValue("?", item.codigo_produto)
                    .AddWithValue("?", item.descricao)
                    .AddWithValue("?", item.quantidade)
                    .AddWithValue("?", item.valor_unitario)
                    .AddWithValue("?", item.unidade_medida)
                    .AddWithValue("?", item.valor_total)
                    .AddWithValue("?", item.cfop)
                    .AddWithValue("?", item.ncm)
                End With
                cmdItem.ExecuteNonQuery()
                ' Confirma transação
                trans.Commit()
                
                Console.WriteLine($"Item {item.codigo_produto} salvo com sucesso na venda {item.venda_id}")

            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Erro ao salvar dados: " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub SalvarCondicaoPagamento(Pagamentos As aCodicaoPagamento)
        ' Verifica se o pagamento já existe para evitar duplicação
        If DuplicacaoHelper.PagamentoJaExiste(Pagamentos.venda_id, Pagamentos.tipo_pagamento, Pagamentos.valor) Then
            Console.WriteLine($"Pagamento {Pagamentos.tipo_pagamento} de R$ {Pagamentos.valor} já existe na venda {Pagamentos.venda_id}. Pulando inserção.")
            Return
        End If
        
        Using conn As OleDbConnection = GetConnection()
            conn.Open()

            ' Início da transação
            Dim trans As OleDbTransaction = conn.BeginTransaction()

            Try
                ' Gerar venda_id sequencial se for 0
                If Pagamentos.venda_id = 0 Then
                    Dim cmdMax As New OleDbCommand("SELECT MAX(venda_id) FROM pedido_venda", conn, trans)
                    Dim maxResult = cmdMax.ExecuteScalar()
                    Dim novoVendaId As Integer = If(IsDBNull(maxResult) OrElse maxResult Is Nothing, 1, CInt(maxResult) + 1)
                    Pagamentos.venda_id = novoVendaId
                End If

                ' Inserir formas de pagamento

                Dim cmdPgto As New OleDbCommand("
                        INSERT INTO pedido_venda_condicaopagamento  
                        ( data, venda_id, tipo_pagamento, valor, tef_payment_type, tef_parcela, tef_tipo_ordem,tef_documento, tef_nome) 
                        VALUES (?,?,?,?,?,?,?,?,?)", conn, trans)
                With cmdPgto.Parameters

                    .AddWithValue("?", Pagamentos.dat_pagamento)
                    .AddWithValue("?", Pagamentos.venda_id)
                    .AddWithValue("?", Pagamentos.tipo_pagamento)
                    .AddWithValue("?", Pagamentos.valor)
                    .AddWithValue("?", Pagamentos.tef_payment_type)
                    .AddWithValue("?", Pagamentos.tef_parcela)
                    .AddWithValue("?", Pagamentos.tef_tipo_ordem)
                    .AddWithValue("?", Pagamentos.tef_documento)
                    .AddWithValue("?", Pagamentos.tef_nome)

                End With
                cmdPgto.ExecuteNonQuery()

                '.AddWithValue("?", Pagamentos.tef_idtef)
                '.AddWithValue("?", Pagamentos.tef_status)

                ' Confirma transação
                trans.Commit()
                
                Console.WriteLine($"Pagamento {Pagamentos.tipo_pagamento} de R$ {Pagamentos.valor} salvo com sucesso na venda {Pagamentos.venda_id}")

            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Erro ao salvar dados: " & ex.Message)
            End Try
        End Using
    End Sub
    Public Function BuscarDadosPedido(numeroPedido As Integer) As aPedido
        Using conn As OleDbConnection = GetConnection()
            conn.Open()

            Dim sql As String = "SELECT * FROM pedido_venda WHERE venda_id = ?"

            Using cmd As New OleDbCommand(sql, conn)
                cmd.Parameters.AddWithValue("?", numeroPedido)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return New aPedido With {
                        .nota_fiscal = reader("nota_fiscal").ToString(),
                        .serie = reader("serie").ToString(),
                        .operador = Convert.ToInt32(reader("operador")),
                        .caixa = Convert.ToInt32(reader("caixa")),
                        .total_valor = Convert.ToDouble(reader("total_valor")),
                        .total_recebido = Convert.ToDouble(reader("total_recebido")),
                        .total_itens = Convert.ToInt16(reader("total_itens")),
                        .vdesc = Convert.ToDouble(reader("vdesc")),
                        .troco = Convert.ToDouble(reader("troco")),
                        .nf_chave = reader("nfce_chave").ToString(),
                .nf_protocolo = reader("nfce_protocolo").ToString(),
                .nf_dt_autorizacao = reader("nf_dt_autorizacao").ToString(),
                .nf_url_qrcode = reader("nfce_qrcode").ToString(),
                        .nf_status = reader("nf_status").ToString()
                    }
                    End If
                End Using
            End Using
        End Using

        Return Nothing
    End Function



End Class


Public Class aPedido
    Public Property data As Date
    Public Property venda_id As Integer
    Public Property emitente_cnpj As String
    Public Property credenciado_client_id As String
    Public Property documento As String
    Public Property nome As String

    Public Property caixa As Integer
    Public Property operador As Integer
    Public Property natureza_operacao As String
    Public Property uf_origem As String
    Public Property uf_destino As String
    Public Property vend01 As String
    Public Property vend02 As String
    Public Property vend03 As String
    Public Property vend04 As String
    Public Property vend05 As String

    Public Property vend01_porcentagem As Double
    Public Property vend02_porcentagem As Double
    Public Property vend03_porcentagem As Double
    Public Property vend04_porcentagem As Double
    Public Property vend05_porcentagem As Double

    Public Property vend01_comissao As Double
    Public Property vend02_comissao As Double
    Public Property vend03_comissao As Double
    Public Property vend04_comissao As Double
    Public Property vend05_comissao As Double
    Public Property terminal_tef As String
    Public Property valor_comissao As Double
    Public Property vfrete As Double
    Public Property vseg As Double
    Public Property vdesc As Double
    Public Property voutros As Double
    Public Property total_valor As Double
    Public Property total_recebido As Double
    Public Property troco As Double
    Public Property obs As String
    Public Property nota_fiscal As String
    Public Property serie As String
    Public Property nf_api_id As Integer
    Public Property nf_modelo As String 'modelo de nota fiscal  55 Danfe / 65 NFC-e
    Public Property nf_chave As String
    Public Property nf_protocolo As String
    Public Property nf_dt_autorizacao As String
    Public Property nf_url_qrcode As String
    Public Property nf_status As String
    Public Property nfce_xml As String
    Public Property code64_nfce As String
    Public Property total_itens As Integer
    Public Property dat_lancamento As Date
    Public Property vlr_ininial As Double
    Public Property vlr_suprimento As Double
    Public Property vlr_retirada As Double
    Public Property Status As String


End Class


Public Class aItensPedido
    Public Property venda_id As Integer
    Public Property dat_venda As date
    Public Property codigo_produto As String
    Public Property descricao As String
    Public Property quantidade As Double
    Public Property valor_unitario As Double
    Public Property unidade_medida As String

    Public Property valor_total As Double
    Public Property cfop As String
    Public Property ncm As String

End Class

Public Class aCodicaoPagamento

    Public Property dat_pagamento As Date
    Public Property venda_id As Integer
    Public Property tipo_pagamento As String
    Public Property valor As Double
    Public Property tef_payment_type As String
    Public Property tef_parcela As Integer
    Public Property tef_tipo_ordem As String
    Public Property tef_documento As String
    Public Property tef_nome As String
    Public Property tef_idtef As String
    Public Property tef_status As String

End Class
