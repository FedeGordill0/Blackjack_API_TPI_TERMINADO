using System;
using System.Collections.Generic;

#nullable disable

namespace Blackjack.Models
{
    public partial class Partidum
    {
        public Partidum()
        {
            CartaJugada = new HashSet<CartaJugadum>();
        }

        public int IdPartida { get; set; }
        public int IdJugada { get; set; }
        public int PuntosJugador { get; set; }
        public int PuntosCroupier { get; set; }
        public short Estado { get; set; }
        public string Resultado { get; set; }

        public virtual Jugadum IdJugadaNavigation { get; set; }
        public virtual ICollection<CartaJugadum> CartaJugada { get; set; }
    }
}
