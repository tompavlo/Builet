using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Builet.User;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        

        try
        {
            var userDto = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchUser([FromQuery] string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return BadRequest("Identifier cannot be empty.");
        }
        
        try
        {
            var userDto = await _userService.GetUserByIdentifierAsync(identifier);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}