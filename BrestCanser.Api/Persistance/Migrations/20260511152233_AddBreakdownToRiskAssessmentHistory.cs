using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrestCanser.Api.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddBreakdownToRiskAssessmentHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BreakdownFamilyHistory",
                table: "RiskAssessmentHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BreakdownGeneticFactors",
                table: "RiskAssessmentHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BreakdownLifestyle",
                table: "RiskAssessmentHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakdownFamilyHistory",
                table: "RiskAssessmentHistories");

            migrationBuilder.DropColumn(
                name: "BreakdownGeneticFactors",
                table: "RiskAssessmentHistories");

            migrationBuilder.DropColumn(
                name: "BreakdownLifestyle",
                table: "RiskAssessmentHistories");
        }
    }
}
