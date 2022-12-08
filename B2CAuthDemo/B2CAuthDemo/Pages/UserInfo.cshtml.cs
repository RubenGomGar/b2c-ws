using Microsoft.AspNetCore.Mvc.RazorPages;

namespace B2CAuthDemo.Pages
{
    public class UserInfoModel : PageModel
    {
        private readonly ILogger<UserInfoModel> _logger;

        public UserInfoModel(ILogger<UserInfoModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}