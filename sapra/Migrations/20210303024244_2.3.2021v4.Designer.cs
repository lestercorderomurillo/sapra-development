﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using sapra.App;

namespace sapra.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210303024244_2.3.2021v4")]
    partial class _232021v4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("sapra.Models.MapLayer", b =>
                {
                    b.Property<int>("MapLayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("MapLayerId");

                    b.ToTable("MapLayer");
                });

            modelBuilder.Entity("sapra.Models.MapLayerField", b =>
                {
                    b.Property<int>("MapLayerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("MapLayerId", "Name");

                    b.ToTable("MapLayerField");
                });

            modelBuilder.Entity("sapra.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<int>("ParentRoleId")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("RoleRepository");
                });

            modelBuilder.Entity("sapra.Models.Role_Permission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("PermissionName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleId", "PermissionName");

                    b.ToTable("RolePermissionRepository");
                });

            modelBuilder.Entity("sapra.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("RecoveryHash")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("RecoveryTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRepository");
                });

            modelBuilder.Entity("sapra.Models.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("nvarchar(18)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)");

                    b.Property<string>("TypeIdentity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserInfoRepository");
                });

            modelBuilder.Entity("sapra.Models.UserPhone", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "Number");

                    b.ToTable("UserPhoneRepository");
                });

            modelBuilder.Entity("sapra.Models.MapLayerField", b =>
                {
                    b.HasOne("sapra.Models.MapLayer", "MapLayer")
                        .WithMany("MapLayerFields")
                        .HasForeignKey("MapLayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MapLayer");
                });

            modelBuilder.Entity("sapra.Models.Role_Permission", b =>
                {
                    b.HasOne("sapra.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("sapra.Models.User", b =>
                {
                    b.HasOne("sapra.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("sapra.Models.UserInfo", b =>
                {
                    b.HasOne("sapra.Models.User", "User")
                        .WithOne("UserInfo")
                        .HasForeignKey("sapra.Models.UserInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("sapra.Models.UserPhone", b =>
                {
                    b.HasOne("sapra.Models.UserInfo", "UserInfo")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("sapra.Models.MapLayer", b =>
                {
                    b.Navigation("MapLayerFields");
                });

            modelBuilder.Entity("sapra.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("sapra.Models.User", b =>
                {
                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("sapra.Models.UserInfo", b =>
                {
                    b.Navigation("PhoneNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
