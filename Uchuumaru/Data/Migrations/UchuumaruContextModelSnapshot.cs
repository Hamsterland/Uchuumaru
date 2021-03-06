﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Uchuumaru.Data;

namespace Uchuumaru.Migrations
{
    [DbContext(typeof(UchuumaruContext))]
    partial class UchuumaruContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Uchuumaru.Data.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.HasIndex("GuildId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Expression")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GuildId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Filter");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<long>("BirthdayChannelId")
                        .HasColumnType("bigint");

                    b.Property<bool>("EnabledFilter")
                        .HasColumnType("boolean");

                    b.Property<long>("FilterChannelId")
                        .HasColumnType("bigint");

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<long>("InfractionChannelId")
                        .HasColumnType("bigint");

                    b.Property<long>("MessageChannelId")
                        .HasColumnType("bigint");

                    b.Property<long>("ModeratorRoleId")
                        .HasColumnType("bigint");

                    b.Property<long>("MuteRoleId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReportChannelId")
                        .HasColumnType("bigint");

                    b.Property<long>("TrafficChannelId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuildId")
                        .IsUnique();

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Infraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<int>("GuildId")
                        .HasColumnType("integer");

                    b.Property<long>("ModeratorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeInvoked")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("ModeratorId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Infraction");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.MALUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("MyAnimeListId")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MyAnimeListId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MALUsers");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Nickname", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Value");

                    b.ToTable("Nickname");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("GuildId")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Username", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Value");

                    b.ToTable("Username");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Filter", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.Guild", "Guild")
                        .WithMany("Filters")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Infraction", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.Guild", "Guild")
                        .WithMany("Infractions")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Nickname", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.User", "User")
                        .WithMany("Nicknames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.User", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Username", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.User", "User")
                        .WithMany("Usernames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Guild", b =>
                {
                    b.Navigation("Filters");

                    b.Navigation("Infractions");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.User", b =>
                {
                    b.Navigation("Nicknames");

                    b.Navigation("Usernames");
                });
#pragma warning restore 612, 618
        }
    }
}
