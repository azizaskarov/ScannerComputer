using Microsoft.EntityFrameworkCore;
using Scanner.Models;

namespace Scanner.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    //string connString = "server=localhost;database=demodb; user=root;";


    public DbSet<clock_users_list> clock_users_list => Set<clock_users_list>();
    public DbSet<clock_app_list> clock_app_list => Set<clock_app_list>();
    public DbSet<clock_app_history_list> clock_app_history_list => Set<clock_app_history_list>();
    public DbSet<clock_comp_list> clock_comp_list => Set<clock_comp_list>();


     string connString = "Server=83.69.136.27;Database=clocker;port=3306;User Id=klassik-rbs_uz;password=HA3KDRxD8df4K3jp;" +
                                      "CertificateFile=" + AppDomain.CurrentDomain.BaseDirectory + "images" + @"\" + "Certificate" + @"\" + "main_first.pfx" + ";" +
                                      "CertificatePassword=jQLv$c9R5(nb!uKCFPgg;" +
                                      "SslMode=Required";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseMySql(connString, new MySqlServerVersion(new Version(8, 0, 11)));
        optionsBuilder.UseMySql("datasource = 127.0.0.1; port=3306; username = root; database=clocker", new MySqlServerVersion(new Version(8, 0, 11)));
    }

    
}
