using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: GetByPeriod")
            .WithSummary("Returns all transactions by period")
            .WithDescription("Returns all transactions by period")
            .WithOrder(5)
            .Produces<PagedResponse<Transaction?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageSize = pageSize,
            PageNumber = pageNumber,
            StartDate = startDate,
            EndDate = endDate,
        };
        
        var result = await handler.GetByPeriodAsync(request);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result.Data);
    } 
}