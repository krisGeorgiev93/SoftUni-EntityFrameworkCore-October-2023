namespace SoftUni.Models;
//make new class with the primary keys

public class EmployeeProject
{
    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; }

    public int ProjectId { get; set; }

    public virtual Project Project { get; set; }
}