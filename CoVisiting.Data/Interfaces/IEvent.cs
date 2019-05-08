using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data.Models;

namespace CoVisiting.Data.Interfaces
{
    public interface IEvent
    {
        Event GetById(int id);
        IEnumerable<Event> GetAll();

        Task Add(Event post);
        Task AddEventSubscriber(int id, ApplicationUser user);
        Task DeleteEventSubscriber(int id, ApplicationUser user);
        Task Delete(int id);
        Task EditEventContent(int id, string newContent);

        IEnumerable<Event> GetEventByCategory(int id);
        IEnumerable<Event> GetEventsByUser(string userId);
        IEnumerable<EventReply> GetRepliesForEvent(int id);
        Event GetLastEvent();
        IEnumerable<Event> GetLatestEvents(int n);
        IEnumerable<Event> GetFilteredEvents(Category category, string searchQuery);
        IEnumerable<Event> GetFilteredEvents(string searchQuery);
    }
}
