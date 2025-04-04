namespace IDC.EventBus;

public interface IIntegrationBaseEvent
{
    DateTime CreationDate { get; }
    Guid Id { get; }
}
