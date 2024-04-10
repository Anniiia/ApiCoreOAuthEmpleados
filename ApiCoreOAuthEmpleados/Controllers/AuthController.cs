using ApiCoreOAuthEmpleados.Helpers;
using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiCoreOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;
        //cuando generemos el token, debemos de integrar dentro de dicho token,issuer, audience..oara que lo valide cuando nos lo envien
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryEmpleados repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //necesitamos un metodo post para validar el usuario y que recibira login model
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Empleado empleado = await this.repo.LoginEmpleadoAsync(model.UserName, int.Parse(model.Password));

            if (empleado == null)
            {
                return Unauthorized();
            }
            else 
            {
                //debemos crear unas credenciales para incluirlas dentro del token y que estaran compuestas por el secretKey cifrado y el tipo de cifrado que deseeos incluir en el token
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                //el token se genera con una clase y debemosindicar los elementos que almacenara dentro de dicho token, por ejemplo, issuer, audience o el tiempo de validacion del token

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer:this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    notBefore: DateTime.UtcNow
                    );

                //devolvemos una respuesta afirmativa con un objeto anonimo en formato json
                return Ok(new
                {
                    response=new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

        }
    }
}
