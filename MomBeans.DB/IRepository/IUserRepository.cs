using System.Collections.Generic;
using MomBeans.Models;

namespace MomBeans.DB.IRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Insert operation
        /// </summary>
        /// <param name="userModel"></param>
        string Insert(UserModel userModel);

        /// <summary>
        /// Read Operation
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAll();

        /// <summary>
        /// Read By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetById(int id);
        /// <summary>
        /// Get Role of the user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetRole(string name);
        /// <summary>
        /// Update Operation
        /// </summary>
        /// <param name="userModel"></param>
        bool Update(UserModel userModel);

        /// <summary>
        /// Delete Operation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Delete(string name);
    }
}
