using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatbotAI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignalR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat");

            migrationBuilder.CreateTable(
                name: "chatbot",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<bool>(type: "bit", nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    interrupted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatbot", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatbot");

            migrationBuilder.CreateTable(
                name: "chat",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    interrupted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    rating = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat", x => x.id);
                });
        }
    }
}
