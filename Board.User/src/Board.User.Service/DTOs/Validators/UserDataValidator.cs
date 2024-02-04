//
// Purpose: This file contains the validator for the user data transfer object.
//
// The `UserDataValidator` class is responsible for validating the properties of a user data transfer object (DTO).
// It inherits from the `AbstractValidator<T>` class, where `T` is a type that implements the `IUserDto` interface.
//
// The validator defines rules for each property of the DTO using the `RuleFor` method from the FluentValidation library.
// The rules ensure that the properties meet certain criteria, such as not being empty, not exceeding a maximum length,
// having a valid email format, and being a valid department value.
//
// The `UserDataValidator` class is used to validate user data before it is processed or stored in the system.
//
// Example usage:
// UserDataValidator<UserDto> validator = new UserDataValidator<UserDto>();
// ValidationResult result = validator.Validate(userDto);
// if (!result.IsValid)
// {
//     // Handle validation errors
// }
//
// Note: The `UserDataValidator` class is generic, allowing it to be used with different types of user DTOs.
// The type parameter `T` must implement the `IUserDto` interface.
//
// Dependencies:
// - Board.User.Service.Models
// - FluentValidation
//
// See also:
// - IUserDto: Interface representing a user data transfer object
// - AbstractValidator<T>: Base class for creating validators using FluentValidation
// Purpose: This file contains the validator for the user data transfer object.

using Board.User.Service.Models;
using FluentValidation;

namespace Board.User.Service.DTOs.Validators;

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