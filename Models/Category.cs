namespace ModelNTQ.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã danh mục")]
        public int Id { get; set; }

        [Required(ErrorMessage ="Tên danh mục không được đế trống")]
        [StringLength(255)]
        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
