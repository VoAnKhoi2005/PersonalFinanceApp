﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalFinanceApp.Database;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241024163222_AddResources")]
    partial class AddResources
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("PersonalFinanceApp.Model.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExBMonth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExBYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Resources")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ExBMonth", "ExBYear", "UserID");

                    b.ToTable("CATEGORY");
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Expense", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("ExBMonth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExBYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Recurring")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly?>("RecurringDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Resources")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("Date");

                    b.HasIndex("ExBMonth", "ExBYear", "UserID");

                    b.ToTable("EXPENSE", t =>
                        {
                            t.HasCheckConstraint("CK_Amount", "[Amount] >= 0");

                            t.HasCheckConstraint("CK_Recurring", "[Recurring] IN (0, 1)");
                        });
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.ExpensesBook", b =>
                {
                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserID")
                        .HasColumnType("TEXT");

                    b.Property<long>("Budget")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Resources")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<long>("Spending")
                        .HasColumnType("INTEGER");

                    b.HasKey("Month", "Year", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("EXPENSESBOOK", t =>
                        {
                            t.HasCheckConstraint("CK_Budget", "[Budget] >= 0");

                            t.HasCheckConstraint("CK_Month", "[Month] >= 1 AND [Month] <= 12");

                            t.HasCheckConstraint("CK_Spending", "[Spending] >= 0");

                            t.HasCheckConstraint("CK_Year", "[Year] >= 0");
                        });
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.User", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("TEXT");

                    b.Property<long>("Goal")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MonthlyIncome")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Resources")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<long>("Saving")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("USER", t =>
                        {
                            t.HasCheckConstraint("CK_Goal", "[Goal] >= 0");

                            t.HasCheckConstraint("CK_MonthlyIncome", "[MonthlyIncome] >= 0");

                            t.HasCheckConstraint("CK_Saving", "[Saving] >= 0");
                        });
                });

            modelBuilder.Entity("PersonalFinanceApp.Model.Category", b =>
                {
                    b.HasOne("PersonalFinanceApp.Model.ExpensesBook", "ExpensesBook")
                        .WithMany("Categories")
                        .HasForeignKey("ExBMonth", "ExBYear", "UserID")
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
                        .HasForeignKey("ExBMonth", "ExBYear", "UserID")
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
