using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawal",
                table: "Withdrawal");

            migrationBuilder.RenameTable(
                name: "Withdrawal",
                newName: "Withdrawals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Payee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurringDate = table.Column<int>(type: "int", nullable: false),
                    UpcomingPaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentAmount = table.Column<double>(type: "float", nullable: false),
                    AcccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bill_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_AccountId",
                table: "Bill",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals");

            migrationBuilder.RenameTable(
                name: "Withdrawals",
                newName: "Withdrawal");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawal",
                table: "Withdrawal",
                column: "Id");
        }
    }
}
