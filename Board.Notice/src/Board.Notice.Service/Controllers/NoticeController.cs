using AutoMapper;
using Board.Common.Interfaces;
using Board.Common.Response;
using Board.Notice.Service.DTOs;
using Board.Notice.Service.DTOs.Validators;
using Board.Notice.Service.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Notice.Service.Controllers
{
    /// <summary>
    /// Controller class for managing notices in a specific channel.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/{channelId}/notices")]
    public class NoticeController : ControllerBase
    {
        private readonly IGenericRepository<Model.Notice> _noticeRepository;
        private readonly IGenericRepository<ChannelItem> _channelRepository;
        private readonly IMapper _mapper;
        private readonly NotificationClient _notificationClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoticeController"/> class.
        /// </summary>
        /// <param name="noticeRepository">The notice repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="notificationClient">The notification client.</param>
        public NoticeController(IGenericRepository<Model.Notice> noticeRepository, IGenericRepository<ChannelItem> channelRepository, IMapper mapper, NotificationClient notificationClient)
        {
            _noticeRepository = noticeRepository;
            _channelRepository = channelRepository;
            _mapper = mapper;
            _notificationClient = notificationClient;
        }

        /// <summary>
        /// Gets the notices for a specific channel.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response with the list of notices.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<List<GeneralNoticeDto>>>> GetNoticesAsync(Guid channelId)
        {
            var notices = await _noticeRepository.GetAllAsync(x => x.ChannelId == channelId);
            return Ok(CommonResponse<List<GeneralNoticeDto>>.Success(_mapper.Map<List<GeneralNoticeDto>>(notices)));
        }

        /// <summary>
        /// Gets a specific notice from a channel.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="noticeId">The notice identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response with the notice.</returns>
        [HttpGet("/{noticeId}")]
        public async Task<IActionResult> GetNoticeAsync(Guid channelId, Guid noticeId)
        {
            var notice = await _noticeRepository.GetAsync(noticeId);
            return Ok(CommonResponse<GeneralNoticeDto>.Success(_mapper.Map<GeneralNoticeDto>(notice)));
        }

        /// <summary>
        /// Creates a new notice in a channel. Only channel creator is allowed access.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="createNoticeDto">The create notice DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response with the created notice.</returns>
        [Authorize(Policy = "ChannelCreatorPolicy")]
        [HttpPost()]
        public async Task<IActionResult> CreateNoticeAsync(Guid channelId, [FromBody] CreateNoticeDto createNoticeDto)
        {
            createNoticeDto.ChannelId = channelId;
            var validationResult = await new CreateNoticeValidator(_channelRepository).ValidateAsync(createNoticeDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(CommonResponse<GeneralNoticeDto>.Fail("Input Error", validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
            }
            var notice = _mapper.Map<Model.Notice>(createNoticeDto);
            await _noticeRepository.CreateAsync(notice);
            await _notificationClient.SendNotification(channelId.ToString(), notice.Id.ToString());

            return Ok(CommonResponse<GeneralNoticeDto>.Success(_mapper.Map<GeneralNoticeDto>(notice)));
        }

        /// <summary>
        /// Updates an existing notice in a channel. Only Channel creator is allowed access.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="noticeId">The notice identifier.</param>
        /// <param name="updateNoticeDto">The update notice DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response with the updated notice.</returns>
        [Authorize(Policy = "ChannelCreatorPolicy")]
        [HttpPut("{noticeId}")]
        public async Task<ActionResult<CommonResponse<GeneralNoticeDto>>> UpdateNoticeAsync(Guid channelId, Guid noticeId, [FromBody] UpdateNoticeDto updateNoticeDto)
        {
            updateNoticeDto.ChannelId = channelId;
            updateNoticeDto.Id = noticeId;

            var validationResult = await new UpdateNoticeValidator(_channelRepository, _noticeRepository).ValidateAsync(updateNoticeDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(CommonResponse<GeneralNoticeDto>.Fail("Input Error", validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
            }
            var notice = await _noticeRepository.GetAsync(noticeId);
            if (notice == null)
            {
                return NotFound();
            }
            await _noticeRepository.UpdateAsync(_mapper.Map<Model.Notice>(updateNoticeDto));
            return NoContent();
        }

        /// <summary>
        /// Deletes a notice from a channel. Only channel creator is allowed access.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="noticeId">The notice identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response indicating success or failure.</returns>
        [Authorize(Policy = "ChannelCreatorPolicy")]
        [HttpDelete("{noticeId}")]
        public async Task<IActionResult> DeleteNoticeAsync(Guid channelId, Guid noticeId)
        {
            await _noticeRepository.RemoveAsync(noticeId);
            return NoContent();
        }
    }
}