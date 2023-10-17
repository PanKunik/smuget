﻿// <auto-generated />
using System;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(SmugetDbContext))]
    [Migration("20231017171451_Refresh tokens refactoring")]
    partial class Refreshtokensrefactoring
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Persistance.Entities.ExpenseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTimeOffset>("ExpenseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("MoneyAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("MoneyCurrency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("Expenses", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.IncomeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<bool>("Include")
                        .HasColumnType("boolean");

                    b.Property<decimal>("MoneyAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("MoneyCurrency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MonthlyBillingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("MonthlyBillingId");

                    b.ToTable("Incomes", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.MonthlyBillingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Month")
                        .HasColumnType("integer");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("MonthlyBillings", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.PlanEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<decimal>("MoneyAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("MoneyCurrency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("MonthlyBillingId")
                        .HasColumnType("uuid");

                    b.Property<long>("SortOrder")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MonthlyBillingId");

                    b.ToTable("Plans", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpirationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Invalidated")
                        .HasColumnType("boolean");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Used")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SecuredPassword")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.ExpenseEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistance.Entities.PlanEntity", null)
                        .WithMany("Expenses")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.IncomeEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistance.Entities.MonthlyBillingEntity", null)
                        .WithMany("Incomes")
                        .HasForeignKey("MonthlyBillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.PlanEntity", b =>
                {
                    b.HasOne("Infrastructure.Persistance.Entities.MonthlyBillingEntity", null)
                        .WithMany("Plans")
                        .HasForeignKey("MonthlyBillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.MonthlyBillingEntity", b =>
                {
                    b.Navigation("Incomes");

                    b.Navigation("Plans");
                });

            modelBuilder.Entity("Infrastructure.Persistance.Entities.PlanEntity", b =>
                {
                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}
