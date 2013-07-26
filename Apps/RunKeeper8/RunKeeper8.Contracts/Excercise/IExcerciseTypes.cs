using System.Collections.Generic;

namespace RunKeeper8.Contracts.Excercise
{
    public interface IExcerciseTypes
    {
        IList<IExcerciseType> Types { get; set; }
    }
}
