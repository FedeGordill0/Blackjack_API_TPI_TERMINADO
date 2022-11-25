using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blackjack.Comandos.Jugada;
using Blackjack.Resultados;
using Blackjack.Models;
using Blackjack.Comandos.Partida;

namespace Blackjack.Controllers;

public class JugadaController : ControllerBase
{
    private readonly blackjack_tpi_finalContext _context;

    public JugadaController(blackjack_tpi_finalContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/jugada/listaJugada/{id_usuario}")]
    public async Task<ActionResult<ResultadoJugadas>> listaJugadas(int id_usuario)
    {
        try
        {
            var resultado = new ResultadoJugadas();
            var jugadas = await _context.Jugada.Where(j => j.IdUsuario.Equals(id_usuario) && j.Estado.Equals(0)).
            Include(c => c.IdUsuarioNavigation).OrderByDescending(j => j.IdJugada).ToListAsync();
            if (jugadas != null)
            {
                foreach (var jugaga in jugadas)
                {
                    var resultAux = new ResultadoJuego()
                    {
                        id_jugada = jugaga.IdJugada,
                        usuario = jugaga.IdUsuarioNavigation.Usuario1
                    };
                    resultado.listaJugadas.Add(resultAux);
                }
                return Ok(resultado);
            }
            else
            {
                return Ok(resultado);
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/jugada/nuevaJugada/{id_usuario}")]
    public async Task<ActionResult<ResultadoJuego>> nuevaJugada([FromBody] ComandoJugada cmd, int id_usuario)
    {
        try
        {
            var usuario = await _context.Usuarios.Where(u => u.Id.Equals(id_usuario)).FirstOrDefaultAsync();
            var resultado = new ResultadoJuego();

            if (usuario != null)
            {
                var jugada = new Jugadum()
                {
                    IdUsuario = usuario.Id,
                    Estado = 0
                };
                await _context.AddAsync(jugada);
                _context.SaveChanges();

                resultado.id_jugada = jugada.IdJugada;
                resultado.usuario = jugada.IdUsuarioNavigation.Usuario1;

                return (resultado);
            }
            else
            {
                return Ok();
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpGet]
    [Route("api/jugada/continuarJugada/{id_jugada}+{id_usuario}")]
    public async Task<ActionResult<ResultadoJugadaContinuar>> continuar(int id_jugada, int id_usuario)
    {
        try
        {
            var resultado = new ResultadoJugadaContinuar();
            var partida = await _context.Partida.Where(p => p.IdJugada.Equals(id_jugada) && p.Estado.Equals(0) &&
            p.IdJugadaNavigation.IdUsuario.Equals(id_usuario) && p.IdJugadaNavigation.Estado.Equals(0)).
            OrderBy(p => p.IdPartida).LastOrDefaultAsync();

            if (partida != null)
            {
                var cartaJugada = await _context.CartaJugada.Where(c => c.IdPartida.Equals(partida.IdPartida)).
                Include(p => p.IdCartaNavigation).ToListAsync();

                if (cartaJugada != null)
                {
                    foreach (var c in cartaJugada)
                    {
                        var res = new ResultadoCartaUnica()
                        {
                            id = c.IdCarta,
                            carta = c.IdCartaNavigation.Carta,
                            valor = c.IdCartaNavigation.Valor,
                            imagen = c.IdCartaNavigation.Imagen,
                        };
                        resultado.cartasJugadas.Add(res);
                    }
                    resultado.idJugada = partida.IdPartida;
                    resultado.puntosJugador = partida.PuntosJugador;
                    resultado.puntosCroupier = partida.PuntosCroupier;
                }
                else
                {
                    return Ok("No hay partidas disponibles");
                }
            }
            else
            {
                return Ok("No hay jugadas disponibles");
            }
            return Ok(resultado);
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPut]
    [Route("api/jugada/terminar/{id_jugada}+{id_usuario}")]
    public async Task<ActionResult<bool>> finalizarJugada(int id_jugada, int id_usuario)
    {
        try
        {
            var jugada = await _context.Jugada.Where(j => j.IdJugada.Equals(id_jugada) && j.IdUsuario.Equals(id_usuario)).
            OrderBy(j => j.IdJugada).LastOrDefaultAsync();

            if (jugada != null && jugada.Estado.Equals(0))
            {
                jugada.Estado = 1;

                _context.Update(jugada);
                await _context.SaveChangesAsync();
            }
            return Ok(true);
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpGet]
    [Route("api/jugada/buscarPartida/{id_jugada}+{id_usuario}")]
    public async Task<ActionResult<ResultadoPartida>> getPartida(int id_jugada, int id_usuario)
    {
        try
        {
            var resultado = new ResultadoPartida();
            var partida = await _context.Partida.Where(p => p.IdJugada.Equals(id_jugada) && p.Estado.Equals(0) &&
            p.IdJugadaNavigation.IdUsuario.Equals(id_usuario) && p.IdJugadaNavigation.Estado.Equals(0)).FirstOrDefaultAsync();

            if (partida != null)
            {
                resultado.idPartida = partida.IdPartida;
                if (partida.Estado.Equals(0))
                {

                    var nueva = new Partidum()
                    {
                        IdJugada = id_jugada,
                        PuntosCroupier = 0,
                        PuntosJugador = 0,
                        Estado = 0,
                        Resultado = ""
                    };
                    await _context.AddAsync(nueva);

                    _context.SaveChanges();
                    resultado.idPartida = nueva.IdPartida;
                }
            }
            return Ok(resultado);
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpGet]
    [Route("api/jugada/getJugadas/{id_usuario}")]
    public async Task<ActionResult<ResultadoJugadas>> getJugadas(int id_usuario)
    {
        try
        {
            var resultado = new ResultadoJugadas();
            var jugadas = await _context.Jugada.Where(j => j.IdUsuario.Equals(id_usuario)).
            Include(c => c.IdUsuarioNavigation).OrderByDescending(j => j.IdJugada).ToListAsync();
            if (jugadas != null)
            {
                foreach (var jugaga in jugadas)
                {
                    var resultado2 = new ResultadoJuego()
                    {
                        id_jugada = jugaga.IdJugada,
                        usuario = jugaga.IdUsuarioNavigation.Usuario1
                    };
                    resultado.listaJugadas.Add(resultado2);
                }
                return Ok(resultado);
            }
            else
            {
                return Ok(resultado);
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}
