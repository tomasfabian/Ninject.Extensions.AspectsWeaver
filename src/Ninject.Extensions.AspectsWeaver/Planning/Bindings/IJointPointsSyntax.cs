using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    public interface IJointPointsSyntax
    {
        IWeaveIntoSyntax JointPoints(IJointPointSelector selector);
    }
}