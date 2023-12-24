using FluentValidation;
using Board.Common.Interfaces;

namespace Board.User.Service.DTOs.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        private readonly IGenericRepository<Models.User> _userRepository;
        public LoginRequestValidator(IGenericRepository<Models.User> userRepository)
        {
            _userRepository = userRepository;
            RuleFor(request => request.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
                .MustAsync(async (userName, cancellationToken) =>
                {
                    var user = await userRepository.GetAsync(x => x.UserName == userName);
                    return user != null;

                }).WithMessage("Username does not exist.");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }
}
