namespace Blackjack.Resultados;

public class Respuesta
{
    public bool Ok { get; set; } = true;
    public string Error { get; set; }
    public int StatusCode { get; set; }

    public void setError(string mensajeError)
    {
        Ok = false;
        Error = mensajeError;
    }
}
