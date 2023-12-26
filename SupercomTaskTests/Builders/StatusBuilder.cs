using SupercomTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupercomTaskTests.Builders
{
    internal class StatusBuilder
    {
        private Status _status;
        
        public StatusBuilder()
        {
            _status = new Status();
            _status.Name = "To Do";
        }

        public StatusBuilder WithName(string name)
        {
            _status.Name = name;
            return this;
        }

        public Status Build()
        {
            return _status;
        }
    }
}
