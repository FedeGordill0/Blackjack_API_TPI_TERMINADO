using Blackjack.Comandos.Carta;
using Blackjack.Models;
using Blackjack.Resultados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Controllers;

public class CartaController : ControllerBase
{
    private readonly blackjack_tpi_finalContext _context;

    public CartaController(blackjack_tpi_finalContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("api/mazo/cartas")]
    public async Task<ActionResult<ResultadoMazo>> getCartas()
    {
        try
        {
            var resultado = new ResultadoMazo();
            var cartas = await _context.Carta.ToListAsync();

            if (cartas != null)
            {
                foreach (var carta in cartas)
                {
                    var resultadoCarta = new ResultadoCarta
                    {
                        valor = carta.Valor,
                        id = carta.IdCarta,
                        carta = carta.Carta,
                        imagen = carta.Imagen,
                        isAs = carta.IsAs
                    };
                    resultado.mazo.Add(resultadoCarta);

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

    [HttpGet]
    [Route("api/mazo/getCartaID/{idCarta}")]
    public async Task<ActionResult<ResultadoCartaUnica>> getCartaID(int idCarta)
    {
        try
        {
            var resultado = new ResultadoCartaUnica();
            var carta = await _context.Carta.Where(c => c.IdCarta.Equals(idCarta)).FirstOrDefaultAsync();

            if (carta != null)
            {
                resultado.valor = carta.Valor;
                resultado.id = carta.IdCarta;
                resultado.carta = carta.Carta;
                resultado.imagen = carta.Imagen;
            }
            return resultado;
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }

    [HttpPost]
    [Route("api/cartaJugada/nuevaCarta/{id_partida}+{id_jugada}")]
    public async Task<ActionResult<bool>> cartaJugada([FromBody] ComandoCartaJugada cmd, int id_partida, int id_jugada)
    {
        try
        {
            var partida = await _context.Partida.Where(p => p.IdPartida.Equals(id_partida) && p.IdJugada.Equals(id_jugada)).
            Include(j => j.IdJugadaNavigation).OrderBy(p => p.IdPartida).LastOrDefaultAsync();

            if (partida != null && partida.Estado.Equals(0))
            {
                var cartaJugada = new CartaJugadum()
                {
                    IdCarta = cmd.idCarta,
                    IdPartida = id_partida,
                    Jugador = cmd.jugador,
                };

                await _context.AddAsync(cartaJugada);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
        catch (Exception)
        {
            return BadRequest("No se puede realizar esta acción");
        }
    }
}










