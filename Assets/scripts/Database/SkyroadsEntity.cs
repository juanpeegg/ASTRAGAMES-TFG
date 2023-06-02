using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class SkyroadsEntity
    {
        public string _juego;
        public string _jugador;
        public int _velRobot;
        public int _sensibilidad;

        public SkyroadsEntity(string juego, string jugador, int velRobot, int sensibilidad)
        {
            _juego = juego;
            _jugador = jugador;
            _velRobot = velRobot;
            _sensibilidad = sensibilidad;
        }
    }
}