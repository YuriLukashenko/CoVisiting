using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;

namespace CoVisiting.Service
{
    public class ReplyService: IReply
    {
        private readonly ApplicationDbContext _context;

        public ReplyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public EventReply GetById(int id)
        {
            return _context.EventReplies.FirstOrDefault(reply => reply.Id == id);
        }

        public async Task Add(EventReply reply)
        {
            _context.Add(reply);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int replyId)
        {
            _context.EventReplies.Remove(GetById(replyId));

            await _context.SaveChangesAsync();
        }
    }
}
