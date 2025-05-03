using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SurveysApi.DbModels;

namespace SurveysApi.Data;

public partial class MiDbContext : DbContext
{
    public MiDbContext(DbContextOptions<MiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answered> Answereds { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answered>(entity =>
        {
            entity.HasKey(e => e.id).HasName("answered_pk");

            entity.ToTable("Answered");

            entity.Property(e => e.id).UseIdentityAlwaysColumn();
            entity.Property(e => e.date_end).HasMaxLength(40);
            entity.Property(e => e.date_start).HasMaxLength(40);

            entity.HasOne(d => d.survey).WithMany(p => p.Answereds)
                .HasForeignKey(d => d.survey_id)
                .HasConstraintName("surveyanswered_surveys_id_fk");

            entity.HasOne(d => d.user).WithMany(p => p.Answereds)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("surveyanswered_users_id_fk");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.id).HasName("questions_pk");

            entity.Property(e => e.id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ask).HasMaxLength(250);
            entity.Property(e => e.type_ask).HasMaxLength(40);

            entity.HasOne(d => d.surveys).WithMany(p => p.Questions)
                .HasForeignKey(d => d.surveys_id)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("questions_ surveys_id_fk");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.id).HasName("surveys_pk");

            entity.HasIndex(e => e.name, "surveys_pk_2").IsUnique();

            entity.Property(e => e.id).UseIdentityAlwaysColumn();
            entity.Property(e => e.date_register).HasMaxLength(40);
            entity.Property(e => e.description).HasMaxLength(100);
            entity.Property(e => e.name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pk");

            entity.Property(e => e.id).UseIdentityAlwaysColumn();
            entity.Property(e => e.last_name).HasMaxLength(100);
            entity.Property(e => e.name).HasMaxLength(100);
            entity.Property(e => e.password).HasMaxLength(200);
            entity.Property(e => e.user_name).HasMaxLength(100);
            entity.Property(e => e.user_rol).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
