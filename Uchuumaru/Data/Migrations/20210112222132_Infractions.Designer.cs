﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Uchuumaru.Data;

namespace Uchuumaru.Migrations
{
    [DbContext(typeof(UchuumaruContext))]
    [Migration("20210112222132_Infractions")]
    partial class Infractions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.ToTable("Channel");
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

                    b.Property<long>("GuildId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuildId")
                        .IsUnique();

                    b.ToTable("Guild");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Infraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Duration")
                        .HasColumnType("timestamp without time zone");

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

            modelBuilder.Entity("Uchuumaru.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("GuildId")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("User");
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

            modelBuilder.Entity("Uchuumaru.Data.Models.User", b =>
                {
                    b.HasOne("Uchuumaru.Data.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Uchuumaru.Data.Models.Guild", b =>
                {
                    b.Navigation("Filters");

                    b.Navigation("Infractions");
                });
#pragma warning restore 612, 618
        }
    }
}
