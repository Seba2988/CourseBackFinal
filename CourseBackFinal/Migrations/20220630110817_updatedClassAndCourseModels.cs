using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseBackFinal.Migrations
{
    public partial class updatedClassAndCourseModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Classes_ClassModelId",
                table: "Absences");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Name",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Absences_ClassModelId",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "ClassModelId",
                table: "Absences");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Absences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name_ProfessorId",
                table: "Courses",
                columns: new[] { "Name", "ProfessorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Absences_ClassId",
                table: "Absences",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Classes_ClassId",
                table: "Absences",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Classes_ClassId",
                table: "Absences");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Name_ProfessorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Absences_ClassId",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Absences");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ClassModelId",
                table: "Absences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Name",
                table: "Courses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Absences_ClassModelId",
                table: "Absences",
                column: "ClassModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Classes_ClassModelId",
                table: "Absences",
                column: "ClassModelId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
