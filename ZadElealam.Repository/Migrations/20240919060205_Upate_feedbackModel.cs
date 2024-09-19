using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZadElealam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Upate_feedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YouTubePlaylistId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_YouTubePlaylistId",
                table: "Feedbacks",
                column: "YouTubePlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_YouTubePlaylists_YouTubePlaylistId",
                table: "Feedbacks",
                column: "YouTubePlaylistId",
                principalTable: "YouTubePlaylists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_YouTubePlaylists_YouTubePlaylistId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_YouTubePlaylistId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "YouTubePlaylistId",
                table: "Feedbacks");
        }
    }
}
