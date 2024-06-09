using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using backend.Repositories;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameHistoriesController : ControllerBase
    {

        private AsyncRepository<GameHistory, HalmaDbContext> GameHistoryRepository { get; set; }
        public GameHistoriesController(RepositoryFactory<HalmaDbContext> repositoryFabric)
        {
            GameHistoryRepository = repositoryFabric.GetRepo<GameHistory>();
        }

        // GET: api/GameHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameHistory>>> GetGamesHistory()
        {
            var result = await GameHistoryRepository.GetAllAsync();
            return new ActionResult<IEnumerable<GameHistory>>(result);
        }

        // GET: api/GameHistories/5
        [HttpGet("{guid}")]
        public async Task<ActionResult<GameHistory>> GetGameHistory(string guid)
        {
            var gameHistory = await GameHistoryRepository.GetAsync(guid);

            if (gameHistory == null)
            {
                return NotFound();
            }

            return gameHistory;
        }

        // PUT: api/GameHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{guid}")]
        public async Task<IActionResult> PutGameHistory(string guid, GameHistory gameHistory)
        {
            if (guid != gameHistory.GameHistoryGuid)
            {
                return BadRequest();
            }

            GameHistoryRepository.Entry(gameHistory).State = EntityState.Modified;

            try
            {
                await GameHistoryRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool ifExists = await GameHistoryExists(guid);
                if (!ifExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameHistory>> PostGameHistory(GameHistory gameHistory)
        {
            await GameHistoryRepository.AddAsync(gameHistory);
            
            return CreatedAtAction("GetGameHistory", new { guid = gameHistory.GameHistoryGuid }, gameHistory);
        }

        // DELETE: api/GameHistories/5
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteGameHistory(string guid)
        {
            var gameHistory = await GameHistoryRepository.GetAsync(guid);
            if (gameHistory == null)
            {
                return NotFound();
            }

            await GameHistoryRepository.DeleteAsync(gameHistory);

            return NoContent();
        }

        private async  Task<bool> GameHistoryExists(string guid)
        {
            return await GameHistoryRepository.ContainsAsync(guid);
        }
    }
}
