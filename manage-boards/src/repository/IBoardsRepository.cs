using manage_boards.src.models;
using manage_boards.src.models.requests;

namespace manage_boards.src.repository
{
    public interface IBoardsRepository
    {
        public Task<Board> GetBoard(int boardId, int userId);

        public Task<BoardList> GetBoards(int userId);

        public void CreateBoard(CreateBoard createBoardRequest);

        public void UpdateBoard(UpdateBoard updateBoardRequest);

        public void DeleteBoard(int boardId, int userId);
    }
}