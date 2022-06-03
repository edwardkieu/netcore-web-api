using Application.Commons.Extensions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var getAllQuery = _mapper.Map<GetAllProductsParameter>(request);
            var (query, totalCount) = _productRepository
                .FindAll()
                .ToPaginatedList(getAllQuery.PageNumber, getAllQuery.PageSize);
            var products = await query
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var productViewModel = _mapper.Map<IEnumerable<GetAllProductsViewModel>>(products);

            return new PagedResponse<IEnumerable<GetAllProductsViewModel>>(productViewModel, totalCount, getAllQuery.PageNumber, getAllQuery.PageSize);
        }
    }
}