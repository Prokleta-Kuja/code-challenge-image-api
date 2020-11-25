using FluentValidation;
using ImageApi.Core.Entities;
using ImageApi.Core.Requests;

namespace ImageApi.Core.Validators
{
    public class UploadImageValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageValidator()
        {
            RuleFor(x => x.Image).NotNull();

            RuleFor(x => x.Description).NotEmpty();

            RuleFor(x => x.Type).NotEmpty()
                                .Must(x => Image.IsValidContentType(x))
                                .WithMessage(Image.InvalidContentTypeMessage);

            RuleFor(x => x.Size).Must(x => Image.IsValidSize(x))
                                .WithMessage(Image.InvalidSizeMessage);
        }
    }
}