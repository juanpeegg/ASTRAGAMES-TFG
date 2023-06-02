using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurationDatabase
{
    public class NinjaDb : SqliteHelper
    {
        private const String TABLE_NAME = "ConfiguracionNinja";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_SENSIBILIDAD = "sensibilidad";
        private const String KEY_NIVEL = "nivel";
        private const String KEY_POSICION = "posicion";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public NinjaDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_SENSIBILIDAD + " INTEGER, " +
                KEY_NIVEL + " INTEGER, " +
                KEY_POSICION + " INTEGER, " +
                "CONSTRAINT PK_CONFIGURACION PRIMARY KEY (" + KEY_JUEGO + ", " + KEY_JUGADOR + "))";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(NinjaEntity Configuracion)
        {
            //Comprobamos si existe ya una configuracion previa
            IDataReader reader = getConfiguracion(Configuracion._jugador);

            //Si existe se debe sobreescribir
            if (reader.Read())
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " +  KEY_SENSIBILIDAD + " = '" + Configuracion._sensibilidad + "', "
                                                                     + KEY_NIVEL + " = '" + Configuracion._nivel + "', "
                                                                     + KEY_POSICION + " = '" + Configuracion._posicion
                                                                     + "' WHERE "  + KEY_JUGADOR + " = '" + Configuracion._jugador + "'";
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
                + KEY_SENSIBILIDAD + ", "
                + KEY_NIVEL + ", "
                + KEY_POSICION + " ) "

                + "VALUES ( '"
                + Configuracion._juego + "', '"
                + Configuracion._jugador + "', '"
                + Configuracion._sensibilidad + "', '"
                + Configuracion._nivel + "', '"
                + Configuracion._posicion + "' )";
            dbcmd.ExecuteNonQuery();
            }
        }

        public NinjaEntity getConfigurationByName(string jugador)
        {
            IDataReader reader = getConfiguracion(jugador);
            if (reader.Read())
            {
                NinjaEntity configNinja = new NinjaEntity(reader["juego"].ToString(), reader["jugador"].ToString(), int.Parse(reader["sensibilidad"].ToString()), int.Parse(reader["nivel"].ToString()), int.Parse(reader["posicion"].ToString()));
                return configNinja;
            }
            else
            {
                return new NinjaEntity("Error", "Error", -1, -1,-1);
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
