using ItStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ItStore.Infrastructure
{
    [HtmlTargetElement("td", Attributes="identity-role")]
    public class TagHelperAttribute : TagHelper
    {
        private UserManager<AppUser> userManager { get; set; }
        private RoleManager<IdentityRole> roleManager { get; set; }
        public TagHelperAttribute(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HtmlAttributeName ("identity-role")]
        public string Role { get; set; }
        public override async Task ProcessAsync(TagHelperContext tagHelperContext, TagHelperOutput output)
        {
            List<string> Names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (role!=null && await userManager.IsInRoleAsync(user,role.Name))
                    {
                        Names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(Names.Count ==0 ? "No Users" : string.Join(",",Names));
        }
    }
}
