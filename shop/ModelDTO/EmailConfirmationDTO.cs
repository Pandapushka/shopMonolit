using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO
{
    public class ConfirmEmailRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }

    public class ResendCodeRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
