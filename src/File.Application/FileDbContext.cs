using File.Entity;
using Microsoft.EntityFrameworkCore;

namespace File.Application;

public class FileDbContext : DbContext
{
    public required  DbSet<UserInfo> UserInfos { get; set; }

    public required DbSet<RouteMapping> RouteMappings { get; set; }

    public required DbSet<InterfaceStatistics> InterfaceStatistics { get; set; }
    
    public FileDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserInfo>(u =>
        {
            u.ToTable("UserInfos", "用户");

            u.HasIndex(x => x.Id).IsUnique();
            u.HasKey(x => x.Id);
            u.HasIndex(x => x.Username).IsUnique();


            u.Property(x => x.Username).HasComment("用户名（唯一）");
            u.Property(x => x.Password).HasComment("密码");
            u.Property(x => x.Avatar).HasComment("头像");

        });

        builder.Entity<RouteMapping>(x =>
        {
            x.ToTable("RouteMappings", "路由映射缓存表");

            x.HasIndex(x => x.Id).IsUnique();
            x.HasKey(x => x.Id);
            x.HasIndex(x => x.CreateUserInfoId);

            x.Property(x => x.Path).HasComment("绝对路径");
            x.Property(x => x.Visitor).HasComment("是否同意他人访问");
            x.Property(x => x.Route).HasComment("路由");
            x.Property(x => x.Type).HasComment("地址类型");
            x.Property(x => x.CreateUserInfoId).HasComment("创建人");
        });

        builder.Entity<InterfaceStatistics>(x =>
        {
            x.ToTable("InterfaceStatistics", "接口访问统计");

            x.HasIndex(x => x.Id).IsUnique();
            x.HasIndex(x => x.UserId);

            x.Property(x => x.UserId).HasComment("具体访问人id");
            x.Property(x => x.CreatedTime).HasComment("访问时间");
            x.Property(x => x.Query).HasComment("访问时携带的参数");

        });

        var userInfo = new UserInfo("admin","Aa123456.","https://blog-simple.oss-cn-shenzhen.aliyuncs.com/logo.png");

        builder.Entity<UserInfo>().HasData(userInfo);
        
    }
}
