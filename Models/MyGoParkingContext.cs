using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyGoParking.Models;

public partial class MyGoParkingContext : DbContext
{
    public MyGoParkingContext()
    {
    }

    public MyGoParkingContext(DbContextOptions<MyGoParkingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    public virtual DbSet<Car> Car { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<EntryExitManagement> EntryExitManagement { get; set; }

    public virtual DbSet<Ewallet> Ewallet { get; set; }

    public virtual DbSet<MonApplyList> MonApplyList { get; set; }

    public virtual DbSet<MonthlyRental> MonthlyRental { get; set; }

    public virtual DbSet<ParkingLot> ParkingLot { get; set; }

    public virtual DbSet<ParkingSlot> ParkingSlot { get; set; }

    public virtual DbSet<Reservation> Reservation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRoleClaims>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetRoles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserClaims>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogins>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserTokens>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRoles>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Car__4C9A0DB36100E731");

            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.LicensePlate).HasColumnName("license_plate");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Car)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Car__user_id__6D0D32F4");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Customer__B9BE370F58BED3BE");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BlackCount).HasColumnName("black_count");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsBlack).HasColumnName("is_black");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        modelBuilder.Entity<EntryExitManagement>(entity =>
        {
            entity.HasKey(e => e.EntryexitId).HasName("PK__EntryExi__FD3EA5F64D4216F7");

            entity.Property(e => e.EntryexitId).HasColumnName("entryexit_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.EntryTime)
                .HasColumnType("datetime")
                .HasColumnName("entry_time");
            entity.Property(e => e.ExitTime)
                .HasColumnType("datetime")
                .HasColumnName("exit_time");
            entity.Property(e => e.IsFinish).HasColumnName("is_finish");
            entity.Property(e => e.LicensePlateKeyinTime)
                .HasColumnType("datetime")
                .HasColumnName("license_plate_keyin_time");
            entity.Property(e => e.LicensePlatePhoto).HasColumnName("license_plate_photo");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.Parktype).HasColumnName("parktype");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");
            entity.Property(e => e.ValidTime)
                .HasColumnType("datetime")
                .HasColumnName("valid_time");

            entity.HasOne(d => d.Car).WithMany(p => p.EntryExitManagement)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EntryExit__car_i__6E01572D");

            entity.HasOne(d => d.Lot).WithMany(p => p.EntryExitManagement)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EntryExit__lot_i__6EF57B66");
        });

        modelBuilder.Entity<Ewallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Ewallet__0EE6F041D971A2E6");

            entity.Property(e => e.WalletId).HasColumnName("wallet_id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.UpdatedTime)
                .HasColumnType("datetime")
                .HasColumnName("updated_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Ewallet)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ewallet__user_id__6FE99F9F");
        });

        modelBuilder.Entity<MonApplyList>(entity =>
        {
            entity.HasKey(e => e.ApplyId).HasName("PK__MonApply__8260CA8239CAB2AE");

            entity.Property(e => e.ApplyId).HasColumnName("apply_id");
            entity.Property(e => e.ApplyDate)
                .HasColumnType("datetime")
                .HasColumnName("apply_date");
            entity.Property(e => e.ApplyStatus).HasColumnName("apply_status");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.LotId).HasColumnName("lot_id");

            entity.HasOne(d => d.Car).WithMany(p => p.MonApplyList)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MonApplyL__car_i__70DDC3D8");

            entity.HasOne(d => d.Lot).WithMany(p => p.MonApplyList)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MonApplyL__lot_i__71D1E811");
        });

        modelBuilder.Entity<MonthlyRental>(entity =>
        {
            entity.HasKey(e => e.MonId).HasName("PK__MonthlyR__9C5E114DFB9D3CF2");

            entity.Property(e => e.MonId).HasColumnName("mon_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.NotificationStatus).HasColumnName("notification_status");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");
            entity.Property(e => e.RenewalStatus).HasColumnName("renewal_status");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Car).WithMany(p => p.MonthlyRental)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MonthlyRe__car_i__72C60C4A");

            entity.HasOne(d => d.Lot).WithMany(p => p.MonthlyRental)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MonthlyRe__lot_i__73BA3083");

            entity.HasOne(d => d.Slot).WithMany(p => p.MonthlyRental)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__MonthlyRe__slot___74AE54BC");
        });

        modelBuilder.Entity<ParkingLot>(entity =>
        {
            entity.HasKey(e => e.LotId).HasName("PK__ParkingL__38CAA92BE9499DF9");

            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.Contract).HasColumnName("contract");
            entity.Property(e => e.Etcqty).HasColumnName("etcqty");
            entity.Property(e => e.IsMonStatus).HasColumnName("is_mon_status");
            entity.Property(e => e.IsResStatus).HasColumnName("is_res_status");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(18, 9)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(18, 15)")
                .HasColumnName("longitude");
            entity.Property(e => e.LotAddress).HasColumnName("lot_address");
            entity.Property(e => e.LotName).HasColumnName("lot_name");
            entity.Property(e => e.Monqty).HasColumnName("monqty");
            entity.Property(e => e.Qty).HasColumnName("qty");
        });

        modelBuilder.Entity<ParkingSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__ParkingS__971A01BB3A371FC0");

            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.IsRented).HasColumnName("is_rented");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.SlotNumber).HasColumnName("slot_number");

            entity.HasOne(d => d.Lot).WithMany(p => p.ParkingSlot)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ParkingSl__lot_i__75A278F5");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ResId).HasName("PK__Reservat__2090B50DF0321256");

            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.DepositStatus)
                .HasDefaultValue(false)
                .HasColumnName("deposit_status");
            entity.Property(e => e.IsCanceled)
                .HasDefaultValue(false)
                .HasColumnName("is_canceled");
            entity.Property(e => e.IsFinish)
                .HasDefaultValue(false)
                .HasColumnName("is_finish");
            entity.Property(e => e.IsOverdue)
                .HasDefaultValue(false)
                .HasColumnName("is_overdue");
            entity.Property(e => e.IsRefoundDeposit)
                .HasDefaultValue(false)
                .HasColumnName("is_refound_deposit");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.NotificationStatus)
                .HasDefaultValue(false)
                .HasColumnName("notification_status");
            entity.Property(e => e.ReservationTime)
                .HasColumnType("datetime")
                .HasColumnName("reservation_time");
            entity.Property(e => e.ValidUntil)
                .HasColumnType("datetime")
                .HasColumnName("valid_until");

            entity.HasOne(d => d.Car).WithMany(p => p.Reservation)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__car_i__7D439ABD");

            entity.HasOne(d => d.Lot).WithMany(p => p.Reservation)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__lot_i__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
