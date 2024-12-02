using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Database;

public static class DBManagerNotify {
    #region Create

    public static bool Insert<T>(T entity) where T : class {
        if (!CheckTypeDatabase(typeof(T))) {
            Notify("Insert", "Data type not found in database.", false);
            return false;
        }

        try {
            using var context = new AppDbContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
            Notify("Insert", $"Successfully inserted entity of type {typeof(T).Name}.", true);
            return true;
        }
        catch (Exception ex) {
            Notify("Insert", $"Failed to insert entity: {ex.Message}", false);
            return false;
        }
    }

    public static bool Insert<T>(IEnumerable<T> entities) where T : class {
        if (!CheckTypeDatabase(typeof(T))) {
            Notify("Insert", "Data type not found in database.", false);
            return false;
        }

        try {
            using var context = new AppDbContext();
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
            Notify("Insert", $"Successfully inserted multiple entities of type {typeof(T).Name}.", true);
            return true;
        }
        catch (Exception ex) {
            Notify("Insert", $"Failed to insert entities: {ex.Message}", false);
            return false;
        }
    }

    #endregion Create

    #region Read

    public static T? GetFirst<T>(Expression<Func<T, bool>> condition, bool haveForeignKey = true) where T : class {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        try {
            using var context = new AppDbContext();
            IQueryable<T> query = context.Set<T>().Where(condition);
            if (haveForeignKey)
                query = GetIncludes(query, context);

            T? entity = query.FirstOrDefault();
            Notify("Read", $"Successfully retrieved entity of type {typeof(T).Name}.", entity != null);
            return entity;
        }
        catch (Exception ex) {
            Notify("Read", $"Failed to retrieve entity: {ex.Message}", false);
            return null;
        }
    }

    public static List<T> GetAll<T>(bool haveForeignKey = true) where T : class {
        if (!CheckTypeDatabase(typeof(T)))
            throw new InvalidOperationException("Data type not found in database.");

        try {
            using var context = new AppDbContext();
            IQueryable<T> query = context.Set<T>();
            if (haveForeignKey)
                query = GetIncludes(query, context);

            List<T> entities = query.ToList();
            Notify("Read", $"Successfully retrieved {entities.Count} entities of type {typeof(T).Name}.", true);
            return entities;
        }
        catch (Exception ex) {
            Notify("Read", $"Failed to retrieve entities: {ex.Message}", false);
            return new List<T>();
        }
    }

    #endregion Read

    #region Update

    public static bool Update<T>(T entity) where T : class {
        if (!CheckTypeDatabase(typeof(T))) {
            Notify("Update", "Data type not found in database.", false);
            return false;
        }

        try {
            using var context = new AppDbContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
            Notify("Update", $"Successfully updated entity of type {typeof(T).Name}.", true);
            return true;
        }
        catch (Exception ex) {
            Notify("Update", $"Failed to update entity: {ex.Message}", false);
            return false;
        }
    }

    #endregion Update

    #region Delete

    public static void Remove<T>(T entity) where T : class {
        if (!CheckTypeDatabase(typeof(T))) {
            Notify("Delete", "Data type not found in database.", false);
            return;
        }

        try {
            using var context = new AppDbContext();
            context.Set<T>().Remove(entity);
            context.SaveChanges();
            Notify("Delete", $"Successfully deleted entity of type {typeof(T).Name}.", true);
        }
        catch (Exception ex) {
            Notify("Delete", $"Failed to delete entity: {ex.Message}", false);
        }
    }

    #endregion Delete

    #region Helpers

    private static bool CheckTypeDatabase(Type type) {
        using var context = new AppDbContext();
        return context.Model.FindEntityType(type) != null;
    }

    private static IQueryable<T> GetIncludes<T>(IQueryable<T> query, AppDbContext context) where T : class {
        var navigationProperties = context.Model.FindEntityType(typeof(T))?.GetNavigations().Select(n => n.Name);
        if (navigationProperties != null) {
            foreach (var navigationProperty in navigationProperties)
                query = query.Include(navigationProperty);
        }

        return query;
    }

    private static void Notify(string operation, string message, bool isSuccess) {
        string status = isSuccess ? "SUCCESS" : "FAILURE";
        Console.WriteLine($"[{DateTime.Now}] {operation.ToUpper()}: {status} - {message}");
    }

    #endregion Helpers
}
