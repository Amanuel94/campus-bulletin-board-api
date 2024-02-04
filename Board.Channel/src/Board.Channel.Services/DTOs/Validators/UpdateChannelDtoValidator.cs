// Purpose: This file contains the UpdateChannelDtoValidator class. This class is used to validate the UpdateChannelDto class using FluentValidation.

using FluentValidation;
using Board.Channel.Service.DTOs;
using Board.Common.Interfaces;
using Board.Channel.Service.Model;

namespace Board.Channel.Services.DTOs.Validators
{
    /// <summary>
    /// Validator for the UpdateChannelDto class.
    /// </summary>
    public class UpdateChannelDtoValidator : AbstractValidator<UpdateChannelDto>
    {
        private readonly IGenericRepository<UserItem> _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateChannelDtoValidator"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UpdateChannelDtoValidator(IGenericRepository<UserItem> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage("Channel ID is required.");

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Channel name is required.")
                .MaximumLength(50).WithMessage("Channel name must not exceed 50 characters.");

            RuleFor(dto => dto.Description)
                .MaximumLength(200).WithMessage("Channel description must not exceed 200 characters.");

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
