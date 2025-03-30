using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerceAPI.Models
{
    public class CustomObjectBinder : IModelBinder
    {
        // The BindModelAcun method is invoked by the ASP.NET core model binding system 
        // to bind the incoming request data to the target model.

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Retrieve the value of the model binding field from the request
            var value  = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;

            // Check if the value is null or empty, indicating  a field binding attempt
            
            if(string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Failed();

                return Task.CompletedTask;
            }

            // Split the incoming strin by colons to extract the individual values

            var parts = value.Split(':');

            if (parts.Length == 3)
            {
                // Create a new instance of the CustomObject model and populate it with the extracted values
                var customObject = new CustomObject
                {
                    Name = parts[0],             // First part is the Name
                    Age = int.Parse(parts[1]),   // Second part is the Age (converted to integer)
                    Location = parts[2]          // Third part is the Location
                };
                // Mark the binding as successful and set the bound model
                bindingContext.Result = ModelBindingResult.Success(customObject);
            }
            else
            {
                // If the format is incorrect, mark the binding attempt as failed
                bindingContext.Result = ModelBindingResult.Failed();
            }

            // Return a completed task to indicate that the binding process has finished
            return Task.CompletedTask;
        }
    }
}
