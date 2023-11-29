namespace Lab6AIS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SmartWatch")]
    public partial class SmartWatch
    {
        [Column(TypeName = "numeric")]
        public decimal id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Required]
        [StringLength(256)]
        public string price { get; set; }

        [Required]
        [StringLength(256)]
        public string link { get; set; }

        [Required]
        [StringLength(256)]
        public string brand { get; set; }

        [Required]
        [StringLength(256)]
        public string battery_capacity { get; set; }

        [Required]
        [StringLength(256)]
        public string screed_diagonal { get; set; }
    }
}
