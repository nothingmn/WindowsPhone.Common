namespace WindowsPhone.Contracts.Core
{
    public interface IInjectionParameter
    {
        string Name { get; set; }
        object Value { get; set; }
        bool ShouldInherit { get; set; }
    }



}
