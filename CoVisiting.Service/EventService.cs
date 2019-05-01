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
            return _context.Events.Where(e => e.Id == id)
                .Include(e => e.Author)
                .Include(e => e.Replies).ThenInclude(reply => reply.Sender)
                .Include(e => e.Replies).ThenInclude(reply => reply.Reciever)
                .Include(e => e.Category)
                .Include(e => e.BeforeEvent)
                .Include(e => e.AfterEvent)
                .Include(e => e.Subscribers)
                .First();
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(e => e.Author)
                .Include(e => e.Replies).ThenInclude(reply => reply.Sender)
                .Include(e => e.Replies).ThenInclude(reply => reply.Reciever)
                .Include(e => e.Category)
                .Include(e => e.BeforeEvent)
                .Include(e => e.AfterEvent)
                .Include(e => e.Subscribers);
        }

        public IEnumerable<Event> GetEventsByUser(string userId)
        {
            return _context.Events.Where(newEvent => newEvent.Author.Id == userId)
                .Include(post => post.Author)
                .Include(post => post.Replies).ThenInclude(reply => reply.Sender)
                .Include(newEvent => newEvent.Replies).ThenInclude(reply => reply.Reciever)
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
            if (updatingEvent.Subscribers == null || updatingEvent.Subscribers.Count() == 0)
            {
                var inital = new List<UserEventJoinTable>()
                {
                    new UserEventJoinTable
                    {
                        EventId = id,
                        Event = updatingEvent,
                        UserId = user.Id,
                        ApplicationUser = user
                    }
                };
                updatingEvent.Subscribers = inital;
                _context.Update(updatingEvent);
                await _context.SaveChangesAsync();
            }
            else
            {
                var temp = updatingEvent.Subscribers.ToList();
                if (temp.FirstOrDefault(s => s.UserId == user.Id) == null)
                {
                    temp.Add(new UserEventJoinTable
                    {
                        EventId = id,
                        Event = updatingEvent,
                        UserId = user.Id,
                        ApplicationUser = user
                    });
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
                if (temp.FirstOrDefault(s => s.UserId == user.Id) != null)
                {
                    temp.Remove(temp.First(s => s.UserId == user.Id));
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

        public Event GetLastEvent()
        {
            return GetAll().Last();
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
