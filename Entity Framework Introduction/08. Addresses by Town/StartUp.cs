using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext context = new SoftUniContext();
    }

    //problem 08
    public static string GetAddressesByTown(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();

        var addresses = context.Addresses
            .OrderByDescending(e => e.Employees.Count())
        .ThenBy(a => a.Town!.Name)
        .ThenBy(a => a.AddressText)
        .Take(10)
        .ToArray();

        foreach (var a in addresses)
        {
            sb.AppendLine($"{a.AddressText}, {a.Town!.Name} - {a.Employees.Count()} employees");
        }

        return sb.ToString().TrimEnd();
    }
}
