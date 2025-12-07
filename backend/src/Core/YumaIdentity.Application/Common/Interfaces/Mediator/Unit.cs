namespace YumaIdentity.Application.Common.Interfaces.Mediator
{
    public readonly struct Unit
    {
        public static readonly Unit Value = new Unit();
        public static readonly Task<Unit> Task = System.Threading.Tasks.Task.FromResult(Value);
    }
}
