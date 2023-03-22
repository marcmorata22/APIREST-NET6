namespace AutoresApi.DTOs
{
    public class PageDTO
    {
        public int Page { get; set; } = 1;
        private int RecordsPage = 10;
        private readonly int MaxPage  = 50;

        public int _RecordsPage
        {
            get
            {
                return RecordsPage;
            }
            set 
            {
                RecordsPage = (value > MaxPage) ? MaxPage : value;
            }

        }
    }
}
