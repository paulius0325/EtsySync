﻿namespace SharedProject.Dtos
{
    public class InvoiceItemDto
    {
        public int ItemNumber { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
