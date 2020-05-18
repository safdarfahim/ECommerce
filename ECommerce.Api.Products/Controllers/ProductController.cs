using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        public readonly IProductProvider productProvider;
        public ProductController(IProductProvider productProvider)
        {
            this.productProvider = productProvider;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await productProvider.GetProductsAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Products);

            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await productProvider.GetProductAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Product);

            }
            return NotFound();
        }
    }
}
