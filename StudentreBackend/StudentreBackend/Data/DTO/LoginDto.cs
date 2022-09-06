using FluentValidation;
using StudentreBackend.Data.Extensions;
using System.ComponentModel.DataAnnotations;

namespace StudentreBackend.Data.DTO
{
    public class LoginDto : IValidatableObject
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<LoginDto>(v =>
            {
                v.RuleFor(x => x.Login).NotEmpty().WithMessage("[[[Login is required]]]");
                v.RuleFor(x => x.Password).NotEmpty().WithMessage("[[[Password is required]]]");
            })
            .Validate(this)
            .Result();
        }
    }
}
