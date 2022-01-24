using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cashier.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_supplier",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_supplier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_user",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_goods",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    priceSell = table.Column<int>(type: "int", nullable: false),
                    priceBuy = table.Column<int>(type: "int", nullable: false),
                    stok = table.Column<int>(type: "int", nullable: false),
                    Supplierid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Categoryid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_goods", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_t_goods_tb_m_category_Categoryid",
                        column: x => x.Categoryid,
                        principalTable: "tb_m_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_t_goods_tb_m_supplier_Supplierid",
                        column: x => x.Supplierid,
                        principalTable: "tb_m_supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_account",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roleid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_account", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_m_account_tb_m_role_Roleid",
                        column: x => x.Roleid,
                        principalTable: "tb_m_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_m_account_tb_m_user_id",
                        column: x => x.id,
                        principalTable: "tb_m_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_request_goods",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date_trs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Supplierid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_request_goods", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_t_request_goods_tb_m_supplier_Supplierid",
                        column: x => x.Supplierid,
                        principalTable: "tb_m_supplier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_t_request_goods_tb_m_user_Userid",
                        column: x => x.Userid,
                        principalTable: "tb_m_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_transaction",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date_trs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total = table.Column<int>(type: "int", nullable: false),
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_t_transaction_tb_m_user_Userid",
                        column: x => x.Userid,
                        principalTable: "tb_m_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_detail_request_goods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestGoodsid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Goodsid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_detail_request_goods", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_t_detail_request_goods_tb_t_goods_Goodsid",
                        column: x => x.Goodsid,
                        principalTable: "tb_t_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_t_detail_request_goods_tb_t_request_goods_RequestGoodsid",
                        column: x => x.RequestGoodsid,
                        principalTable: "tb_t_request_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_detail_transaction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transactionid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Goodsid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_detail_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_tb_t_detail_transaction_tb_t_goods_Goodsid",
                        column: x => x.Goodsid,
                        principalTable: "tb_t_goods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_t_detail_transaction_tb_t_transaction_Transactionid",
                        column: x => x.Transactionid,
                        principalTable: "tb_t_transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_account_Roleid",
                table: "tb_m_account",
                column: "Roleid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_detail_request_goods_Goodsid",
                table: "tb_t_detail_request_goods",
                column: "Goodsid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_detail_request_goods_RequestGoodsid",
                table: "tb_t_detail_request_goods",
                column: "RequestGoodsid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_detail_transaction_Goodsid",
                table: "tb_t_detail_transaction",
                column: "Goodsid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_detail_transaction_Transactionid",
                table: "tb_t_detail_transaction",
                column: "Transactionid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_goods_Categoryid",
                table: "tb_t_goods",
                column: "Categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_goods_Supplierid",
                table: "tb_t_goods",
                column: "Supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_request_goods_Supplierid",
                table: "tb_t_request_goods",
                column: "Supplierid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_request_goods_Userid",
                table: "tb_t_request_goods",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_transaction_Userid",
                table: "tb_t_transaction",
                column: "Userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_account");

            migrationBuilder.DropTable(
                name: "tb_t_detail_request_goods");

            migrationBuilder.DropTable(
                name: "tb_t_detail_transaction");

            migrationBuilder.DropTable(
                name: "tb_m_role");

            migrationBuilder.DropTable(
                name: "tb_t_request_goods");

            migrationBuilder.DropTable(
                name: "tb_t_goods");

            migrationBuilder.DropTable(
                name: "tb_t_transaction");

            migrationBuilder.DropTable(
                name: "tb_m_category");

            migrationBuilder.DropTable(
                name: "tb_m_supplier");

            migrationBuilder.DropTable(
                name: "tb_m_user");
        }
    }
}
