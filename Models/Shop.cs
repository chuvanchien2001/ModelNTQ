namespace ModelNTQ.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ShopName { get; set; }

        [Required]
        [StringLength(255)]
        public string ShopAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string ShopPhone { get; set; }
    }
}
