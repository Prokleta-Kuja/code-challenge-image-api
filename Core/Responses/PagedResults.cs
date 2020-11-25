using System.Collections.Generic;

namespace ImageApi.Core.Responses
{
    public class PagedResults<T>
    {
        public PagedResults() { }
        public PagedResults(int number, int size, IEnumerable<T> collection = default)
        {
            Number = number;
            Size = size;
            Results = collection;
        }

        public int Number { get; set; }
        public int Size { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}