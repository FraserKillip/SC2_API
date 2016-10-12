﻿using System;
using System.IO;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SandwichClub.Api.GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Middleware
{
    /// <summary>
    ///     Provides middleware for hosting GraphQL.
    /// </summary>
    public sealed class GraphQLMiddleware
    {
        private readonly string graphqlPath;
        private readonly RequestDelegate next;
        // private readonly ISchema schema;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GraphQLMiddleware" /> class.
        /// </summary>
        /// <param name="next">
        ///     The next request delegate.
        /// </param>
        /// <param name="options">
        ///     The GraphQL options.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Throws <see cref="ArgumentNullException" /> if <paramref name="next" /> or <paramref name="options" /> is null.
        /// </exception>
        public GraphQLMiddleware(RequestDelegate next , IOptions<GraphQLOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.next = next;
            var optionsValue = options.Value;
            graphqlPath = string.IsNullOrEmpty(optionsValue?.GraphQLPath) ? GraphQLOptions.DefaultGraphQLPath : optionsValue.GraphQLPath;
            // schema = optionsValue?.Schema;
        }

        /// <summary>
        ///     Invokes the middleware with the specified context.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <returns>
        ///     A <see cref="Task" /> representing the middleware invocation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Throws <see cref="ArgumentNullException" /> if <paramref name="context" />.
        /// </exception>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var queryObj = context.RequestServices.GetService<ISandwichClubSchema>();


            if (ShouldRespondToRequest(context.Request))
            {
                var executionResult = await ExecuteAsync(context.Request, new Schema { Query = queryObj }).ConfigureAwait(true);
                await WriteResponseAsync(context.Response , executionResult).ConfigureAwait(true);
                return;
            }

            await next(context).ConfigureAwait(true);
        }

        private async Task<ExecutionResult> ExecuteAsync(HttpRequest request, ISchema schema)
        {
            string requestBodyText;
            using (var streamReader = new StreamReader(request.Body))
            {
                requestBodyText = await streamReader.ReadToEndAsync().ConfigureAwait(true);
            }
            var graphqlRequest = JsonConvert.DeserializeObject<GraphQLRequest>(requestBodyText);
            var result = await new DocumentExecuter().ExecuteAsync(schema , null , graphqlRequest.Query , graphqlRequest.OperationName , graphqlRequest.Variables.ToInputs()).ConfigureAwait(true);
            return result;
        }

        private bool ShouldRespondToRequest(HttpRequest request)
        {
            bool a = string.Equals(request.Method , "POST" , StringComparison.OrdinalIgnoreCase);
            bool b = request.Path.Equals(graphqlPath);
            return a && b;
        }

        private static Task WriteResponseAsync(HttpResponse response , ExecutionResult executionResult)
        {
            response.ContentType = "application/json";
            response.StatusCode = executionResult.Errors == null || executionResult.Errors?.Count == 0 ? 200 : 400;
            var graphqlResponse = new DocumentWriter().Write(executionResult);
            return response.WriteAsync(graphqlResponse);
        }
    }
}
