using Microsoft.EntityFrameworkCore.Migrations;

namespace Salvo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                //Se crea una plantilla para poder crear la tabla
                name: "Players", //Tablla Players
                columns: table => new //Tiene las siguientes columnas
                {
                    //El atributo o campo Id es del tipo long que para efectos sql es del tipo bigint,
                    //no permite nulos y que para sqlserver es un identity o sea un primary key
                    //es autoincrementable, parte del 1 y se incrementa de 1 en 1.
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    //Se crea un indice que se llama PK_Players y va a estar relacionado al Id
                    table.PrimaryKey("PK_Players", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
