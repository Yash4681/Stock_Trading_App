using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StocksAppWithXUnit.Controllers;
using StocksAppWithXUnit.Models;

namespace StocksAppWithXUnit.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if(context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;

                if (orderRequest != null)
                {

                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(temp => temp.Errors).Select(e => e.ErrorMessage).ToList();
                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, StockSymbol = orderRequest.StockSymbol, Price = orderRequest.Price, Quantity = orderRequest.Quantity };

                        context.Result = tradeController.View(nameof(TradeController.Index), stockTrade);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
