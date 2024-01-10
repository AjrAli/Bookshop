﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookshop.Persistence.Migrations
{
    public partial class UpdateOrderForAddingSubTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Orders");
        }
    }
}
