using Microsoft.EntityFrameworkCore;
using _netCoreBackend.Models.Enums;
using Npgsql;

namespace _netCoreBackend.Models
{
    public class ManagerContext : DbContext
    {
        public ManagerContext() {}

        public ManagerContext(DbContextOptions options):base(options)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Role>();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Status>();
            // NpgsqlConnection.GlobalTypeMapper
        }

        public DbSet<User> Users {get;set;}
        public DbSet<PrivateTask> PrivateTasks{get;set;}
        public DbSet<SharedTasks> SharedTasks {get;set;}
        public DbSet<Group> Groups {get;set;}
        public DbSet<UserGroup> Memberships {get;set;}
        public DbSet<Credentials> Credentials {get;set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //specifying child entity Admin:
            modelBuilder.Entity<Admin>();

            //converting abstract class to table:
            modelBuilder.Entity<Task>().ToTable("Tasks");
            
            //unique constraint on user's email:
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            //enums:
            modelBuilder.HasPostgresEnum<Role>();
            modelBuilder.HasPostgresEnum<Status>();

            //private task
            modelBuilder.Entity<PrivateTask>()
                        .HasOne( pt => pt.OwnerAccount )
                        .WithMany( u => u.PrivateTasks )
                        .HasForeignKey(pt => pt.OwnerId);
            //shared task
            modelBuilder.Entity<SharedTasks>()
                .HasOne(st => st.Group)
                .WithMany(g => g.SharedTasks)
                .HasForeignKey(st => st.GroupId);

            //group
            modelBuilder.Entity<Group>()
                        .HasOne(g => g.AdminAccount)
                        .WithMany(u => u.Groups )
                        .HasForeignKey(g => g.AdminUsername);

            modelBuilder.Entity<Credentials>()
                        .HasOne(c=> c.User)
                        .WithMany(u => u.Credentials)
                        .HasForeignKey(c=> c.User_Id);


            //Membership/UserGroup many to many relationship:            
                //Composite key:
            modelBuilder.Entity<UserGroup>()
                        .HasKey( ug => new { ug.GroupID , ug.MemberUsername });
            
                //2 one to many relationships:
                    //from usergroup to user:
            modelBuilder.Entity<UserGroup>()
                        .HasOne( ug => ug.UserAccount )
                        .WithMany( u => u.Memberships )
                        .HasForeignKey( ug => ug.MemberUsername );

                    //from usergroup to group:
            modelBuilder.Entity<UserGroup>()
                        .HasOne(ug => ug.Group )
                        .WithMany(g=> g.Memberships)
                        .HasForeignKey(ug => ug.GroupID);
            
            
            //setting default to user custom color propety
            modelBuilder.Entity<User>()
                        .Property( u => u.CustomBackgroundColor )
                        .HasDefaultValue("#e1e1e1");
        }

    }
}