using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Contracts.Services
{
    public interface IPublishActivity
    {

        void Publish(IActivity activity);

    }
}
