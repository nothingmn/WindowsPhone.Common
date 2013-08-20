using WindowsPhone.Contracts.Core;

namespace WindowsPhone.Contracts.DI
{
    public interface IDIContainer
    {
        T Get<T>(params IInjectionParameter[] parameters);
        T Get<T>(string name, params IInjectionParameter[] parameters);

        object UnderlyingProvider { get; }
    }
}
