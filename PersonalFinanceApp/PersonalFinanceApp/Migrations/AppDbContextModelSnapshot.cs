﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalFinanceApp.Database;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("PersonalFinanceApp.Model.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExpensesBookID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ExpensesBookID");

                    b.ToTable("CategoriesTable");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Expense", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<int>("CategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExpensesBookID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Recurring")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("RecurringDate")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ExpensesBookID");

                    b.ToTable("ExpensesTable");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.ExpensesBook", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Spending")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("ExpensesBooksTable");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.User", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Goal")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MonthlyIncome")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Saving")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("UserTable");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Category", b =>
                {
                    b.HasOne("PersonalFinanceApp.Model.ExpensesBook", "ExpensesBook")
                        .WithMany("Categories")
                        .HasForeignKey("ExpensesBookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpensesBook");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Expense", b =>
                {
                    b.HasOne("PersonalFinanceApp.Model.Category", "Category")
                        .WithMany("Expenses")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonalFinanceApp.Model.ExpensesBook", "ExpensesBook")
                        .WithMany("Expenses")
                        .HasForeignKey("ExpensesBookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("ExpensesBook");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.ExpensesBook", b =>
                {
                    b.HasOne("PersonalFinanceApp.Model.User", "User")
                        .WithMany("ExpensesBooks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Category", b =>
                {
                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.ExpensesBook", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.User", b =>
                {
                    b.Navigation("ExpensesBooks");
                });
#pragma warning restore 612, 618
        }
    }
}
