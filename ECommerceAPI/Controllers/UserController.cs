using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromForm] UserModel user)
        {
           if(user.ProfilePicture != null)
            {
                Console.WriteLine(user);
                var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                if(!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var filePath = Path.Combine(uploadFolderPath, user.ProfilePicture.FileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    user.ProfilePicture.CopyTo(stream);
                }

                // handle the user data, e.g., save it to the database
                var response = new
                {
                    Success = true,
                    Message = $"User {user.Name} created successfully",
                    ProfilePictureName = user?.ProfilePicture?.FileName,
                    Code = StatusCodes.Status200OK,
                };

                return Ok(response);
            }

            return BadRequest("Profile picture is required");
        }
    }
}
