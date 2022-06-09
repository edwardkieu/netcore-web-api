using Application.ViewModels.Products;
using Application.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<Response<ProductViewModel>> CreateProductAsync(CreateProductViewModel vm);

        Task<Response<IEnumerable<ProductViewModel>>> GetAllAsync();
    }
}