namespace Login.Models;

public class Hour
{
    public int Id { get; set; }
    public DateTime? EntryDate { get; set; }
    public DateTime? OutDate { get; set; }
    
    public int EmployeeId { get; set; }
}