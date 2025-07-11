﻿using System.ComponentModel.DataAnnotations;

namespace Builet.Transaction;

public class TransactionDto
{
    [Required(ErrorMessage = "Transaction Id is required to perform a purchase.")]
    public long Id { get; set; }

    public Guid SellerId { get; set; }
    
    public string SellerUsername { get; set; } = string.Empty;

    public Guid? BuyerId { get; set; }
    
    public string? BuyerUsername { get; set; }

    public long StockId { get; set; }

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public TransactionStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}