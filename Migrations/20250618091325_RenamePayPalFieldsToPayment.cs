using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenamePayPalFieldsToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayPalPaymentId",
                table: "Orders",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "PayPalPayerId",
                table: "Orders",
                newName: "PayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Orders",
                newName: "PayPalPaymentId");

            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "Orders",
                newName: "PayPalPayerId");
        }
    }
}
