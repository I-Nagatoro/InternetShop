using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternetShop.Models;

public partial class AfanasyevContext : DbContext
{
    public AfanasyevContext()
    {
    }

    public AfanasyevContext(DbContextOptions<AfanasyevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<BasketProduct> BasketProducts { get; set; }

    public virtual DbSet<Comm> Comms { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=79.174.88.58;Port=16639;Username=Afanasyev;Password=Afanasyev123.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("en_US.UTF-8");

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.BasketId).HasName("basket_pk");

            entity.ToTable("basket");

            entity.Property(e => e.BasketId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("basket_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<BasketProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("basket_products_pk");

            entity.ToTable("basket_products");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BasketId).HasColumnName("basket_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
        });

        modelBuilder.Entity<Comm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comm_pk");

            entity.ToTable("comm");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Comm1)
                .HasColumnType("character varying")
                .HasColumnName("comm");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pk");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.SumCost).HasColumnName("sum_cost");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_products_pk");

            entity.ToTable("order_products");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductsId).HasColumnName("products_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pk");

            entity.ToTable("products");

            entity.Property(e => e.ProductId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("product_id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.ProductCaption)
                .HasColumnType("character varying")
                .HasColumnName("product_caption");
            entity.Property(e => e.ProductImage)
                .HasColumnType("character varying")
                .HasColumnName("product_image");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pk");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Role1)
                .HasColumnType("character varying")
                .HasColumnName("role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pk");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
