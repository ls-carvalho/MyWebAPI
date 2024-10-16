﻿using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }

    public ICollection<AccountProduct> Accounts { get; set; } = new List<AccountProduct>();
    public ICollection<Addon> Addons { get; set; } = new List<Addon>();
}
