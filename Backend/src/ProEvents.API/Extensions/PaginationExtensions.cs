using System.Text.Json;

namespace ProEvents.API.Extensions
{
    public static class PaginationExtensions
    {
        public static void AddPagination(this HttpResponse response,
            int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(
                new
                {
                    currentPage = currentPage,
                    itemsPerPage = itemsPerPage,
                    totalItems = totalItems,
                    totalPages = totalPages
                }, options
            ));

            // Permite a exposição da propriedade Pagination no HeaderResponse
            response.Headers.Add("Acess-Control-Expose-Headers", "Pagination");
        }
    }
}