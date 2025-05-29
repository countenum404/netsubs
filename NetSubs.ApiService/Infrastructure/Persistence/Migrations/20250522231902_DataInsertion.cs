using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetSubs.ApiService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DataInsertion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SubscriptionTypes",
                columns: new[] { "Id", "TypeName", "MonthlyPrice", "IsActive" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Базовая подписка", 10m, true },
                    { Guid.NewGuid(), "Премиальная подписка", 20m, true },
                    { Guid.NewGuid(), "Корпоративная подписка", 50m, true }
                });
            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "user1@example.com", "Игорь Петров" },
                    { Guid.NewGuid(), "user2@example.com", "Анна Иванова" },
                    { Guid.NewGuid(), "user3@example.com", "Михаил Сидоров" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
