//
// Purpose: This file contains the CreateUserDtoValidator class which is used to validate the CreateUserDto class.
//
// The CreateUserDtoValidator class is responsible for validating the properties of the CreateUserDto class, which represents the data required to create a new user.
// It inherits from the UserDataValidator class, which provides common validation rules for user data.
//
// The CreateUserDtoValidator constructor takes an instance of IGenericRepository<Models.User> as a parameter, which is used to check if a user with the same username or email already exists in the repository.
//
// The validation rules for the CreateUserDto properties are defined using the FluentValidation library. The rules include:
// - PasswordHash: It must not be empty, not null, have a minimum length of 8 characters, and a maximum length of 20 characters.
// - UserName: It must not be empty, have a maximum length of 50 characters, a minimum length of 8 characters, not contain spaces, and must be unique in the repository.
// - Email: It must not be empty, be a valid email address, and must be unique in the repository.
// - CreatedDate: It must not be empty or null.
// - ModifiedDate: It must not be empty or null.
//
// If any of the validation rules fail, an appropriate error message is generated.
//
// Example usage:
// var userRepository = new UserRepository();
// var validator = new CreateUserDtoValidator(userRepository);
// var validationResult = validator.Validate(createUserDto);
// if (!validationResult.IsValid)
// {
//     foreach (var error in validationResult.Errors)
//     {
//         Console.WriteLine(error.ErrorMessage);
//     }
// }
//
// Note: This code assumes the existence of the UserDataValidator and Models.User classes, as well as the IGenericRepository interface and its implementation.
// Purpose: This file contains the CreateUserDtoValidator class which is used to validate the CreateUserDto class.

using Board.Common.Interfaces;
using FluentValidation;


namespace Board.User.Service.DTOs.Validators;

public class CreateUserDtoValidator : UserDataValidator<CreateUserDto>
{
    private readonly IGenericRepository<Models.User> _userRepository;
    public CreateUserDtoValidator(IGenericRepository<Models.User> userRepository)
    {
        _userRepository = userRepository;
        RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Password is required.")
                                    .NotNull().WithMessage("Password cannot be null.")
                                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                                    .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.");


        RuleFor(x => x.UserName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Username is required.")
                                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                                .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
                                .Must(x => !x.Contains(" ")).WithMessage("Username cannot contain spaces.")
                                .MustAsync(async (userName, cancellationToken) =>
                                {
                                    var user = await _userRepository.GetAsync(x => x.UserName == userName);
                                    return user == null;
                                }).WithMessage("Username already exists.");

        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required.")
                                .EmailAddress().WithMessage("Email is not valid.")
                                .MustAsync(async (email, cancellationToken) =>
                                {
                                    var user = await _userRepository.GetAsync(x => x.Email == email);
                                    return user == null;
                                }).WithMessage("Email already exists.");

        RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("Created date is required.")
                                  .NotNull().WithMessage("Created date cannot be null.");

        RuleFor(x => x.ModifiedDate).NotEmpty().WithMessage("Modified date is required.")
                                   .NotNull().WithMessage("Modified date cannot be null.");
    }
}