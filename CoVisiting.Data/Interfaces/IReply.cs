using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoVisiting.Data.Models;

namespace CoVisiting.Data.Interfaces
{
    public interface IReply
    {
        EventReply GetById(int id);
        Task Add(EventReply reply);
        Task Delete(int replyId);
    }
}
