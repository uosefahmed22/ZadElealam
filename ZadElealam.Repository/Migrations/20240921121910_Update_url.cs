using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZadElealam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Update_url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "YouTubeVideos",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "YouTubePlaylists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "YouTubePlaylists");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "YouTubeVideos",
                newName: "VideoUrl");
        }
    }
}
