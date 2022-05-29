namespace ItStore.Models.DataFolder
{
    public class FeedBack
    {
        public int id { get; set; }
        public string MainEmail { get; set; }
        public string? AuxiliaryEmail { get; set; }
        public string TypeMessage { get; set; }
        public string Login { get; set; }
        public string? Name { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public bool TypeFeedback { get; set; }
        public string? ProductName { get; set; }

       

        public DateTime Closed { get; set; }
        public string? Admin { get; set; }
        public string? AdminCommentaries { get; set; }
        public bool FeedbakStatus { get; set; }

    }
}
