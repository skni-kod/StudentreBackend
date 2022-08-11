using FluentValidation;
using StudentreBackend.Data.Extensions;
using System.ComponentModel.DataAnnotations;

namespace StudentreBackend.DTO
{
    public class RegisterDto : IValidatableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string StudentId { get; set; }
        public string FieldOfStudy { get; set; }
        public string Term { get; set; }
        public string Collage { get; set; }
        public string Department { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            return this.Rules<RegisterDto>(v =>
            {
                v.RuleFor(x => x.Login).NotEmpty().WithMessage("[[[Login is required]]]");
                v.RuleFor(x => x.FirstName).Length(1, 150).WithMessage("[[[FirstName have to have min 1 and max 150 characters]]]");
                v.RuleFor(x => x.LastName).Length(1, 150).WithMessage("[[[LastName have to have min 1 and max 150 characters]]]");
                v.RuleFor(x => x.Password).NotEmpty().Equal(p => p.PasswordConfirm).WithMessage("[[[Passwords must be the same]]]");
                v.RuleFor(x => x.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$").WithMessage("[[[Incorrect password]]]");
                v.RuleFor(x => x.Email).NotEmpty().WithMessage("[[[Email is required]]]");
                v.RuleFor(x => x.Email).EmailAddress().WithMessage("[[[Incorrect email address]]]");
                v.RuleFor(x => x.Collage).NotEmpty().WithMessage("[[[Collage is required]]]");
                v.RuleFor(x => x.StudentId).NotEmpty().WithMessage("[[[Student identifier is required]]]");


            })
            .Validate(this)
            .Result();
        }

    }
}
