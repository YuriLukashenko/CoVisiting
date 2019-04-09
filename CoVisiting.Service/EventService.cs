using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;

namespace CoVisiting.Service
{
    public class EventService: IEvent
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Add(Event post)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditEventContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetEventByCategory(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetLatestEvents(int n)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetFilteredEvents(Category category, string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetFilteredEvents(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
