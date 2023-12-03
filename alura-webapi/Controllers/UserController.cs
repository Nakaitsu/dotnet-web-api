using alura_webapi.Models;
using alura_webapi.Models.DTO;
using alura_webapi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace alura_webapi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IRepository<User> _UserRepository;
    private readonly IConfiguration _Configs;

    public UserController(IRepository<User> repoService, IConfiguration configuration)
    {
      _UserRepository = repoService;
      _Configs = configuration;
    }

    [HttpGet(nameof(IsAllowed))]
    public IActionResult IsAllowed()
    {
      try 
      {
        if(HttpContext.Request.Headers.Authorization.IsNullOrEmpty())
          return Unauthorized(new {Message = "Acesso não autenticado!"});

        if(ValidateAccessToken(HttpContext.Request.Headers.Authorization[0])) 
          return Ok(new {Message = "Acesso autenticado!"});
        else
          return Unauthorized(new {Message = "Token inválido!"});
      }
      catch(Exception)
      {
        return Unauthorized(new {Message = "Esse token não é mais válido!"});
      }
    }

    [HttpPost(nameof(SignIn))]
    public IActionResult SignIn([FromBody] UserDTO data)
    {
      try
      {
        if (data.Username.IsNullOrEmpty() || data.Password.IsNullOrEmpty())
          return BadRequest(new { Errors = "Usuários e senhas inválidos!" });

        User user = _UserRepository.Items.SingleOrDefault(u => u.Username == data.Username);

        if (user != null && user.VerifyPassword(data.Password))
        {
          string authToken = CreateAuthToken();

          return Ok(new
          {
            Message = "Autenticado!",
            AccessToken = authToken
          });
        }
        else
        {
          return BadRequest(new { Errors = "Credenciais Inválidas!" });
        }
      }
      catch (Exception ex)
      {
        return BadRequest(new { Errors = "Não foi possível processar sua solicitação. " + ex.Message });
      }
    }

    [HttpPost(nameof(SignUp))]
    public IActionResult SignUp([FromBody] UserDTO data)
    {
      if(data.Email != null) {
        User user = new User
        {
          Username = data.Username,
          Email = data.Email,
          Password = Models.User.HashPassword(data.Password)
        };

        if (_UserRepository.ValidateEntity(user))
        {
          _UserRepository.Create(user);

          return Ok(new
          {
            Message = "Usuário criado com sucesso!",
            User = new { user.Username, user.Email }
          });
        }
        else
        {
          return BadRequest(new { Errors = "Email já está em uso!" });
        }
      }
      else
      {
        return BadRequest(new { Errors = "Credenciais inválidas!" });
      }
    }

    /// <summary>
    /// Criar um token de identificação para usuários autenticado
    /// </summary>
    private string CreateAuthToken()
    {
      string issuer = _Configs["Jwt:Issuer"];
      string audience = _Configs["Jwt:Audience"];
      DateTime expiryDate = DateTime.Now.AddDays(1);
      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configs["Jwt:Key"]));
      SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(issuer: issuer, audience: audience,
        expires: expiryDate, signingCredentials: credentials);
      string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

      return generatedToken;
    }

    private bool ValidateAccessToken(string token) 
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      if(tokenHandler.CanReadToken(token))
      {
        TokenValidationParameters validationParameters = new TokenValidationParameters{
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configs["Jwt:Key"])),
          ValidateIssuer = true,
          ValidIssuer = _Configs["Jwt:Issuer"],
          ValidateAudience = true,
          ValidAudience = _Configs["Jwt:Audience"],
          ValidateLifetime = true,
          ClockSkew = TimeSpan.Zero,
        };

        tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        return true;
      }
      else
        return false;
    }
  }
}