using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using FluentValidation;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Validators
{
    public class GetDiplomasQueryValidator : AbstractValidator<GetDiplomasQuery>
    {
        public GetDiplomasQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0);
        }
    }
}
