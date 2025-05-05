using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using QuizApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Shared
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts){}

        public DbSet<Submission> Submissions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Submission>(entity => 
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Choices)
                    .HasColumnType("SMALLINT[]") //postgres array type
                    .HasConversion(
                    v => v.Select(b => (ushort?) b).ToArray(),
                    v => v.Select(us => (byte?)us).ToArray()
                    );

                entity.Property(e => e.SubmissionPersonName).IsRequired();
                entity.Property(e => e.DateSubmitted).IsRequired();
            });
            base.OnModelCreating(builder);
        }
    }
}
