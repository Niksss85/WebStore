using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.Entities.Base.Interfaces
{
    public interface INamedEntity : IEntity
    {
        [Required]
        string Name { get; set; }
    }
}