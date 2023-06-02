using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurationDatabase
{
    public class BurbujasDb : SqliteHelper
    {
        private const String TABLE_NAME = "ConfiguracionColor";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_TIEMPO = "tiempo";
        private const String KEY_FORMATO = "formato";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public BurbujasDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_TIEMPO + " INTEGER, " +
                KEY_FORMATO + " INTEGER, " +
                "CONSTRAINT PK_CONFIGURACION PRIMARY KEY (" + KEY_JUEGO + ", " + KEY_JUGADOR + "))";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(BurbujasEntity Configuracion)
        {
            //Comprobamos si existe ya una configuracion previa
            IDataReader reader = getConfiguracion(Configuracion._jugador);

            //Si existe se debe sobreescribir
            if (reader.Read())
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_TIEMPO + " = '" + Configuracion._tiempo +
                    "', " + KEY_FORMATO + " = '" + Configuracion._formato +
                    "' WHERE "  + KEY_JUGADOR + " = '" + Configuracion._jugador + "'";
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
                + KEY_TIEMPO + ", "
                + KEY_FORMATO + " ) "

                + "VALUES ( '"
                + Configuracion._juego + "', '"
                + Configuracion._jugador + "', '"
                + Configuracion._tiempo + "', '"
                + Configuracion._formato + "' )";
            dbcmd.ExecuteNonQuery();
            }
        }

        public BurbujasEntity getConfigurationByName(string jugador)
        {
            IDataReader reader = getConfiguracion(jugador);
            if (reader.Read())
            {
                BurbujasEntity configBurbuja = new BurbujasEntity(reader["juego"].ToString(), reader["jugador"].ToString(), int.Parse(reader["tiempo"].ToString()), int.Parse(reader["formato"].ToString()));
                return configBurbuja;
            }
            else
            {
                return new BurbujasEntity("Error", "Error", -1, -1);
            }
        }

        public IDataReader getConfiguracion(string jugador)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_JUGADOR + " = '" + jugador + "'";
            return dbcmd.ExecuteReader();
        }

        public void deleteUsuario(string jugador)
        {
            Debug.Log("Deleting Usuario: " + jugador);

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_JUGADOR + " = '" + jugador + "'";
            dbcmd.ExecuteNonQuery();
        }
    }
}
