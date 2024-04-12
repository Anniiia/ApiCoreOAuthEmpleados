using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCoreOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> GetEmpleados()
        {

            return await this.repo.GetEmpleadosAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> FindEmpleado(int id)
        {
            return await this.repo.FindEmpleadoAsync(id);
        }
        
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>> PerfilEmpleado()
        {
            //internamente, cuando recibimos el token, el usuairo es validado y almacena datos como HttpContext.User.Identity.IsAuthenticated. Como hemos incluido la Key de los claims, automaticamente tambien tenemos dichos claims como en las aplicaciones MCV
            Claim claim = HttpContext.User.FindFirst(x => x.Type == "userData");
            //recuperamos el json del empleado 
            string jsonEmpleado = claim.Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);

            return empleado;
        
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]

        public async Task<ActionResult<List<Empleado>>> CompisCurro()
        {
            string jsonEmpleado = HttpContext.User.FindFirst(x => x.Type == "userData").Value;

            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);

            List<Empleado> compis = await this.repo.GetCompisDepartamentoAsync(empleado.IdDepartamento); 

            return compis;
        }





    }
}
