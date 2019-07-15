using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsHeadline.Models.Entities;

namespace NewsHeadline.Data
{
    /// <summary>
    /// Application database context.
    /// Defines connection between database and ORM (LINQ).
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor to database.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define table and model entity mappings for the ORM.
        // Tables will be created if not already existent.
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<UserSettingNewsCategory> UserSettingNewsCategories { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
    }
}
