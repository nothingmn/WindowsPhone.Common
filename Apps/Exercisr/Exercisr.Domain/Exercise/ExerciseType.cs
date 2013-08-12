using Exercisr.Contracts.Exercise;

namespace Exercisr.Domain.Exercise
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