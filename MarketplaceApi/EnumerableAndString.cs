using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi
{
    public class EnumerableAndString<T>
    {
        public readonly IEnumerable<T> QueryResult;
        public readonly string TextResult;

        public EnumerableAndString(IEnumerable<T> queryResult, string resultText)
        {
            QueryResult = new EnumerableQuery<T>(queryResult);
            TextResult = new string(resultText);
        }
    }
}