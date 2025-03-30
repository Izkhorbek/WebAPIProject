using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        // Limited to Built-in Sources
        // Limitation: 
        // The model binder can 'not bind the CustomObject model directly from a custom string format. like "Name:Age:Location"
        [HttpGet("custom-object-binding")]
        public IActionResult CustomObjectBinding([FromQuery] string complexData)
        {
            // The data is in the custom format "Name:Age:Location"
            var parts = complexData.Split(':');
            if(parts?.Length == 3)
            {
                var customObject = new CustomObject
                {
                    Name = parts[0],
                    Age = int.Parse(parts[1]),
                    Location = parts[2]
                };

               return Ok(customObject);
            }

            return BadRequest("Invalid custom format");
        }

        // Lacks Flexibility
        // Limitation:
        // The default model binder cannot easily handle merging data form multiple sources(Header, Query, Body) into a single model.
        [HttpGet("multi-source-binding")]
        public IActionResult MultiSourceBinding([FromHeader(Name ="X-Custom-Header")] string headerValue,
            [FromBody] ComplexBodyModel bodyModel)
        {
            // Merging data from header, query, and body
            var mergedResult = new MergedModel
            {
                Header = headerValue,
                Query = "",
                BodyData = bodyModel.Data
            };
            return Ok(mergedResult);
        }

        // No Support for Sepcial Data Types
        // Limitation:
        // Tuples cannot be easily bound by the default model binder without additional configutation or custom model binder.
        [HttpPost("tuple-binding")]
        public IActionResult TupleBinding([FromBody] CustomTupleModel tupleModel)
        {
            return Ok($"Tuple Data: Item1 = {tupleModel.Item1 }, Item2: {tupleModel.Item2}");
        }

        // Performance Issues for Large Data
        // Limitation:
        // For large data, Frombody reads the entire request bosy into memory, which can be inefficient and lead to performance issues.
        [HttpPost("large-data-binding")]
        public IActionResult LargeDataBinding([FromBody] LargeDataModel model)
        {
            return Ok($"Large Data Count: {model.LargeDataList.Count}");
        }
    }
}
