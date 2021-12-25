using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepository;

            public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.FindByIdAsync(command.Id);
                if (product == null)
                    throw new ApiException($"Product Not Found.");

                await _productRepository.RemoveAsync(product);

                return new Response<int>(product.Id);
            }
        }
    }
}