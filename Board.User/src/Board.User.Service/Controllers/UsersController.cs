using AutoMapper;
using Board.Common.Interfaces;
using Board.User.Services.DTOs;
using Board.User.Services.Jwt;
using Board.User.Services.Jwt.Interfaces;
using Board.User.Services.PasswordService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.User.Services.Controller;


[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly IGenericRepository<Models.User> _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public UsersController(IMapper mapper, IGenericRepository<Models.User> userRepository, IJwtService jwtService, IPasswordHasher passwordHasher)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
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

    [HttpPost("register")]
    public async Task<ActionResult<GeneralUserDto>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
    {
        createUserDto.PasswordHash = _passwordHasher.HashPassword(createUserDto.PasswordHash);
        createUserDto.CreatedDate = DateTime.UtcNow;
        createUserDto.ModifiedDate = DateTime.UtcNow;
        var user = _mapper.Map<Models.User>(createUserDto);
        await _userRepository.CreateAsync(user);
        var userDto = _mapper.Map<GeneralUserDto>(user);
        return CreatedAtAction(nameof(GetUserAsync), new { id = userDto.Id }, userDto);
    }

    [Authorize]
    [HttpPut()]
    public async Task<ActionResult<GeneralUserDto>> UpdateUserAsync([FromBody] UpdateUserDto updateUserDto)
    {
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);
        var id = identityProvider.GetUserId();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        user.ModifiedDate = DateTime.UtcNow;
        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateAsync(user);
        return Ok(_mapper.Map<GeneralUserDto>(user));
    }


    [Authorize]
    [HttpDelete()]
    public async Task<ActionResult> DeleteUserAsync()
    {
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);
        var id = identityProvider.GetUserId();

        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound("User not found");
        }
        await _userRepository.RemoveAsync(user);
        return NoContent();
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.GetAsync(x => x.UserName == loginRequestDto.UserName);
        if (user == null)
        {
            return NotFound();
        }
        if (_passwordHasher.VerifyPassword(loginRequestDto.Password, user.PasswordHash) == false)
        {
            return Unauthorized();
        }
        var loginResponseDto = new LoginResponseDto()
        {
            RefreshToken = Guid.NewGuid().ToString(),
            Token = _jwtService.GenerateToken(user)
        };
        return Ok(loginResponseDto);
    }
}