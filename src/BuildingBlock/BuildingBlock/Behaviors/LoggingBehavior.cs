using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlock.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response}, RequestData = {Data}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        if(timer.Elapsed.Seconds>4)
               logger.LogWarning("[PERFORMANCE] The Request {Request} took {time} seconds",
                   typeof(TRequest).Name, timer.Elapsed.Seconds);


        logger.LogInformation("[END] Handle request={Request} with Response={Response}",
            typeof(TRequest).Name, response);
        return response;


    }
}
