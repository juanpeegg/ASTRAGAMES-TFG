using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class NinjaEntity
    {
        public string _juego;
        public string _jugador;
        public int _sensibilidad;
        public int _nivel;
        public int _posicion;

        public NinjaEntity(string juego, string jugador, int sensibilidad, int nivel, int posicion)
        {
            _juego = juego;
            _jugador = jugador;
            _sensibilidad = sensibilidad;
            _nivel = nivel;
            _posicion = posicion;
        }
    }
}