using System;
using System.Collections.Generic;

#nullable disable

namespace Blackjack.Models
{
    public partial class Jugadum
    {
        public Jugadum()
        {
            Partida = new HashSet<Partidum>();
        }

        public int IdJugada { get; set; }
        public int IdUsuario { get; set; }
        public short Estado { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Partidum> Partida { get; set; }
    }
}
