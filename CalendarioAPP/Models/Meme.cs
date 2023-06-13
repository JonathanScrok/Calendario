using System;
using System.Collections.Generic;

namespace CalendarioAPP.Models
{
    public class Meme
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string MemeSobreQuem { get; set; }
        public string linkVideoYoutube { get; set; }
        public string linkInstagram { get; set; }
        public string UrlImg { get; set; }
        public string Autor { get; set; }
        public DateTime Dta_Cadastro { get; set; }
        public string Comentario { get; set; }
    }
}
