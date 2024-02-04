// Purpose: This file contains the BaseEntity class which is the base class for all entities in the application. It contains the Id, CreatedDate, and ModifiedDate properties.

namespace Board.Common.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}