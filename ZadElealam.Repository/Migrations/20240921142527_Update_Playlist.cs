using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZadElealam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Update_Playlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CourseHours",
                table: "YouTubePlaylists",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "VideosCount",
                table: "YouTubePlaylists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseHours",
                table: "YouTubePlaylists");

            migrationBuilder.DropColumn(
                name: "VideosCount",
                table: "YouTubePlaylists");
        }
    }
}
