﻿using CookBookApp.Data;
using CookBookApp.Models;
using CookBookApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CookBookApp.Services
{
    public class ProfilePictureService : IProfilePictureService
    {
        private readonly ApplicationDbContext Context;

        public ProfilePictureService(ApplicationDbContext context)
        {
            Context = context;
        }

        public void Add(ProfilePicture picture)
        {
            Context.ProfilePictures.Add(picture);
            Context.SaveChanges();
        }

        public ProfilePicture GetUserAvatar(string userId)
        {
            var avatar = Context.ProfilePictures.Where(f => f.UserId == userId).FirstOrDefault(f => f.FileType == FileType.Avatar);
            return avatar;
        }

        public void Remove(ProfilePicture picture)
        {
            Context.ProfilePictures.Remove(picture);
            Context.SaveChanges();
        }
    }
}
