using AutoMapper;
using Board.Channel.Service.DTOs;
using Board.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Board.Channel.Service.Controllers;

[ApiController]
[Route("api/channels")]
public class ChannelController : ControllerBase
{
    private readonly IGenericRepository<Model.Channel> _channelRepository;
    private readonly IMapper _mapper;

    public ChannelController(IGenericRepository<Model.Channel> channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<GeneralChannelDto>>>> GetAllChannels()
    {
        var channels = await _channelRepository.GetAllAsync();
        var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
        var response = Response<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<GeneralChannelDto>>> GetChannelById(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null));
        }
        var channelDto = _mapper.Map<GeneralChannelDto>(channel);
        return Ok(Response<GeneralChannelDto>.Success(channelDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateChannel(CreateChannelDto createChannelDto)
    {
        var channel = _mapper.Map<Model.Channel>(createChannelDto);
        await _channelRepository.CreateAsync(channel);
        return CreatedAtAction(nameof(GetChannelById), new { id = channel.Id }, channel);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<GeneralChannelDto>>> UpdateChannel(Guid id, UpdateChannelDto updateChannelDto)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null));
        }
        _mapper.Map(updateChannelDto, channel);
        await _channelRepository.UpdateAsync(channel);
        return Ok(Response<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        await _channelRepository.RemoveAsync(channel);
        return Ok(Response<GeneralChannelDto>.Success(null!));
    }


    [HttpGet("search/by-name/{name}")]
    public async Task<ActionResult<Response<GeneralChannelDto>>> GetChannelByName([FromQuery] string name)
    {
        var channel = await _channelRepository.GetAsync(x => x.Name == name);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        var channelDto = _mapper.Map<GeneralChannelDto>(channel);
        return Ok(Response<GeneralChannelDto>.Success(channelDto));
    }

    // GET /api/channels/subscribed
    [HttpGet("subscribed")]
    public async Task<ActionResult<Response<IEnumerable<GeneralChannelDto>>>> GetSubscribedChannels()
    {
        var channels = await _channelRepository.GetAllAsync(x => x.Members.Contains(Guid.Parse("d860efca-22d9-47fd-8249-791ba61b07c7")));
        var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
        var response = Response<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
        return Ok(response);
    }

    // POST /api/channels/subscribe/id
    [HttpPost("subscribe/{id}")]
    public async Task<IActionResult> SubscribeToChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        channel.Members.Add(Guid.Parse("d860efca-22d9-47fd-8249-791ba61b07c7"));
        await _channelRepository.UpdateAsync(channel);
        return Ok(Response<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

    // POST /api/channels/unsubscribe/id
    [HttpPost("unsubscribe/{id}")]
    public async Task<IActionResult> UnsubscribeFromChannel(Guid id)
    {
        var channel = await _channelRepository.GetAsync(id);
        if (channel == null)
        {
            return NotFound(Response<GeneralChannelDto>.Fail("Channel not found", null!));
        }
        channel.Members.Remove(Guid.Parse("d860efca-22d9-47fd-8249-791ba61b07c7"));
        await _channelRepository.UpdateAsync(channel);
        return Ok(Response<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
    }

}
