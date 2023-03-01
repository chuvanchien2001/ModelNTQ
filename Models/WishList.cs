namespace ModelNTQ.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WishList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
