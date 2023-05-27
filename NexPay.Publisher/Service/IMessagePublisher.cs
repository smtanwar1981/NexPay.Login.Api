using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexPay.Publisher.Service
{
    public interface IMessagePublisher
    {
        public void PublishMessage<T>(T message);
    }
}
