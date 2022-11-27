using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File.Application.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "路由映射缓存表");

            migrationBuilder.EnsureSchema(
                name: "用户");

            migrationBuilder.CreateTable(
                name: "UserInfos",
                schema: "用户",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false, comment: "用户名（唯一）"),
                    Password = table.Column<string>(type: "TEXT", nullable: false, comment: "密码"),
                    Avatar = table.Column<string>(type: "TEXT", nullable: false, comment: "头像")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteMappings",
                schema: "路由映射缓存表",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Route = table.Column<string>(type: "TEXT", nullable: false, comment: "路由"),
                    Path = table.Column<string>(type: "TEXT", nullable: false, comment: "绝对路径"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false, comment: "地址类型"),
                    Visitor = table.Column<bool>(type: "INTEGER", nullable: false, comment: "是否同意他人访问"),
                    CreateUserInfoId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "创建人")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteMappings_UserInfos_CreateUserInfoId",
                        column: x => x.CreateUserInfoId,
                        principalSchema: "用户",
                        principalTable: "UserInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "用户",
                table: "UserInfos",
                columns: new[] { "Id", "Avatar", "Password", "Username" },
                values: new object[] { new Guid("53e76abc-f1b0-4d71-ba5e-d478d0017fef"), "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png", "Aa123456.", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_RouteMappings_CreateUserInfoId",
                schema: "路由映射缓存表",
                table: "RouteMappings",
                column: "CreateUserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteMappings_Id",
                schema: "路由映射缓存表",
                table: "RouteMappings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_Id",
                schema: "用户",
                table: "UserInfos",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_Username",
                schema: "用户",
                table: "UserInfos",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteMappings",
                schema: "路由映射缓存表");

            migrationBuilder.DropTable(
                name: "UserInfos",
                schema: "用户");
        }
    }
}
