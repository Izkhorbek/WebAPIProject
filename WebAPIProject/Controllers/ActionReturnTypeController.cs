using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Data;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionReturnTypeController : ControllerBase
    {
        [HttpGet("name")]
        public string GetName() // this method returns primitive type 
        {
            return "Return from GetName()";
        }

        [HttpGet("Details/{Id}")]
        public Employee GetEmployeeDetails([FromRoute] int Id) // this method returns complex type
        {
            return new Employee()
            {
                Id = 1,
                Gender = "Male",
                Name = "John",
                Department = "IT",
                City = "Tashkent"
            };
        }

        [HttpGet("GetEmployeeDetails")]
        public IEnumerable<Employee> GetAllEmployees() // this method returns collection of complex type
        {
            return EmployeeData.Employees;
        }
    }
}
