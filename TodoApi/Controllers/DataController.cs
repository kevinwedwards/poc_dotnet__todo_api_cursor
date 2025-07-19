using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApi.Services;
using TodoApi.Configuration;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Controller for managing the in-memory data store
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DataController : ControllerBase
    {
        private readonly InMemoryDataStore _dataStore;
        private readonly DataStoreOptions _options;

        public DataController(InMemoryDataStore dataStore, IOptions<DataStoreOptions> options)
        {
            _dataStore = dataStore;
            _options = options.Value;
        }

        /// <summary>
        /// Gets the current status of the in-memory data store
        /// </summary>
        /// <returns>Information about the current data</returns>
        /// <response code="200">Returns the data store status</response>
        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> GetDataStatus()
        {
            return Ok(new
            {
                UserCount = _dataStore.Users.Count,
                TodoCount = _dataStore.Todos.Count,
                LastUpdated = DateTime.UtcNow,
                Message = "Data persists across requests due to singleton pattern",
                Configuration = new
                {
                    InitializeSampleData = _options.InitializeSampleData
                }
            });
        }

        /// <summary>
        /// Resets the data store to initial sample data
        /// </summary>
        /// <returns>Confirmation of the reset operation</returns>
        /// <response code="200">Data store has been reset</response>
        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> ResetData()
        {
            _dataStore.ClearAllData();
            return Ok(new
            {
                Message = "Data store has been reset to initial sample data",
                UserCount = _dataStore.Users.Count,
                TodoCount = _dataStore.Todos.Count,
                ResetTime = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Manually initializes sample data
        /// </summary>
        /// <returns>Confirmation of the initialization operation</returns>
        /// <response code="200">Sample data has been initialized</response>
        [HttpPost("initialize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> InitializeSampleData()
        {
            _dataStore.InitializeData();
            return Ok(new
            {
                Message = "Sample data has been initialized",
                UserCount = _dataStore.Users.Count,
                TodoCount = _dataStore.Todos.Count,
                InitializeTime = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Gets a summary of all data in the store
        /// </summary>
        /// <returns>Summary of all users and todos</returns>
        /// <response code="200">Returns the data summary</response>
        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> GetDataSummary()
        {
            var userSummary = _dataStore.Users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                TodoCount = _dataStore.Todos.Count(t => t.CreatedByUserId == u.Id)
            });

            var todoSummary = _dataStore.Todos.Select(t => new
            {
                t.Id,
                t.Description,
                t.Order,
                CreatedBy = _dataStore.Users.FirstOrDefault(u => u.Id == t.CreatedByUserId)?.Name ?? "Unknown",
                t.CreatedOn,
                t.PlannedDate,
                t.DueDate,
                IsOverdue = t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow
            });

            return Ok(new
            {
                Users = userSummary,
                Todos = todoSummary,
                TotalUsers = _dataStore.Users.Count,
                TotalTodos = _dataStore.Todos.Count,
                OverdueTodos = _dataStore.Todos.Count(t => t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow)
            });
        }
    }
} 