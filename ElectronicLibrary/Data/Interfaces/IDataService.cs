using ElectronicLibrary.Data.Models;

namespace ElectronicLibrary.Data.Interfaces
{
    public interface IDataService
    {
        // Книги
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task AddOrUpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);

        // Читатели
        Task<List<Reader>> GetAllReadersAsync();
        Task<Reader?> GetReaderByIdAsync(int id);
        Task AddOrUpdateReaderAsync(Reader reader);
        Task DeleteReaderAsync(int id);

        // Записи о выдаче книг
        Task<List<BorrowRecord>> GetAllBorrowRecordsAsync();
        Task<BorrowRecord?> GetBorrowRecordByIdAsync(int id);
        Task BorrowBookAsync(BorrowRecord record);
        Task ReturnBookAsync(int borrowRecordId);
        Task DeleteBorrowRecordAsync(int id);
    }
}