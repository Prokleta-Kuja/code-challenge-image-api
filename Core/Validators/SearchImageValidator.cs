using FluentValidation;
using ImageApi.Core.Entities;
using ImageApi.Core.Requests;

namespace ImageApi.Core.Validators
{
    public class SearchImageValidator : AbstractValidator<SearchImagesRequest>
    {
        public SearchImageValidator()
        {
            RuleFor(x => x.MinSize).Must(x => !x.HasValue || Image.IsValidSize(x.Value))
                                   .WithMessage(Image.InvalidSizeMessage);

            RuleFor(x => x.MaxSize).GreaterThan(x => x.MinSize)
                                   .Unless(x => !x.MinSize.HasValue)
                                   .Must(x => !x.HasValue || Image.IsValidSize(x.Value))
                                   .WithMessage(Image.InvalidSizeMessage);

            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize).GreaterThan(0)
                                    .LessThanOrEqualTo(20);
        }
    }
}