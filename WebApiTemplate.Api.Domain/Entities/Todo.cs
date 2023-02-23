using AspNetCoreHero.Abstractions.Domain;

namespace WebApiTemplate.Api.Domain.Entities
{
    public partial class Todo : AuditableEntity
    {
        public int UserId { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }
    }
}
