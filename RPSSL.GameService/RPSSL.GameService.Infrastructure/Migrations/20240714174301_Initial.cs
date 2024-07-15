using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RPSSL.GameService.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class Initial : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					Role = table.Column<int>(type: "integer", maxLength: 24, nullable: false),
					UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
					NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
					NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					PasswordHash = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					SecurityStamp = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					PhoneNumber = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
					AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					CreatedBy = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastModifiedBy = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					Active = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Choice",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Name = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
					Active = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Choice", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ChoicesHistory",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					PlayerId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
					PlayerChoiceId = table.Column<int>(type: "integer", nullable: false),
					ComputerChoiceId = table.Column<int>(type: "integer", nullable: false),
					Active = table.Column<bool>(type: "boolean", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					CreatedBy = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastModifiedBy = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChoicesHistory", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Language",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
					LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
					CountryId = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
					Active = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Language", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					RoleId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					ClaimType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					ClaimValue = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					UserId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					ClaimType = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					ClaimValue = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					ProviderKey = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					ProviderDisplayName = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
					UserId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					RoleId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					LoginProvider = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					Name = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: false),
					Value = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ChoiceWin",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ChoiceId = table.Column<int>(type: "integer", nullable: false),
					BeatsChoiceId = table.Column<int>(type: "integer", nullable: false),
					ActionName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
					Active = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ChoiceWin", x => x.Id);
					table.ForeignKey(
						name: "FK_ChoiceWin_Choice_ChoiceId",
						column: x => x.ChoiceId,
						principalTable: "Choice",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "LocalizationLabel",
				columns: table => new
				{
					Key = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
					LanguageId = table.Column<int>(type: "integer", nullable: false),
					Value = table.Column<string>(type: "character varying(192)", maxLength: 192, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_LocalizationLabel", x => new { x.Key, x.LanguageId });
					table.ForeignKey(
						name: "FK_LocalizationLabel_Language_LanguageId",
						column: x => x.LanguageId,
						principalTable: "Language",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_Email",
				table: "AspNetUsers",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_UserName",
				table: "AspNetUsers",
				column: "UserName");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Choice_Name",
				table: "Choice",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ChoicesHistory_PlayerId",
				table: "ChoicesHistory",
				column: "PlayerId");

			migrationBuilder.CreateIndex(
				name: "IX_ChoiceWin_ChoiceId",
				table: "ChoiceWin",
				column: "ChoiceId");

			migrationBuilder.CreateIndex(
				name: "IX_LocalizationLabel_LanguageId",
				table: "LocalizationLabel",
				column: "LanguageId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "ChoicesHistory");

			migrationBuilder.DropTable(
				name: "ChoiceWin");

			migrationBuilder.DropTable(
				name: "LocalizationLabel");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Choice");

			migrationBuilder.DropTable(
				name: "Language");
		}
	}
}
