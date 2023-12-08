using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Beyond.SearchEngine.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(12)", nullable: false),
                    OrcId = table.Column<string>(type: "char(20)", nullable: false),
                    Name = table.Column<string>(type: "varchar(63)", nullable: false),
                    WorksCount = table.Column<int>(type: "int", nullable: false),
                    CitationCount = table.Column<int>(type: "int", nullable: false),
                    HIndex = table.Column<int>(type: "int", nullable: false),
                    Institution = table.Column<string>(type: "longtext", nullable: false),
                    Concepts = table.Column<string>(type: "longtext", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(12)", nullable: false),
                    Name = table.Column<string>(type: "varchar(127)", nullable: false),
                    Type = table.Column<string>(type: "varchar(15)", nullable: false),
                    Country = table.Column<string>(type: "char(8)", nullable: false),
                    HomepageUrl = table.Column<string>(type: "varchar(63)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(127)", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "varchar(127)", nullable: false),
                    Concepts = table.Column<string>(type: "longtext", nullable: false),
                    AssociatedInstitutions = table.Column<string>(type: "longtext", nullable: false),
                    WorksCount = table.Column<int>(type: "int", nullable: false),
                    CitationCount = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UpdateHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Completed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<string>(type: "varchar(15)", nullable: false),
                    UpdatedTime = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateHistories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "char(63)", nullable: false),
                    Password = table.Column<string>(type: "char(63)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(16)", nullable: false),
                    Doi = table.Column<string>(type: "varchar(63)", nullable: false),
                    Title = table.Column<string>(type: "varchar(127)", nullable: false),
                    Abstract = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<string>(type: "varchar(15)", nullable: false),
                    Language = table.Column<string>(type: "char(8)", nullable: false),
                    PdfUrl = table.Column<string>(type: "varchar(127)", nullable: false),
                    Concepts = table.Column<string>(type: "longtext", nullable: false),
                    Keywords = table.Column<string>(type: "longtext", nullable: false),
                    RelatedWorks = table.Column<string>(type: "longtext", nullable: false),
                    ReferencedWorks = table.Column<string>(type: "longtext", nullable: false),
                    Authors = table.Column<string>(type: "longtext", nullable: false),
                    CitationCount = table.Column<int>(type: "int", nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<string>(type: "char(12)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AssociatedInstitutionData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false),
                    Country = table.Column<string>(type: "longtext", nullable: false),
                    Relation = table.Column<string>(type: "longtext", nullable: false),
                    InstitutionId = table.Column<string>(type: "char(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociatedInstitutionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssociatedInstitutionData_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuthorData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Position = table.Column<string>(type: "longtext", nullable: false),
                    OrcId = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    WorkId = table.Column<string>(type: "varchar(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorData_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConceptData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "double", nullable: false),
                    AuthorId = table.Column<string>(type: "char(12)", nullable: true),
                    InstitutionId = table.Column<string>(type: "char(12)", nullable: true),
                    WorkId = table.Column<string>(type: "varchar(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptData_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptData_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConceptData_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AssociatedInstitutionData_InstitutionId",
                table: "AssociatedInstitutionData",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorData_WorkId",
                table: "AuthorData",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptData_AuthorId",
                table: "ConceptData",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptData_InstitutionId",
                table: "ConceptData",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptData_WorkId",
                table: "ConceptData",
                column: "WorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssociatedInstitutionData");

            migrationBuilder.DropTable(
                name: "AuthorData");

            migrationBuilder.DropTable(
                name: "ConceptData");

            migrationBuilder.DropTable(
                name: "UpdateHistories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Institutions");

            migrationBuilder.DropTable(
                name: "Works");
        }
    }
}
