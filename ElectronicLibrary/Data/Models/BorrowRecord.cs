using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary.Data.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите книгу")]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Выберите читателя")]
        public int ReaderId { get; set; }

        public Reader Reader { get; set; } = null!;

        public DateTime BorrowDate { get; set; }

        [Required(ErrorMessage = "Укажите срок возврата")]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }
    }
}