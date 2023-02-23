namespace WebApiTemplate.Api.Application.DTOs.Todos
{
    public class TodoDTO : IBaseDTO
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
