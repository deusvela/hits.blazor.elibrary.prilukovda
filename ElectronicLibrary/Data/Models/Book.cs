using System.ComponentModel.DataAnnotations;

namespace ElectronicLibrary.Data.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название книги")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Введите автора")]
        public string Author { get; set; } = "";

        [Required(ErrorMessage = "Введите жанр")]
        public string Genre { get; set; } = "";

        [Required(ErrorMessage = "Укажите дату издания")]
        public DateTime PublicationDate { get; set; } = DateTime.Today;

        [Range(1, 1000, ErrorMessage = "Количество экземпляров должно быть от 1 до 1000")]
        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}