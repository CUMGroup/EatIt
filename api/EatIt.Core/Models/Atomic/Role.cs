using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Models.Atomic {
    public class Role : IdentityRole<Guid> {

        public Role() : base(){ }

        public Role(String role) : base(role){
        }
    }
}
