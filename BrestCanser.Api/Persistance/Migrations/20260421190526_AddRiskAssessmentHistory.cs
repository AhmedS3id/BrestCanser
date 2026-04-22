using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrestCanser.Api.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskAssessmentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskAssessmentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeGroup = table.Column<int>(type: "int", nullable: false),
                    Ethnicity = table.Column<int>(type: "int", nullable: false),
                    BmiCategory = table.Column<int>(type: "int", nullable: false),
                    MenarcheAge = table.Column<int>(type: "int", nullable: false),
                    PregnancyHistory = table.Column<int>(type: "int", nullable: false),
                    MenopauseStatus = table.Column<int>(type: "int", nullable: false),
                    FamilyHistoryLevel = table.Column<int>(type: "int", nullable: false),
                    EarlyFamilyDiagnosis = table.Column<int>(type: "int", nullable: false),
                    BrcaMutation = table.Column<int>(type: "int", nullable: false),
                    BreastDensity = table.Column<int>(type: "int", nullable: false),
                    BiopsyResult = table.Column<int>(type: "int", nullable: false),
                    RadiationHistory = table.Column<int>(type: "int", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskProbability = table.Column<double>(type: "float", nullable: false),
                    Classification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reasoning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskAssessmentHistories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskAssessmentHistories");
        }
    }
}
