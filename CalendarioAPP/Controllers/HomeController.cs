﻿using NossoCalendario.Models;
using NossoCalendario.Models.Mongo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace NossoCalendario.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // TIPOS DE LOG:
        //_logger.LogInformation($"Testando o NLog em {DateTime.UtcNow}");
        //_logger.LogWarning("NLog - Alerta");
        //_logger.Log(LogLevel.Error, ex, ex.Message);

        public IActionResult Index()
        {
            _logger.LogInformation(" Parte 1 - Abrindo a tela inicial ");

            return View();
        }

        [HttpGet]
        public JsonResult ListarEventosJson()
        {
            _logger.LogInformation(" Parte 2 - Chamou o ListarEventosJson ");
            try
            {
                EventoDbContext dbContext = new EventoDbContext();
                List<Evento> listaEventos = dbContext.Eventos.FindSync(m => true).ToList();

                if (listaEventos.Count > 0)
                    _logger.LogInformation($" Parte 3 - Carregou eventos! Eventos: {listaEventos.Count}");
                else
                    _logger.LogWarning($"NÃO CARREGOU EVENTOS! Eventos: {listaEventos.Count}");

                foreach (var evento in listaEventos)
                {
                    if (!string.IsNullOrEmpty(evento.Horario))
                    {
                        evento.Titulo = evento.Horario + " - " + evento.Titulo;
                    }
                }

                string json = JsonConvert.SerializeObject(listaEventos);

                _logger.LogInformation(" Parte 4 - ListarEventosJson realizado com sucesso! ");

                return new JsonResult(listaEventos);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("ERRO AO CARREGAR OS EVENTOS!!");
                _logger.Log(LogLevel.Error, ex, ex.Message);
                List<Evento> listaEventosVazia = new List<Evento>();
                return new JsonResult(listaEventosVazia);
            }
            
        }

        [HttpPost]
        public void CriarEvento(string titulo, string horario, string data, string id)
        {
            titulo = titulo.Substring(0, 1).ToUpper() + titulo.Substring(1);

            EventoDbContext dbContext = new EventoDbContext();
            Evento evento = new Evento();
            evento.Id = Guid.NewGuid();
            evento.IdCriado = id;
            evento.Titulo = titulo;

            string formato = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Horário Padrão de Brasília)'";
            DateTime dataFinal = DateTime.ParseExact(data, formato, System.Globalization.CultureInfo.InvariantCulture);

            string Minuto = string.Empty;
            string Hora = string.Empty;

            if (!string.IsNullOrEmpty(horario))
            {
                var detalheHorario = horario.Split(":");

                if (detalheHorario.Length > 0)
                {
                    if (!string.IsNullOrEmpty(detalheHorario[0]))
                    {
                        Hora = Regex.Replace(detalheHorario[0], "[^0-9]", "");

                        if (!string.IsNullOrEmpty(Hora))
                        {
                            if (Convert.ToInt32(Hora) > 23)
                                Hora = "23";
                        }
                    }
                }

                if (detalheHorario.Length > 1)
                {
                    if (!string.IsNullOrEmpty(detalheHorario[1]))
                    {
                        Minuto = Regex.Replace(detalheHorario[1], "[^0-9]", "");

                        if (Convert.ToInt32(Minuto) > 59)
                            Minuto = "59";
                    }
                }

                if (string.IsNullOrEmpty(Hora) && string.IsNullOrEmpty(Minuto))
                    evento.Horario = "";

                else
                    evento.Horario = (string.IsNullOrEmpty(Hora) ? "00" : Hora) + ":" + (string.IsNullOrEmpty(Minuto) ? "00" : Minuto);

            }
            else
            {
                evento.Horario = "";
            }

            //if (!string.IsNullOrEmpty(Hora))
            //    dataFinal = dataFinal.AddHours(Convert.ToInt32(Hora));
            //if (!string.IsNullOrEmpty(Minuto))
            //    dataFinal = dataFinal.AddMinutes(Convert.ToInt32(Minuto));

            evento.DataEvento = dataFinal.ToString("yyyy-MM-dd");

            dbContext.Eventos.InsertOne(evento);
        }

        [HttpPost]
        public void DeletarEvento(string id)
        {
            EventoDbContext dbContext = new EventoDbContext();
            dbContext.Eventos.DeleteOne(m => m.IdCriado == id);
            //Deletar
        }
    }
}
