using AutoMapper;
using Board.Channel.Service.DTOs;
using Board.Channel.Service.Jwt;
using Board.Common.Interfaces;
using Board.Channel.Service.Jwt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Board.Channel.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/channels")]
public class ChannelController : ControllerBase
{
    private readonly IGenericRepository<Model.Channel> _channelRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public ChannelController(IGenericRepository<Model.Channel> channelRepository, IMapper mapper, IJwtService jwtService)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    [HttpGet]
    public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetAllChannels()
    {
        var channels = await _channelRepository.GetAllAsync();
        var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
        var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
        return Ok(response);
    }

    [HttpGet("creator/{id}")]
    public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetChannelsByCreatorId(Guid id)
    {
        var channels = await _channelRepository.GetAllAsync(x => x.CreatorId == id);
        var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
        var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
        return Ok(response);
    }




    [HttpGet("{id}")]
    public async Task<ActionResult<CommonResponse<GeneralChannelDto>>> GetChannelById(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        var channelDto = _mapper.Map<GeneralChannelDto>(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(channelDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateChannel(CreateChannelDto createChannelDto)
    {
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);
        createChannelDto.CreatorId = identityProvider.GetUserId();
        var channel = _mapper.Map<Model.Channel>(createChannelDto);
        await _channelRepository.CreateAsync(channel);
        return CreatedAtAction(nameof(GetChannelById), new { id = channel.Id }, channel);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommonResponse<GeneralChannelDto>>> UpdateChannel(Guid id, UpdateChannelDto updateChannelDto)
    {
        var channel = await _channelRepository.GetAsync(id);
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);

        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        if (channel.CreatorId !=  identityProvider.GetUserId())
        {
            return Unauthorized(CommonResponse<GeneralChannelDto>.Fail("Unauthorized to update the channel", null!));
        }
        _mapper.Map(updateChannelDto, channel);
        await _channelRepository.UpdateAsync(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);

        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }

        if (channel.CreatorId !=  identityProvider.GetUserId())
        {
            return Unauthorized(CommonResponse<GeneralChannelDto>.Fail("Unauthorized to delete the channel", null!));
        }

        await _channelRepository.RemoveAsync(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(null!));
    }


    [HttpGet("search/{name}")]
    public async Task<ActionResult<CommonResponse<GeneralChannelDto>>> GetChannelByName([FromQuery] string name)
    {
        var channel = await _channelRepository.GetAsync(x => x.Name.ToLower() == name.ToLower());
        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        var channelDto = _mapper.Map<GeneralChannelDto>(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(channelDto));
    }

    // GET /api/channels/subscribed
    [HttpGet("subscribed")]
    public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetSubscribedChannels()
    {
        var userId = new IdentityProvider(HttpContext, _jwtService).GetUserId();
        var channels = await _channelRepository.GetAllAsync(x => x.Members.Contains(userId));
        var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
        var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
        return Ok(response);
    }

    // POST /api/channels/subscribe/id
    [HttpPost("subscribe/{id}")]
    public async Task<IActionResult> SubscribeToChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        var userId = new IdentityProvider(HttpContext, _jwtService).GetUserId();

        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        channel.Members.Add(userId);
        channel.JoinDates.Add(userId, DateTime.Now);
        await _channelRepository.UpdateAsync(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

    // POST /api/channels/unsubscribe/id
    [HttpPost("unsubscribe/{id}")]
    public async Task<IActionResult> UnsubscribeFromChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        var userId = new IdentityProvider(HttpContext, _jwtService).GetUserId();
        channel.Members.Remove(userId);
        channel.LeaveDates.Add(userId, DateTime.Now);
        await _channelRepository.UpdateAsync(channel);
        return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

}
