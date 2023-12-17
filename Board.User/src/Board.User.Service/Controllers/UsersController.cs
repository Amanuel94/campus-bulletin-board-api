using AutoMapper;
using Board.Common.Interfaces;
using Board.User.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Board.User.Services.Controller;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly IGenericRepository<Models.User> _userRepository;

    public UsersController(IMapper mapper, IGenericRepository<Models.User> userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GeneralUserDto>>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(_mapper.Map<IReadOnlyCollection<GeneralUserDto>>(users));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GeneralUserDto>> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GeneralUserDto>(user));
    }




    [HttpPost]
    public async Task<ActionResult<GeneralUserDto>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
    {
        var user = _mapper.Map<Models.User>(createUserDto);
        await _userRepository.CreateAsync(user);
        var userDto = _mapper.Map<GeneralUserDto>(user);
        return CreatedAtAction(nameof(GetUserAsync), new { id = userDto.Id }, userDto);
    }
}