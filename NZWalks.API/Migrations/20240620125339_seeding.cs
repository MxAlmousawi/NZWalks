using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2359a0be-6e42-495f-b50b-dc4e623ccdb2"), "" },
                    { new Guid("9ebaf802-a467-41ea-9ec7-ba3f2a9ed229"), "" },
                    { new Guid("e718c795-1405-4a29-bd70-ead243c8ae41"), "" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("5be09984-07e4-47c7-a09c-27cd5f326dbb"), "CCC", "Region c", "image.com" },
                    { new Guid("ba6553c1-0b0d-4875-9e7c-67dbb16fc80e"), "AAA", "Region a", "image.com" },
                    { new Guid("e4db930c-6717-4284-bbf1-d8323e52c09c"), "BBB", "Region b", "image.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("2359a0be-6e42-495f-b50b-dc4e623ccdb2"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("9ebaf802-a467-41ea-9ec7-ba3f2a9ed229"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e718c795-1405-4a29-bd70-ead243c8ae41"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("5be09984-07e4-47c7-a09c-27cd5f326dbb"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("ba6553c1-0b0d-4875-9e7c-67dbb16fc80e"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("e4db930c-6717-4284-bbf1-d8323e52c09c"));
        }
    }
}
