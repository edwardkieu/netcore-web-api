using Application.Services.Interfaces;
using Application.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Route("api/v1/products")]
    [AllowAnonymous]
    public class ProductController : ApiControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductViewModel vm)
        {
            var productVm = await _productService.CreateProductAsync(vm);
            return Ok(productVm);
        }
    }
}