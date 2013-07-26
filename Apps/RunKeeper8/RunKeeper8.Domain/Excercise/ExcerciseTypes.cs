using RunKeeper8.Contracts.Excercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunKeeper8.Domain.Excercise
{
    public class ExcerciseTypes : IExcerciseTypes
    {
        public ExcerciseTypes()
        {
            Types = new List<IExcerciseType>()
                {
                    new ExcerciseType() {DisplaName = "Walking", DisplayOrder = 0, ExcersiceType = "MapTracking"},
                    new ExcerciseType() {DisplaName = "Running", DisplayOrder = 1, ExcersiceType = "MapTracking"},
                };
        }

        public IList<IExcerciseType> Types { get; set; }
    }
}
