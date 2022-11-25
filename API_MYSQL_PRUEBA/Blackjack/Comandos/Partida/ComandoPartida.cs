namespace Blackjack.Comandos.Partida;

public class ComandoPartida
{
    public int idJugada { get; set; }
    public int puntosCroupier { get; set; }
    public int puntosJugador { get; set; }
    public short estado { get; set; }
    public string resultado { get; set; }
}
