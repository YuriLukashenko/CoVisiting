using System;
using System.Collections.Generic;
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

        public async Task Add(EventReply reply)
        {
            _context.Add(reply);
            await _context.SaveChangesAsync();
        }
    }
}
