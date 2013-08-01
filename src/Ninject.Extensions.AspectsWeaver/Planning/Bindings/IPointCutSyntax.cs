using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    public interface IPointCutSyntax
    {
        IWeaveIntoSyntax PointCuts(IPointCutSelector cutSelector);
    }
}