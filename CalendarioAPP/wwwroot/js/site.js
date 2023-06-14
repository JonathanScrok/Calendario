document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var jsonString;
    var objetoEventos;
    var listaobj = [];
    //var calendar;
    var events = [];
    let SITE_URL = window.location.origin;

    $.ajax({
        method: "GET",
        url: SITE_URL + "/home/ListarEventosJson/",
        contentType: "application/json"
    }).done(function (result) {

        result.forEach(function (elemento) {
            //console.log(elemento.titulo);
            events.push({
                title: elemento.titulo,
                start: elemento.dataEvento,
                end: elemento.dataEvento,
                id: elemento.idCriado
            });
        })
        //console.log(events);
        GenerateCalender(events);

        function GenerateCalender(events) {

            var calendar = new FullCalendar.Calendar(calendarEl, {
                locale: 'pt-br',
                headerToolbar: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                //initialDate: '2023-01-12',
                navLinks: true, // can click day/week names to navigate views
                selectable: true,
                selectMirror: true,
                select: function (arg) {
                    var horarioevento = prompt('Horário do evento:');
                    var tituloevento = prompt('Adicionar Evento:');

                    if (tituloevento) {
                        var dataevento = arg.start;
                        const data = new Date(dataevento);
                        const dia = data.getDate();
                        const mes = data.getMonth() + 1;
                        const ano = data.getFullYear();
                        let idevento = `${dia < 10 ? '0' + dia : dia}${mes < 10 ? '0' + mes : mes}${ano}`;
                        let numeroAleatorio = Math.floor(Math.random() * 200) + 1;
                        idevento = idevento + "" + numeroAleatorio

                        $.ajax({
                            method: "POST",
                            url: SITE_URL + "/home/criarevento/?titulo=" + tituloevento + "&horario=" + horarioevento + "&data=" + dataevento + "&id=" + idevento,
                            contentType: "application/json"
                        }).done(function (result) {
                            //console.log(err);
                        }).fail(function (result) {
                            //console.log(err);
                        });

                        tituloevento = primeiraLetraMaiuscula(tituloevento);
                        if (horarioevento) {
                            let horarioDivisao = horarioevento.split(":");
                            let horaFinal;
                            let horas = horarioDivisao[0];
                            let minutos = horarioDivisao[1];

                            if (horas != null && horas != "") {
                                horas = horas.replace(/[a-zA-Z]/g, "");
                            }

                            if (minutos != null && minutos != "") {
                                minutos = minutos.replace(/[a-zA-Z]/g, "");
                            }

                            if (horas == "" && minutos == "") {
                                horaFinal = "";
                            }
                            if (minutos == "" || minutos == null) {
                                minutos = "00";
                            }
                            if (horas == "" || horas == null) {
                                horas = "00";
                            }

                            if (parseInt(horas) > 23)
                                horas = "23";

                            if (parseInt(minutos) > 59)
                                minutos = "59";

                            horaFinal = horas + ":" + minutos;

                            if (horaFinal != "") {
                                tituloevento = horaFinal + " - " + tituloevento
                            }
                        }

                        calendar.addEvent({
                            title: tituloevento,
                            id: idevento,
                            start: arg.start,
                            end: arg.end,
                            allDay: arg.allDay
                        })
                    }

                    calendar.unselect()
                },
                eventClick: function (arg) {
                    if (confirm('Deseja excluir este evento?')) {

                        let id = arg.event._def.publicId;
                        $.ajax({
                            method: "POST",
                            url: SITE_URL + "/home/deletarevento/?id=" + id,
                            contentType: "application/json"
                        }).done(function (result) {
                            //console.log(err);
                        }).fail(function (result) {
                            //console.log(err);
                        });

                        arg.event.remove()
                    }
                },
                editable: false,
                dayMaxEvents: true, // allow "more" link when too many events
                events: events
            });

            calendar.render();
        }

    }).fail(function (err) {
        //console.log(err);
    });

    function primeiraLetraMaiuscula(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }

    //function esperarValorVariavel(callback) {
    //    var intervalo = setInterval(function () {
    //        if (events) {
    //            clearInterval(intervalo);
    //            callback();
    //        }
    //    }, 100); // Verificar a cada 100 milissegundos
    //}

    //esperarValorVariavel(function () {
    //    calendar.render();
    //});

});