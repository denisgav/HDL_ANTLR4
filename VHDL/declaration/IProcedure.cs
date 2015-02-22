namespace VHDL.declaration
{
    // NOTE: necessary just to distinguish from ISubprogram when referes from call.
    // TODO: refactor subprograms: should be func/proc specification containing in body
    public interface IProcedure : ISubprogram
    {
    }
}