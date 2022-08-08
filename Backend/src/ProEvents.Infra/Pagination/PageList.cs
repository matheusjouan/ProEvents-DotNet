using Microsoft.EntityFrameworkCore;

namespace ProEvents.Infra.Pagination
{

    // Herda de List<T>, atém de ter todas as props de List terá essas novas
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Construtor usado para o Mapper manual da paginação
        public PageList() { }

        /*
         List<T>: será a lista de itens que desejamos paginar
         Será uma lista do tipo <T> que é o tipo dos Itens que desejamos paginar
        */

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            // Herda de List que recebe uma coleção de dados
            AddRange(items);
        }

        /*
            IQueryable<T>: é a query no BD
        */
        public static async Task<PageList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize)
        {
            // Quantidade de itens
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}