﻿// <auto-generated />
using Gimify.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gimify.Migrations
{
    [DbContext(typeof(Efcontext))]
    [Migration("20250303130327_PasswordAudate")]
    partial class PasswordAudate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Gimify.Entities.Exercises", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Gimify.Entities.Exercises_Man", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("Exercisesid")
                        .HasColumnType("integer");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Exercisesid");

                    b.ToTable("Exercises_Man");
                });

            modelBuilder.Entity("Gimify.Entities.Favourite", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("Postsid")
                        .HasColumnType("integer");

                    b.HasKey("id", "UserId");

                    b.HasIndex("Postsid");

                    b.ToTable("Favourite");
                });

            modelBuilder.Entity("Gimify.Entities.Posts", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("FavouriteCount")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Gimify.Entities.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<int>("PasswordSalt")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Gimify.Entities.Exercises_Man", b =>
                {
                    b.HasOne("Gimify.Entities.Exercises", "Exercises")
                        .WithMany("Exercises_Man")
                        .HasForeignKey("Exercisesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("Gimify.Entities.Favourite", b =>
                {
                    b.HasOne("Gimify.Entities.Posts", "Posts")
                        .WithMany("Favourite")
                        .HasForeignKey("Postsid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Gimify.Entities.Exercises", b =>
                {
                    b.Navigation("Exercises_Man");
                });

            modelBuilder.Entity("Gimify.Entities.Posts", b =>
                {
                    b.Navigation("Favourite");
                });
#pragma warning restore 612, 618
        }
    }
}
