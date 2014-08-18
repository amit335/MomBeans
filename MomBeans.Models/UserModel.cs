/********************************************************************************
 * File Name    : UserModel.cs 
 * Author       : Amit Kumar
 * Created On   : 14/05/2014
 * Description  : This is a property class for the Users
 *******************************************************************************/

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MomBeans.Models
{
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
            ErrorMessage = "Invalid Email Address!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Between 6-20 Characters.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        
        
        [Required]
        [UIHint("Roles")]
        public string Role { get; set; }
        
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}

