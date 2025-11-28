using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Represents a query that produces a response of the specified type.
/// </summary>
/// <typeparam name="TResponse">The type of the response produced by the query. This type must be non-nullable.</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
	where TResponse : notnull
{
}
