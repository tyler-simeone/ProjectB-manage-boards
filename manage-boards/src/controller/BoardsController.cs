using manage_boards.src.models;
using manage_boards.src.models.requests;
using manage_boards.src.repository;
using manage_boards.src.util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace manage_boards.src.controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : Controller
    {
        IRequestValidator _validator;
        IBoardsRepository _boardsRepository;

        public BoardsController(IBoardsRepository boardsRepository, IRequestValidator requestValidator)
        {
            _validator = requestValidator;
            _boardsRepository = boardsRepository;
        }

        [HttpGet("{boardId}")]
        [ProducesResponseType(typeof(Column), StatusCodes.Status200OK)]
        public async Task<ActionResult<Column>> GetBoard(int boardId, int userId)
        {
            if (_validator.ValidateGetBoard(userId, boardId))
            {
                try
                {
                    Board board = await _boardsRepository.GetBoard(boardId, userId);
                    return Ok(board);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("boardId and userId are required.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(BoardList), StatusCodes.Status200OK)]
        public async Task<ActionResult<BoardList>> GetBoards(int userId)
        {
            if (_validator.ValidateGetBoards(userId))
            {
                try
                {
                    BoardList boardList = await _boardsRepository.GetBoards(userId);
                    return Ok(boardList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("boardId and userId are required.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult CreateBoard(CreateBoard createBoardRequest)
        {
            if (_validator.ValidateCreateBoard(createBoardRequest))
            {
                try
                {
                    _boardsRepository.CreateBoard(createBoardRequest);
                    return Ok("Board Created");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("createBoardRequest is required.");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateBoard(UpdateBoard updateBoardRequest)
        {
            if (_validator.ValidateUpdateBoard(updateBoardRequest))
            {
                try
                {
                    _boardsRepository.UpdateBoard(updateBoardRequest);
                    return Ok("Board Updated");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("updateBoardRequest is required.");
            }
        }

        [HttpDelete("{boardId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteBoard(int boardId, int userId)
        {
            if (_validator.ValidateDeleteBoard(boardId, userId))
            {
                try
                {
                    _boardsRepository.DeleteBoard(boardId, userId);
                    return Ok("Board Deleted");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("boardId and userId are required.");
            }
        }
    }
}