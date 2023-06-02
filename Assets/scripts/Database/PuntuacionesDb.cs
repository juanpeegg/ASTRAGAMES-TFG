using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScoresDatabase
{
    public class PuntuacionesDb : SqliteHelper
    {
        private const String TABLE_NAME = "Puntuaciones";
        private const String KEY_ID = "idRanking";
        private const String KEY_JUEGO = "juego";
        private const String KEY_JUGADOR = "jugador";
        private const String KEY_PUNTUACION = "puntuacion";
        private const String KEY_FECHA = "fecha";

        // Este constructor llamara a SqliteHelper.SqliteHelper()
        public PuntuacionesDb() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                KEY_JUEGO + " TEXT NOT NULL, " +
                KEY_JUGADOR + " TEXT NOT NULL REFERENCES Usuarios(nombre), " +
                KEY_PUNTUACION + " INTEGER, " +
                KEY_FECHA + " DATETIME DEFAULT (datetime('now','localtime')))"; //por defecto UTC, lo cambiamos a CEST
            dbcmd.ExecuteNonQuery();
        }

        public void addData(PuntuacionesEntity Puntuacion)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_JUEGO + ", "
                + KEY_JUGADOR + ", "
                + KEY_PUNTUACION + " ) "

                + "VALUES ( '"
                + Puntuacion._juego + "', '"
                + Puntuacion._jugador + "', '"
                + Puntuacion._puntuacion + "' )";
            dbcmd.ExecuteNonQuery();
        }

        public int getBestScorePlayer(string strJuego, string strJugador)
        {
            IDataReader reader = getPuntuacionJugador(strJuego, strJugador);
            if (reader.Read())
            {
                return int.Parse(reader["puntuacion"].ToString());
            }
            else
            {
                return -1;
            }
        }

        public List<PuntuacionesEntity> getScoresPlayer(string strJuego, string strJugador)
        {
            List<PuntuacionesEntity> puntuacionesJugador = new List<PuntuacionesEntity>();
            IDataReader reader = getPuntuacionJugador(strJuego, strJugador);
            int i = 0;
            while (reader.Read() && i < 5)
            {
                PuntuacionesEntity puntuacion = new PuntuacionesEntity(strJuego, strJugador, int.Parse(reader["puntuacion"].ToString()), reader["fecha"].ToString());
                puntuacionesJugador.Add(puntuacion);
                i++;
            }
            return puntuacionesJugador;
        }

        public List<PuntuacionesEntity> getGlobalScore(string strJuego)
        {
            List<PuntuacionesEntity> puntuaciones = new List<PuntuacionesEntity>();
            IDataReader reader = getPuntuacionGlobal(strJuego);
            while (reader.Read())
            {
                PuntuacionesEntity puntuacion = new PuntuacionesEntity(strJuego, reader["jugador"].ToString(), int.Parse(reader["maxPuntuacion"].ToString()));
                puntuaciones.Add(puntuacion);
            }
            return puntuaciones;
        }

        public IDataReader getPuntuacionJugador(string strJuego, string strJugador)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_JUEGO + " = '" + strJuego + "'" + " AND "  + KEY_JUGADOR + " = '" + strJugador + "'" + " ORDER BY " + KEY_FECHA + " DESC";
            return dbcmd.ExecuteReader();
        }

        public IDataReader getPuntuacionGlobal(string strJuego)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT jugador, MAX(puntuacion) AS maxPuntuacion FROM " + TABLE_NAME + " WHERE " + KEY_JUEGO + " = '" + strJuego + "'" + " GROUP BY " + KEY_JUGADOR + " ORDER BY " + KEY_PUNTUACION + " DESC";
            return dbcmd.ExecuteReader();
        }

        public void deleteUsuario(string nombre)
        {
            base.deleteUsuario(TABLE_NAME, nombre);
        }
    }
}
