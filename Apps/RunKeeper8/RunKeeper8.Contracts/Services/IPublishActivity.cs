using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Contracts.Services
{
    public delegate void PublishComplete(IPublishActivity publishActivity, DateTime timestamp, bool success, Exception e, string body);

    public interface IPublishActivity
    {

        event PublishComplete OnPublishComplete;
        void Publish(IActivity activity);

    }
}
