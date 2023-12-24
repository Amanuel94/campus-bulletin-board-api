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
                                    var user = await _userRepository.GetAsync(x=> x.UserName == userName);
                                    return user == null;
                                }).WithMessage("Username already exists.");

        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required.")
                                .EmailAddress().WithMessage("Email is not valid.")
                                .MustAsync(async (email, cancellationToken) =>
                                {
                                    var user = await _userRepository.GetAsync(x=> x.Email == email);
                                    return user == null;
                                }).WithMessage("Email already exists.");

        RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("Created date is required.")
                                  .NotNull().WithMessage("Created date cannot be null.");

        RuleFor(x => x.ModifiedDate).NotEmpty().WithMessage("Modified date is required.")
                                   .NotNull().WithMessage("Modified date cannot be null.");
    }
}