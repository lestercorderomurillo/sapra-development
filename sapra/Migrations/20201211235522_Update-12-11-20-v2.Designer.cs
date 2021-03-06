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
    [Migration("20201211235522_Update-12-11-20-v2")]
    partial class Update121120v2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("sapra.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<int?>("ParentRoleRoleId")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.HasIndex("ParentRoleRoleId");

                    b.ToTable("RoleRepository");
                });

            modelBuilder.Entity("sapra.Models.Role_Permission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

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
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("LoginAttemptRecoveryTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoginAttempts")
                        .HasColumnType("int");

                    b.Property<string>("RecoveryHash")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("RecoveryTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

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

                    b.Property<int>("TypeIdentity")
                        .HasColumnType("int");

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

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("UserId", "Number");

                    b.ToTable("UserPhoneRepository");
                });

            modelBuilder.Entity("sapra.Models.Role", b =>
                {
                    b.HasOne("sapra.Models.Role", "ParentRole")
                        .WithMany()
                        .HasForeignKey("ParentRoleRoleId");

                    b.Navigation("ParentRole");
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
