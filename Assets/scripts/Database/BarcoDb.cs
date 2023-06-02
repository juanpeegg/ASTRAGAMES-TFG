using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurationDatabase
{
    public class BarcoDb : SqliteHelper
    {
        private const String TABLE_NAME = "ConfiguracionInvasion";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_VEL_BARCO = "velocidadBarco";
        private const String KEY_ENEMIGOS_IZQDA = "enemigosIzquierda";
        private const String KEY_ENEMIGOS_DCHA = "enemigosDerecha";
        private const String KEY_FREC_DISPARO = "frecuenciaDisparo";
        private const String KEY_BOSS = "boss";
        private const String KEY_SENSIBILIDAD = "sensibilidad";
        private const String KEY_NIVEL = "nivel";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public BarcoDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_VEL_BARCO + " INTEGER, " +
                KEY_ENEMIGOS_IZQDA + " INTEGER, " +
                KEY_ENEMIGOS_DCHA + " INTEGER, " +
                KEY_FREC_DISPARO + " INTEGER, " +
                KEY_BOSS + " INTEGER, " +
                KEY_SENSIBILIDAD + " INTEGER, " +
                KEY_NIVEL + " INTEGER, " +
                "CONSTRAINT PK_CONFIGURACION PRIMARY KEY (" + KEY_JUEGO + ", " + KEY_JUGADOR + "))";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(BarcoEntity Configuracion)
        {
            //Comprobamos si existe ya una configuracion previa
            IDataReader reader = getConfiguracion(Configuracion._jugador);

            //Si existe se debe sobreescribir
            if (reader.Read())
            {
                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_VEL_BARCO + " = '" + Configuracion._velBarco +
                    "', " + KEY_ENEMIGOS_IZQDA + " = '" + Configuracion._enemigosIzquierda +
                    "', " + KEY_ENEMIGOS_DCHA + " = '" + Configuracion._enemigosDerecha +
                    "', " + KEY_FREC_DISPARO + " = '" + Configuracion._frecuenciaDisparo +
                    "', " + KEY_BOSS + " = '" + Configuracion._boss +
                    "', " + KEY_SENSIBILIDAD + " = '" + Configuracion._sensibilidad +
                    "', " + KEY_NIVEL + " = '" + Configuracion._nivel +
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
                    + KEY_VEL_BARCO + ", "
                    + KEY_ENEMIGOS_IZQDA + ", "
                    + KEY_ENEMIGOS_DCHA + ", "
                    + KEY_FREC_DISPARO + ", "
                    + KEY_BOSS + ", "
                    + KEY_SENSIBILIDAD + ", "
                    + KEY_NIVEL + " ) " 

                    + "VALUES ( '"
                    + Configuracion._juego + "', '"
                    + Configuracion._jugador + "', '"
                    + Configuracion._velBarco + "', '" 
                    + Configuracion._enemigosIzquierda + "', '" 
                    + Configuracion._enemigosDerecha + "', '" 
                    + Configuracion._frecuenciaDisparo + "', '"
                    + Configuracion._boss + "', '"
                    + Configuracion._sensibilidad + "', '"
                    + Configuracion._nivel + "' )";
                dbcmd.ExecuteNonQuery();
            }
        }

        public BarcoEntity getConfigurationByName(string jugador)
        {
            IDataReader reader = getConfiguracion(jugador);
            if (reader.Read())
            {
                BarcoEntity configBarco = new BarcoEntity(reader["juego"].ToString(), reader["jugador"].ToString(),
                            int.Parse(reader["velocidadBarco"].ToString()), int.Parse(reader["enemigosIzquierda"].ToString()),
                            int.Parse(reader["enemigosDerecha"].ToString()), int.Parse(reader["frecuenciaDisparo"].ToString()),
                            int.Parse(reader["boss"].ToString()), int.Parse(reader["sensibilidad"].ToString()),
                            int.Parse(reader["nivel"].ToString()));
                return configBarco;
            }
            else
            {
                return new BarcoEntity("Error", "Error", -1, -1, -1, -1, -1, -1,-1);
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
