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
    public class StatisticsController : ControllerBase
    {
        private readonly AsyncRepository<Statistic, HalmaDbContext> _repository;

        public StatisticsController(RepositoryFactory<HalmaDbContext> factory)
        {
            _repository = factory.GetRepo<Statistic>();
        }

        // GET: api/Statistics
        [HttpGet]
        public async Task<IEnumerable<Statistic>> GetStatistics()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/Statistics/Player/5
        [HttpGet("Player/{guid}")]
        public async Task<ActionResult<Statistic>> GetStatisticOfPlayer(string guid)
        {
            var statistic = await _repository.FindAsyncRefPlayer(g => g.Equals(guid));

            if (statistic == null)
            {
                return NotFound();
            }

            return statistic;
        }


        // GET: api/Statistics/5
        [HttpGet("{guid}")]
        public async Task<ActionResult<Statistic>> GetStatistic(string guid)
        {
            var statistic = await _repository.GetAsync(guid);

            if (statistic == null)
            {
                return NotFound();
            }

            return statistic;
        }

        // PUT: api/Statistics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{guid}")]
        public async Task<IActionResult> PutStatistic(string guid, Statistic statistic)
        {
            if (guid != statistic.StatisticGuid)
            {
                return BadRequest();
            }

            _repository.Entry(statistic).State = EntityState.Modified;

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool ifExist = await StatisticExists(guid);
                if (ifExist)
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

        // POST: api/Statistics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Statistic>> PostStatistic(Statistic statistic)
        {
            await _repository.Add(statistic);

            return CreatedAtAction("GetStatistic", new { id = statistic.StatisticGuid }, statistic);
        }

        // DELETE: api/Statistics/5
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteStatistic(string guid)
        {
            var statistic = await _repository.GetAsync(guid);
            if (statistic == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(statistic);

            return NoContent();
        }

        private async Task<bool> StatisticExists(string guid)
        {
            return await _repository.Any(e => e.StatisticGuid.Equals(guid));
        }
    }
}
