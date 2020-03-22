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
        public DbSet<SharedTaskAssignment> SharedTaskAssignments {get;set;}
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
                        .HasOne( pt => pt.User )
                        .WithMany( u => u.PrivateTasks )
                        .HasForeignKey(pt => pt.OwnerId);
            //shared task
            modelBuilder.Entity<SharedTasks>()
                        .HasOne(st => st.Admin)
                        .WithMany( u => u.SharedTasks )
                        .HasForeignKey( st => st.AdminId );

                
            //group
            modelBuilder.Entity<Group>()
                        .HasOne(g => g.Admin)
                        .WithMany(u => u.Groups )
                        .HasForeignKey(g => g.AdminId);

            modelBuilder.Entity<Credentials>()
                        .HasOne(c=> c.User)
                        .WithMany(u => u.Credentials)
                        .HasForeignKey(c=> c.User_Id);


            //Membership/UserGroup many to many relationship:            
                //Composite key:
            modelBuilder.Entity<UserGroup>()
                        .HasKey( ug => new { ug.Group_ID , ug.Member_ID });
            
                //2 one to many relationships:
                    //from usergroup to user:
            modelBuilder.Entity<UserGroup>()
                        .HasOne( ug => ug.User )
                        .WithMany( u => u.Memberships )
                        .HasForeignKey( ug => ug.Member_ID );

                    //from usergroup to group:
            modelBuilder.Entity<UserGroup>()
                        .HasOne(ug => ug.Group )
                        .WithMany(g=> g.Memberships)
                        .HasForeignKey(ug => ug.Group_ID);
            
            //SharedTaskAssignment many to many relationship:
                //composite key:
            modelBuilder.Entity<SharedTaskAssignment>()
                        .HasKey( shared => new { shared.SharedTaskId , shared.MembershipGroupId , shared.MemberShipUserId });
            
                //2 one to many relationships
                    //from shared to usergroup
            modelBuilder.Entity<SharedTaskAssignment>()
                        .HasOne( shared => shared.Membership )
                        .WithMany( ug => ug.SharedTaskAssignments )
                        .HasForeignKey( shared => new { shared.MembershipGroupId , shared.MemberShipUserId });

                    //from shared to sharedTask
            modelBuilder.Entity<SharedTaskAssignment>()
                        .HasOne(shared => shared.SharedTask)
                        .WithMany( task => task.SharedTaskAssignments )
                        .HasForeignKey( shared => shared.SharedTaskId );

            
            //setting default to user custom color propety:
            modelBuilder.Entity<User>()
                        .Property( u => u.CustomBackgroundColor )
                        .HasDefaultValue("#e1e1e1");

           
                        
            

        }

    }
}