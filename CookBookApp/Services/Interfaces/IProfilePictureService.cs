using CookBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services.Interfaces
{
    public interface IProfilePictureService
    {
        void Add(ProfilePicture picture);
        void Remove(ProfilePicture picture);
        ProfilePicture GetUserAvatar(string userId);
        string GetAvatarPath(ProfilePicture profilePicture);
    }
}
