using CookBookApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookBookApp.Models
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger,
            ApplicationDbContext context) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.context = context;
        }

        public virtual Task<string> GetNameAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Name);
        }

        public virtual Task<string> GetLocationAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Location);
        }

        public virtual Task<string> GetDescriptionAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Description);
        }

        public virtual Task<Gender> GetGenderAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Gender);
        }

        public virtual Task<IdentityResult> SetNameAsync(ApplicationUser user, string name)
        {
            var u = context.Users.FirstOrDefault(u => u.Id == user.Id);
            u.Name = name;
            context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public virtual Task<IdentityResult> SetLocationAsync(ApplicationUser user, string location)
        {
            var u = context.Users.FirstOrDefault(u => u.Id == user.Id);   
            u.Location = location;
            context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public virtual Task<IdentityResult> SetGenderAsync(ApplicationUser user, Gender gender)
        {
            var u = context.Users.FirstOrDefault(u => u.Id == user.Id);
            u.Gender = gender;
            context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }

        public virtual Task<IdentityResult> SetDescriptionAsync(ApplicationUser user, string description)
        {
            var u = context.Users.FirstOrDefault(u => u.Id == user.Id);
            u.Description = description;
            context.SaveChanges();

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
