using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NossoCalendario.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

    }
}
