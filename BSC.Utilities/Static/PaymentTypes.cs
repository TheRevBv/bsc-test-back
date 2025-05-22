namespace BSC.Utilities.Static
{
    public enum PaymentTypes
    {
        Cash = 1,
        // Efectivo
        CreditCard = 2,
        // Tarjeta de crédito
        DebitCard = 3,
        // Tarjeta de débito
        BankTransfer = 4,
        // Transferencia bancaria
        OnlinePayment = 5,
        // Pago en línea (por ejemplo, PayPal, Stripe, etc.)
        Other = 6
        // Otro tipo de pago no especificado
    }
}

