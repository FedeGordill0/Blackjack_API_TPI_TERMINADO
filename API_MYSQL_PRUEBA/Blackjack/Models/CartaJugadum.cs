using System;
using System.Collections.Generic;

#nullable disable

namespace Blackjack.Models
{
    public partial class CartaJugadum
    {
        public int IdCartaJugada { get; set; }
        public int IdCarta { get; set; }
        public int IdPartida { get; set; }
        public string Jugador { get; set; }

        public virtual Cartum IdCartaNavigation { get; set; }
        public virtual Partidum IdPartidaNavigation { get; set; }
    }
}
