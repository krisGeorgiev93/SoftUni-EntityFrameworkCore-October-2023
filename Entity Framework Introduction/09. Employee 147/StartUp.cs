using SoftUni.Data;
using SoftUni.Models;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();
            string result = GetEmployee147(context);
            Console.WriteLine(result);
        }

     

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees.FirstOrDefault(e => e.EmployeeId == 147);

            var projects = context.Projects
                .Where(p => p.EmployeesProjects.Any(p => p.EmployeeId == employee147.EmployeeId))
                .Select(p => new { ProjectName = p.Name })
                .OrderBy(ep => ep.ProjectName);

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in projects)
            {
                sb.AppendLine(project.ProjectName);
            }
            return sb.ToString().TrimEnd();
        }


    }
}

