using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercisr.Contracts.Services
{
    public delegate void PublishComplete(IPublishActivity publishActivity, DateTime timestamp, bool success, Exception e, string body);

    public interface IPublishActivity
    {

        event PublishComplete OnPublishComplete;
        void Publish(IActivity activity);

    }
}
