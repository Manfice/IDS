﻿using System.Collections.Generic;
using IndDev.Domain.Entity.Products;

namespace IndDev.Domain.Entity.Menu
{
    public class ProductMenu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public int Priority { get; set; }
        public virtual ProdMenuImage Image { get; set; }    
        public virtual ICollection<ProductMenuItem> MenuItems { get; set; }
    }

    public class ProductMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public bool IsRus { get; set; }
        public virtual ProdMenuImage Image { get; set; }
        public virtual ProductMenu ProductMenu { get; set; }
        public ICollection<Product> Products { get; set; }  
    }

    public class ProdMenuImage
    {
        public int Id { get; set; }
        public string AltText { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
    }
    public class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasChild { get; set; }
        public virtual Menu ParentItem { get; set; }
    }
}