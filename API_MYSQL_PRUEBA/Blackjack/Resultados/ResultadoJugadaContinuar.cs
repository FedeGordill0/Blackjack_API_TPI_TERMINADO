using Blackjack.Resultados;
namespace Blackjack.Resultados;

public class ResultadoJugadaContinuar
{
    public List<ResultadoCartaUnica> cartasJugadas { get; set; } = new List<ResultadoCartaUnica>();
    public int puntosJugador { get; set; }
    public int puntosCroupier { get; set; }
    public int idJugada { get; set; }
}
