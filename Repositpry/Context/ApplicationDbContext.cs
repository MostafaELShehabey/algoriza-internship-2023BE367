using Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Repository.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //One - to - One relationship between ApplicationUser and Doctor
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.User_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-One relationship between ApplicationUser and Patient
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.User_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One relationship between Doctor and Specialization
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.Specialize_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One relationship between Patient and Discount
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Discount)
                .WithMany(d => d.Patients)
                .HasForeignKey(p => p.Discount_Id)
                .OnDelete(DeleteBehavior.Restrict);

            //Booking Table 

            // Many-to-One relationship between Booking and Appointment
            modelBuilder.Entity<Booking>()
                .HasOne(a => a.Appointment)
                .WithMany(d => d.Bookings)
                .HasForeignKey(a => a.Appointment_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One relationship between Booking and Doctor
            modelBuilder.Entity<Booking>()
                .HasOne(a => a.Doctors)
                .WithMany(d => d.Bookings)
                .HasForeignKey(a => a.Dcotor_Id)
                .OnDelete(DeleteBehavior.Restrict);

            //Many - to - One relationship between Booking and Patient
            modelBuilder.Entity<Booking>()
                .HasOne(a => a.Patient)
                .WithMany(b => b.Bookings)
                .HasForeignKey(a => a.Patient_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One relationship between Booking and Discount
            modelBuilder.Entity<Booking>()
                .HasOne(a => a.Discount)
                .WithMany(d => d.Bookings)
                .HasForeignKey(a => a.Discount_Id)
                .OnDelete(DeleteBehavior.Restrict);



            // Many-to-One relationship between Time and Appointment
            modelBuilder.Entity<Time>()
                .HasOne(t => t.Appointment)
                .WithMany(a => a.Times)
                .HasForeignKey(t => t.Appointment_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One relationship between Doctor and Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(t => t.Doctor)
                .WithMany(a => a.Appointments)
                .HasForeignKey(t => t.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many relationship between Doctor and DoctorAvailability
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Availabilities)
                .WithOne(da => da.Doctor)
                .HasForeignKey(da => da.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
    }
}
