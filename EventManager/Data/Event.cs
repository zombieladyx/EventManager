namespace EventManager.Data
{
    public class Event
    {
        public int EVENT_ID { get; set; }
        public string SHORT_DESC { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;
        public int SPACE_LIMIT { get; set; }
        public int CURR_SPACE {  get; set; }
        public DateOnly EVENT_DATE {  get; set; }
    }
}
