using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SupercomTask.BLL.Interfaces;
using SupercomTask.DTO;
using SupercomTask.Exceptions;
using SupercomTask.Models;
using System;

namespace SupercomTask.Controllers
{
    [ApiController]
    [Route("cards")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardBLL _cardBLL;
        private readonly IValidator<CardDTO> _cardDtoValidator;

        public CardController(ILogger<CardController> logger, ICardBLL cardBLL, IValidator<CardDTO> cardDtoValidator)
        {
            _logger = logger;
            _cardBLL = cardBLL;
            _cardDtoValidator = cardDtoValidator;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<ActionResult<CardDTO>> GetAll()
        {
            List<CardDTO> cards = await _cardBLL.GetCards();
            return Ok(cards);
        }

        [HttpPost(Name = "PostCard")]
        [ActionName(nameof(CardDTO))]
        public async Task<ActionResult<CardDTO>> Post([FromBody] CardDTO cardDTO)
        {
            CardDTO inserted;

            ValidationResult result = await _cardDtoValidator.ValidateAsync(cardDTO);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

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
