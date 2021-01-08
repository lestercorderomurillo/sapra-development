﻿using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using sapra.Models;

namespace sapra.App
{
    
	public class DatabaseContext : DbContext
	{
		public DbSet<User> UserRepository { get; set; }

		public DbSet<UserInfo> UserInfoRepository { get; set; }

        public DbSet<UserPhone> UserPhoneRepository { get; set; }

        public DbSet<Role> RoleRepository { get; set; }

        public DbSet<Role_Permission> RolePermissionRepository { get; set; }

        public SqlConnection Connection = new SqlConnection(Startup.RootAccess.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Keys
            modelBuilder.Entity<User>().HasKey(e => e.UserId);
            modelBuilder.Entity<UserInfo>().HasKey(e => e.UserId);

            // Composed Keys
            modelBuilder.Entity<UserPhone>().HasKey(e => new { e.UserId, e.Number });
            modelBuilder.Entity<Role>().HasKey(e => e.RoleId );
            modelBuilder.Entity<Role_Permission>().HasKey(e => new { e.RoleId, e.PermissionName });

            // One to One Relationships 
            modelBuilder.Entity<User>()
                .HasOne(e => e.UserInfo)
                .WithOne(e => e.User)
                .HasForeignKey<UserInfo>(e => e.UserId);

            // One to Many Relationships 
            modelBuilder.Entity<User>()
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.PhoneNumbers)
                .WithOne(e => e.UserInfo);

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DatabaseContext(){ 
            OnConfiguring(new DbContextOptionsBuilder());
            Connection.Open();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(Connection).EnableSensitiveDataLogging().EnableDetailedErrors();
		}

        public void DetachAllEntities()
	    {
		    var changedEntriesCopy = ChangeTracker.Entries();

		    foreach (var entry in changedEntriesCopy){ 
			    entry.State = EntityState.Detached;
		    }
	    }

    }
    public class DatabaseInitializer
	{
		public void Initialize(DatabaseContext db)
		{


            var testUsers = new User[]{ 
                new User(),
                new User()
            };
			
			db.Database.EnsureCreated();

			foreach (User user in testUsers)
			{
				db.UserRepository.Add(user);
				db.SaveChanges();
				db.DetachAllEntities();
			}

		}

        

	}

}