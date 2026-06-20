using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SurveysApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date_register = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("surveys_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_rol = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ask = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    type_ask = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    required = table.Column<int>(type: "integer", nullable: false),
                    options = table.Column<string>(type: "text", nullable: true),
                    surveys_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("questions_pk", x => x.id);
                    table.ForeignKey(
                        name: "questions_ surveys_id_fk",
                        column: x => x.surveys_id,
                        principalTable: "Surveys",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answered",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    data_surveys = table.Column<string>(type: "text", nullable: false),
                    date_start = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    date_end = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    survey_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("answered_pk", x => x.id);
                    table.ForeignKey(
                        name: "surveyanswered_surveys_id_fk",
                        column: x => x.survey_id,
                        principalTable: "Surveys",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "surveyanswered_users_id_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answered_survey_id",
                table: "Answered",
                column: "survey_id");

            migrationBuilder.CreateIndex(
                name: "IX_Answered_user_id",
                table: "Answered",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_surveys_id",
                table: "Questions",
                column: "surveys_id");

            migrationBuilder.CreateIndex(
                name: "surveys_pk_2",
                table: "Surveys",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answered");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Surveys");
        }
    }
}
