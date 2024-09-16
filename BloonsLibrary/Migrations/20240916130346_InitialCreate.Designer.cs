﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BloonLibrary.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20240916130346_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Lives")
                        .HasColumnType("int");

                    b.Property<double>("Money")
                        .HasColumnType("double");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
