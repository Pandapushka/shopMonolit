namespace shop.Seed
{
    public static class OrderStatus
    {
        public const string Accepted = "Принят в работу";
        public const string Payment = "Олачен";
        public const string Packed = "Собран";
        public const string Sent = "Отправлен";
        public const string Completed = "Выполнен";
        public const string Cancelled = "Отменен";

        public static IReadOnlyList<string> AllStatus
        {
            get => new List<string> { Accepted, Payment, Packed, Sent, Completed, Cancelled };
        }


    }
}
