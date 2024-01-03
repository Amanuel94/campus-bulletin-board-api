using AutoMapper;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using Board.User.Service.DTOs;
using Board.User.Service.DTOs.Validators;
using Board.User.Service.Jwt;
using Board.User.Service.Jwt.Interfaces;
using Board.User.Service.PasswordService.Interfaces;
using Board.User.Service.Response;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Board.User.Service.Controller;


[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{

    private readonly IMapper _mapper;
    private readonly IGenericRepository<Models.User> _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPublishEndpoint _publishEndpoint;

    public UsersController(IMapper mapper, IGenericRepository<Models.User> userRepository, IJwtService jwtService, IPasswordHasher passwordHasher, IPublishEndpoint publishEndpoint)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<CommonResponse<IReadOnlyCollection<GeneralUserDto>>>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(CommonResponse<IReadOnlyCollection<GeneralUserDto>>.Success(_mapper.Map<IReadOnlyCollection<GeneralUserDto>>(users)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommonResponse<GeneralUserDto>>> GetUserAsync(Guid id)
    {

        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(CommonResponse<GeneralUserDto>.Success(_mapper.Map<GeneralUserDto>(user)));
    }

    [HttpPost("register")]
    public async Task<ActionResult<GeneralUserDto>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
    {
        createUserDto.CreatedDate = DateTime.UtcNow;
        createUserDto.ModifiedDate = DateTime.UtcNow;
        var validator = new CreateUserDtoValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(createUserDto);
        if (validationResult.IsValid == false)
        {
            return BadRequest(CommonResponse<GeneralUserDto>.Fail("Check your inputs", validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
        }

        createUserDto.PasswordHash = _passwordHasher.HashPassword(createUserDto.PasswordHash);
        var user = _mapper.Map<Models.User>(createUserDto);
        await _userRepository.CreateAsync(user);
        var userDto = _mapper.Map<GeneralUserDto>(user);
        var userCreated = _mapper.Map<UserCreated>(user);
        await _publishEndpoint.Publish(userCreated);
        return CreatedAtAction(nameof(GetUserAsync), new { id = userDto.Id }, userDto);
    }

    [Authorize]
    [HttpPut()]
    public async Task<ActionResult<CommonResponse<GeneralUserDto>>> UpdateUserAsync([FromBody] UpdateUserDto updateUserDto)
    {
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);
        var id = identityProvider.GetUserId();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound(CommonResponse<GeneralUserDto>.Fail("User not found", new List<string>()));
        }
        updateUserDto.Id = id;
        updateUserDto.ModifiedDate = DateTime.UtcNow;
        var validator = new UpdateUserDtoValidator(_userRepository);
        var validationResult = await validator.ValidateAsync(updateUserDto);
        if (validationResult.IsValid == false)
        {
            return BadRequest(CommonResponse<GeneralUserDto>.Fail("Check your inputs", validationResult.Errors.Select(x => x.ErrorMessage).ToList()));
        }
        _mapper.Map(updateUserDto, user);
        user.ModifiedDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        await _publishEndpoint.Publish(_mapper.Map<UserUpdated>(user));

        return Ok(CommonResponse<GeneralUserDto>.Success(_mapper.Map<GeneralUserDto>(user)));
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
        await _publishEndpoint.Publish(_mapper.Map<UserDeleted>(user));
        return NoContent();
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<CommonResponse<LoginResponseDto>>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var user = await _userRepository.GetAsync(x => x.UserName == loginRequestDto.UserName);
        if (user == null)
        {
            return NotFound(CommonResponse<LoginResponseDto>.Fail("User not found", new List<string>()));
        }
        if (_passwordHasher.VerifyPassword(loginRequestDto.Password, user.PasswordHash) == false)
        {
            return Unauthorized(CommonResponse<LoginResponseDto>.Fail("Password is not correct", new List<string>()));
        }
        var loginResponseDto = new LoginResponseDto()
        {
            RefreshToken = Guid.NewGuid().ToString(),
            Token = _jwtService.GenerateToken(user)
        };
        return Ok(CommonResponse<LoginResponseDto>.Success(loginResponseDto));
    }

    [HttpPut]
    [Route("change-password")]
    public async Task<ActionResult<CommonResponse<int>>> ChangePasswordAsync(UpdatePasswordDto updatePasswordDto)
    {
        var identityProvider = new IdentityProvider(HttpContext, _jwtService);
        var id = identityProvider.GetUserId();
        var user = await _userRepository.GetAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        if (_passwordHasher.VerifyPassword(updatePasswordDto.CurrentPassword, user.PasswordHash) == false)
        {
            return Unauthorized(CommonResponse<int>.Fail("Current password is not correct", new List<string>()));
        }
        user.PasswordHash = _passwordHasher.HashPassword(updatePasswordDto.NewPassword);
        user.ModifiedDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        return Ok(CommonResponse<GeneralUserDto>.Success(_mapper.Map<GeneralUserDto>(user)));
    }
}