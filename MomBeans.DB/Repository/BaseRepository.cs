/********************************************************************************
 * File Name    : BaseRepository.cs
 * Author       : Amit Kumar
 * Created On   : 18/08/2014
 * Description  : This is Repository class which provides implementation
 *                to IRepository<T> methods
 *******************************************************************************/

using System.Data.Entity;
using System.Linq;
using MomBeans.DB.IRepository;

namespace MomBeans.DB.Repository
{
    /// <summary>
    /// This is a Repository class that provides implementation to IRepository methods
    /// </summary>
    /// <typeparam name="T">Type of repository</typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class
    {

        #region Properties

        /// <summary>
        /// Get or set instance of GrootEntities
        /// </summary>
        public MomBeansEntities Context { get; private set; }

        #endregion Properties


        /// <summary>
        /// Constructor to Initialize a new repository
        /// </summary>
        /// <param name="context"></param>
        public BaseRepository(MomBeansEntities context)
        {
            Context = context;
        }

        /// <summary>
        /// Insert Operation
        /// </summary>
        /// <param name="entity"></param>
        public bool Insert(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Read All Operation
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        /// <summary>
        /// Search an entity operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        /// Update Operation
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete Operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(int id)
        {
            var foundEntity = Context.Set<T>().Find(id);

            if (foundEntity == null)
            {
                return false;
            }

            Context.Set<T>().Remove(foundEntity);
            Context.SaveChanges();
            return true;
        }
    }
}
