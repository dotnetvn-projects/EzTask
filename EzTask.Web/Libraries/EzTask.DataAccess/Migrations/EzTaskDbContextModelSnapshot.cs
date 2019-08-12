﻿// <auto-generated />
using System;
using EzTask.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EzTask.DataAccess.Migrations
{
    [DbContext(typeof(EzTaskDbContext))]
    partial class EzTaskDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EzTask.Entity.Data.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName");

                    b.Property<short>("AccountStatus");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int?>("ManageAccountId");

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("ManageAccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("EzTask.Entity.Data.AccountInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<DateTime?>("BirthDay");

                    b.Property<string>("Comment");

                    b.Property<byte[]>("DisplayImage");

                    b.Property<string>("DisplayName");

                    b.Property<byte[]>("Document");

                    b.Property<string>("Education");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("Introduce");

                    b.Property<bool>("IsPublished");

                    b.Property<string>("JobTitle");

                    b.Property<string>("LangDisplay");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("AccountInfo");
                });

            modelBuilder.Entity("EzTask.Entity.Data.AccountSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int>("SkillId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("SkillId");

                    b.ToTable("Account_Skill");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedDate");

                    b.Property<int>("AddedUser");

                    b.Property<byte[]>("FileData");

                    b.Property<string>("FileName");

                    b.Property<string>("FileType");

                    b.Property<int>("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("AddedUser");

                    b.HasIndex("TaskId");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("Content");

                    b.Property<short>("Context");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("HasViewed");

                    b.Property<string>("RefData");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Phase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("PhaseName");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime?>("StartDate");

                    b.Property<short>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Phase");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<int>("MaximumUser");

                    b.Property<int>("Owner");

                    b.Property<string>("ProjectCode");

                    b.Property<string>("ProjectName");

                    b.Property<short>("Status");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("EzTask.Entity.Data.ProjectMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActiveCode");

                    b.Property<DateTime>("AddDate");

                    b.Property<bool>("IsPending");

                    b.Property<int>("MemberId");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Project_Member");
                });

            modelBuilder.Entity("EzTask.Entity.Data.RecoverSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<Guid>("Code");

                    b.Property<DateTime>("ExpiredTime");

                    b.Property<bool>("IsUsed");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RecoverSession");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SkillName");

                    b.HasKey("Id");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("EzTask.Entity.Data.TaskHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<int>("TaskId");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UpdatedUser");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("UpdatedUser");

                    b.ToTable("TaskHistory");
                });

            modelBuilder.Entity("EzTask.Entity.Data.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssigneeId");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int>("MemberId");

                    b.Property<int>("PercentCompleted");

                    b.Property<int>("PhaseId");

                    b.Property<short>("Priority");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime?>("StartDate");

                    b.Property<short>("Status");

                    b.Property<string>("TaskCode");

                    b.Property<string>("TaskDetail");

                    b.Property<string>("TaskTitle");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("MemberId");

                    b.HasIndex("PhaseId");

                    b.HasIndex("ProjectId");

                    b.ToTable("TaskItem");
                });

            modelBuilder.Entity("EzTask.Entity.Data.ToDoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CompleteOn");

                    b.Property<int>("Owner");

                    b.Property<short>("Priority");

                    b.Property<short>("Status");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("ToDoItem");
                });

            modelBuilder.Entity("EzTask.Entity.Data.Account", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "ManageAccount")
                        .WithMany()
                        .HasForeignKey("ManageAccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.AccountInfo", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithOne("AccountInfo")
                        .HasForeignKey("EzTask.Entity.Data.AccountInfo", "AccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.AccountSkill", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.Attachment", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "User")
                        .WithMany()
                        .HasForeignKey("AddedUser")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.TaskItem", "Task")
                        .WithMany("Attachments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.Notification", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.Phase", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.Project", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithMany()
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.ProjectMember", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Member")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Project", "Project")
                        .WithMany("ProjectMembers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.RecoverSession", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.TaskHistory", b =>
                {
                    b.HasOne("EzTask.Entity.Data.TaskItem", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Account", "User")
                        .WithMany()
                        .HasForeignKey("UpdatedUser")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.TaskItem", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Account", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Phase", "Phase")
                        .WithMany()
                        .HasForeignKey("PhaseId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EzTask.Entity.Data.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EzTask.Entity.Data.ToDoItem", b =>
                {
                    b.HasOne("EzTask.Entity.Data.Account", "Account")
                        .WithMany()
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
