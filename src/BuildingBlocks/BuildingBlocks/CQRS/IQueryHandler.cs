using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Defines a handler for processing queries of a specified type and returning a response.
/// </summary>
/// <remarks>This interface extends <see cref="IRequestHandler{TRequest, TResponse}"/> to provide a specialized
/// contract for handling queries. Implementations of this interface are responsible for executing the query logic and
/// returning the appropriate response.</remarks>
/// <typeparam name="TQuery">The type of the query to be handled. Must implement <see cref="IQuery{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the handler. Must be a non-nullable type.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
	: IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>
	where TResponse : notnull
{
}