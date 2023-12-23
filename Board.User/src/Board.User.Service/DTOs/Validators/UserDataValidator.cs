using Board.User.Services.Models;
using FluentValidation;

namespace Board.User.Services.DTOs.Validation;

public class UserDataValidator<T> : AbstractValidator<T> where T : IUserDto
{
    public UserDataValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                                .NotNull().WithMessage("First name cannot be null.")
                                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                               .NotNull().WithMessage("Last name cannot be null.")
                               .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.")
                                  .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

        RuleFor(x => x.Department).NotEmpty().WithMessage("Department is required.")
                                 .NotNull().WithMessage("Department cannot be null.")
                                 .Must(x => Enum.IsDefined(typeof(Department), x!)).WithMessage("Invalid department value.");

        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.")
                               .NotNull().WithMessage("Username cannot be null.")
                               .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.Avatar).NotNull().WithMessage("Avatar cannot be null.")
                             .Must(x => x!.Length > 0).WithMessage("Avatar length must be greater than 0.");


    }
}