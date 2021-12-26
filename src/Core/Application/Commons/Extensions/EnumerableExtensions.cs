using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Application.Commons.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T[]> Partition<T>(this IEnumerable<T> source, int partitionMaxSize = 4000)
        {
            var result = new T[partitionMaxSize];
            var counter = 0;
            foreach (var item in source)
            {
                result[counter++] = item;
                if (counter == partitionMaxSize)
                {
                    counter = 0;
                    yield return result;
                    result = new T[partitionMaxSize];
                }
            }
            if (counter > 0)
                yield return result.Skip(0).Take(counter).ToArray();
        }

        public static void ForAll<T>(this IEnumerable<T> source, Action<T> action, CancellationToken stoppingToken)
        {
            foreach (var item in source)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                action(item);
            }
        }

        public static void ForAll<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void ForAll<T>(this IEnumerable<T> source, Action<T, int> action, CancellationToken stoppingToken)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                action(item, index++);
            }
        }

        public static void ForAll<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        public static Task<IEnumerable<TR>> SelectAsync<T, TR>(this Task<IEnumerable<T>> source, Func<T, TR> selector)
        {
            return source.ContinueWith(c => c.Result.Select(selector));
        }

        public static Task<TR[]> ToArrayAsync<T, TR>(this Task<IEnumerable<T>> source, Func<T, TR> selector)
        {
            return source.ContinueWith(c => c.Result?.Select(selector).ToArray() ?? Array.Empty<TR>());
        }

        public static Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> source)
        {
            return source.ContinueWith(c => c.Result?.ToArray() ?? Array.Empty<T>());
        }

        public static Task<T> FirstOrDefaultAsync<T>(this Task<List<T>> source)
        {
            return source.ContinueWith(c =>
            {
                var result = c.Result;
                if (result == null)
                {
                    return default;
                }
                return result.FirstOrDefault();
            }
            );
        }

        public static Task<TR[]> ToArrayAsync<T, TR>(this Task<T[]> source, Func<T, TR> selector)
        {
            return source.ContinueWith(c => c.Result?.Select(selector).ToArray() ?? Array.Empty<TR>());
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler scheduler = null)
        {
            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            if (scheduler != null)
                options.TaskScheduler = scheduler;
            var block = new ActionBlock<T>(body, options);
            foreach (var item in source)
                block.Post(item);
            block.Complete();

            return block.Completion;
        }
    }
}