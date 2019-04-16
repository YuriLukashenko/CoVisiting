using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.Events.Where(newEvent => newEvent.Id == id)
                .Include(newEvent => newEvent.User)
                .Include(newEvent => newEvent.Replies).ThenInclude(reply => reply.User)
                .Include(newEvent => newEvent.Category)
                .Include(newEvent => newEvent.BeforeEvent)
                .Include(newEvent => newEvent.AfterEvent)
                .First();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Category)
                .Include(newEvent => newEvent.BeforeEvent)
                .Include(newEvent => newEvent.AfterEvent);
        }

        public async Task Add(Event newEvent)
        {
            _context.Add(newEvent);
            await _context.SaveChangesAsync();
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
            return GetAll().OrderByDescending(newEvent => newEvent.Created).Take(n);
        }

        public IEnumerable<Event> GetFilteredEvents(Category category, string searchQuery)
        {
            return String.IsNullOrEmpty(searchQuery)
                ? category.Events
                : category.Events.Where(newEvent 
                    => newEvent.Title.Contains(searchQuery)
                       || newEvent.Content.Contains(searchQuery)
                       || newEvent.EventCity.Contains(searchQuery)
                       || newEvent.EventPlace.Contains(searchQuery));
        }

        public IEnumerable<Event> GetFilteredEvents(string searchQuery)
        {
            return GetAll()
                .Where(newEvent
                => newEvent.Title.Contains(searchQuery)
                || newEvent.Content.Contains(searchQuery)
                || newEvent.EventCity.Contains(searchQuery)
                || newEvent.EventPlace.Contains(searchQuery));
        }
    }
}
