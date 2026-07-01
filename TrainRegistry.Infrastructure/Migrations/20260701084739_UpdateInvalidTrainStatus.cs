using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainRegistry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvalidTrainStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Trains\" SET \"TrainStatus\" = 'Inactive' WHERE \"TrainStatus\" IS NULL OR \"TrainStatus\" = ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This data migration cannot be automatically reversed.
            // Original values were NULL or empty string before TrainStatus was introduced.
            // To rollback, manually inspect and restore affected rows if needed.
        }
    }
}
