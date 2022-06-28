using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseBackFinal.Migrations
{
    public partial class updatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CourseModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CourseModelId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "CourseModelStudentUserModel",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModelStudentUserModel", x => new { x.CoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseModelStudentUserModel_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseModelStudentUserModel_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseModelStudentUserModel_StudentsId",
                table: "CourseModelStudentUserModel",
                column: "StudentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseModelStudentUserModel");

            migrationBuilder.AddColumn<int>(
                name: "CourseModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CourseModelId",
                table: "AspNetUsers",
                column: "CourseModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseModelId",
                table: "AspNetUsers",
                column: "CourseModelId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
