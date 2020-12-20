﻿// <auto-generated />
using System;
using Directory.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Directory.Migrations
{
    [DbContext(typeof(DirectoryContext))]
    [Migration("20201220113323_DirectoryDB")]
    partial class DirectoryDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Directory.Entity.Communication", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContactID")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Type")
                        .HasColumnType("char(1)");

                    b.HasKey("ID");

                    b.HasIndex("ContactID");

                    b.ToTable("Communications");
                });

            modelBuilder.Entity("Directory.Entity.Contact", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirmName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Surname")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Directory.Entity.Communication", b =>
                {
                    b.HasOne("Directory.Entity.Contact", null)
                        .WithMany("CommunicationList")
                        .HasForeignKey("ContactID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Directory.Entity.Contact", b =>
                {
                    b.Navigation("CommunicationList");
                });
#pragma warning restore 612, 618
        }
    }
}