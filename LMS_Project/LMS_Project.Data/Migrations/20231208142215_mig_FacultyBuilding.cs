using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Project.Data.Migrations
{
    public partial class mig_FacultyBuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacultyBuildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: false),
                    StreetNumber = table.Column<string>(maxLength: 150, nullable: false),
                    StreetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacultyBuildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacultyBuildings_Streets_StreetId",
                        column: x => x.StreetId,
                        principalTable: "Streets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacultyBuildings_StreetId",
                table: "FacultyBuildings",
                column: "StreetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacultyBuildings");
        }
    }
}
