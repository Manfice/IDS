using System;

namespace IndDev.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage);
    }
}