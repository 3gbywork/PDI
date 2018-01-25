using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PDI.NetCoreDemos.Migrations
{
    public partial class Version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VCardDTO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Categories = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    FormattedName = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LastRevision = table.Column<DateTime>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    Organization = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    SortString = table.Column<string>(nullable: true),
                    SortableName = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Units = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VCardDTO", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VCardDTO");
        }
    }
}
