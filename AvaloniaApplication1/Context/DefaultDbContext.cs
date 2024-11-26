using System;
using System.Collections.Generic;
using AvaloniaApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication1.Context;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext()
    {
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<FileList> FileLists { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<ListTag> ListTags { get; set; }

    public virtual DbSet<Posehenium> Posehenia { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<VisitTabl> VisitTabls { get; set; }

    public virtual DbSet<Клиенты> Клиентыs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=91.186.197.80:5432;Database=default_db;Username=gen_user;password=6?,65\\fIp7(lqq");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("file_pk");

            entity.ToTable("file");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdKlient).HasColumnName("id_klient");
            entity.Property(e => e.IdSpisok).HasColumnName("id_spisok");

            entity.HasOne(d => d.IdKlientNavigation).WithMany(p => p.FilesNavigation)
                .HasForeignKey(d => d.IdKlient)
                .HasConstraintName("file_клиенты_fk");

            entity.HasOne(d => d.IdSpisokNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.IdSpisok)
                .HasConstraintName("file_file_list_fk");
        });

        modelBuilder.Entity<FileList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("file_list_pk");

            entity.ToTable("file_list");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.File).HasColumnName("file");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gender_pk");

            entity.ToTable("gender");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<ListTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_tag_pk");

            entity.ToTable("list_tag");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ListTags)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("list_tag_клиенты_fk");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.ListTags)
                .HasForeignKey(d => d.IdTag)
                .HasConstraintName("list_tag_tags_fk");
        });

        modelBuilder.Entity<Posehenium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("posehenia_pk");

            entity.ToTable("posehenia");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IdKlient).HasColumnName("id_klient");
            entity.Property(e => e.IdPosh).HasColumnName("id_posh");

            entity.HasOne(d => d.IdKlientNavigation).WithMany(p => p.Posehenia)
                .HasForeignKey(d => d.IdKlient)
                .HasConstraintName("posehenia_клиенты_fk");

            entity.HasOne(d => d.IdPoshNavigation).WithMany(p => p.Posehenia)
                .HasForeignKey(d => d.IdPosh)
                .HasConstraintName("posehenia_visit_tabl_fk");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pk");

            entity.ToTable("tags");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Color).HasColumnName("color");
        });

        modelBuilder.Entity<VisitTabl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("visit_tabl_pk");

            entity.ToTable("visit_tabl");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Клиенты>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("клиенты_pk");

            entity.ToTable("клиенты");

            entity.HasIndex(e => e.Visit, "клиенты_unique").IsUnique();

            entity.HasIndex(e => e.Tagpers, "клиенты_unique_1").IsUnique();

            entity.HasIndex(e => e.Files, "клиенты_unique_2").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.ColvoVisit).HasColumnName("colvo_visit");
            entity.Property(e => e.DateRegistr).HasColumnName("date_registr");
            entity.Property(e => e.EmailAdress)
                .HasColumnType("character varying")
                .HasColumnName("email_adress");
            entity.Property(e => e.Files).HasColumnName("files");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.MiddleName)
                .HasColumnType("character varying")
                .HasColumnName("middle_name");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.NumberPhone)
                .HasColumnType("character varying")
                .HasColumnName("number_phone");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
            entity.Property(e => e.Tagpers).HasColumnName("tagpers");
            entity.Property(e => e.Visit).HasColumnName("visit");

            entity.HasOne(d => d.Files1).WithOne(p => p.Клиенты)
                .HasForeignKey<Клиенты>(d => d.Files)
                .HasConstraintName("клиенты_file_fk");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Клиентыs)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("клиенты_gender_fk");

            entity.HasOne(d => d.TagpersNavigation).WithOne(p => p.Клиенты)
                .HasForeignKey<Клиенты>(d => d.Tagpers)
                .HasConstraintName("клиенты_list_tag_fk");

            entity.HasOne(d => d.VisitNavigation).WithOne(p => p.Клиенты)
                .HasForeignKey<Клиенты>(d => d.Visit)
                .HasConstraintName("клиенты_posehenia_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
