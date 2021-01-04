using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OData.Client.Demo
{
    public static class AsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<TSource> Take<TSource>(
            this IAsyncEnumerable<TSource> source,
            int count,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
        {
            var counted = 0;
            
            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                yield return element;
                counted += 1;
                
                if (counted >= count)
                {
                    yield break;
                }
            }
        }
    }
}