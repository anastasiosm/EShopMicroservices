using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Defines a handler for processing commands of type <typeparamref name="TCommand"/>.
/// </summary>
/// <remarks>This interface extends <see cref="IRequestHandler{TRequest, TResponse}"/> with a fixed response type
/// of <see cref="Unit"/>,  indicating that command handlers do not return a value. Implementations of this interface
/// are responsible for executing  the logic associated with the given command.</remarks>
/// <typeparam name="TCommand">The type of the command to be handled. Must implement the <see cref="ICommand"/> interface.</typeparam>
public interface ICommandHandler<in TCommand> 
	: IRequestHandler<TCommand, Unit>
	where TCommand : ICommand<Unit>
{
}

/// <summary>
/// Defines a contract for handling commands of a specified type and producing a response.
/// </summary>
/// <remarks>This interface extends <see cref="IRequestHandler{TRequest, TResponse}"/> to provide a specialized
/// handler for commands. Implementations of this interface are responsible for processing the command and returning an
/// appropriate response.</remarks>
/// <typeparam name="TCommand">The type of the command to be handled. Must implement <see cref="ICommand{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response produced by handling the command. Must be a non-nullable type.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> 
	: IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>
	where TResponse : notnull
{
}

