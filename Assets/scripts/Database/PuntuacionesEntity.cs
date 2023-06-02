using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoresDatabase
{
    public class PuntuacionesEntity
    {
        public string _juego;
        public string _jugador;
        public int _puntuacion;
        public String _fecha;


        public PuntuacionesEntity(string juego, string jugador, int puntuacion)
        {
            _juego = juego;
            _jugador = jugador;
            _puntuacion = puntuacion;
        }

        public PuntuacionesEntity(string juego, string jugador, int puntuacion, String fecha)
        {
            _juego = juego;
            _jugador = jugador;
            _puntuacion = puntuacion;
            _fecha = fecha;
        }
    }
}