namespace IDC.EventBus.Events;

public class CompanyCreatedEvent
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
}
