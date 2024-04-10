using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCoreOAuthEmpleados.Helpers
{
    public class HelperActionServicesOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperActionServicesOAuth(IConfiguration configuration)
        {
            this.Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");
        }

        //necesitamos un metodo para generar el token que se basa en el token key
        public SymmetricSecurityKey GetKeyToken()
        {
            //convertimos el secret key a bytes[]
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            //devolvemos la key generada mediante los butes[]
            return new SymmetricSecurityKey(data);
        }

        //hemos creado esta clase para quitar codigo dentro de program en los services


        //metodo para la configuracion de la validacion del token
        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                //indicamos que deseamos invalidad de nuestro token, issuer, audience, time
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer=true,
                    ValidateAudience = true,
                    ValidateLifetime= true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer= this.Issuer,
                    ValidAudience=this.Audience,
                    IssuerSigningKey= this.GetKeyToken()
                };
            });

            return options;
        }

        //metodo para indicar el esquema de la validacion

        public Action<AuthenticationOptions> GetAuthenticateSchema()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}
