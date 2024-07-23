using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Discount.API.Services;

public class DiscountService : DiscountProtoServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IMediator mediator, ILogger<DiscountService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext callContext)
    {
        var query = new GetDiscountQuery(request.ProductName);
        var result = await _mediator.Send(query);
        _logger.LogInformation($"Discount is retrieved for the Product Name: {request.ProductName} and Amount: {result.Amount}");
        return result;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var createDiscountCommand = new CreateDiscountCommand
        {
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };

        var result = await _mediator.Send(createDiscountCommand);
        _logger.LogInformation($"Discount is Successfully created for the Product Name: {result.ProductName} ");
        return result;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var updateDiscountCommand = new UpdateDiscountCommand
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Amount = request.Coupon.Amount,
            Description = request.Coupon.Description
        };

        var result = await _mediator.Send(updateDiscountCommand);
        _logger.LogInformation($"Discount is Successfully update Product Name: {result.ProductName} ");
        return result;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var deleteDiscountCommand = new DeleteDiscountCommand(request.ProductName);
        var deleted = await _mediator.Send(deleteDiscountCommand);
        var response = new DeleteDiscountResponse
        {
            Success = deleted
        };
        return response;
    }
}
