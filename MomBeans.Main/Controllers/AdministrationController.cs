//********************************************************************************
// * File Name    : AdministrationController.cs 
// * Author       : Amit Kumar
// * Created On   : 14/05/2014
// * Description  : Controller class for the admin/user management screen
// *******************************************************************************/

using System;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using MomBeans.DB.IRepository;
using MomBeans.Utility;

namespace MomBeans.Main.Controllers
{
    [Authorize(Roles = Utilities.Constants.Admin)]
    [Authorize(Roles = Utilities.Constants.Active)]
    public class AdministrationController : Controller
    {
        //Error logging
        protected static ILog Log
        {
            get { return LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType + MethodBase.GetCurrentMethod().Name); }
        }

        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor for the Administration controller class
        /// </summary>
        /// <param name="userRepository">Instance of user repository that is to
        ///  be created by dependency injection</param>
        public AdministrationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //
        // GET: /Administration/
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.Error(Utilities.Errors.ReadError + exception.Message, exception);
                //return Json(ModelState.ToDataSourceResult());
                return null;
            }
        }

        ///// <summary>
        ///// Method to return all the users to the grid in the admin screen
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    try
        //    {
        //        var users = _userRepository.GetAll();
        //        return Json(users.ToDataSourceResult(request));
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Error(Utilities.Errors.ReadError + exception.Message, exception);
        //        ModelState.AddModelError(Utilities.Constants.Read, Utilities.Errors.ReadError);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //}


        ///// <summary>
        ///// Create a user method
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="userModel"></param>
        ///// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create([DataSourceRequest] DataSourceRequest request, UserModel userModel)
        //{
        //    try
        //    {
        //        if (userModel == null || !ModelState.IsValid)
        //            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));

        //        if (User.IsInRole(Utilities.Constants.School))
        //        {
        //            //lets fetch the school id of the current user
        //            var schoolId = Session[Utilities.Constants.LoggedInSchoolId] != null ?
        //                    Convert.ToInt32(Session[Utilities.Constants.LoggedInSchoolId])
        //                    : _userRepository.GetSchoolId(User.Identity.Name);

        //            //School role can't create admin/school or create users of other school
        //            if (String.Equals(userModel.Role, Utilities.Constants.Admin) ||
        //                     String.Equals(userModel.Role, Utilities.Constants.School) ||
        //                     userModel.SchoolId != schoolId)
        //            {
        //                ModelState.AddModelError(Utilities.Constants.Create, Utilities.Errors.UnAuthorized);
        //                return Json(ModelState.ToDataSourceResult());
        //            }
        //        }

        //        //If not Admin and school not selected
        //        if (!(String.Equals(userModel.Role, Utilities.Constants.Admin)) && userModel.SchoolId == 0)
        //        {
        //            ModelState.AddModelError(Utilities.Constants.SchoolId, Utilities.Errors.SchoolNotSelected);
        //            return Json(ModelState.ToDataSourceResult());
        //        }
        //        var success = _userRepository.Insert(userModel);
        //        if (!string.IsNullOrEmpty(success))
        //        {
        //            userModel.UserId = success;
        //            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        //        }

        //        ModelState.AddModelError(Utilities.Constants.Create, Utilities.Errors.DuplicatUserName);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Error(Utilities.Errors.CreateError + exception.Message, exception);

        //        ModelState.AddModelError(Utilities.Constants.Create, Utilities.Errors.CreateError);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //}

        ///// <summary>
        ///// Method to update/edit a user
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="userModel"></param>
        ///// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Update([DataSourceRequest] DataSourceRequest request, UserModel userModel)
        //{
        //    try
        //    {
        //        ModelState.Remove(Utilities.Constants.Password);
        //        if (userModel == null || !ModelState.IsValid)
        //        {
        //            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        //        }

        //        if (User.IsInRole(Utilities.Constants.School))
        //        {
        //            //lets fetch the school id of the current user
        //            var schoolId = Session[Utilities.Constants.LoggedInSchoolId] != null ?
        //                    Convert.ToInt32(Session[Utilities.Constants.LoggedInSchoolId])
        //                    : _userRepository.GetSchoolId(User.Identity.Name);

        //            //School role can't create admin/school or create users of other school
        //            if (String.Equals(userModel.Role, Utilities.Constants.Admin) ||
        //                     String.Equals(userModel.Role, Utilities.Constants.School) ||
        //                     userModel.SchoolId != schoolId)
        //            {
        //                ModelState.AddModelError(Utilities.Constants.Create, Utilities.Errors.UnAuthorized);
        //                return Json(ModelState.ToDataSourceResult());
        //            }
        //        }

        //        //non-admin user and school not selected
        //        if (!(String.Equals(userModel.Role, Utilities.Constants.Admin)) && userModel.SchoolId == 0)
        //        {
        //            ModelState.AddModelError(Utilities.Constants.School, Utilities.Errors.SchoolNotSelected);
        //            return Json(ModelState.ToDataSourceResult());
        //        }

        //        if (String.Equals(userModel.Role, Utilities.Constants.Admin))
        //        {
        //            userModel.SchoolId = 0;
        //        }

        //        //Get the previous user details
        //        var prevRole = _userRepository.GetRole(userModel.UserName);

        //        //Check if the school/role of the user is being updated for the coach users
        //        if (String.Equals(prevRole, Utilities.Constants.Coach))
        //        {
        //            var prevSchoolId = _userRepository.GetSchoolId(userModel.UserName);
                
        //            if (prevSchoolId != userModel.SchoolId||(!String.Equals(prevRole,userModel.Role)))
        //            {
        //                var assignedStudents = _studentRepository.GetAll(userModel.SchoolId)
        //                    .Where(student => student.CoachId == userModel.UserId);

        //                foreach (var student in assignedStudents)
        //                {
        //                    //"0" is assigned below.
        //                    student.CoachId = Utilities.Constants.DefaultInLogId;
        //                    _studentRepository.Update(student);
        //                }
        //            }
        //        }
        //        //Check if the school/role of the user is being updated for the advisor users
        //        else if (String.Equals(prevRole, Utilities.Constants.Advisor))
        //        {
        //            var prevSchoolId = _userRepository.GetSchoolId(userModel.UserName);

        //            if (prevSchoolId != userModel.SchoolId || (!String.Equals(prevRole, userModel.Role)))
        //            {
        //                var assignedStudents = _studentRepository.GetAll(userModel.SchoolId)
        //                    .Where(student => student.AdvisorId == userModel.UserId);
        //                foreach (var student in assignedStudents)
        //                {
        //                    //"0" is being assigned below.
        //                    student.AdvisorId = Utilities.Constants.DefaultInLogId;
        //                    _studentRepository.Update(student);
        //                }
        //            }
        //        }
        //        var success = _userRepository.Update(userModel);
        //        if (success)
        //        {
        //            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        //        }

        //        ModelState.AddModelError(Utilities.Constants.Update, Utilities.Errors.UpdateError);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Error(Utilities.Errors.UpdateError + exception.Message, exception);
        //        ModelState.AddModelError(Utilities.Constants.Update, Utilities.Errors.UpdateError);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //}

        ///// <summary>
        ///// Delete a user Method
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="userModel"></param>
        ///// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Destroy([DataSourceRequest] DataSourceRequest request, UserModel userModel)
        //{
        //    try
        //    {
        //        ModelState.Remove(Utilities.Constants.Password);

        //        if (User.IsInRole(Utilities.Constants.School) &&
        //                 (String.Equals(userModel.Role, Utilities.Constants.Admin) ||
        //                 String.Equals(userModel.Role, Utilities.Constants.School)))
        //        {
        //            ModelState.AddModelError(Utilities.Constants.Delete, Utilities.Errors.UnAuthorized);
        //            return Json(ModelState.ToDataSourceResult());
        //        }

        //        //Dissociate the students if he was a mentor of some
        //        if (String.Equals(userModel.Role, Utilities.Constants.Coach))
        //        {
        //           var assignedStudents = _studentRepository.GetAll(userModel.SchoolId)
        //               .Where(student=>student.CoachId==userModel.UserId).ToList();

        //            foreach (var student in assignedStudents)
        //            {
        //                //"0" is assigned below.
        //                student.CoachId = Utilities.Constants.DefaultInLogId;
        //                _studentRepository.Update(student);
        //            }
        //        }
        //        if (String.Equals(userModel.Role, Utilities.Constants.Advisor))
        //        {
        //            var assignedStudents = _studentRepository.GetAll(userModel.SchoolId)
        //                .Where(student => student.AdvisorId == userModel.UserId);
        //            foreach (var student in assignedStudents)
        //            {
        //                //"0" is being assigned below.
        //                student.AdvisorId = Utilities.Constants.DefaultInLogId;
        //                _studentRepository.Update(student);
        //            }
        //        }

        //        var success = _userRepository.Delete(userModel.UserName);
        //        if (success)
        //        {
        //            return Json(new[] { userModel }.ToDataSourceResult(request, ModelState));
        //        }
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.Error(Utilities.Errors.DeleteError + exception.Message, exception);
        //        ModelState.AddModelError(Utilities.Constants.Delete, Utilities.Errors.DeleteError);
        //        return Json(ModelState.ToDataSourceResult());
        //    }
        //}
    }
}