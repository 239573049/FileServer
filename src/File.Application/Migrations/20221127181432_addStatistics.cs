using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File.Application.Migrations
{
    /// <inheritdoc />
    public partial class addStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "用户",
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: new Guid("53e76abc-f1b0-4d71-ba5e-d478d0017fef"));

            migrationBuilder.EnsureSchema(
                name: "接口访问统计");

            migrationBuilder.CreateTable(
                name: "InterfaceStatistics",
                schema: "接口访问统计",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Succeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "访问时间"),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true, comment: "具体访问人id"),
                    Query = table.Column<string>(type: "TEXT", nullable: false, comment: "访问时携带的参数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfaceStatistics", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "用户",
                table: "UserInfos",
                columns: new[] { "Id", "Avatar", "Password", "Username" },
                values: new object[] { new Guid("6f3ff718-7db1-4576-b0e8-cd6e4cb17279"), "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png", "Aa123456.", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_InterfaceStatistics_Id",
                schema: "接口访问统计",
                table: "InterfaceStatistics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterfaceStatistics_UserId",
                schema: "接口访问统计",
                table: "InterfaceStatistics",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterfaceStatistics",
                schema: "接口访问统计");

            migrationBuilder.DeleteData(
                schema: "用户",
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: new Guid("6f3ff718-7db1-4576-b0e8-cd6e4cb17279"));

            migrationBuilder.InsertData(
                schema: "用户",
                table: "UserInfos",
                columns: new[] { "Id", "Avatar", "Password", "Username" },
                values: new object[] { new Guid("53e76abc-f1b0-4d71-ba5e-d478d0017fef"), "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png", "Aa123456.", "admin" });
        }
    }
}
