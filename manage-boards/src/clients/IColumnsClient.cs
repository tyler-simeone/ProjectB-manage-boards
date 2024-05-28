using manage_boards.src.models;

namespace manage_boards.src.clients
{
    public interface IColumnsClient
    { 
        public Task<List<Column>> GetColumns(int boardId, int userId);
    }
}