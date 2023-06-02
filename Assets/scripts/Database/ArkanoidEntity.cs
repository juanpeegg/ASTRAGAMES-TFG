using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class ArkanoidEntity
    {
        public string _juego;
        public string _jugador;
        public int _tamRaqueta;
        public int _velBola;
        public int _sensibilidad;
        public int _nivel;

        public ArkanoidEntity(string juego, string jugador, int tamRaqueta, int velBola, int sensibilidad, int nivel)
        {
            _juego = juego;
            _jugador = jugador;
            _tamRaqueta = tamRaqueta;
            _velBola = velBola;
            _sensibilidad = sensibilidad;
            _nivel = nivel;
        }
    }
}