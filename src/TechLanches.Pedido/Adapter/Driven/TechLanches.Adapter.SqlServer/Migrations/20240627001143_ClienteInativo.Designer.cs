﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechLanches.Adapter.SqlServer;

#nullable disable

namespace TechLanches.Adapter.SqlServer.Migrations
{
    [DbContext(typeof(TechLanchesDbContext))]
    [Migration("20240627001143_ClienteInativo")]
    partial class ClienteInativo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ClienteInativo")
                        .HasColumnType("bit");

                    b.Property<string>("StatusPedido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("StatusPedido");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Valor");

                    b.HasKey("Id");

                    b.ToTable("Pedidos", (string)null);
                });

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Deletado")
                        .HasColumnType("bit");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Nome");

                    b.ToTable("Produtos", (string)null);
                });

            modelBuilder.Entity("TechLanches.Domain.ValueObjects.ItemPedido", b =>
                {
                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecoProduto")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("PrecoProduto");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int")
                        .HasColumnName("Quantidade");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Valor");

                    b.HasKey("ProdutoId", "PedidoId");

                    b.HasIndex("PedidoId");

                    b.HasIndex("ProdutoId", "PedidoId")
                        .IsUnique();

                    b.ToTable("ItemPedido", (string)null);
                });

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Pedido", b =>
                {
                    b.OwnsOne("TechLanches.Domain.ValueObjects.Cpf", "Cpf", b1 =>
                        {
                            b1.Property<int>("PedidoId")
                                .HasColumnType("int");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("nvarchar(11)")
                                .HasColumnName("Cpf");

                            b1.HasKey("PedidoId");

                            b1.HasIndex("PedidoId", "Numero")
                                .IsUnique();

                            b1.ToTable("Pedidos");

                            b1.WithOwner()
                                .HasForeignKey("PedidoId");
                        });

                    b.Navigation("Cpf")
                        .IsRequired();
                });

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Produto", b =>
                {
                    b.OwnsOne("TechLanches.Domain.ValueObjects.CategoriaProduto", "Categoria", b1 =>
                        {
                            b1.Property<int>("ProdutoId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("CategoriaId");

                            b1.HasKey("ProdutoId");

                            b1.HasIndex("Id");

                            b1.ToTable("Produtos");

                            b1.WithOwner()
                                .HasForeignKey("ProdutoId");
                        });

                    b.Navigation("Categoria")
                        .IsRequired();
                });

            modelBuilder.Entity("TechLanches.Domain.ValueObjects.ItemPedido", b =>
                {
                    b.HasOne("TechLanches.Domain.Aggregates.Pedido", "Pedido")
                        .WithMany("ItensPedido")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechLanches.Domain.Aggregates.Produto", "Produto")
                        .WithMany("ItensPedidos")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Pedido", b =>
                {
                    b.Navigation("ItensPedido");
                });

            modelBuilder.Entity("TechLanches.Domain.Aggregates.Produto", b =>
                {
                    b.Navigation("ItensPedidos");
                });
#pragma warning restore 612, 618
        }
    }
}