using System;
using manage_boards.src.models;
using manage_boards.src.models.requests;
using MySql.Data.MySqlClient;

namespace manage_boards.src.dataservice
{
    public class BoardsDataservice : IBoardsDataservice
    { 
        private IConfiguration _configuration;

        public BoardsDataservice(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<Board> GetBoard(int boardId, int userId)
        {
            var connectionString = _configuration.GetConnectionString("ProjectBLocalConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"CALL ProjectB.BoardGetByUserIdAndBoardId(@paramUserId, @paramBoardId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@paramUserId", userId);
                    command.Parameters.AddWithValue("@paramBoardId", boardId);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Board board = ExtractBoardFromReader(reader);
                                return board;
                            }

                            return new Board();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<BoardList> GetBoards(int userId)
        {
            var connectionString = _configuration.GetConnectionString("ProjectBLocalConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"CALL ProjectB.BoardGetListByUserId(@paramUserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@paramUserId", userId);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            var boardList = new BoardList();

                            while (reader.Read())
                            {
                                Board board = ExtractBoardFromReader(reader);
                                boardList.Boards.Add(board);
                            }

                            return boardList;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void CreateBoard(CreateBoard createBoardRequest)
        {
            var connectionString = _configuration.GetConnectionString("ProjectBLocalConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"CALL ProjectB.BoardPersist(@paramUserId, @paramBoardName, @paramBoardDescription, @paramCreateUserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@paramUserId", createBoardRequest.UserId);
                    command.Parameters.AddWithValue("@paramBoardName", createBoardRequest.BoardName);
                    command.Parameters.AddWithValue("@paramBoardDescription", createBoardRequest.BoardDescription);
                    command.Parameters.AddWithValue("@paramCreateUserId", createBoardRequest.UserId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void UpdateBoard(UpdateBoard updateBoardRequest)
        {

            var connectionString = _configuration.GetConnectionString("ProjectBLocalConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"CALL ProjectB.BoardUpdate(@paramBoardId, @paramBoardName, @paramBoardDescription, @paramUpdateUserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@paramBoardId", updateBoardRequest.BoardId);
                    command.Parameters.AddWithValue("@paramBoardName", updateBoardRequest.BoardName);
                    command.Parameters.AddWithValue("@paramBoardDescription", updateBoardRequest.BoardDescription);
                    command.Parameters.AddWithValue("@paramUpdateUserId", updateBoardRequest.UserId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void DeleteBoard(int boardId, int userId)
        {
            var connectionString = _configuration.GetConnectionString("ProjectBLocalConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $"CALL ProjectB.BoardDelete(@paramBoardId, @paramUpdateUserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@paramBoardId", boardId);
                    command.Parameters.AddWithValue("@paramUpdateUserId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        #region HELPERS
        
        private static Board ExtractBoardFromReader(MySqlDataReader reader)
        {
            int boardId = reader.GetInt32("BoardId");
            int userId = reader.GetInt32("UserId");
            string name = reader.GetString("BoardName");
            string description = reader.GetString("BoardDescription");
            DateTime createDatetime = reader.GetDateTime("CreateDatetime");
            int createUserId = reader.GetInt32("CreateUserId");
            DateTime? updateDatetime = reader.IsDBNull(reader.GetOrdinal("UpdateDatetime")) ? null : reader.GetDateTime("UpdateDatetime");
            int? updateUserId = reader.IsDBNull(reader.GetOrdinal("UpdateUserId")) ? null : reader.GetInt32("UpdateUserId");

            return new Board()
            {
                BoardId = boardId,
                UserId = userId,
                BoardName = name,
                BoardDescription = description,
                CreateDatetime = createDatetime,
                CreateUserId = createUserId,
                UpdateDatetime = updateDatetime,
                UpdateUserId = updateUserId
            };
        }

        #endregion
    }
}