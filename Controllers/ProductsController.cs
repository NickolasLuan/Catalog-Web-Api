using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly APICatalogoContext _context;

    public ProductsController(APICatalogoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        var products = _context.Products?.ToList();

        if (products is null)
        {
            return NotFound("Products not found");
        }

        return products;
    }

    [HttpGet("{productId:int}", Name="GetProduct")]
    public ActionResult<Product> GetProduct(int productId)
    {

        var productById = _context.Products?.FirstOrDefault((p => p.ProductId == productId));

        if (productById == null)
        {
            return NotFound();
        }
        return productById;
    }

    [HttpPost]
    public ActionResult CreateProduct(Product product)
    {
        if (product == null) {
            return BadRequest();
        }

        _context.Products.Add(product); //Adiciona no contexto em memória
        _context.SaveChanges(); // Confirma a transação no banco

       return new CreatedAtRouteResult("GetProduct", new { productId = product.ProductId }, product);
    }

    [HttpPut("{productId:int}")]
    public ActionResult changeProduct(int productId, Product product) {
        if (productId != product.ProductId)
        {
            return BadRequest();
        }
        
        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(product);
    }

    [HttpDelete("{productId:int}")]
    public ActionResult DeleteProduct(int productId)
    {
        var product = _context.Products?.FirstOrDefault(p => p.ProductId == productId);

        if (product == null)
        {
            return NotFound("Product not found");
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}