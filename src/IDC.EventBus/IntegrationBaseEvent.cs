namespace IDC.EventBus;

public record IntegrationBaseEvent() : IIntegrationBaseEvent
{
    public DateTime CreationDate { get; } = DateTime.UtcNow;
    public Guid Id { get; set; }
}
