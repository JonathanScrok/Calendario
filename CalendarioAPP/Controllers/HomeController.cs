using CalendarioAPP.Models;
using CalendarioAPP.Models.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CalendarioAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<List<Evento>> ListarEventos()
        {
            EventoDbContext dbContext = new EventoDbContext();
            List<Evento> listaEventos = dbContext.Eventos.FindSync(m => true).ToList();

            return listaEventos;
        }

        [HttpGet]
        public void CriarEvento(string titulo, string horario, string data, string id)
        {
            EventoDbContext dbContext = new EventoDbContext();
            Evento evento = new Evento();
            evento.Id = Guid.NewGuid();
            evento.IdCriado = id;
            evento.Titulo = titulo;
            evento.Horario = horario;
            evento.DataEvento = data;

            dbContext.Eventos.InsertOne(evento);
        }

        [HttpGet]
        public void DeletarEvento(string titulo, string horario, string data, string id)
        {
            EventoDbContext dbContext = new EventoDbContext();
            //Deletar
        }
    }
}
