/********************************************************************************
 * File Name    : IRepository.cs 
 * Author       : Amit Kumar
 * Created On   : 18/08/2014
 * Description  : This is an interface containing the common crud operations
 *******************************************************************************/

using System.Linq;

namespace MomBeans.DB.IRepository
{
    /// <summary>
    /// Interface containing the common crud operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Insert operation
        /// </summary>
        /// <param name="entity"></param>
        bool Insert(T entity);

        /// <summary>
        /// Read Operation
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Read By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// Update Operation
        /// </summary>
        /// <param name="entity"></param>
        bool Update(T entity);

        /// <summary>
        /// Delete Operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
    }
}
