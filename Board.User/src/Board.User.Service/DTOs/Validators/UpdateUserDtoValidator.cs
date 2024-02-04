//
// Purpose: This file contains the UpdateUserDtoValidator class. This class is used to validate the UpdateUserDto class.
//
// UpdateUserDtoValidator is responsible for validating the properties of the UpdateUserDto class, which represents the data required to update a user.
// It performs various validation rules on the UserName, Email, and ModifiedDate properties of the UpdateUserDto.
// The validation rules include checking for required fields, length constraints, uniqueness of the UserName and Email, and presence of the ModifiedDate.
// It uses FluentValidation library to define and enforce these validation rules.
// The UpdateUserDtoValidator class inherits from the UserDataValidator base class, which provides common validation rules for user data.
// It also depends on an IGenericRepository<Models.User> instance to check the uniqueness of the UserName and Email properties.
//
// Example usage:
// var userRepository = new UserRepository();
// var validator = new UpdateUserDtoValidator(userRepository);
// var validationResult = validator.Validate(updateUserDto);
// if (!validationResult.IsValid)
// {
//     // Handle validation errors
// }
//
// Dependencies:
// - Board.Common.Interfaces: Provides the IGenericRepository interface for accessing user data.
// - FluentValidation: A popular validation library for .NET applications.
//
// See also: UpdateUserDto, UserDataValidator

using Board.Common.Interfaces;
using FluentValidation;

namespace Board.User.Service.DTOs.Validators
{
    /// <summary>
    /// Validates the properties of the UpdateUserDto class.
    /// </summary>t
    public class UpdateUserDtoValidator : UserDataValidator<UpdateUserDto>
    {
        private readonly IGenericRepository<Models.User> _userRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateUserDtoValidator class.
        /// </summary>
        /// <param name="userRepository">The repository for accessing user data.</param>
        public UpdateUserDtoValidator(IGenericRepository<Models.User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
                .Must(x => !x.Contains(" ")).WithMessage("Username cannot contain spaces.")
                .MustAsync(async (x, userName, cancellationToken) =>
                {
                    var user = await _userRepository.GetAsync(x => x.UserName == userName);
                    return user == null || user.Id == x.Id;
                }).WithMessage("Username already exists.");

            RuleFor(x => x.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(async (x, email, cancellationToken) =>
                {
                    var user = await _userRepository.GetAsync(x => x.Email == email);
                    return user == null || user.Id == x.Id;
                }).WithMessage("Email already exists.");

            RuleFor(x => x.ModifiedDate)
                .NotEmpty().WithMessage("Modified date is required.")
                .NotNull().WithMessage("Modified date cannot be null.");
        }
    }
}