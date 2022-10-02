using System;
using System.Collections.Generic;

namespace TZ.Models
{
    public partial class Unit
    {
        public Unit()
        {
            InverseParentnameNavigation = new HashSet<Unit>();
        }

        public string Name { get; set; } = null!;
        public bool Status { get; set; }

        public string? Parentname { get; set; }


        // Данные родителя
        public virtual Unit? ParentnameNavigation { get; set; }

        // Данные потомков
        public virtual ICollection<Unit> InverseParentnameNavigation { get; set; }
    }
}
