using System.Linq.Expressions;
using GeliosFill.Api.DTOs;
using GeliosFill.Models;

namespace GeliosFill.Api.ViewModels;

public static class UserFineViewModel
{ 
    public static readonly Func<ReceivedSms, FineDtos> Create = Projection.Compile();

    private static Expression<Func<ReceivedSms, FineDtos>> Projection =>
        receivedSms => new FineDtos
        {
            Sender =receivedSms.Sender
            , ReceivedDate =receivedSms.ReceivedDate
            , Text= receivedSms.Text
            , CarNumber =receivedSms.CarNumber
            , Article =receivedSms.Article
            , Street  =receivedSms.Street
            , DateOfFine =receivedSms.DateOfFine
            , ReceiptNumber =receivedSms.ReceiptNumber
            , Amount =receivedSms.Amount
            , Term =receivedSms.Term
            , LastDateOfPayment =receivedSms.LastDateOfPayment
            , FineStatus =receivedSms.FineStatus
        };
}