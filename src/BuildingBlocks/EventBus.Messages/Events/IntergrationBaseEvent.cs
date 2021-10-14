using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntergrationBaseEvent
    {
        public IntergrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.UtcNow;
        }
        public IntergrationBaseEvent(Guid guid,DateTime dateTime)
        {
            this.Id = guid;
            this.CreationTime = dateTime;
        }

        public Guid Id { get; private set; }
        public DateTime CreationTime { get; private set; }


    }
}
