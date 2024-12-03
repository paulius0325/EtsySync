﻿// <auto-generated />
using System;
using EtsySync.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EtsySync.Migrations
{
    [DbContext(typeof(InvoiceDbContext))]
    partial class InvoiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SharedProject.Models.Client", b =>
                {
                    b.Property<Guid>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Buyer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("SharedProject.Models.CompressedFile", b =>
                {
                    b.Property<Guid>("ZipFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ZipFileId");

                    b.ToTable("ZipFiles");
                });

            modelBuilder.Entity("SharedProject.Models.EncryptionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EncryptionKeys");
                });

            modelBuilder.Entity("SharedProject.Models.InvoiceItem", b =>
                {
                    b.Property<Guid>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemNumber")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("InvoiceId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("SharedProject.Models.SalesItem", b =>
                {
                    b.Property<Guid>("SalesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("InvoiceItemInvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("SerialNumber")
                        .HasColumnType("bigint");

                    b.HasKey("SalesId");

                    b.HasIndex("ClientId");

                    b.HasIndex("InvoiceItemInvoiceId");

                    b.ToTable("SalesItems");
                });

            modelBuilder.Entity("SharedProject.Models.SalesItem", b =>
                {
                    b.HasOne("SharedProject.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("SharedProject.Models.InvoiceItem", "InvoiceItem")
                        .WithMany()
                        .HasForeignKey("InvoiceItemInvoiceId");

                    b.Navigation("Client");

                    b.Navigation("InvoiceItem");
                });
#pragma warning restore 612, 618
        }
    }
}
