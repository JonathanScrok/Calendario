using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarioAPP.Models
{
    public class Evento
    {
        public Guid Id { get; set; }
        public string IdCriado { get; set; }
        public string Titulo { get; set; }
        public string Horario { get; set; }
        public string DataEvento { get; set; }
    }
}
