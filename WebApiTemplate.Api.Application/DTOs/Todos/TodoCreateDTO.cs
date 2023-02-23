namespace WebApiTemplate.Api.Application.DTOs.Todos
{
    public class TodoCreateDTO
    {
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
