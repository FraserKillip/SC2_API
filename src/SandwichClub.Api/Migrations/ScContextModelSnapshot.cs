using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SandwichClub.Api.Repositories;

namespace SandwichClub.Api.Migrations
{
    [DbContext(typeof(ScContext))]
    partial class ScContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901");

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("BankDetails");

                    b.Property<string>("BankName");

                    b.Property<string>("Email");

                    b.Property<string>("FacebookId");

                    b.Property<bool>("FirstLogin");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("Shopper");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.Week", b =>
                {
                    b.Property<int>("WeekId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<int?>("ShopperUserId");

                    b.HasKey("WeekId");

                    b.ToTable("Weeks");
                });

            modelBuilder.Entity("SandwichClub.Api.Repositories.Models.WeekUserLink", b =>
                {
                    b.Property<int>("WeekId");

                    b.Property<int>("UserId");

                    b.Property<double>("Paid");

                    b.Property<int>("Slices");

                    b.HasKey("WeekId", "UserId");

                    b.ToTable("WeekUserLinks");
                });
        }
    }
}
