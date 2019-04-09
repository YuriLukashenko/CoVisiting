using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoVisiting.Data.Interfaces;
using CoVisiting.Models.Event;
using Microsoft.AspNetCore.Mvc;

namespace CoVisiting.Controllers
{
    public class EventController : Controller
    {
        private readonly IEvent _eventService;

        public EventController(IEvent eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index(int id)
        {
            var newEvent = _eventService.GetById(id);

            var model = new EventIndexModel
            {

            };

            return View();
        }
    }
}