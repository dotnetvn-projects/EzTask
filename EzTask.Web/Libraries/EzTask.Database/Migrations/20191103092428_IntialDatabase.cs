using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EzTask.Database.Migrations
{
    public partial class IntialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManageAccountId = table.Column<int>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    AccountStatus = table.Column<short>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Account_ManageAccountId",
                        column: x => x.ManageAccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    JobTitle = table.Column<string>(nullable: true),
                    Education = table.Column<string>(nullable: true),
                    DisplayImage = table.Column<byte[]>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Introduce = table.Column<string>(nullable: true),
                    Document = table.Column<byte[]>(nullable: true),
                    LangDisplay = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountInfo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Context = table.Column<short>(nullable: false),
                    RefData = table.Column<string>(nullable: true),
                    HasViewed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Owner = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    MaximumUser = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Account_Owner",
                        column: x => x.Owner,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecoverSession",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(nullable: false),
                    ExpiredTime = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecoverSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecoverSession_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Priority = table.Column<short>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    CompleteOn = table.Column<DateTime>(nullable: false),
                    Owner = table.Column<int>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItem_Account_Owner",
                        column: x => x.Owner,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account_Skill",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    SkillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Skill_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Skill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    PhaseName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phase_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project_Member",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    AddDate = table.Column<DateTime>(nullable: false),
                    IsPending = table.Column<bool>(nullable: false),
                    ActiveCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Member_Account_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Project_Member_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    TaskTitle = table.Column<string>(nullable: true),
                    TaskDetail = table.Column<string>(nullable: true),
                    AssigneeId = table.Column<int>(nullable: true),
                    MemberId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    PhaseId = table.Column<int>(nullable: false),
                    Priority = table.Column<short>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PercentCompleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItem_Account_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskItem_Account_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskItem_Phase_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskItem_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    FileData = table.Column<byte[]>(nullable: true),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    AddedUser = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Account_AddedUser",
                        column: x => x.AddedUser,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachment_TaskItem_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UpdatedUser = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskHistory_TaskItem_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskHistory_Account_UpdatedUser",
                        column: x => x.UpdatedUser,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ManageAccountId",
                table: "Account",
                column: "ManageAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Skill_AccountId",
                table: "Account_Skill",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Skill_SkillId",
                table: "Account_Skill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfo_AccountId",
                table: "AccountInfo",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_AddedUser",
                table: "Attachment",
                column: "AddedUser");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_TaskId",
                table: "Attachment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AccountId",
                table: "Notification",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Phase_ProjectId",
                table: "Phase",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Owner",
                table: "Project",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Member_MemberId",
                table: "Project_Member",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_Member_ProjectId",
                table: "Project_Member",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RecoverSession_AccountId",
                table: "RecoverSession",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistory_TaskId",
                table: "TaskHistory",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskHistory_UpdatedUser",
                table: "TaskHistory",
                column: "UpdatedUser");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_AssigneeId",
                table: "TaskItem",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_MemberId",
                table: "TaskItem",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_PhaseId",
                table: "TaskItem",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_ProjectId",
                table: "TaskItem",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_Owner",
                table: "ToDoItem",
                column: "Owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Skill");

            migrationBuilder.DropTable(
                name: "AccountInfo");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Project_Member");

            migrationBuilder.DropTable(
                name: "RecoverSession");

            migrationBuilder.DropTable(
                name: "TaskHistory");

            migrationBuilder.DropTable(
                name: "ToDoItem");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "TaskItem");

            migrationBuilder.DropTable(
                name: "Phase");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
