using FluentValidation;
using Board.Notice.Service.Model;
using Board.Common.Interfaces;

namespace Board.Notice.Service.DTOs.Validators
{
    public class UpdateNoticeValidator : AbstractValidator<UpdateNoticeDto>
    {
        private readonly IGenericRepository<ChannelItem> _channelRepository;
        private readonly IGenericRepository<Model.Notice> _noticeRepository;
        public UpdateNoticeValidator(IGenericRepository<ChannelItem> channelRepository, IGenericRepository<Model.Notice> noticeRepository)
        {
            _channelRepository = channelRepository;
            _noticeRepository = noticeRepository;

        RuleFor(dto => dto.Id)
            .NotEmpty().WithMessage("Id is required.")
            .MustAsync(async (id, cancellation) =>
            {
                var notice = await _noticeRepository.GetAsync(id);
                return notice != null;
            }).WithMessage("Notice is invalid.");

        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(dto => dto.Body)
            .NotEmpty().WithMessage("Body is required.")
            .MaximumLength(500).WithMessage("Body must not exceed 500 characters.");

        RuleFor(dto => dto.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(dto => dto.Categories)
            .NotEmpty().WithMessage("Categories are required.")
            .Must(x => x.Count <= 3).WithMessage("Categories must not exceed 3.")
            .ForEach(category => category.Must(x => {
                return Enum.IsDefined(typeof(Category), x);
            }).WithMessage("Category is invalid."));

        RuleFor(dto => dto.Importance)
            .NotEmpty().WithMessage("Importance is required.")
            .Must(x => Enum.IsDefined(typeof(Importance), x)).WithMessage("Importance is invalid.");

        RuleFor(dto => dto.Issuer)
            .NotEmpty().WithMessage("Issuer is required.")
            .MaximumLength(100).WithMessage("Issuer must not exceed 100 characters.");
        RuleFor(dto => dto.Audience)
            .NotEmpty().WithMessage("Audience is required.")
            .MaximumLength(100).WithMessage("Audience must not exceed 100 characters.");

        RuleFor(dto => dto.ChannelId).MustAsync(async (channelId, cancellation) =>
        {
             var user = await _channelRepository.GetAsync(channelId);
             return user != null;

        }).WithMessage("Channel is invalid.");


        }
    }
}
