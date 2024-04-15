using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<Product> products = new List<Product>();

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseRouting();

        app.MapGet("/products", GetAllProducts);
        app.MapGet("/products/{id}", GetProductById);
        app.MapPost("/products", AddProduct);
        app.MapPut("/products/{id}", UpdateProduct);
        app.MapDelete("/products/{id}", DeleteProduct);

        app.Run();
    }

    static void GetAllProducts(HttpContext context)
    {
        var json = JsonConvert.SerializeObject(products);
        context.Response.ContentType = "application/json";
        context.Response.WriteAsync(json);
    }

    static void GetProductById(HttpContext context)
    {
        var id = int.Parse(context.Request.RouteValues["id"].ToString());
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            var json = JsonConvert.SerializeObject(product);
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(json);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }

    static async void AddProduct(HttpContext context)
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();
        var product = JsonConvert.DeserializeObject<Product>(body);
        product.Id = products.Count + 1;
        products.Add(product);
        context.Response.StatusCode = 201;
    }

    static async void UpdateProduct(HttpContext context)
    {
        var id = int.Parse(context.Request.RouteValues["id"].ToString());
        var productIndex = products.FindIndex(p => p.Id == id);
        if (productIndex != -1)
        {
            using var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var updatedProduct = JsonConvert.DeserializeObject<Product>(body);
            updatedProduct.Id = id;
            products[productIndex] = updatedProduct;
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }

    static void DeleteProduct(HttpContext context)
    {
        var id = int.Parse(context.Request.RouteValues["id"].ToString());
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            products.Remove(product);
        }
        else
        {
            context.Response.StatusCode = 404;
        }
    }
}

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
