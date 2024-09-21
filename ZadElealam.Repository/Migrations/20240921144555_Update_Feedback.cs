using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZadElealam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Update_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "FeedbackMessage",
                table: "Feedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FeedbackMessage",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
