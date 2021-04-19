using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public class Selectable
    {
        public bool Selected { get; protected set; } = false;

        public virtual void Unselect() => Selected = false;
        public virtual void Select() => Selected = true;

    }
}