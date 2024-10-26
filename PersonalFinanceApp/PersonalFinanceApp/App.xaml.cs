﻿using System.Windows;
using PersonalFinanceApp.Database;

namespace PersonalFinanceApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Create database
            using (var context = new AppDbContext())
            {
                context.EnsureDatabaseCreated();
            }
        }
    }

}
