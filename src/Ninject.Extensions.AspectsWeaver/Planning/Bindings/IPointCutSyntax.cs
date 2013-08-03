using Ninject.Extensions.AspectsWeaver.Aspects;

namespace Ninject.Extensions.AspectsWeaver.Planning.Bindings
{
    /// <summary>
    /// Used to add pointcut selectors.
    /// </summary>
    public interface IPointCutSyntax
    {
        /// <summary>
        /// Indicates that the interception should be limited to selected pointcuts.
        /// </summary>
        /// <param name="pointcutSelector">The pointcuts selector.</param>
        /// <returns>The weaver builder's fluent syntax.</returns>
        IWeaveIntoSyntax PointCuts(IPointCutSelector pointcutSelector);
    }
}