using Board.Common.Interfaces;
using FluentValidation;


namespace Board.User.Service.DTOs.Validators;

public class UpdateUserDtoValidator : UserDataValidator<UpdateUserDto>
{
    private readonly IGenericRepository<Models.User> _userRepository;
    public UpdateUserDtoValidator(IGenericRepository<Models.User> userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserName).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Username is required.")
                                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                                .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
                                .Must(x => !x.Contains(" ")).WithMessage("Username cannot contain spaces.")
                                .MustAsync(async (x, userName, cancellationToken) =>
                                {
                                    var user = await _userRepository.GetAsync(x=> x.UserName == userName);
                                    return user ==  null || user.Id == x.Id;
                                }).WithMessage("Username already exists.");

        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required.")
                                .EmailAddress().WithMessage("Email is not valid.")
                                .MustAsync(async (x, email, cancellationToken) =>
                                {
                                    var user = await _userRepository.GetAsync(x=> x.Email == email);
                                    return user == null || user.Id == x.Id;
                                }).WithMessage("Email already exists.");


        RuleFor(x => x.ModifiedDate).NotEmpty().WithMessage("Modified date is required.")
                                   .NotNull().WithMessage("Modified date cannot be null.");
    }
}