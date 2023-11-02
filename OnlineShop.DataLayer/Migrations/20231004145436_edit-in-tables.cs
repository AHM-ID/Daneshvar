using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class editintables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypesTypeID",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "WalletTypesTypeID",
                table: "Wallets",
                newName: "WalletTypeTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_WalletTypesTypeID",
                table: "Wallets",
                newName: "IX_Wallets_WalletTypeTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeID",
                table: "Wallets",
                column: "WalletTypeTypeID",
                principalTable: "WalletTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypeTypeID",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "WalletTypeTypeID",
                table: "Wallets",
                newName: "WalletTypesTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_WalletTypeTypeID",
                table: "Wallets",
                newName: "IX_Wallets_WalletTypesTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_WalletTypes_WalletTypesTypeID",
                table: "Wallets",
                column: "WalletTypesTypeID",
                principalTable: "WalletTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
