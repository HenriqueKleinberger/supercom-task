using SupercomTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupercomTaskTests.Builders
{
    internal class CardBuilder
    {
        private Card _card;
        
        public CardBuilder()
        {
            _card = new Card();
            _card.DeadLine = DateTime.Now;
            _card.Status = new Status()
            {
                Name = "To Do",
            };
            _card.CreatedAt = DateTime.Now;
            _card.Description = "Card Description";
            _card.Title = "Card Title";
        }

        public CardBuilder WithTitle(string title)
        {
            _card.Title = title;
            return this;
        }

        public CardBuilder WithStatus(Status status)
        {
            _card.Status = status;
            return this;
        }

        public CardBuilder WithDescription(string description)
        {
            _card.Description = description;
            return this;
        }

        public CardBuilder WithDeadLine(DateTime deadline)
        {
            _card.DeadLine = deadline;
            return this;
        }

        public Card Build()
        {
            return _card;
        }
    }
}
