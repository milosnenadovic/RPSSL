namespace RPSSL.GameService.Domain.Models.Base;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool Active { get; set; }
}