using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio1.Models
{
    //POCO: Plain Old C# Object

    public class Libro
    {
        public string Titulo { get; set; } = "";
        public string Autor { get; set; } = "";
        public string ImagenPortada { get; set; } = "";
    }

}
