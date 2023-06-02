using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsersDatabase
{
    public class UsuarioDb : SqliteHelper
    {
        private const String TABLE_NAME = "Usuarios";
        private const String KEY_JUGADOR = "jugador";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public UsuarioDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUGADOR + " TEXT PRIMARY KEY ) ";
            dbcmd.ExecuteNonQuery();
        }

        public int addData(UsuarioEntity Usuario)
        {
            //Comprobar que el nombre es unico
            IDataReader reader = getNombre(Usuario._nombre);

            //Si ya existe ese usuario
            if (reader.Read())
            {
                Debug.Log("Ya existe un usuario con ese nombre");
                return 1;
            }
            else
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText =
                    "INSERT INTO " + TABLE_NAME
                    + " ( "
                    + KEY_JUGADOR + " ) "

                    + "VALUES ( '"
                    + Usuario._nombre + "' )";
                dbcmd.ExecuteNonQuery();
                return 0;
            }
        }

        public UsuarioEntity getUserByName(string str)
        {
            IDataReader reader = getNombre(str);
            if (reader.Read())
            {
                UsuarioEntity usuario = new UsuarioEntity(reader[0].ToString());
                return usuario;
            }
            else
            {
                return new UsuarioEntity("Error");
            }
        }

        public List<UsuarioEntity> getAllUsers()
        {
            List<UsuarioEntity> usuarios = new List<UsuarioEntity>();
            IDataReader reader = getAllData();
            while (reader.Read())
            {
                UsuarioEntity usuario = new UsuarioEntity(reader[0].ToString());
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        public IDataReader getNombre(string str)
        {
            Debug.Log("Getting Usuario: " + str);

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_JUGADOR + " = '" + str + "'";
            return dbcmd.ExecuteReader();
        }

        public void deleteUsuario(string nombre)
        {
            base.deleteUsuario(TABLE_NAME, nombre);
        }

        public IDataReader getAllData()
        {
            return base.getAllData(TABLE_NAME);
        }
    }
}
