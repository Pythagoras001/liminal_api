using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liminal_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("202611110001_AddLevelClassAndReportTables")]
    public class AddLevelClassAndReportTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE TABLE IF NOT EXISTS "SurvivalClasses" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_SurvivalClasses" PRIMARY KEY AUTOINCREMENT,
                    "Type" TEXT NOT NULL,
                    "ClassNumber" TEXT NULL,
                    "SecurityLevel" TEXT NOT NULL,
                    "Legitimacy" TEXT NOT NULL,
                    "DangerLevel" TEXT NOT NULL,
                    "IconImageId" TEXT NOT NULL,
                    "Description" TEXT NOT NULL,
                    CONSTRAINT "FK_SurvivalClasses_Images_IconImageId"
                        FOREIGN KEY ("IconImageId") REFERENCES "Images" ("Id") ON DELETE RESTRICT
                );
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_SurvivalClasses_IconImageId"
                    ON "SurvivalClasses" ("IconImageId");

                CREATE TABLE IF NOT EXISTS "Evidences" (
                    "Id" TEXT NOT NULL CONSTRAINT "PK_Evidences" PRIMARY KEY,
                    "Description" TEXT NOT NULL,
                    "ImageId" TEXT NOT NULL,
                    "GaleryReportId" INTEGER NULL,
                    CONSTRAINT "FK_Evidences_Images_ImageId"
                        FOREIGN KEY ("ImageId") REFERENCES "Images" ("Id") ON DELETE RESTRICT,
                    CONSTRAINT "FK_Evidences_Reports_GaleryReportId"
                        FOREIGN KEY ("GaleryReportId") REFERENCES "Reports" ("Id") ON DELETE CASCADE
                );
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_Evidences_ImageId"
                    ON "Evidences" ("ImageId");
                CREATE INDEX IF NOT EXISTS "IX_Evidences_GaleryReportId"
                    ON "Evidences" ("GaleryReportId");

                CREATE TABLE IF NOT EXISTS "Reports" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Reports" PRIMARY KEY AUTOINCREMENT,
                    "AuthorId" INTEGER NOT NULL,
                    "Title" TEXT NOT NULL,
                    "Nivel" TEXT NOT NULL,
                    "LevelClassId" INTEGER NOT NULL,
                    "Description" TEXT NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "PrincipalEvidenceId" TEXT NOT NULL,
                    CONSTRAINT "FK_Reports_Users_AuthorId"
                        FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_Reports_SurvivalClasses_LevelClassId"
                        FOREIGN KEY ("LevelClassId") REFERENCES "SurvivalClasses" ("Id") ON DELETE RESTRICT,
                    CONSTRAINT "FK_Reports_Evidences_PrincipalEvidenceId"
                        FOREIGN KEY ("PrincipalEvidenceId") REFERENCES "Evidences" ("Id") ON DELETE RESTRICT
                );
                CREATE INDEX IF NOT EXISTS "IX_Reports_AuthorId" ON "Reports" ("AuthorId");
                CREATE INDEX IF NOT EXISTS "IX_Reports_LevelClassId" ON "Reports" ("LevelClassId");
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_Reports_PrincipalEvidenceId"
                    ON "Reports" ("PrincipalEvidenceId");

                CREATE TABLE IF NOT EXISTS "ReportLikes" (
                    "UserId" INTEGER NOT NULL,
                    "ReportId" INTEGER NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    CONSTRAINT "PK_ReportLikes" PRIMARY KEY ("UserId", "ReportId"),
                    CONSTRAINT "FK_ReportLikes_Users_UserId"
                        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_ReportLikes_Reports_ReportId"
                        FOREIGN KEY ("ReportId") REFERENCES "Reports" ("Id") ON DELETE CASCADE
                );
                CREATE INDEX IF NOT EXISTS "IX_ReportLikes_ReportId" ON "ReportLikes" ("ReportId");
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM "Reports";
                DELETE FROM "Evidences";
                DELETE FROM "SurvivalClasses";
                DROP TABLE IF EXISTS "ReportLikes";
                DROP TABLE IF EXISTS "Reports";
                DROP TABLE IF EXISTS "Evidences";
                DROP TABLE IF EXISTS "SurvivalClasses";
                """);
        }
    }
}
