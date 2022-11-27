﻿// <auto-generated />
using System;
using File.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace File.Application.Migrations
{
    [DbContext(typeof(FileDbContext))]
    [Migration("20221127160657_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("File.Entity.RouteMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreateUserInfoId")
                        .HasColumnType("TEXT")
                        .HasComment("创建人");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("绝对路径");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("路由");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER")
                        .HasComment("地址类型");

                    b.Property<bool>("Visitor")
                        .HasColumnType("INTEGER")
                        .HasComment("是否同意他人访问");

                    b.HasKey("Id");

                    b.HasIndex("CreateUserInfoId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("RouteMappings", "路由映射缓存表");
                });

            modelBuilder.Entity("File.Entity.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("头像");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("密码");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasComment("用户名（唯一）");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("UserInfos", "用户");

                    b.HasData(
                        new
                        {
                            Id = new Guid("53e76abc-f1b0-4d71-ba5e-d478d0017fef"),
                            Avatar = "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png",
                            Password = "Aa123456.",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("File.Entity.RouteMapping", b =>
                {
                    b.HasOne("File.Entity.UserInfo", "CreateUserInfo")
                        .WithMany()
                        .HasForeignKey("CreateUserInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreateUserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
