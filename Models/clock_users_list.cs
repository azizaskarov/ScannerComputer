namespace Scanner.Models;

public class clock_users_list
{
    public int Id { get; set; }
    public string id_org { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public string status { get; set; }
    public string status_rus { get; set; }
    public string email { get; set; }
    public string email_kod { get; set; }
    public string phone { get; set; }
    public string qr_code { get; set; }
    public string user_ip { get; set; }
    public int security { get; set; } = 0;
    public string enter_date { get; set; }
    public string enter_time { get; set; } = "0:00:00";
}