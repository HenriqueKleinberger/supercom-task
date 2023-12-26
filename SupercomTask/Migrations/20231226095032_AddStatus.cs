using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupercomTask.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Status Values ('To Do')");
            migrationBuilder.Sql("INSERT INTO Status Values ('In Progress')");
            migrationBuilder.Sql("INSERT INTO Status Values ('Done')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Status WHERE Name = 'To Do'");
            migrationBuilder.Sql("DELETE FROM Status WHERE Name =  'In Progress'");
            migrationBuilder.Sql("DELETE FROM Status WHERE Name = 'Done'");
        }
    }
}
