using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace WebAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ModelsContext _context;

        public AccountController(ModelsContext context)
        {
            _context = context;
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.HttpGet("verify")]
        public async Task<ActionResult<Card>> Verify()
        {
            Card card = _context.Cards.FirstOrDefault(e => e.Email == User.Claims.ToList()[0].Value);
            if (card == null)
                return Unauthorized();
            return card;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("updatetoken")]
        public async Task UpdateToken()
        {
            Card card = await _context.Cards.FirstOrDefaultAsync(e => e.Email == User.Claims.ToList()[0].Value);
            string passwordHash = _context.Cards.First(m => m.Email == card.Email).PasswordHash;
            await Token(new Card() { Email = card.Email, PasswordHash = passwordHash });
        }


        [Microsoft.AspNetCore.Mvc.HttpPost("token")]
        public async Task Token(Card card)
        {
            var existingCard = await _context.Cards.FirstOrDefaultAsync(e => e.Email == card.Email);
            bool isVerified = Hashing.VerifyHashedPassword(existingCard.PasswordHash, card.PasswordHash);

            if (!isVerified)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }
            var identity = GetIdentity(card.Email, existingCard.PasswordHash);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                email = identity.Name,
                cardId = existingCard.CardId,
                cardOwnerName = existingCard.CardOwnerName
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(string email, string passwordHash)
        {
            Card card = _context.Cards.FirstOrDefault(x => x.Email == email && x.PasswordHash == passwordHash);
            if (card != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, card.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, card.IsAdmin.ToString()),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}