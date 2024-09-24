using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace talk2note.API.Migrations
{
    /// <inheritdoc />
    public partial class NotesTableEdits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Notes");
        }
    }
}
