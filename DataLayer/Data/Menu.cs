﻿using System;

namespace DataLayer.Data
{
    public class Menu
    {
        public int IdMenu { get; set; }
        public DateTime ForDate { get; set; }
        public MenuItem[] Items { get; set; } = new MenuItem[3];
        public float Price { get; set; }
        public ServingPlace ServingPlace{get;set;}
	}
}