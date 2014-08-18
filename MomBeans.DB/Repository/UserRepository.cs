/***************************************************************************************
 * File Name    : UserRepository.cs 
 * Author       : Amit Kumar
 * Created On   : 13/05/2014
 * Description  : This is repository class for Users,contains methods related to Users
 ***************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using MomBeans.DB.IRepository;
using MomBeans.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MomBeans.Utility;

namespace MomBeans.DB.Repository
{
    public class UserRepository : IUserRepository
    {
        #region Properties

        public ApplicationDbContext Context { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }

        #endregion Properties

        /// <summary>
        /// Constructor to Initialize a new repository
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        /// <summary>
        /// Insert a new user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string Insert(UserModel userModel)
        {
            //Search if the user doesn't exist
            if (UserManager.FindByName(userModel.UserName) == null)
            {
                //create the default user
                var user = new ApplicationUser
                {
                    UserName = userModel.UserName
                };
                var result = UserManager.Create(user, userModel.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, userModel.Role);
                }
                if (userModel.IsActive)
                {
                    UserManager.AddToRole(user.Id, Utilities.Constants.Active);
                }
                return user.Id;
            }
            return null;
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserModel> GetAll()
        {
            var users = Context.Users;
            var userModels = new List<UserModel>();
            foreach (var user in users)
            {
                var userModel = new UserModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                //Lets get the role of the user, active role we have checked later
                var role = user.Roles.Select(r => r.Role.Name).FirstOrDefault(r => r != Utilities.Constants.Active);

                userModel.Role = role;

                //Check whether the user is in active role
                userModel.IsActive = UserManager.IsInRole(user.Id, Utilities.Constants.Active);
                
            }
            return userModels;
        }

        public UserModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public bool Update(UserModel userModel)
        {
            var user = UserManager.FindByName(userModel.UserName);

            //Lets get the role of the user, active role we have checked later
            var role = user.Roles.Select(r => r.Role.Name).FirstOrDefault(r => r != Utilities.Constants.Active);
            if (userModel.Role != role)
            {
                UserManager.RemoveFromRole(user.Id, role);
                UserManager.AddToRole(user.Id, userModel.Role);
            }

            //If an active user is inactivated
            if (UserManager.IsInRole(user.Id, Utilities.Constants.Active) && !userModel.IsActive)
            {
                UserManager.RemoveFromRole(user.Id, Utilities.Constants.Active);
            }

            //If an inactive user is activated
            if (!UserManager.IsInRole(user.Id, Utilities.Constants.Active) && userModel.IsActive)
            {
                UserManager.AddToRole(user.Id, Utilities.Constants.Active);
            }

            UserManager.Update(user);
            Context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete a user with the supplied UserName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Delete(string name)
        {
            var user = UserManager.FindByName(name);
            var roles = user.Roles;
            foreach (var role in roles.ToList())
            {
                UserManager.RemoveFromRole(user.Id, role.Role.Name);
            }

            Context.Users.Remove(user);
            Context.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// Get Role of the user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetRole(string name)
        {
            var user = UserManager.FindByName(name);
            return user.Roles.Select(r => r.Role.Name).FirstOrDefault(r => r != Utilities.Constants.Active);
        }
    }
}
