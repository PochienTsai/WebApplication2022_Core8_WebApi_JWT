using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserLogin
{
    public partial class MVC_UserLoginContext : DbContext
    {
        public MVC_UserLoginContext()
        {
        }

        public MVC_UserLoginContext(DbContextOptions<MVC_UserLoginContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbUser> DbUsers { get; set; } = null!;
        public virtual DbSet<DbUserRight> DbUserRights { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=MVC_UserLogin;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbUser>(entity =>
            {
                entity.ToTable("db_user");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserApproved)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength();

                entity.Property(e => e.UserName).HasMaxLength(20);

                entity.Property(e => e.UserPassword).HasMaxLength(50);

                entity.Property(e => e.UserRank).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<DbUserRight>(entity =>
            {
                entity.ToTable("db_UserRight");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeleteRight)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'N')")
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("EMail");

                entity.Property(e => e.EmailGuid)
                    .HasColumnName("EMail_GUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.EmailMd5id)
                    .HasMaxLength(50)
                    .HasColumnName("EMail_MD5ID");

                entity.Property(e => e.EnableTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Enable_Time");

                entity.Property(e => e.OldUserPassword).HasMaxLength(100);

                entity.Property(e => e.UpdateRight)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'N')")
                    .IsFixedLength();

                entity.Property(e => e.UserApproved)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'N')")
                    .IsFixedLength();

                entity.Property(e => e.UserName).HasMaxLength(20);

                entity.Property(e => e.UserPassword).HasMaxLength(50);

                entity.Property(e => e.UserRank).HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
