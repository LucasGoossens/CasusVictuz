﻿// <auto-generated />
using System;
using CasusVictuz.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CasusVictuz.Migrations
{
    [DbContext(typeof(VictuzDb))]
    partial class VictuzDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CasusVictuz.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Casusvictuz.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Casusvictuz.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Spots")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Casusvictuz.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");

                    b.HasDiscriminator().HasValue("Post");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Casusvictuz.Registration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOrganised")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("Casusvictuz.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Casusvictuz.Comment", b =>
                {
                    b.HasBaseType("Casusvictuz.Post");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("ThreadId");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("Casusvictuz.Thread", b =>
                {
                    b.HasBaseType("Casusvictuz.Post");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Thread");
                });

            modelBuilder.Entity("CasusVictuz.Models.Tag", b =>
                {
                    b.HasOne("Casusvictuz.Event", null)
                        .WithMany("Tags")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casusvictuz.Event", b =>
                {
                    b.HasOne("Casusvictuz.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Casusvictuz.Post", b =>
                {
                    b.HasOne("Casusvictuz.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casusvictuz.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Casusvictuz.Registration", b =>
                {
                    b.HasOne("Casusvictuz.Event", "Event")
                        .WithMany("Registrations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Casusvictuz.User", "User")
                        .WithMany("Registrations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Casusvictuz.Comment", b =>
                {
                    b.HasOne("Casusvictuz.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Casusvictuz.Thread", "Thread")
                        .WithMany("Comments")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Thread");
                });

            modelBuilder.Entity("Casusvictuz.Event", b =>
                {
                    b.Navigation("Registrations");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Casusvictuz.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Registrations");
                });

            modelBuilder.Entity("Casusvictuz.Comment", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("Casusvictuz.Thread", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
