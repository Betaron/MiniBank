using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minibank.Data.Migrations
{
    public partial class SnakeCaseConvention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bank_accounts_users_user_id",
                table: "bank_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_history_bank_accounts_from_account_id",
                table: "transactions_history");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_history_bank_accounts_to_account_id",
                table: "transactions_history");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_history_to_account_id",
                table: "transactions_history",
                newName: "ix_transactions_history_to_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_history_from_account_id",
                table: "transactions_history",
                newName: "ix_transactions_history_from_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_bank_accounts_user_id",
                table: "bank_accounts",
                newName: "ix_bank_accounts_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_bank_accounts_users_user_id",
                table: "bank_accounts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_history_bank_accounts_from_account_id",
                table: "transactions_history",
                column: "from_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_transactions_history_bank_accounts_to_account_id",
                table: "transactions_history",
                column: "to_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bank_accounts_users_user_id",
                table: "bank_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_history_bank_accounts_from_account_id",
                table: "transactions_history");

            migrationBuilder.DropForeignKey(
                name: "fk_transactions_history_bank_accounts_to_account_id",
                table: "transactions_history");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_history_to_account_id",
                table: "transactions_history",
                newName: "IX_transactions_history_to_account_id");

            migrationBuilder.RenameIndex(
                name: "ix_transactions_history_from_account_id",
                table: "transactions_history",
                newName: "IX_transactions_history_from_account_id");

            migrationBuilder.RenameIndex(
                name: "ix_bank_accounts_user_id",
                table: "bank_accounts",
                newName: "IX_bank_accounts_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bank_accounts_users_user_id",
                table: "bank_accounts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_history_bank_accounts_from_account_id",
                table: "transactions_history",
                column: "from_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_history_bank_accounts_to_account_id",
                table: "transactions_history",
                column: "to_account_id",
                principalTable: "bank_accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
