using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using Application.ViewModels.Products;
using Application.Wrappers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepositoryAsync _productRepository;

        public ProductService(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<ProductViewModel>> CreateProductAsync(CreateProductViewModel vm)
        {
            var product = _mapper.Map<Product>(vm);
            await _productRepository.AddAsync(product, true);
            var result = new Response<ProductViewModel>(_mapper.Map<ProductViewModel>(product));

            return result;
        }

        public async Task<Response<IEnumerable<ProductViewModel>>> GetAllAsync()
        {
            var products = await _productRepository.FindAll().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            var result = new Response<IEnumerable<ProductViewModel>>(products);

            return result;
        }
    }
}