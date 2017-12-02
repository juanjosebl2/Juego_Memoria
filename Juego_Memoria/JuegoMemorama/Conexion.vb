Imports MySql.Data
Imports MySql.Data.MySqlClient

Module Conexion
    Dim conexion As New MySqlConnection

    Sub Conectar()
        Try
            conexion.ConnectionString = "server=localhost;User Id=root;database=desarrollointerfaces;Password="
            conexion.Open()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Module
