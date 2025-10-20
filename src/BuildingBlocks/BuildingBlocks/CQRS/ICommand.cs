using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Represents a command that can be executed, typically in the context of a command pattern.
/// </summary>
/// <remarks>This interface extends <see cref="ICommand{TResult}"/> with a default result type of <see
/// cref="Unit"/>,  indicating that the command does not produce a meaningful result. It is commonly used for commands 
/// that perform actions without returning a value.</remarks>
public interface ICommand : ICommand<Unit>
{
}

/// <summary>
/// Represents a command that produces a response of the specified type.
/// </summary>
/// <typeparam name="TResponse">The type of the response produced by the command.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

