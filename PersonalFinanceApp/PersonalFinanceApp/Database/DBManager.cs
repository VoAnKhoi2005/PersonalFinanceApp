using System.Data.Entity;
using System.Linq.Expressions;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Database
{
    public static class DBManager
    {
        //CRUD
        //Create, read, update, and delete function

        #region Create

        public static void Insert<T>(T entity) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public static void Insert<T>(IEnumerable<T> entities) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }

        #endregion Create

        #region Read

        public static T? GetFirst<T>(Func<T, bool> condition) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            return context.Set<T>().FirstOrDefault(condition);
        }

        /// <summary>
        /// Read from database with a condition. Set haveForeignKey to true if you want to access foreign key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="haveForeignKey"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>    
        public static IEnumerable<T> GetCondition<T>(Expression<Func<T, bool>> condition, bool haveForeignKey = false) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            var query = context.Set<T>().Where(condition);
            if (haveForeignKey)
                query = GetInclude(query, context);
            return query.ToList();
        }

        /// <summary>
        /// Returned object will have references to the related entities.
        /// Use this to get custom includes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<T> GetWithInclude<T>(
            Expression<Func<T, bool>> condition,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            IQueryable<T> query = context.Set<T>().Where(condition);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public static IQueryable<T> GetInclude<T>(IQueryable<T> query, AppDbContext? context = null) where T : class
        {
            if (context == null)
                context = new AppDbContext();

            var navigationProperties = context.Model.FindEntityType(typeof(T)).GetNavigations().Select(n => n.Name);
            if (navigationProperties != null)
            {
                foreach (var navigationProperty in navigationProperties)
                {
                    query = query.Include(navigationProperty);
                }
            }

            return query;
        }

        public static IEnumerable<T> GetAll<T>() where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            return context.Set<T>();
        }

        #endregion Read

        #region Update

        public static void Update<T>(T entity) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public static void Update<T>(IEnumerable<T> entities) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().UpdateRange(entities);
            context.SaveChanges();
        }

        #endregion Update

        #region Delete

        public static void Remove<T>(T entity) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public static void Remove<T>(IEnumerable<T> entities) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            if (entities.Any())
                context.RemoveRange(entities);
            context.SaveChanges();
        }

        public static void Remove<T>(Func<T, bool> condition) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new InvalidOperationException("Data type not found in database.");

            using var context = new AppDbContext();
            var entityToRemove = context.Set<T>().Where(condition).ToList();
            if (!entityToRemove.Any())
                return;
            context.Set<T>().RemoveRange(entityToRemove);
            context.SaveChanges();
        }

        public static void DeleteDataTable(Type type)
        {

        }

        public static void DeleteAllData()
        {
            using var context = new AppDbContext();
        }

        #endregion Delete

        private static bool CheckTypeDatabase(Type type)
        {
            using var context = new AppDbContext();
            return context.Model.FindEntityType(type) != null;
        }
    }
}