using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurationDatabase
{
    public class ArkanoidDb : SqliteHelper
    {
        private const String TABLE_NAME = "ConfiguracionArkanoid";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_RAQUETA = "tamanoRaqueta";
        private const String KEY_BOLA = "velocidadBola";
        private const String KEY_SENSIBILIDAD = "sensibilidad";
        private const String KEY_NIVEL = "nivel";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public ArkanoidDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_RAQUETA + " INTEGER, " +
                KEY_BOLA + " INTEGER, " +
                KEY_SENSIBILIDAD + " INTEGER, " +
                KEY_NIVEL + " INTEGER, " +
                "CONSTRAINT PK_CONFIGURACION PRIMARY KEY (" + KEY_JUEGO + ", " + KEY_JUGADOR + "))";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(ArkanoidEntity Configuracion)
        {
            //Comprobamos si existe ya una configuracion previa
            IDataReader reader = getConfiguracion(Configuracion._jugador);

            //Si existe se debe sobreescribir
            if (reader.Read())
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_RAQUETA + " = '" + Configuracion._tamRaqueta + "', " + KEY_BOLA + " = '" + Configuracion._velBola + "', " + KEY_SENSIBILIDAD + " = '" + Configuracion._sensibilidad + "', " + KEY_NIVEL + " = '" + Configuracion._nivel + "' WHERE "  + KEY_JUGADOR + " = '" + Configuracion._jugador + "'";
                dbcmd.ExecuteNonQuery();
            }
            else
            {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_JUEGO + ", "
                + KEY_JUGADOR + ", "
                + KEY_RAQUETA + ", " 
                + KEY_BOLA + ", " 
                + KEY_SENSIBILIDAD + ", " 
                + KEY_NIVEL + " ) "

                + "VALUES ( '"
                + Configuracion._juego + "', '"
                + Configuracion._jugador + "', '"
                + Configuracion._tamRaqueta + "', '" 
                + Configuracion._velBola + "', '" 
                + Configuracion._sensibilidad + "', '" 
                + Configuracion._nivel + "' )";
            dbcmd.ExecuteNonQuery();
            }
        }

        public ArkanoidEntity getConfigurationByName(string jugador)
        {
            IDataReader reader = getConfiguracion(jugador);
            if (reader.Read())
            {
                ArkanoidEntity configArkanoid = new ArkanoidEntity(reader["juego"].ToString(), reader["jugador"].ToString(), int.Parse(reader["tamanoRaqueta"].ToString()), int.Parse(reader["velocidadBola"].ToString()), int.Parse(reader["sensibilidad"].ToString()), int.Parse(reader["nivel"].ToString()));
                return configArkanoid;
            }
            else
            {
                return new ArkanoidEntity("Error", "Error", -1, -1, -1, -1);
            }
        }

        public IDataReader getConfiguracion(string jugador)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_JUGADOR + " = '" + jugador + "'";
            return dbcmd.ExecuteReader();
        }

        public void deleteUsuario(string nombre)
        {
            base.deleteUsuario(TABLE_NAME, nombre);
        }
    }
}
