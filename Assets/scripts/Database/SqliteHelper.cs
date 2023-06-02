using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Data;

public class SqliteHelper
{
    private const string database_name = "myDB.sqlite";

    public string db_connection_string;
    public IDbConnection db_connection;


    public SqliteHelper()
    {
        db_connection_string = "URI=file:" + Application.streamingAssetsPath + "/" + database_name;
        //Debug.Log("Mi conexion SQL se lanza desde el fichero: " + db_connection_string);
        db_connection = new SqliteConnection(db_connection_string);
        db_connection.Open();
    }

    ~SqliteHelper()
    {
        db_connection.Close();
    }

    //helper functions
    public IDbCommand getDbCommand()
    {
        return db_connection.CreateCommand();
    }


    public IDataReader getAllData(string table_name)
    {
        IDbCommand dbcmd = db_connection.CreateCommand();
        dbcmd.CommandText = "SELECT * FROM " + table_name;
        IDataReader reader = dbcmd.ExecuteReader();
        return reader;
    }

    public void close()
    {
        db_connection.Close();
    }

    public void deleteUsuario(string table_name, string usuario)
    {
        Debug.Log("Deleting Usuario: " + usuario);

        IDbCommand dbcmd = getDbCommand();
        dbcmd.CommandText = "DELETE FROM " + table_name + " WHERE jugador = '" + usuario + "'";
        dbcmd.ExecuteNonQuery();
    }
}
