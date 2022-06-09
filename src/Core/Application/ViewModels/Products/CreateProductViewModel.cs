using Application.Interfaces.Repositories;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }

    public class CreateProductViewModelValidator : AbstractValidator<CreateProductViewModel>
    {
        private readonly IProductRepositoryAsync _productRepository;

        public CreateProductViewModelValidator(IProductRepositoryAsync productRepository)
        {
            _productRepository = productRepository;

            RuleFor(p => p.Barcode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueBarcode).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }

        private async Task<bool> IsUniqueBarcode(string barcode, CancellationToken cancellationToken)
        {
            return await _productRepository.IsUniqueBarcodeAsync(barcode);
        }
    }
}