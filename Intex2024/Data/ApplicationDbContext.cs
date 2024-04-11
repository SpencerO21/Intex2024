﻿using Intex2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Intex2024.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,  
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,  
    IdentityRoleClaim<string>, IdentityUserToken<string>>  
{  
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }  
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }  
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }  
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {  
        base.OnModelCreating(modelBuilder);  
        modelBuilder.Entity<ApplicationUser>(b =>  
        {     
            // Each User can have many entries in the UserRole join table  
            b.HasMany(e => e.UserRoles)  
                .WithOne(e => e.User)  
                .HasForeignKey(ur => ur.UserId)  
                .IsRequired();  
        });  

        modelBuilder.Entity<ApplicationRole>(b =>  
        {  
            // Each Role can have many entries in the UserRole join table  
            b.HasMany(e => e.UserRoles)  
                .WithOne(e => e.Role)  
                .HasForeignKey(ur => ur.RoleId)  
                .IsRequired();  
        });  
    }  
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  
        : base(options)  
    {  
    }  
}