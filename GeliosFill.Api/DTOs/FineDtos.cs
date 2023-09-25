using GeliosFill.Models;

namespace GeliosFill.Api.DTOs;

public class FineDtos
{
    public string? Sender { get; set; }
    public DateTimeOffset? ReceivedDate { get; set; }
    public string? Text { get; set; }
    public string? CarNumber { get; set; }
    public string? Article { get; set; }
    public string? Street { get; set; }
    public DateTimeOffset? DateOfFine { get; set; }
    public string? ReceiptNumber { get; set; }
    public int? Amount { get; set; }
    public int? Term { get; set; }
    public DateTimeOffset? LastDateOfPayment { get; set; }
    public FineStatus FineStatus { get; set; }
}