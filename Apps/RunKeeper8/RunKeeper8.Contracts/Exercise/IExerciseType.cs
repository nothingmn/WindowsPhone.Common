namespace RunKeeper8.Contracts.Exercise
{
    public interface IExerciseType
    {
        /// <summary>
        /// Storage unique id
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// The type of UI excercize tpe
        /// </summary>
        string UIExerciseType { get; set; }
        /// <summary>
        /// Something to display on the screen
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// order in which to display the item on the screen
        /// </summary>
        int DisplayOrder { get; set; }
        /// <summary>
        /// Internal type name (for the service)
        /// </summary>
        string TypeName { get; set; }
        /// <summary>
        /// Icon to show on the screen
        /// </summary>
        string Icon { get; set; }
    }
}
