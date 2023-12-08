﻿// <auto-generated />
using System;
using Beyond.SearchEngine.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Beyond.SearchEngine.Migrations
{
    [DbContext(typeof(BeyondContext))]
    partial class BeyondContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Author", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(12)");

                    b.Property<int>("CitationCount")
                        .HasColumnType("int");

                    b.Property<string>("Concepts")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("HIndex")
                        .HasColumnType("int");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(63)");

                    b.Property<string>("OrcId")
                        .IsRequired()
                        .HasColumnType("char(20)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WorksCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Institution", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(12)");

                    b.Property<string>("AssociatedInstitutions")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CitationCount")
                        .HasColumnType("int");

                    b.Property<string>("Concepts")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("char(8)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HomepageUrl")
                        .IsRequired()
                        .HasColumnType("varchar(63)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(127)");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WorksCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Institutions");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Work", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(16)");

                    b.Property<string>("Abstract")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CitationCount")
                        .HasColumnType("int");

                    b.Property<string>("Concepts")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Doi")
                        .IsRequired()
                        .HasColumnType("varchar(63)");

                    b.Property<string>("Keywords")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("char(8)");

                    b.Property<string>("PdfUrl")
                        .IsRequired()
                        .HasColumnType("varchar(127)");

                    b.Property<string>("PublicationDate")
                        .IsRequired()
                        .HasColumnType("char(12)");

                    b.Property<int>("PublicationYear")
                        .HasColumnType("int");

                    b.Property<string>("ReferencedWorks")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RelatedWorks")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Update.Models.UpdateHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<DateOnly>("UpdatedTime")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("UpdateHistories");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Update.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("char(63)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("char(63)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Beyond.Shared.Data.AssociatedInstitutionData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("InstitutionId")
                        .HasColumnType("char(12)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Relation")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.ToTable("AssociatedInstitutionData");
                });

            modelBuilder.Entity("Beyond.Shared.Data.AuthorData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OrcId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("WorkId")
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("WorkId");

                    b.ToTable("AuthorData");
                });

            modelBuilder.Entity("Beyond.Shared.Data.ConceptData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AuthorId")
                        .HasColumnType("char(12)");

                    b.Property<string>("InstitutionId")
                        .HasColumnType("char(12)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Score")
                        .HasColumnType("double");

                    b.Property<string>("WorkId")
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("WorkId");

                    b.ToTable("ConceptData");
                });

            modelBuilder.Entity("Beyond.Shared.Data.AssociatedInstitutionData", b =>
                {
                    b.HasOne("Beyond.SearchEngine.Modules.Search.Models.Institution", null)
                        .WithMany("AssociatedInstitutionList")
                        .HasForeignKey("InstitutionId");
                });

            modelBuilder.Entity("Beyond.Shared.Data.AuthorData", b =>
                {
                    b.HasOne("Beyond.SearchEngine.Modules.Search.Models.Work", null)
                        .WithMany("AuthorList")
                        .HasForeignKey("WorkId");
                });

            modelBuilder.Entity("Beyond.Shared.Data.ConceptData", b =>
                {
                    b.HasOne("Beyond.SearchEngine.Modules.Search.Models.Author", null)
                        .WithMany("ConceptList")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Beyond.SearchEngine.Modules.Search.Models.Institution", null)
                        .WithMany("ConceptList")
                        .HasForeignKey("InstitutionId");

                    b.HasOne("Beyond.SearchEngine.Modules.Search.Models.Work", null)
                        .WithMany("ConceptList")
                        .HasForeignKey("WorkId");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Author", b =>
                {
                    b.Navigation("ConceptList");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Institution", b =>
                {
                    b.Navigation("AssociatedInstitutionList");

                    b.Navigation("ConceptList");
                });

            modelBuilder.Entity("Beyond.SearchEngine.Modules.Search.Models.Work", b =>
                {
                    b.Navigation("AuthorList");

                    b.Navigation("ConceptList");
                });
#pragma warning restore 612, 618
        }
    }
}