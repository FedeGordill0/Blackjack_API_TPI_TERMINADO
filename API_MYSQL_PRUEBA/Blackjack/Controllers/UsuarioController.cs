using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blackjack.Comandos.Usuario;
using Blackjack.Resultados;
using Blackjack.Models;

namespace Blackjack.Controllers;

public class UsuarioController : ControllerBase
{
    private readonly blackjack_tpi_finalContext _context;

    public UsuarioController(blackjack_tpi_finalContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/usuario/login")]
    public async Task<ActionResult<ResultadoUsuarioLogin>> loginUsuario([FromBody] ComandoUsuario cmd)
    {
        try
        {
            var resultado = new ResultadoUsuarioLogin();
            var usuario = await _context.Usuarios.Where(c => c.Usuario1.Equals(cmd.Usuario) &&
             c.Clave.Equals(cmd.Clave)).FirstOrDefaultAsync();

            if (User != null)
            {
                resultado.usuario = usuario.Usuario1;
                resultado.StatusCode = 200;
                resultado.Id = usuario.Id;
                return Ok(resultado);
            }
            else
            {
                resultado.setError("Usuario no encontrado");
                resultado.StatusCode = 400;
                return Ok(resultado);
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/usuario/registrar")]
    public async Task<ActionResult<ResultadoUsuarioLogin>> registrarUsuario([FromBody] ComandoUsuario cmd)
    {
        try
        {
            var resultado = new ResultadoUsuarioLogin();

            var jugador = new Usuario()
            {
                Usuario1 = cmd.Usuario,
                Clave = cmd.Clave
            };

            await _context.AddAsync(jugador);
            await _context.SaveChangesAsync();


            resultado.Id = jugador.Id;
            resultado.usuario = jugador.Usuario1;

            return resultado;
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }


    [HttpGet]
    [Route("api/usuario/getUsuarioID/{id}")]
    public async Task<ActionResult<ResultadoUsuario>> getUsuarioID(int id)
    {
        try
        {
            var resultado = new ResultadoUsuario();
            var usuario = await _context.Usuarios.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();

            if (usuario != null)
            {
                resultado.Id = usuario.Id;
                resultado.nombre = usuario.Usuario1;
            }
            return resultado;
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }


    [HttpGet]
    [Route("api/usuario/getUsuarios")]
    public async Task<ActionResult<ResultadoUsuariosALL>> getUsuarios()
    {
        try
        {
            var resultadoALL = new ResultadoUsuariosALL();
            var usuarios = await _context.Usuarios.ToListAsync();

            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    var resultadoUsuario = new ResultadoUsuario
                    {
                        Id = usuario.Id,
                        nombre = usuario.Usuario1
                    };
                    resultadoALL.listadoUsuarios.Add(resultadoUsuario);
                }
                return Ok(resultadoALL);
            }
            else
            {
                return Ok(resultadoALL);
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}
