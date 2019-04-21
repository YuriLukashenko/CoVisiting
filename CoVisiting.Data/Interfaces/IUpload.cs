using CoVisiting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoVisiting.Data.Interfaces
{
    public interface IUpload
    {
        void UploadProfileImage(ApplicationUser applicationUser);
    }
}
