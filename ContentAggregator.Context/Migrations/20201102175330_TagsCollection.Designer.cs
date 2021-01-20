﻿// <auto-generated />
using System;
using ContentAggregator.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ContentAggregator.Context.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201102175330_TagsCollection")]
    partial class TagsCollection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ContentAggregator.Context.Entities.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Dislikes")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Hash", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Hashes");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Comment>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .HasColumnType("text");

                    b.Property<bool>("IsLike")
                        .HasColumnType("boolean");

                    b.HasKey("UserId", "EntityId");

                    b.HasIndex("EntityId");

                    b.ToTable("CommentLikes");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Post>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .HasColumnType("text");

                    b.Property<bool>("IsLike")
                        .HasColumnType("boolean");

                    b.HasKey("UserId", "EntityId");

                    b.HasIndex("EntityId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Response>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .HasColumnType("text");

                    b.Property<bool>("IsLike")
                        .HasColumnType("boolean");

                    b.HasKey("UserId", "EntityId");

                    b.HasIndex("EntityId");

                    b.ToTable("ResponseLikes");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Picture", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("MimeType")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Post", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(2000)")
                        .HasMaxLength(2000);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Dislikes")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("_tags")
                        .HasColumnName("TagCollection")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Response", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CommentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("character varying(1000)")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Dislikes")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CommentId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PostsNumber")
                        .HasColumnType("integer");

                    b.HasKey("Name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string[]>("BlackListedTags")
                        .HasColumnType("text[]");

                    b.Property<string[]>("BlackListedUserIds")
                        .HasColumnType("text[]");

                    b.Property<byte>("CredentialLevel")
                        .HasColumnType("smallint");

                    b.Property<string>("Description")
                        .HasColumnType("character varying(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("FollowedTags")
                        .HasColumnType("text[]");

                    b.Property<string[]>("FollowedUserIds")
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(25)")
                        .HasMaxLength(25);

                    b.Property<string>("PictureId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Comment", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentAggregator.Context.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Comment>", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.Comment", "Entity")
                        .WithMany("CommentLikes")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentAggregator.Context.Entities.User", "User")
                        .WithMany("CommentLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Post>", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.Post", "Entity")
                        .WithMany("PostLikes")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentAggregator.Context.Entities.User", "User")
                        .WithMany("PostLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Likes.BaseLikeEntity<ContentAggregator.Context.Entities.Response>", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.Response", "Entity")
                        .WithMany("ResponseLikes")
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentAggregator.Context.Entities.User", "User")
                        .WithMany("ResponseLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Picture", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.User", "User")
                        .WithOne("Picture")
                        .HasForeignKey("ContentAggregator.Context.Entities.Picture", "UserId");
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Post", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContentAggregator.Context.Entities.Response", b =>
                {
                    b.HasOne("ContentAggregator.Context.Entities.User", "Author")
                        .WithMany("Responses")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContentAggregator.Context.Entities.Comment", "Comment")
                        .WithMany("Responses")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
