using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AutoresApi.Utilities
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParameterPageInHead<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            double amount = await queryable.CountAsync();
            httpContext.Response.Headers.Add("AmountRecords", amount.ToString());       

        }
    }
}
