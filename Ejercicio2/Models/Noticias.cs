using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio2.Models
{
    public class Noticias
    {
        public string Titulo { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Contenido { get; set; } = "";
        public string URLImagen { get; set; } = "";
    }
}
