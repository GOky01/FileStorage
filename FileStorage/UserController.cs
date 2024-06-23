using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserController(UserDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        if (_context.Users.Any(u => u.Username == userDto.Username))
        {
            return BadRequest("User already exists.");
        }

        var user = new User(userDto.Username, userDto.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
        UserManager.SetCurrentUser(user);  // Set current user upon registration
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == userDto.Username);
        if (user == null || !user.ValidatePassword(userDto.Password))
        {
            return Unauthorized("Invalid username or password.");
        }

        UserManager.SetCurrentUser(user); 
        return Ok(new { message = "Login successful." });
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    [HttpPost("upload")]
    public IActionResult UploadFile([FromBody] FileDto fileDto)
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            return Unauthorized("You must be logged in to upload a file.");
        }

        try
        {
            string encryptedFilePath = FileSecurity.EncryptFile(fileDto.FilePath);
            currentUser.AddFile(encryptedFilePath);
            _context.Users.Update(currentUser);
            _context.SaveChanges();

            return Ok($"File encrypted and saved to: {encryptedFilePath}");
        }
        catch (FileNotFoundException)
        {
            return NotFound($"File not found: {fileDto.FilePath}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("download")]
    public IActionResult DownloadFile([FromBody] FileDto fileDto)
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            return Unauthorized("You must be logged in to download a file.");
        }

        if (!currentUser.Files.Contains(fileDto.FilePath))
        {
            return Unauthorized("You do not have access to this file.");
        }

        try
        {
            string decryptedFilePath = FileSecurity.DecryptFile(fileDto.FilePath);
            return Ok($"File decrypted and saved to: {decryptedFilePath}");
        }
        catch (FileNotFoundException)
        {
            return NotFound($"File not found: {fileDto.FilePath}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("files")]
    public IActionResult ViewFiles()
    {
        var currentUser = UserManager.GetCurrentUser();
        if (currentUser == null)
        {
            return Unauthorized("You must be logged in to view files.");
        }

        return Ok(currentUser.Files);
    }
}

public class UserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class FileDto
{
    public string FilePath { get; set; }
}

