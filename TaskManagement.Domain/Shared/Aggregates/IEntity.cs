namespace TaskManagement.Domain.Shared.Aggregates;

public interface IEntity<out TPrimaryKey>
{
    TPrimaryKey Id { get; }
}
