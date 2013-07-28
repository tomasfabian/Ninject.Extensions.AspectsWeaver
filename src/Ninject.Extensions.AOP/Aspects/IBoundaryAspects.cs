namespace Ninject.Extensions.AOP.Aspects
{
    public interface IBoundaryAspects
    {
        void OnEntry();
        void OnSuccess();
        void OnException();
        void OnExit();
    }
}
