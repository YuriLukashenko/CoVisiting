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
                .Include(newEvent => newEvent.Author)
                .Include(newEvent => newEvent.Replies).ThenInclude(reply => reply.Sender)
                .Include(newEvent => newEvent.Category)
                .Include(newEvent => newEvent.BeforeEvent)
                .Include(newEvent => newEvent.AfterEvent)
                .Include(newEvent => newEvent.Subscribers)
                .First();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(post => post.Author)
                .Include(post => post.Replies).ThenInclude(reply => reply.Sender)
                .Include(post => post.Category)
                .Include(newEvent => newEvent.BeforeEvent)
                .Include(newEvent => newEvent.AfterEvent)
                .Include(newEvent => newEvent.Subscribers);
        }

        public async Task Add(Event newEvent)
        {
            _context.Add(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task AddEventSubscriber(int id, ApplicationUser user)
        {
            var updatingEvent = GetById(id);
            if (updatingEvent.Subscribers == null)
            {
                var inital = new List<ApplicationUser>();
                inital.Add(user);
                updatingEvent.Subscribers = inital;
                _context.Update(updatingEvent);
                await _context.SaveChangesAsync();
            }
            else
            {
                var temp = updatingEvent.Subscribers.ToList();
                if (!temp.Contains(user))
                {
                    temp.Add(user);
                    updatingEvent.Subscribers = temp;
                    _context.Update(updatingEvent);
                    await _context.SaveChangesAsync();
                }   
            }        
        }

        public async Task DeleteEventSubscriber(int id, ApplicationUser user)
        {
            var updatingEvent = GetById(id);
            if (updatingEvent.Subscribers != null)
            {
                var temp = updatingEvent.Subscribers.ToList();
                if (temp.Contains(user))
                {
                    temp.Remove(user);
                    updatingEvent.Subscribers = temp;
                    _context.Update(updatingEvent);
                    await _context.SaveChangesAsync();
                }
            }
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
