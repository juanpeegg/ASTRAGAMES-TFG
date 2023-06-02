using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class ColorEntity
    {
        public string _juego;
        public string _jugador;
        public int _tiempo;

        public ColorEntity(string juego, string jugador, int tiempo)
        {
            _juego = juego;
            _jugador = jugador;
            _tiempo = tiempo;
        }
    }
}