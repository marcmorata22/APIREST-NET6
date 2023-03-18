namespace AutoresApi.DTOs
{
    public class DataHATEOASDTO
    {
        public string Link { get; private set; }
        public string Description { get; private set; }
        public string Method { get; private set; }

        public DataHATEOASDTO(string link, string description, string method)
        {
            Link = link;
            Description = description;
            Method = method;
        }
    }
}
