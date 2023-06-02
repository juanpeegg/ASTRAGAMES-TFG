using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class BarcoEntity
    {
        public string _juego;
        public string _jugador;
        public int _velBarco;
        public int _enemigosIzquierda;
        public int _enemigosDerecha;
        public int _frecuenciaDisparo;
        public int _boss;
        public int _sensibilidad;
        public int _nivel;

        public BarcoEntity(string juego, string jugador, int velBarco, int enemigosIzquierda, 
            int enemigosDerecha, int frecuenciaDisparo, int boss, int sensibilidad, int nivel)
        {
            _juego = juego;
            _jugador = jugador;
            _velBarco = velBarco;
            _enemigosIzquierda = enemigosIzquierda;
            _enemigosDerecha = enemigosDerecha;
            _frecuenciaDisparo = frecuenciaDisparo;
            _boss = boss;
            _sensibilidad = sensibilidad;
            _nivel = nivel;
        }
    }
}