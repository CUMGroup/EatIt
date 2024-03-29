﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EatIt.Core.Models.Joined;

namespace EatIt.Core.Models.Atomic {
    public class ShoppingList {

        [Key, ForeignKey("User"), Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<ShoppingIngredient> Ingredients { get; set;}
    }
}
