using kolokwium1.Models.DTOs;

namespace kolokwium1.Repositories;

public interface IBookRepository
{
    Task<BookAuthor> GetAuthorByID(int id);
    Task<bool> DoesAuthorExist(int id);
    Task AddBookWithAuthor(BookAuthor newBookAuthor);
}