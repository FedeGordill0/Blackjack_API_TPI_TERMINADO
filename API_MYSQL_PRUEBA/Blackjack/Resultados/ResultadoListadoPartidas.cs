namespace Blackjack.Resultados;

public class ResultadoListadoPartidas
{
    public List<RdoPartida2> listaPartida { get; set; } = new List<RdoPartida2>();

}

public class RdoPartida2
{
    public int idPartida { get; set; }
    public int IdJugada { get; set; }
    public int PuntosJugador { get; set; }
    public int PuntosCroupier { get; set; }
    public int Estado { get; set; }

    public string? Resultado { get; set; }


}