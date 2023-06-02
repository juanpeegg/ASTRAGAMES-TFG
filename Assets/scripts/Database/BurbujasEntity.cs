using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationDatabase
{
    public class BurbujasEntity
    {
        public string _juego;
        public string _jugador;
        public int _tiempo;
        public int _formato;

        public BurbujasEntity(string juego, string jugador, int tiempo, int formato)
        {
            _juego = juego;
            _jugador = jugador;
            _tiempo = tiempo;
            _formato = formato;
        }
    }
}