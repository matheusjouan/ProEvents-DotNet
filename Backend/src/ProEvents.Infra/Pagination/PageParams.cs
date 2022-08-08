namespace ProEvents.Infra.Pagination
{
    public class PageParams
    {
        // Máximo de item numa página
        public const int MaxPageSize = 50;

        // Número da página atual
        public int PageNumber { get; set; } = 1;

        // Valor do tamanho da página inicial
        public int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        // Utilizado para o Filtro
        // Caso não tiver nenhuma informação, deverá ser vazio e não null
        public string Term { get; set; } = string.Empty;
    }
}