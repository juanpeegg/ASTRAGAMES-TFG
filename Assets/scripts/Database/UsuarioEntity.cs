using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsersDatabase
{
    public class UsuarioEntity
    {
        public string _nombre;

        public UsuarioEntity(string nombre = "Usuario")
        {
            _nombre = nombre;
        }
    }
}