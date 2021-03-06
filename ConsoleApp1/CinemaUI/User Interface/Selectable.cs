using System;
using System.Collections.Generic;
using System.Text;
using CinemaUI.Builder;

namespace CinemaUI
{
    public class Selectable
    {
        public bool Selected { get; set; } = false;
        public bool Disabled { get; set; } = false;

        public virtual void Unselect() => Selected = false;
        public virtual void Select() => Selected = true;
        public virtual void KeyResponse(ConsoleKeyInfo keyPressed) { }
    }
}