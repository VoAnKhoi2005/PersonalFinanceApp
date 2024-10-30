using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.src;

namespace PersonalFinanceApp.Database;

public static class DBManager
{
    //CRUD
    //Create, read, update, and delete function

    #region Create

    public static bool Insert<T>(T entity) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        try
        {
            using var context = new AppDbContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool Insert<T>(IEnumerable<T> entities) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        try
        {
            using var context = new AppDbContext();
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion Create

    #region Read

    public static T? GetFirst<T>(Expression<Func<T, bool>> condition, bool getDeleted = false) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        Expression<Func<T, bool>> ExCondition = arg => true;
        if (typeof(T).GetProperty("Deleted") != null)
        {
            ExCondition = e => EF.Property<bool>(e, "Deleted") == getDeleted;
        }

        var combineCondition = ExCondition;
         combineCondition = combineCondition.And(condition);

        using var context = new AppDbContext();
        return context.Set<T>().FirstOrDefault(combineCondition);
    }

    /// <summary>
    /// Read from database with a condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="condition"></param>
    /// <param name="getDeleted"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>    
    public static List<T> GetCondition<T>(Expression<Func<T, bool>> condition, bool getDeleted = false) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        Expression<Func<T, bool>> ExCondition = arg => true;
        if (typeof(T).GetProperty("Deleted") != null)
        {
            ExCondition = e => EF.Property<bool>(e, "Deleted") == getDeleted;
        }

        var combineCondition = ExCondition;
        combineCondition = combineCondition.And(condition);

        using var context = new AppDbContext();
        return context.Set<T>().Where(combineCondition).ToList();
    }

    public static List<T> GetIncludes<T>(IEnumerable<T> entities, AppDbContext? context = null) where T : class
    {
        if (context == null)
            context = new AppDbContext();

        IQueryable<T> query = entities.AsQueryable();
        var navigationProperties = context.Model.FindEntityType(typeof(T)).GetNavigations().Select(n => n.Name);
        if (navigationProperties != null)
        {
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
        }

        return query.ToList();
    }

    public static List<T> GetAll<T>(bool getDeleted = false) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        Expression<Func<T, bool>> condition = arg => true;
        var combineCondition = condition;

        if (typeof(T).GetProperty("Deleted") != null)
        {
            Expression<Func<T, bool>> exCondition = e => EF.Property<bool>(e, "Deleted") == getDeleted;
            combineCondition = combineCondition.And(exCondition);
        }

        using var context = new AppDbContext();
        return context.Set<T>().Where(combineCondition).ToList();
    }

    #endregion Read

    #region Update

    public static bool Update<T>(T entity) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");
        try
        {
            using var context = new AppDbContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool Update<T>(IEnumerable<T> entities) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");
        try
        {
            using var context = new AppDbContext();
            context.Set<T>().UpdateRange(entities);
            context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion Update

    #region Delete

    /// <summary>
    /// Delete entity from table. Deletion is cascade.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Remove<T>(T entity) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");


        PropertyInfo? deletedProperty = typeof(T).GetProperty("Deleted");
        if (deletedProperty != null)
        {
            PropertyInfo deletedDateProperty = typeof(T).GetProperty("DeletedDate");
            if (deletedDateProperty != null)
                deletedDateProperty.SetValue(entity, DateTime.Now);
            deletedProperty.SetValue(entity, true);
            Update(entity);
            return;
        }

        using var context = new AppDbContext();
        context.Set<T>().Remove(entity);
        context.SaveChanges();
    }

    public static void Remove<T>(IEnumerable<T> entities) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        PropertyInfo? deletedProperty = typeof(T).GetProperty("Deleted");
        if (deletedProperty != null)
        {
            foreach (T entity in entities)
            {
                PropertyInfo deletedDateProperty = typeof(T).GetProperty("DeletedDate");
                if (deletedDateProperty != null)
                    deletedDateProperty.SetValue(entity, DateTime.Now);
                deletedProperty.SetValue(entity, true);
                Update(entity);
            }

            return;
        }

        using var context = new AppDbContext();
        context.RemoveRange(entities);
        context.SaveChanges();
    }

    public static void RemoveWithCondition<T>(Expression<Func<T, bool>> condition) where T : class
    {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        using var context = new AppDbContext();
        var entitiesToRemove = context.Set<T>().Where(condition);
        if (!entitiesToRemove.Any())
            return;

        PropertyInfo? deletedProperty = typeof(T).GetProperty("Deleted");
        if (deletedProperty != null)
        {
            foreach (T entity in entitiesToRemove)
            {
                PropertyInfo deletedDateProperty = typeof(T).GetProperty("DeletedDate");
                if (deletedDateProperty != null)
                    deletedDateProperty.SetValue(entity, DateTime.Now);
                deletedProperty.SetValue(entity, true);
                Update(entity);
            }

            return;
        }

        context.Set<T>().RemoveRange(entitiesToRemove);
        context.SaveChanges();
    }

    public static bool DeleteAllData()
    {
        RemoveWithCondition<User>(x => true);
        List<User> users = GetCondition<User>(u => true).ToList();
        if (!users.Any())
            return false;
        foreach (User user in users)
        {
            Remove(user);
        }

        return true;
    }

    /// <summary>
    /// Auto delete expense after 30 days from the time of deletion.
    /// </summary>
    public static void AutoDelete()
    {
        using var context = new AppDbContext();
        List<Expense> expenses = GetAll<Expense>(true).ToList();
        expenses = expenses.Where(ex => (DateTime.Now - (DateTime)ex.DeletedDate).TotalDays >= 30).ToList();
        foreach (Expense expense in expenses)
        {
            if (expense.Recurring)
                RemoveWithCondition<RecurringDetail>(rd => rd.ExpenseID == expense.ExpenseID);
            context.Remove(expense);
        }

        context.SaveChanges();
    }

    #endregion Delete

    public static bool CheckTypeDatabase(Type type)
    {
        using var context = new AppDbContext();
        return context.Model.FindEntityType(type) != null;
    }
}