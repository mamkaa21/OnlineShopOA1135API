using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace OnlineShopOA1135API;

public partial class OnlineShopOa1135Context : DbContext
{
    public OnlineShopOa1135Context()
    {
    }

    public OnlineShopOa1135Context(DbContextOptions<OnlineShopOa1135Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderGoodsCross> OrderGoodsCrosses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=95.154.107.102;database=OnlineShopOA1135;user=student;password=student", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CategoryId, "FK_Goods_Category_Id");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Amount).HasColumnType("int(11)");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("Category_Id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Image).HasColumnType("mediumblob");
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.Review).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Goods)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Goods_Category_Id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Order");

            entity.HasIndex(e => e.UserId, "FK_Order_User_Id");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CountProduct).HasColumnType("int(11)");
            entity.Property(e => e.PriceProduct).HasPrecision(10);
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Order_User_Id");
        });

        modelBuilder.Entity<OrderGoodsCross>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderGoodsCross");

            entity.HasIndex(e => e.GoodsId, "FK_OrderGoodsCross_Goods_Id");

            entity.HasIndex(e => e.OrderId, "FK_OrderGoodsCross_Order_Id");

            entity.Property(e => e.GoodsId)
                .HasColumnType("int(11)")
                .HasColumnName("Goods_Id");
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("Order_Id");

            entity.HasOne(d => d.Goods).WithMany()
                .HasForeignKey(d => d.GoodsId)
                .HasConstraintName("FK_OrderGoodsCross_Goods_Id");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderGoodsCross_Order_Id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "FK_User_Role_Id");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("Role_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
