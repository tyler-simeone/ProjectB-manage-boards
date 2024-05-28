namespace manage_boards.src.models
{
    public class BoardList : ResponseBase
    {
        public BoardList()
        {
            Boards = [];
        }

        public List<Board> Boards { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}