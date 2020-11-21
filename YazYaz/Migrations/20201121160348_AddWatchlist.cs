using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace YazYaz.Migrations
{
    public partial class AddWatchlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "WatchlistId",
                table: "Movie",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Watchlist",
                columns: table => new
                {
                    WatchlistId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.WatchlistId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_WatchlistId",
                table: "Movie",
                column: "WatchlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Watchlist_WatchlistId",
                table: "Movie",
                column: "WatchlistId",
                principalTable: "Watchlist",
                principalColumn: "WatchlistId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Watchlist_WatchlistId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "Watchlist");

            migrationBuilder.DropIndex(
                name: "IX_Movie_WatchlistId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "WatchlistId",
                table: "Movie");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Movie",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
