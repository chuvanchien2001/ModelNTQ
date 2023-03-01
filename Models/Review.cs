namespace ModelNTQ.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ProductID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public int Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedAt { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
