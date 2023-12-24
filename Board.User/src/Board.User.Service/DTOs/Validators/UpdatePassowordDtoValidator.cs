using FluentValidation;

namespace Board.User.Service.DTOs.Validators
{
    public class UpdatePassowordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePassowordDtoValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required.")
                                    .NotNull().WithMessage("Password cannot be null.")
                                    .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.")
                                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        }
    }

}

