﻿using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateTime(1981, 3, 15) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateTime(1982, 3, 15) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateTime(1983, 3, 15) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateTime(1984, 3, 15)},
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateTime(1985, 3, 15)},
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateTime(1986, 3, 15)},
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateTime(1987, 3, 15)},
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new DateTime(1988, 3, 15)},
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new DateTime(1989, 3, 15)},
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new DateTime(1990, 3, 15)},
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new DateTime(1991, 3, 15)},
        });

    public DbSet<User>? Users { get; set; }

    public async Task<IQueryable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        => await Task.FromResult(base.Set<TEntity>());

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }

}
