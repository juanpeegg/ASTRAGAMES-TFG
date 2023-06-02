using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurationDatabase
{
    public class SkyroadsDb : SqliteHelper
    {
        private const String TABLE_NAME = "ConfiguracionSkyroads";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_ROBOT = "velocidadRobot";
        private const String KEY_SENSIBILIDAD = "sensibilidad";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public SkyroadsDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_ROBOT + " INTEGER, " +
                KEY_SENSIBILIDAD + " INTEGER, " +
                "CONSTRAINT PK_CONFIGURACION PRIMARY KEY (" + KEY_JUEGO + ", " + KEY_JUGADOR + "))";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(SkyroadsEntity Configuracion)
        {
            //Comprobamos si existe ya una configuracion previa
            IDataReader reader = getConfiguracion(Configuracion._jugador);

            //Si existe se debe sobreescribir
            if (reader.Read())
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_ROBOT + " = '" + Configuracion._velRobot + "', " + KEY_SENSIBILIDAD + " = '" + Configuracion._sensibilidad + "' WHERE "  + KEY_JUGADOR + " = '" + Configuracion._jugador + "'";
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
                + KEY_ROBOT + ", " 
                + KEY_SENSIBILIDAD + " ) " 

                + "VALUES ( '"
                + Configuracion._juego + "', '"
                + Configuracion._jugador + "', '"
                + Configuracion._velRobot + "', '" 
                + Configuracion._sensibilidad + "' )";
            dbcmd.ExecuteNonQuery();
            }
        }

        public SkyroadsEntity getConfigurationByName(string jugador)
        {
            IDataReader reader = getConfiguracion(jugador);
            if (reader.Read())
            {
                SkyroadsEntity configSkyroads = new SkyroadsEntity(reader["juego"].ToString(), reader["jugador"].ToString(), int.Parse(reader["velocidadRobot"].ToString()), int.Parse(reader["sensibilidad"].ToString()));
                return configSkyroads;
            }
            else
            {
                return new SkyroadsEntity("Error", "Error", -1, -1);
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
