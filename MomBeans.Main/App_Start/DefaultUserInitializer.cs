/**********************************************************************************
 * File Name    : DefaultUserInitializer.cs
 * Author       : Amit Kumar
 * Created On   : 01/05/2014
 * Description  : Creates a default user by picking credentials from the web.config
 ***********************************************************************************/

using System;
using System.Linq;
using System.Reflection;
using AriRick.Utility;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MomBeans.Models;
using MomBeans.Utility;

namespace MomBeans.Main
{
    public static class DefaultUserInitializer
    {
        //Error logging
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType +
            MethodBase.GetCurrentMethod().Name);

        public static void AddDefaultUser()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
                    var name = ConfigKeyReader.DefaultUserName;
                    var password = ConfigKeyReader.DefaultUserPassword;
                    var arrRoles = new[] {Utilities.Constants.Admin,Utilities.Constants.User,
                        Utilities.Constants.ContentEditor,
                        Utilities.Constants.Active};
                    foreach (var role in arrRoles.Where(role => !roleManager.RoleExists(role)))
                    {
                        roleManager.Create(new IdentityRole(role));
                    }

                    //Search if the user doesn't exist
                    if ((!String.IsNullOrEmpty(name)) && (!String.IsNullOrEmpty(password))
                        && (userManager.FindByName(name) == null))
                    {
                        //create the default user
                        var user = new ApplicationUser { UserName = name };
                        var result = userManager.Create(user, password);
                        if (result.Succeeded)
                        {
                            userManager.AddToRole(user.Id, Utilities.Constants.Admin);
                            userManager.AddToRole(user.Id, Utilities.Constants.Active);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(Utilities.Errors.DefaultUserError + exception.Message, exception);
            }
        }
    }
}