using AutoMapper;
using Board.Common.Interfaces;
using Board.Common.Response;
using Board.Notice.Service.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.Notice.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/{channelId}/notices")]
public class NoticeController:ControllerBase
{
    private readonly IGenericRepository<Model.Notice> _noticeRepository;
    private readonly IMapper _mapper;

    public NoticeController(IGenericRepository<Model.Notice> noticeRepository, IMapper mapper)
    {
        _noticeRepository = noticeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<CommonResponse<List<GeneralNoticeDto>>>> GetNoticesAsync(Guid channelId)
    {
        var notices = await _noticeRepository.GetAllAsync(x => x.ChannelId == channelId);
        return Ok(CommonResponse<List<GeneralNoticeDto>>.Success( _mapper.Map<List<GeneralNoticeDto>>(notices)));
    }

    [HttpGet("/{noticeId}")]
    public async Task<IActionResult> GetNoticeAsync(Guid channelId, Guid noticeId)
    {
        var notice = await _noticeRepository.GetAsync(noticeId);
        return Ok(CommonResponse<GeneralNoticeDto>.Success(_mapper.Map<GeneralNoticeDto>(notice)));
    }

    [Authorize(Policy = "ChannelCreatorPolicy")]
    [HttpPost()]
    public async Task<IActionResult> CreateNoticeAsync(Guid channelId, [FromBody] CreateNoticeDto createNoticeDto)
    {
        createNoticeDto.ChannelId = channelId;
        var notice = _mapper.Map<Model.Notice>(createNoticeDto);
        await _noticeRepository.CreateAsync(notice);
        return Ok(CommonResponse<GeneralNoticeDto>.Success(_mapper.Map<GeneralNoticeDto>(notice)));
    }

    [Authorize(Policy = "ChannelCreatorPolicy")]
    [HttpPut("{noticeId}")]
    public async Task<ActionResult<CommonResponse<GeneralNoticeDto>>> UpdateNoticeAsync(Guid channelId, Guid noticeId, [FromBody] UpdateNoticeDto updateNoticeDto)
    {
        updateNoticeDto.ChannelId = channelId;
        updateNoticeDto.Id = noticeId;
        var notice = await _noticeRepository.GetAsync(noticeId);
        if (notice == null)
        {
            return NotFound();
        }
        await _noticeRepository.UpdateAsync(_mapper.Map<Model.Notice>(updateNoticeDto));
        return NoContent();
    }

    [Authorize(Policy = "ChannelCreatorPolicy")]
    [HttpDelete("{noticeId}")]
    public async Task<IActionResult> DeleteNoticeAsync(Guid channelId, Guid noticeId)
    {
        await _noticeRepository.RemoveAsync(noticeId);
        return NoContent();
    }

}