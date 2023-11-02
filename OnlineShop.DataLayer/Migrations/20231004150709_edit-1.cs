using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class edit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeID",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_WalletTypeTypeID",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletTypeTypeID",
                table: "Wallets");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_TypeID",
                table: "Wallets",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletTypes_TypeID",
                table: "Wallets",
                column: "TypeID",
                principalTable: "WalletTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletTypes_TypeID",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_TypeID",
                table: "Wallets");

            migrationBuilder.AddColumn<int>(
                name: "WalletTypeTypeID",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletTypeTypeID",
                table: "Wallets",
                column: "WalletTypeTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeID",
                table: "Wallets",
                column: "WalletTypeTypeID",
                principalTable: "WalletTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
