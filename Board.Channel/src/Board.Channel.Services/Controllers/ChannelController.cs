using AutoMapper;
using Board.Channel.Service.DTOs;
using Board.Channel.Service.Jwt;
using Board.Common.Interfaces;
using Board.Channel.Service.Jwt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Board.Channel.Contracts;

namespace Board.Channel.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/channels")]
public class ChannelController : ControllerBase
{
    private readonly IGenericRepository<Model.Channel> _channelRepository;
    private readonly IGenericRepository<Model.UserItem> _userItemRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IPublishEndpoint _publishEndpoint;

    public ChannelController(IGenericRepository<Model.Channel> channelRepository, IMapper mapper, IJwtService jwtService, IGenericRepository<Model.UserItem> userItemRepository, IPublishEndpoint publishEndpoint)
    {
            _channelRepository = channelRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _userItemRepository = userItemRepository;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Retrieves all channels.
        /// </summary>
        /// <returns>An ActionResult containing a CommonResponse of IEnumerable of GeneralChannelDto.</returns>
        [HttpGet]
        public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetAllChannels()
        {
            var channels = await _channelRepository.GetAllAsync();
            var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
            var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all channels created by the admin.
        /// </summary>
        /// <returns>An ActionResult containing a CommonResponse object with a list of GeneralChannelDto.</returns>
        [HttpGet("admin")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetChannelsByAdmin()
        {
            var identityProvider = new IdentityProvider(HttpContext, _jwtService);
            var userId = identityProvider.GetUserId();
            var channels = await _channelRepository.GetAllAsync(x => x.CreatorId == userId);
            var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
            var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of channels created by a specific user.
        /// </summary>
        /// <param name="id">The ID of the creator.</param>
        /// <returns>An ActionResult containing a CommonResponse with a list of GeneralChannelDto objects.</returns>
        [HttpGet("creator/{id}")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetChannelsByCreatorId(Guid id)
        {
            var channels = await _channelRepository.GetAllAsync(x => x.CreatorId == id);
            Console.WriteLine(id);
            Console.WriteLine(new IdentityProvider(HttpContext, _jwtService).GetUserId());
            var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
            var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a channel by its ID.
        /// </summary>
        /// <param name="id">The ID of the channel to retrieve.</param>
        /// <returns>An asynchronous task that represents the operation and contains the action result with the channel information.</returns>
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

        /// <summary>
        /// Creates a new channel.
        /// </summary>
        /// <param name="createChannelDto">The data transfer object containing the channel information.</param>
        /// <returns>An asynchronous task that represents the operation and returns an IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateChannel(CreateChannelDto createChannelDto)
        {
            var identityProvider = new IdentityProvider(HttpContext, _jwtService);
            createChannelDto.CreatorId = identityProvider.GetUserId();
            createChannelDto.Members = new List<Guid> { identityProvider.GetUserId() };
            createChannelDto.JoinDates = new Dictionary<string, DateTime> { { identityProvider.GetUserId().ToString(), DateTime.Now } };
            createChannelDto.LeaveDates = new Dictionary<string, DateTime>();
            var channel = _mapper.Map<Model.Channel>(createChannelDto);
            channel.CreatedDate = DateTime.Now;
            channel.ModifiedDate = DateTime.Now;

            await _channelRepository.CreateAsync(channel);
            var ch = _mapper.Map<ChannelCreated>(channel);
            ch.Id = channel.Id;
            await _publishEndpoint.Publish(ch);
            return CreatedAtAction(nameof(GetChannelById), new { id = channel.Id }, channel);
        }

        /// <summary>
        /// Updates a channel with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the channel to update.</param>
        /// <param name="updateChannelDto">The data to update the channel with.</param>
        /// <returns>An ActionResult containing the updated channel.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CommonResponse<GeneralChannelDto>>> UpdateChannel(Guid id, UpdateChannelDto updateChannelDto)
        {
            var channel = await _channelRepository.GetAsync(id);
            var identityProvider = new IdentityProvider(HttpContext, _jwtService);

            if (channel == null)
            {
                return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
            }
            if (channel.CreatorId != identityProvider.GetUserId())
            {
                return Unauthorized(CommonResponse<GeneralChannelDto>.Fail("Unauthorized to update the channel", null!));
            }
            _mapper.Map(updateChannelDto, channel);
            channel.ModifiedDate = DateTime.Now;
            await _channelRepository.UpdateAsync(channel);
            return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
        }

        /// <summary>
        /// Deletes a channel with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the channel to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChannel(Guid id)
        {
            var channel = await _channelRepository.GetAsync(id);
            var identityProvider = new IdentityProvider(HttpContext, _jwtService);

            if (channel == null)
            {
                return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
            }

            if (channel.CreatorId != identityProvider.GetUserId())
            {
                return Unauthorized(CommonResponse<GeneralChannelDto>.Fail("Unauthorized to delete the channel", null!));
            }

            await _channelRepository.RemoveAsync(channel);
            await _publishEndpoint.Publish(_mapper.Map<ChannelDeleted>(channel));
            return Ok(CommonResponse<GeneralChannelDto>.Success(null!));
        }

        /// <summary>
        /// Retrieves a channel by its name.
        /// </summary>
        /// <param name="name">The name of the channel to retrieve.</param>
        /// <returns>An <see cref="ActionResult"/> containing the channel information.</returns>
        [HttpGet("search/{name}")]
        public async Task<ActionResult<CommonResponse<GeneralChannelDto>>> GetChannelByName([FromQuery] string name)
        {
            var channel = await _channelRepository.GetAsync(x => x.Name.ToLower().Contains(name.ToLower()));
            if (channel == null)
            {
                return NotFound(CommonResponse<GeneralChannelDto>.Fail("Channel not found", null!));
            }
            var channelDto = _mapper.Map<GeneralChannelDto>(channel);
            return Ok(CommonResponse<GeneralChannelDto>.Success(channelDto));
        }

        /// <summary>
        /// Retrieves the list of subscribed channels for the current user.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing a <see cref="CommonResponse{T}"/> with a collection of <see cref="GeneralChannelDto"/>.</returns>
        [HttpGet("subscribed")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GeneralChannelDto>>>> GetSubscribedChannels()
        {
            var userId = new IdentityProvider(HttpContext, _jwtService).GetUserId();
            var channels = await _channelRepository.GetAllAsync(x => x.Members.Contains(userId));
            var channelsDto = _mapper.Map<IEnumerable<GeneralChannelDto>>(channels);
            var response = CommonResponse<IEnumerable<GeneralChannelDto>>.Success(channelsDto);
            return Ok(response);
        }

        /// <summary>
        /// Subscribes a user to a channel.
        /// </summary>
        /// <param name="id">The ID of the channel to subscribe to.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the subscription request.</returns>
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
            channel.JoinDates.Add(userId.ToString(), DateTime.Now);
            await _channelRepository.UpdateAsync(channel);
            return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
        }

        /// <summary>
        /// Unsubscribes a user from a channel.
        /// </summary>
        /// <param name="id">The ID of the channel.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the asynchronous operation.</returns>
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
            channel.LeaveDates.Add(userId.ToString(), DateTime.Now);
            await _channelRepository.UpdateAsync(channel);
            return Ok(CommonResponse<GeneralChannelDto>.Success(_mapper.Map<GeneralChannelDto>(channel)));
        }

        /// <summary>
        /// Retrieves the members of a channel.
        /// </summary>
        /// <param name="id">The ID of the channel.</param>
        /// <returns>An ActionResult containing the response with the list of members.</returns>
        [HttpGet("{id}/members")]
        public async Task<ActionResult<CommonResponse<IEnumerable<MemberDto>>>> GetChannelMembers(Guid id)
        {
            var channel = await _channelRepository.GetAsync(id);
            if (channel == null)
            {
                return NotFound(CommonResponse<MemberDto>.Fail("Channel not found", null!));
            }

            var members = await _userItemRepository.GetAllAsync(x => channel.Members.Contains(x.Id));
            var response = CommonResponse<IEnumerable<MemberDto>>.Success(_mapper.Map<IEnumerable<MemberDto>>(members));
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a member from a channel by their ID and username.
        /// </summary>
        /// <param name="id">The ID of the channel.</param>
        /// <param name="userName">The username of the member.</param>
        /// <returns>An ActionResult containing the member information.</returns>
        [HttpGet("{id}/member")]
        public async Task<ActionResult<CommonResponse<MemberDto>>> GetMember(Guid id, [FromQuery] string userName)
        {
            var channel = await _channelRepository.GetAsync(id);
            if (channel == null)
            {
                return NotFound(CommonResponse<MemberDto>.Fail("Channel not found", null!));
            }
            var member = await _userItemRepository.GetAsync(x => x.UserName.ToLower() == userName.ToLower());
            if (member == null)
            {
                return NotFound(CommonResponse<MemberDto>.Fail("Member not found", null!));
            }
            var memberDto = _mapper.Map<MemberDto>(member);
            var response = CommonResponse<MemberDto>.Success(memberDto);
            return Ok(response);
        }

}
