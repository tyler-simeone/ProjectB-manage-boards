namespace manage_boards.src.models
{
    public class Board : ResponseBase
    {
        public Board()
        {
        }

        public int BoardId { get; set; }

        public int UserId { get; set; }

        public string BoardName { get; set; }

        public string BoardDescription { get; set; }
        
        public int ColumnCount { get; set; }

        public List<Column>? Columns { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDatetime { get; set; }

        public int? UpdateUserId { get; set; }

        public DateTime? UpdateDatetime { get; set; }
    }
}