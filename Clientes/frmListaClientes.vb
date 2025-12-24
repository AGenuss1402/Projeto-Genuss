Imports System.Data.OleDb
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Data

Public Class frmListaClientes
    Dim classClientes As New clsClientes

    Private Sub lblFechar_Click(sender As Object, e As EventArgs)
        frmMetroPrincipal.AbrirFormEnPanel(New frmDhastebord())
    End Sub

    Private Sub frmListaClientes_Load(sender As Object, e As EventArgs) Handles Me.Load
        cmbTipoConsulta.selectedIndex = 1
        dgvClientes.DataSource = classClientes.FCarregaClientesCadastrados
        PFormataDataGridViewClientes()

    End Sub
    Private Sub PFormataDataGridViewClientes()
        With Me.dgvClientes

            .Columns(2).HeaderText = "Codigo"
            .Columns(3).HeaderText = "Data Cadastro"
            .Columns(4).HeaderText = "Nome cliente"
            .Columns(5).HeaderText = "Apelido"
            .Columns(6).HeaderText = "Documento"
            .Columns(7).HeaderText = "Endereço"
            .Columns(8).HeaderText = "Bairro"
            .Columns(9).HeaderText = "Postal"
            .Columns(10).HeaderText = "Cidade"
            .Columns(11).HeaderText = "Estado"
            .Columns(12).HeaderText = "Telefone"
            .Columns(13).HeaderText = "Celular"
            .Columns(14).HeaderText = "Sexo"
            .Columns(15).HeaderText = "E-data_nascimento"
            .Columns(16).HeaderText = "E-Email"

            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(5).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(6).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(7).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(8).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(9).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(10).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(11).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(12).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(13).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(14).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(15).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .Columns(16).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            'Define o alinhamamento das rows
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        End With
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs)
        frmClientes.Show()
    End Sub
    Private Sub lblFechar_Click_1(sender As Object, e As EventArgs)
        frmMetroPrincipal.AbrirFormEnPanel(New frmDhastebord())
    End Sub

    Private Sub lblFechar_Click_2(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub dgvClientes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvClientes.CellContentClick
        If e.ColumnIndex = 0 Then
            intPergunta = MsgBox("Tem certeza que gostaria de excluir permanentemente o cliente `" &
                                               dgvClientes.CurrentRow().Cells(4).Value.ToString & "` da lista?", MsgBoxStyle.Question + MsgBoxStyle.YesNo)
            If intPergunta <> vbYes Then Exit Sub
            PExcluiCliente()
            dgvClientes.Rows.RemoveAt(dgvClientes.CurrentRow.Index)
        End If
    End Sub

    Private Sub dgvClientes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvClientes.CellDoubleClick
        frmClientes.txtCodigo.Text = dgvClientes.CurrentRow().Cells(2).Value
        frmClientes.PLerDadosCliente(frmClientes.txtCodigo.Text)
        frmClientes.ShowDialog()
    End Sub

    Private Sub PExcluiCliente()
        Using con As OleDbConnection = GetConnection()
            Try
                con.Open()
                Dim sql As String = "DELETE FROM tbl_clientes WHERE id_cliente = " & CInt(dgvClientes.CurrentRow().Cells(2).Value)
                Dim cmd As New OleDbCommand(sql, con)
                cmd.ExecuteNonQuery()

                MsgBox("O cliente foi excluído com sucesso.", MsgBoxStyle.Information, My.Application.Info.CompanyName)
            Catch ex As Exception
                MsgBox("Ocorreu um erro ao tentar excluir o cliente.", MsgBoxStyle.Critical, My.Application.Info.CompanyName)
            Finally
                con.Close()
            End Try
        End Using
    End Sub

    Private Sub txtConsultaCliente_OnTextChange(sender As Object, e As EventArgs) Handles txtConsultaCliente.OnTextChange
        If cmbTipoConsulta.selectedIndex = 0 Then
            dgvClientes.DataSource = classClientes.FPesquisaClientesCodigo(txtConsultaCliente.text)
        Else
            dgvClientes.DataSource = classClientes.FConsultaClientesNome(txtConsultaCliente.text)
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub lblFechar_Click_3(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnNovoCliente_Click(sender As Object, e As EventArgs) Handles btnNovoCliente.Click
        frmClientes.Show()
    End Sub

    Private Sub btnAtualizarCliente_Click(sender As Object, e As EventArgs) Handles btnAtualizarCliente.Click
        dgvClientes.DataSource = classClientes.FCarregaClientesCadastrados
        PFormataDataGridViewClientes()
    End Sub

    Private Sub btnEportar_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub lblFechar_Click_4(sender As Object, e As EventArgs) Handles lblFechar.Click
        Me.Close()
    End Sub
End Class