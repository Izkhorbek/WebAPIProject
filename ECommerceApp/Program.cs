
using ECommerceApp.Data;
using ECommerceApp.Services;
using ECommerceApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // This will use the property name as it is defined in the class
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ECommerceAppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string is missing");
                }

                options.UseMySQL(connectionString);
            }, ServiceLifetime.Scoped);

            //Registering the services
            builder.Services.AddScoped<CustomerService>();

            //Registering the AddressService
            builder.Services.AddScoped<IAddressService, AddressService>();

            // Registering the CategoryService
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // Registering the ProductService
            builder.Services.AddScoped<IProductService, ProductService>();

            //Register the ShoppingCartService
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            //Register the OrderServie
            builder.Services.AddScoped<IOrderService, OrderService>();

            //Register EmailService
            builder.Services.AddScoped<EmailService>();

            //Register PaymentService
            builder.Services.AddScoped<PaymentService>();

            //Register Background service
            builder.Services.AddHostedService<PendingPaymentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
