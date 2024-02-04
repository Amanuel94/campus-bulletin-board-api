
// Purpose: This file contains the fluent validation for the CreateChannelDto class.

using FluentValidation;
using Board.Channel.Service.DTOs;
using Board.Channel.Service.Model;
using Board.Common.Interfaces;

namespace Board.Channel.Services.DTOs.Validators
{
    /// <summary>
    /// Represents a fluent validation for the CreateChannelDto class.
    /// </summary>
    public class CreateChannelDtoValidator : AbstractValidator<CreateChannelDto>
    {
        private readonly IGenericRepository<UserItem> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateChannelDtoValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user items.</param>
        public CreateChannelDtoValidator(IGenericRepository<UserItem> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(dto => dto.CreatorId)
                .NotEmpty().WithMessage("CreatorId is required.")
                .MustAsync(async (dto, creatorId, cancellationToken) =>
                {
                    var user = await _userRepository.GetAsync(x => x.Id == creatorId);
                    return user != null;
                }).WithMessage("Creator does not exist.");

            RuleFor(dto => dto.Logo)
                .NotEmpty().WithMessage("Logo is required.");
        }
    }
}
