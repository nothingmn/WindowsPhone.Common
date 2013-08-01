using RunKeeper8.Contracts.Exercise;

namespace RunKeeper8.Domain.Exercise
{
    public class ExerciseType : IExerciseType
    {
        public int Id { get; set; }
        public string UIExerciseType { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public string TypeName { get; set; }
        public string Icon { get; set; }
    }
}