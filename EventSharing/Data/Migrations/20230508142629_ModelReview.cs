using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSharing.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_PartipantsId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "PartipantsId",
                table: "EventUser",
                newName: "ParticipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_PartipantsId",
                table: "EventUser",
                newName: "IX_EventUser_ParticipantsId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser",
                column: "ParticipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventUser_AspNetUsers_ParticipantsId",
                table: "EventUser");

            migrationBuilder.RenameColumn(
                name: "ParticipantsId",
                table: "EventUser",
                newName: "PartipantsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventUser_ParticipantsId",
                table: "EventUser",
                newName: "IX_EventUser_PartipantsId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventUser_AspNetUsers_PartipantsId",
                table: "EventUser",
                column: "PartipantsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
