namespace EventManager.Data
{
    public class Event
    {
        public int Event_ID {  get; set; } 
        public string Short_DESC { get; set; } = string.Empty;
        public string DESC { get; set; } = string.Empty;
        public int SPACE_LIMIT { get; set; }
        public int CURR_SPACE {  get; set; }
        public DateOnly EVENT_DATE {  get; set; }
    }
}
