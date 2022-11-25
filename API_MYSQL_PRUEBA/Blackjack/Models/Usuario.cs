using System;
using System.Collections.Generic;

#nullable disable

namespace Blackjack.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Jugada = new HashSet<Jugadum>();
        }

        public int Id { get; set; }
        public string Usuario1 { get; set; }
        public string Clave { get; set; }

        public virtual ICollection<Jugadum> Jugada { get; set; }
    }
}
