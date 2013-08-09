using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EveLucrum.Domain;
using EveLucrum.Domain.Entities;

namespace EveLucrum.Data
{
    public class LucrumContextEF : DbContext, ILucrumContext
    {
        private class DbIncluder : QueryableExtensions.IIncluder
        {
            public IQueryable<T> Include<T, TProperty>(IQueryable<T> source, Expression<Func<T, TProperty>> path) where T : class
            {
                return DbExtensions.Include(source, path);
            }
        }

        static LucrumContextEF()
        {
            Database.SetInitializer<LucrumContextEF>(new TestDatabaseInitializer());
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            QueryableExtensions.Includer = new DbIncluder();
        }
        
        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public void Add<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Character> Characters { get; set; }

        IQueryable<Account> IRepository.Accounts { get { return Accounts.AsQueryable(); } }
        IQueryable<Character> IRepository.Characters { get { return Characters.AsQueryable(); } }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            const string schema = "lucrum";

            modelBuilder.Entity<Account>().ToTable("Account", schema);
            modelBuilder.Entity<Character>().ToTable("Character", schema);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class TestDatabaseInitializer : DropCreateDatabaseAlways<LucrumContextEF>
    {
        protected override void Seed(LucrumContextEF context)
        {
            var account = new Account()
            {
                KeyID = "144163",
                VerificationCode = "PQW0P8OAnOAIkhCQzE0XXTLAyRnUvLuSJtbTvugSvYqi39Yls3nG4Z8EYmJU0wBw"
            };
            context.Add(account);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
