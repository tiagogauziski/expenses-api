﻿using Expenses.Domain.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Invoice : IInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Recurrence Recurrence { get; set; }
    }
}
