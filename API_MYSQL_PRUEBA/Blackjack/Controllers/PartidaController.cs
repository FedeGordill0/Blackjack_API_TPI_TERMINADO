using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blackjack.Comandos.Jugada;
using Blackjack.Resultados;
using Blackjack.Models;
using Blackjack.Comandos.Partida;


namespace Blackjack.Controllers;

public class PartidaController : ControllerBase
{
    private readonly blackjack_tpi_finalContext _context;

    public PartidaController(blackjack_tpi_finalContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("api/partida/nuevaPartida/{id_usuario}+{id_jugada}")]
    public async Task<ActionResult<ResultadoPartida>> nuevaPartida(int id_usuario, int id_jugada)
    {
        try
        {
            var jugada = await _context.Jugada.Where(j => j.IdJugada.Equals(id_jugada) && j.IdUsuario.Equals(id_usuario)
            && j.Estado.Equals(0)).OrderBy(j => j.IdJugada).LastOrDefaultAsync();

            var resultado = new ResultadoPartida();

            if (jugada != null)
            {
                var partida = new Partidum()
                {
                    IdJugada = id_jugada,
                    PuntosCroupier = 0,
                    PuntosJugador = 0,
                    Estado = 0,
                    Resultado = ""
                };
                await _context.AddAsync(partida);
                _context.SaveChanges();

                resultado.idPartida = partida.IdPartida;

                return resultado;
            }
            else
            {
                return resultado;
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPut]
    [Route("api/partida/plantarse/{id_usuario}+{id_partida}+{id_jugada}")]
    public async Task<ActionResult<bool>> actualizarPartida([FromBody] ComandoPartida cmd, int id_jugada, int id_usuario, int id_partida)
    {
        try
        {
            var partida = await _context.Partida.Where(p => p.IdPartida.Equals(id_partida) && p.IdJugada.Equals(id_jugada)
            && p.IdJugadaNavigation.IdUsuario.Equals(id_usuario)).FirstOrDefaultAsync();

            if (partida != null && partida.Estado.Equals(0))
            {
                partida.IdJugada = id_jugada;
                partida.PuntosCroupier = cmd.puntosCroupier;
                partida.PuntosJugador = cmd.puntosJugador;
                partida.Estado = cmd.estado;
                partida.Resultado = cmd.resultado;


                _context.Update(partida);
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
    [Route("api/partidas/listadoPartidas/{id_usuario}")]
    public async Task<ActionResult<ResultadoListadoPartidas>> getPartidas(int id_usuario)
    {
        try
        {
            var resultado = new ResultadoListadoPartidas();

            var partidas = await _context.Partida.Where(j => j.IdJugadaNavigation.IdUsuario.Equals(id_usuario) && j.Estado.Equals(0)).ToListAsync();
            if (partidas != null)
            {
                foreach (var partida in partidas)
                {
                    var resultadoPartida2 = new RdoPartida2()
                    {
                        idPartida = partida.IdPartida,
                        IdJugada = partida.IdJugada,
                        PuntosCroupier = partida.PuntosCroupier,
                        PuntosJugador = partida.PuntosJugador,
                        Estado = partida.Estado,
                        Resultado = partida.Resultado
                    };
                    resultado.listaPartida.Add(resultadoPartida2);
                }
                return Ok(resultado);
            }
            else
            {
                return Ok(resultado);
            }
        }
        catch (Exception e)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}
