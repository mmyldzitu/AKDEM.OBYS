using Microsoft.AspNetCore.Http;

namespace AKDEM.OBYS.UI.Models
{
    public class AppUserPPUpdateModel
    {
        public int Id { get; set; }
        public IFormFile ImagePath { get; set; }
        public string ImagePath2 { get; set; }

    }
}
