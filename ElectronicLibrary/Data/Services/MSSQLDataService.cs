using ElectronicLibrary.Data.Interfaces;
using ElectronicLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLibrary.Data.Services
{
    public class MSSQLDataService : IDataService
    {
        private readonly ApplicationDbContext _db;

        public MSSQLDataService(ApplicationDbContext db)
        {
            _db = db;
        }

        // ---------------- Книги ----------------

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _db.Books
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _db.Books.FindAsync(id);
        }

        public async Task AddOrUpdateBookAsync(Book book)
        {
            if (book.Id == 0)
            {
                book.AvailableCopies = book.TotalCopies;
                _db.Books.Add(book);
            }
            else
            {
                var existingBook = await _db.Books.FindAsync(book.Id);
                if (existingBook == null)
                    throw new ArgumentException("Книга не найдена");

                int borrowedCopies = existingBook.TotalCopies - existingBook.AvailableCopies;

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.PublicationDate = book.PublicationDate;
                existingBook.TotalCopies = book.TotalCopies;
                existingBook.AvailableCopies = book.TotalCopies - borrowedCopies;

                if (existingBook.AvailableCopies < 0)
                    existingBook.AvailableCopies = 0;
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
        }

        // ---------------- Читатели ----------------

        public async Task<List<Reader>> GetAllReadersAsync()
        {
            return await _db.Readers
                .OrderBy(r => r.FullName)
                .ToListAsync();
        }

        public async Task<Reader?> GetReaderByIdAsync(int id)
        {
            return await _db.Readers.FindAsync(id);
        }

        public async Task AddOrUpdateReaderAsync(Reader reader)
        {
            if (reader.Id == 0)
                _db.Readers.Add(reader);
            else
                _db.Readers.Update(reader);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteReaderAsync(int id)
        {
            var reader = await _db.Readers.FindAsync(id);
            if (reader != null)
            {
                _db.Readers.Remove(reader);
                await _db.SaveChangesAsync();
            }
        }

        // ---------------- Записи о выдаче книг ----------------

        public async Task<List<BorrowRecord>> GetAllBorrowRecordsAsync()
        {
            return await _db.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Reader)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        public async Task<BorrowRecord?> GetBorrowRecordByIdAsync(int id)
        {
            return await _db.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Reader)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task BorrowBookAsync(BorrowRecord record)
        {
            var book = await _db.Books.FindAsync(record.BookId);
            if (book == null)
                throw new ArgumentException("Книга не найдена");

            if (book.AvailableCopies <= 0)
                throw new InvalidOperationException("Нет доступных экземпляров");

            record.BorrowDate = DateTime.Now;
            record.IsReturned = false;
            record.ReturnDate = null;

            book.AvailableCopies--;

            _db.BorrowRecords.Add(record);
            await _db.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(int borrowRecordId)
        {
            var record = await _db.BorrowRecords.FindAsync(borrowRecordId);
            if (record == null)
                throw new ArgumentException("Запись выдачи не найдена");

            if (record.IsReturned)
                throw new InvalidOperationException("Книга уже возвращена");

            var book = await _db.Books.FindAsync(record.BookId);
            if (book == null)
                throw new ArgumentException("Книга не найдена");

            record.IsReturned = true;
            record.ReturnDate = DateTime.Now;
            book.AvailableCopies++;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteBorrowRecordAsync(int id)
        {
            var record = await _db.BorrowRecords.FindAsync(id);
            if (record != null)
            {
                _db.BorrowRecords.Remove(record);
                await _db.SaveChangesAsync();
            }
        }
    }
}