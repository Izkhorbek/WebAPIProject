using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebAPIProject.Data;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")] // Route attribute to define the route for the controller
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        ////GET: api/Employee/1
        //[HttpGet("{Id}")] // Route dynamic parameter to get employee by Id
        //public ActionResult GetEmployeeById(int Id)
        //{
        //    var employee = EmployeeData.Employees.FirstOrDefault(e => e.Id == Id);
        //    if(employee == null)
        //    {
        //        return NotFound("Employee with Id: " + Id + " not found");
        //    }   

        //    return Ok(employee);
        //}

        ////[HttpGet("Gender/{gender}/City/{city}")] // Route to get all employees
        ////public ActionResult<IEnumerable<Employee>> GetEmployeesByGenderAndCity(string gender, string city)
        ////{
        ////    var filteredEmployees = EmployeeData.Employees
        ////        .Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
        ////        e.City.Equals(city, StringComparison.OrdinalIgnoreCase))
        ////        .ToList();

        ////    if (filteredEmployees.Count == 0)
        ////        return NotFound($"No employees found with Gender '{gender}' in City '{city}'.");

        ////    return Ok(filteredEmployees);
        ////}

        //// GET api/Employee/Search?Gender=Male&Department=IT&City=Los Angeles
        //[HttpGet("Search")]
        //public ActionResult<IEnumerable<Employee>> SearchEmployees([FromQuery] EmployeeSearch searchCriteria) // Action parameterlarni bitta alohida class modelga chiqarsak bo'ladi ekan.
        //{
        //    var filteredEmployees = EmployeeData.Employees.AsQueryable();
        //    if (!string.IsNullOrEmpty(searchCriteria.Gender))
        //        filteredEmployees = filteredEmployees.Where(e => e.Gender.Equals(searchCriteria.Gender, StringComparison.OrdinalIgnoreCase));
        //    if (!string.IsNullOrEmpty(searchCriteria.Department))
        //        filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(searchCriteria.Department, StringComparison.OrdinalIgnoreCase));
        //    if (!string.IsNullOrEmpty(searchCriteria.City))
        //        filteredEmployees = filteredEmployees.Where(e => e.City.Equals(searchCriteria.City, StringComparison.OrdinalIgnoreCase));
        //    var result = filteredEmployees.ToList();
        //    if (!result.Any())
        //        return NotFound("No employees match the provided search criteria.");
        //    return Ok(result);
        //}

        //[HttpGet("DirectSearch")]
        //public ActionResult<IEnumerable<Employee>> DirectSearchEmployees(){

        //    var gender = HttpContext.Request.Query["Gender"].ToString();
        //    var department = HttpContext.Request.Query["Department"].ToString();
        //    var city = HttpContext.Request.Query["City"].ToString();

        //    var filteredEmployees = EmployeeData.Employees.AsQueryable();

        //    if (!string.IsNullOrEmpty(gender))
        //    {
        //        filteredEmployees = filteredEmployees.Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        //    }

        //    if(!string.IsNullOrEmpty(department))
        //    {
        //        filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
        //    }

        //    if(!string.IsNullOrEmpty(city))
        //    {
        //        filteredEmployees = filteredEmployees.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        //    }

        //    var result = filteredEmployees.ToList();

        //    if(result.Count == 0)
        //    {
        //        return NotFound("No employees match the provided search criteria.");
        //    }

        //    return Ok(result);
        //}

        //// GET api/Employee/Gender/Male?Department=IT&City=Los Angeles
        //[HttpGet("Gender/{gender}")]
        //public ActionResult<IEnumerable<Employee>> GetEmployeesByGender([FromRoute] string gender, [FromQuery] string? department, [FromQuery] string? city)
        //{
        //    var filteredEmployees = EmployeeData.Employees
        //       .Where(e => e.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
        //    if (!string.IsNullOrEmpty(department))
        //        filteredEmployees = filteredEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
        //    if (!string.IsNullOrEmpty(city))
        //        filteredEmployees = filteredEmployees.Where(e => e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        //    var result = filteredEmployees.ToList();
        //    if (!result.Any())
        //        return NotFound("No employees match the provided search criteria.");
        //    return Ok(result);
        //}

        ////Action method with Multiple Routes
        //[HttpGet("All")]
        //[HttpGet("GetAll")]
        //[HttpGet("AllEmployees")]
        //public ActionResult<IEnumerable<Employee>> GetAllEmployees()
        //{
        //    var employees = EmployeeData.Employees;
        //    return Ok(employees);
        //}

        //[HttpGet]
        //public string GetAllEmployees()
        //{
        //   return "Response from GetAllEmployees Method";
        //}

        //[HttpGet]
        //public string GetAllDepartments()
        //{
        //    return "Response from GetAllDepartments Method";
        //}

        //[HttpGet("{Id}")]
        //public string GetEmployeeById(int Id)
        //{
        //    return $"Response from GetEmployeeById Method, Id : {Id}";
        //}

        // There are two resources with the same route template. The resource name 'GetEmployeeDetails' are same for both resources.
        // But, the parameters are different. So, here, we are getting an error.
        // To resolve this error, we need asp.net ROUTE CONSTRAINT.
        // When parameter is int, then it will call the first method.
        // When parameter is string, then it will call the second method.

        [Route("{EmployeeId:regex(5)}")]
        [HttpGet]
        public string GetEmployeeDetails(int EmployeeId)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeId : {EmployeeId}";
        }
        [Route("{EmployeeName:alpha}")]
        [HttpGet]
        public string GetEmployeeDetails(string EmployeeName)
        {
            return $"Response from GetEmployeeDetails Method, EmployeeName : {EmployeeName}";
        }
    }
}
