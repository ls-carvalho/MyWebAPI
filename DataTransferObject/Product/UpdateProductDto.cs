﻿namespace MyWebAPI.DataTransferObject.Product;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
