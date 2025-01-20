using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrustSeal.Areas.Identity.Data;

namespace TrustSeal.Pages.UserManagement {
    [Authorize]
    class IndexModel: PageModel {
         private readonly TrustSeal.Areas.Identity.Data.TSAuth _context;
         private readonly UserManager<TSUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        private readonly RoleManager<IdentityRole> _roleMager;

        public IndexModel(
            TrustSeal.Areas.Identity.Data.TSAuth context,
            ILogger<IndexModel> logger,
            UserManager<TSUser>  userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleMager = roleManager;
        }

        public class UserWithRoles {
            public string Id {get;set;}
            public string FirstName {get;set;}

            public string LastName {get;set;}

            public string Email {get;set;}

            public bool EmailConfirmed {get;set;}

            public string PhoneNumber {get;set;}

            public IList<string> Roles {get;set;}
        }

        public List<TSUser> Users {get;set;} = new List<TSUser>();
        public List<UserWithRoles> UsersWithRoles {get;set;} = new List<UserWithRoles>();
        
        public async Task OnGetAsync()
        {
            Users = await _context.Users
            .ToListAsync();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersWithRoles.Add(new UserWithRoles{
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles
                });
            }
                                    
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId=Request.Form["User.Id"];
            var user=await _context.Users.FirstOrDefaultAsync(u=>u.Id==userId.ToString());
            if(user!=null)
            {
                user.FirstName=Request.Form["User.FirstName"];
                user.LastName=Request.Form["User.LastName"];
                user.EmailConfirmed = Request.Form["User.EmailConfirmed"].ToString() == "True" ? true: false;
                user.PhoneNumber = Request.Form["User.PhoneNumber"];
                await _context.SaveChangesAsync();
                var roles= await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user,roles);
                await _userManager.AddToRoleAsync(user,Request.Form["User.Role"]);
                await _context.SaveChangesAsync();
                return new JsonResult(new {success=true,message="Update User Successfully"});
            }
            return Page();
        }
    }
}