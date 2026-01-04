using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Application.Modules.Cars.Commands.Create
{
    public sealed class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(x => x.BrandId).GreaterThan(0);
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.CarStatusId).GreaterThan(0);

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Vin)
                .NotEmpty()
                .Length(17); 

            RuleFor(x => x.ProductionYear)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 1); 

            RuleFor(x => x.Mileage)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Color)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Transmission)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.FuelType)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Drivetrain)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Engine)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.HorsePower)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .MaximumLength(2000);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.DiscountedPrice)
                .GreaterThan(0).When(x => x.DiscountedPrice.HasValue)
                .LessThan(x => x.Price).When(x => x.DiscountedPrice.HasValue);

            RuleFor(x => x.DateAdded)
                .LessThanOrEqualTo(DateTime.UtcNow).When(x => x.DateAdded.HasValue);

            // Primary image 
            RuleFor(x => x.PrimaryImageUrl)
                .NotEmpty()
                .MaximumLength(500);

            // Gallery images
            RuleForEach(x => x.GalleryImageUrls)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.GalleryImageUrls)
                .Must(urls => urls == null || urls
                    .Select(u => u?.Trim())
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count()
                    ==
                    urls.Select(u => u?.Trim())
                        .Where(u => !string.IsNullOrWhiteSpace(u))
                        .Count()
                )
                .WithMessage("GalleryImageUrls contains duplicates.");

            // Features
            RuleForEach(x => x.Features)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Features)
                .Must(features => features == null || features
                    .Select(f => f?.Trim())
                    .Where(f => !string.IsNullOrWhiteSpace(f))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count()
                    ==
                    features.Select(f => f?.Trim())
                            .Where(f => !string.IsNullOrWhiteSpace(f))
                            .Count()
                )
                .WithMessage("Features contains duplicates.");

        }
    }
}
