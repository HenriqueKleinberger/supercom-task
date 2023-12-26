using Microsoft.AspNetCore.Mvc;
using SupercomTask.BLL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Exceptions;
using SupercomTask.Models;

namespace SupercomTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardBLL _cardBLL;

        public CardController(ILogger<CardController> logger, ICardBLL cardBLL)
        {
            _logger = logger;
            _cardBLL = cardBLL;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<ActionResult<CardDTO>> GetAll()
        {
            List<CardDTO> cards = await _cardBLL.GetCards();
            return Ok(cards);
        }

        [HttpPost(Name = "PostCard")]
        [ActionName(nameof(CardDTO))]
        public async Task<ActionResult<CardDTO>> Post(CardDTO cardDTO)
        {
            CardDTO inserted;

            try
            {
                inserted = await _cardBLL.InsertCard(cardDTO);
            } catch (InvalidStatusException ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(CardDTO), inserted);
        }
    }
}
