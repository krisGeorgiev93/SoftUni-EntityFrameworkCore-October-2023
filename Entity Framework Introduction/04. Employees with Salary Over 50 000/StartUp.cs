using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
    }
    
    //problem 04
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder stringBuilder = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Salary > 50000)
            .OrderBy(e => e.FirstName)
            .ToArray();

        foreach (var e in employees)
        {
            stringBuilder.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return stringBuilder.ToString().TrimEnd();
    }




}
