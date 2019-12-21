using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AlbumNullableFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Format_FormatId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Album_Genre_GenreId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Album_Style_StyleId",
                table: "Album");

            migrationBuilder.AlterColumn<long>(
                name: "StyleId",
                table: "Album",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "GenreId",
                table: "Album",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "FormatId",
                table: "Album",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Format_FormatId",
                table: "Album",
                column: "FormatId",
                principalTable: "Format",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Genre_GenreId",
                table: "Album",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Style_StyleId",
                table: "Album",
                column: "StyleId",
                principalTable: "Style",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Format_FormatId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Album_Genre_GenreId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Album_Style_StyleId",
                table: "Album");

            migrationBuilder.AlterColumn<long>(
                name: "StyleId",
                table: "Album",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GenreId",
                table: "Album",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FormatId",
                table: "Album",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Format_FormatId",
                table: "Album",
                column: "FormatId",
                principalTable: "Format",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Genre_GenreId",
                table: "Album",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Style_StyleId",
                table: "Album",
                column: "StyleId",
                principalTable: "Style",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
