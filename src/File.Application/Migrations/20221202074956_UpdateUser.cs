using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File.Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "用户",
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: new Guid("d307357e-adf0-4be7-9c5c-f3751c231db1"));

            migrationBuilder.DropColumn(
                name: "Visitor",
                schema: "路由映射缓存表",
                table: "RouteMappings");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "用户",
                table: "UserInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "路由映射缓存表",
                table: "RouteMappings",
                type: "TEXT",
                nullable: true,
                comment: "访问密码");

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                schema: "接口访问统计",
                table: "InterfaceStatistics",
                type: "INTEGER",
                nullable: false,
                comment: "响应状态码",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.InsertData(
                schema: "用户",
                table: "UserInfos",
                columns: new[] { "Id", "Avatar", "Password", "Role", "Username" },
                values: new object[] { new Guid("1ddcfa8d-e603-410d-a3f0-ec4a3136dfd2"), "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png", "Aa123456.", "admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_InterfaceStatistics_CreatedTime",
                schema: "接口访问统计",
                table: "InterfaceStatistics",
                column: "CreatedTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InterfaceStatistics_CreatedTime",
                schema: "接口访问统计",
                table: "InterfaceStatistics");

            migrationBuilder.DeleteData(
                schema: "用户",
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: new Guid("1ddcfa8d-e603-410d-a3f0-ec4a3136dfd2"));

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "用户",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "路由映射缓存表",
                table: "RouteMappings");

            migrationBuilder.AddColumn<bool>(
                name: "Visitor",
                schema: "路由映射缓存表",
                table: "RouteMappings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                comment: "是否同意他人访问");

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                schema: "接口访问统计",
                table: "InterfaceStatistics",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "响应状态码");

            migrationBuilder.InsertData(
                schema: "用户",
                table: "UserInfos",
                columns: new[] { "Id", "Avatar", "Password", "Username" },
                values: new object[] { new Guid("d307357e-adf0-4be7-9c5c-f3751c231db1"), "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png", "Aa123456.", "admin" });
        }
    }
}
