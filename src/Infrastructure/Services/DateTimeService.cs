using consulteer.Application.Common.Interfaces;

namespace consulteer.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
