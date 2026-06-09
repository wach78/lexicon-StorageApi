using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.DTOs;
using StorageApi.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly StorageApiContext _context;
    public ProductsController(StorageApiContext context)
    {
        _context = context;
    }


    // GET: api/products
    // GET: api/products?category=categoriName
    // GET: api/products?name=productname
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string? category,[FromQuery] string? name, CancellationToken cancellationToken)
    {
        IQueryable<Product> query = _context.Product.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
        {
            string trimmedCategory = category.Trim();

            query = query.Where(product => product.Category == trimmedCategory);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            string trimmedName = name.Trim();

            query = query.Where(product => product.Name.Contains(trimmedName));
        }



        List<ProductDto> products = await query
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Count = product.Count,
            })
            .ToListAsync(cancellationToken);

        return Ok(products);
    }

    // GET: api/Product/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
    {
        var product = await _context.Product.FindAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Count = product.Count
        };

        return Ok(productDto);
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] UpdateProductDto updateProductDto)
    {
        if (id != updateProductDto.Id)
        {
            return BadRequest("The id in the URL must match the id in the request body.");
        }

        var product = await _context.Product.FindAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        product.Name = updateProductDto.Name.Trim();
        product.Price = updateProductDto.Price;
        product.Category = updateProductDto.Category.Trim();
        product.Shelf = updateProductDto.Shelf.Trim();
        product.Count = updateProductDto.Count;
        product.Description = updateProductDto.Description?.Trim() ?? string.Empty;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Products
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct([FromBody] CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name.Trim(),
            Price = createProductDto.Price,
            Category = createProductDto.Category.Trim(),
            Shelf = createProductDto.Shelf.Trim(),
            Count = createProductDto.Count,
            Description = createProductDto.Description?.Trim() ?? string.Empty
        };

        _context.Product.Add(product);
        await _context.SaveChangesAsync();

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Count = product.Count
        };

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = productDto.Id },
            productDto
        );
    }

    // DELETE: api/Products/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int? id)
    {
        var product = await _context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/products/stats
    [HttpGet("stats")]
    public async Task<ActionResult<ProductStatsDto>> GetStats(CancellationToken cancellationToken)
    {
        ProductStatsDto? stats = await _context.Product
            .GroupBy(product => 1)
            .Select(group => new ProductStatsDto
            {
                TotalProducts = group.Count(),
                TotalInventoryValue = group.Sum(product => product.Price * product.Count),
                AveragePrice = (Decimal)group.Average(product => product.Price)
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (stats is null)
        {
            return Ok(new ProductStatsDto
            {
                TotalProducts = 0,
                TotalInventoryValue = 0m,
                AveragePrice = 0m
            });
        }

        return Ok(stats);
    }

   
}
