using System;
using System.Collections.Generic;
using System.Text;
using CoVisiting.Data;
using CoVisiting.Data.Interfaces;
using CoVisiting.Data.Models;

namespace CoVisiting.Service
{
    public class UploadSevice : IUpload
    {
        private readonly ApplicationDbContext _context;

        public UploadSevice(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UploadProfileImage(ApplicationUser applicationUser)
        {
            
        }
    }
}
