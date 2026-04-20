using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary.Data.Models
{
    public class Reader
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ФИО читателя")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Введите номер телефона")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; } = "";
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}