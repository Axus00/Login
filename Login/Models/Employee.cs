namespace Login.Models;

public class Employee
{
    public int Id { get; set; }
    public string Names { get; set; }
    public string LastNames { get; set; }
    
    public string Password { get; set; }
    public string Photo { get; set; }
    public string TypeIdentification { get; set; }
    public string Identified { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? OutDate { get; set; }
}