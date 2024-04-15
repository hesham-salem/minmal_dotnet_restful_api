using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}





[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static List<Product> products = new List<Product>();

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            return Ok(product);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        product.Id = products.Count + 1;
        products.Add(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product updatedProduct)
    {
        var productIndex = products.FindIndex(p => p.Id == id);
        if (productIndex != -1)
        {
            updatedProduct.Id = id;
            products[productIndex] = updatedProduct;
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            products.Remove(product);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}

