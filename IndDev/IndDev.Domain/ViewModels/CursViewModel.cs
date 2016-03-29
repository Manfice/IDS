using System;
using System.Collections.Generic;
using IndDev.Domain.Context;

namespace IndDev.Domain.ViewModels
{
    public class CursViewModel
    {
        public DateTime CursDate { get; set; }
        public IEnumerable<Curency> Curses { get; set; }  
    }
}