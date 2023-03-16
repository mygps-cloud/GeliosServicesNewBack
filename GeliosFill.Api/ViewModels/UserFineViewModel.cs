using System.Linq.Expressions;
using GeliosFill.Models;

namespace GeliosFill.Api.ViewModels;

public static class UserFineViewModel
{
    public static readonly Func<ReceivedSms, object> Create = Projection.Compile();

    private static Expression<Func<ReceivedSms, object>> Projection =>
        receivedSms => new
        {
            receivedSms.Sender,
            receivedSms.ReceivedDate,
            receivedSms.Text,
            receivedSms.CarNumber,
            receivedSms.Article,
            receivedSms.Street,
            receivedSms.DateOfFine,
            receivedSms.ReceiptNumber,
            receivedSms.Amount,
            receivedSms.Term,
            receivedSms.LastDateOfPayment,
            receivedSms.FineStatus
        };
}