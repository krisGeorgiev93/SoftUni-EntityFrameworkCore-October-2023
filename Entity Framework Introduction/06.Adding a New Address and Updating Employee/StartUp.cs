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

    //problem 06
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        StringBuilder stringBuilder = new StringBuilder();

        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        //context.Addresses.Add(newAddress); // This is the way for adding into the db

        Employee? employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");

        employee!.Address = newAddress;

        context.SaveChanges();

        var employeeAddresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e=> e.Address!.AddressText)
            .ToArray();

        foreach (var e in employeeAddresses)
        {
            stringBuilder.AppendLine(e.ToString());
        }

        return stringBuilder.ToString().TrimEnd();


    }


}
