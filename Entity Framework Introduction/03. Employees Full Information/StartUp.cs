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


        //problem 03
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder stringBuilder = new StringBuilder();

        var employees = context.Employees.OrderBy(x => x.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            }).ToArray(); // ToArray or ToList ALWAYS ! OR AsNoTracking

        foreach (var e in employees)
        {
            stringBuilder.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return stringBuilder.ToString().TrimEnd();
    }


    
}
