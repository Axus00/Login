namespace Login.Models;

public class cookieOptions
{
    public DateTime Expires { get; set; }
    
    public  bool HttpOnly { get; set; }
    
    public bool secure { get; set; }
}