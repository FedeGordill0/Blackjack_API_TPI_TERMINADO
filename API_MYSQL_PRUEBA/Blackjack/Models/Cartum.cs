using System;
using System.Collections.Generic;

#nullable disable

namespace Blackjack.Models
{
    public partial class Cartum
    {
        public Cartum()
        {
            CartaJugada = new HashSet<CartaJugadum>();
        }

        public int IdCarta { get; set; }
        public int Valor { get; set; }
        public string Carta { get; set; }
        public string Imagen { get; set; }
        public short IsAs { get; set; }

        public virtual ICollection<CartaJugadum> CartaJugada { get; set; }
    }
}
