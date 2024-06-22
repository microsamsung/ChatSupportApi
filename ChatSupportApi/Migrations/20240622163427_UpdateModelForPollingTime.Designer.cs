﻿// <auto-generated />
using System;
using ChatSupportApi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatSupportApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240622163427_UpdateModelForPollingTime")]
    partial class UpdateModelForPollingTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("ChatSupportApi.Models.Agent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Seniority")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Shift")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("ChatSupportApi.Models.ChatQueue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOverflow")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastPollTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ChatQueues");
                });

            modelBuilder.Entity("ChatSupportApi.Models.ChatSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AssignedAgentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChatQueueId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastPolledTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AssignedAgentId");

                    b.HasIndex("ChatQueueId");

                    b.ToTable("ChatSessions");
                });

            modelBuilder.Entity("ChatSupportApi.Models.ChatSession", b =>
                {
                    b.HasOne("ChatSupportApi.Models.Agent", "AssignedAgent")
                        .WithMany()
                        .HasForeignKey("AssignedAgentId");

                    b.HasOne("ChatSupportApi.Models.ChatQueue", null)
                        .WithMany("ChatSessions")
                        .HasForeignKey("ChatQueueId");

                    b.Navigation("AssignedAgent");
                });

            modelBuilder.Entity("ChatSupportApi.Models.ChatQueue", b =>
                {
                    b.Navigation("ChatSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
