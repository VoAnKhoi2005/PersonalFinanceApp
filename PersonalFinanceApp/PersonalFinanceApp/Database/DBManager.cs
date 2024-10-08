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
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public static void Insert<T>(List<T> entities) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }

        #endregion

        #region Read

        public static T? GetFirst<T>(Func<T, bool> predicate) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public static IEnumerable<T> GetCondition<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            return context.Set<T>().Where(predicate);
        }

        //Theta Join with query
        public static IEnumerable<T> GetWithInclude<T>(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            IQueryable<T> query = context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate).ToList();
        }

        public static IEnumerable<T> GetAll<T>() where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            return context.Set<T>();
        }

        #endregion

        #region Update

        

        #endregion

        #region Delete

        public static void Remove<T>(T entity) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public static void Remove<T>(List<T> entities) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            if (entities.Count > 0)
                context.RemoveRange(entities);
            context.SaveChanges();
        }

        public static void Remove<T>(Func<T, bool> predicate) where T : class
        {
            if (!CheckTypeDatabase(typeof(T)))
                throw new Exception("Data type not found in database.");

            using var context = new AppDbContext();
            var entityToRemove = context.Set<T>().Where(predicate).ToList();
            if (!entityToRemove.Any())
                return;
            context.Set<T>().RemoveRange(entityToRemove);
            context.SaveChanges();
        }

        #endregion

        private static bool CheckTypeDatabase(Type type)
        {
            using var context = new AppDbContext();
            return context.Model.FindEntityType(type) != null;
        }
    }
}