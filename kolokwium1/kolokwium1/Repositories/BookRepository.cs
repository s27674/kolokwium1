using kolokwium1.Models.DTOs;
using Microsoft.Data.SqlClient;
namespace kolokwium1.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<BookAuthor> GetAuthorByID(int id)
    {
        var query = @"SELECT books.PK, books.title, authors.first_name, authors.last_name
                  FROM books JOIN books_authors ON books.PK = books_authors.FK_book
                  JOIN authors ON books_authors.FK_author = authors.PK
				  WHERE books.PK = @id";
    
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
    
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
    
        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
    
        await reader.ReadAsync();

        if (!reader.HasRows) throw new Exception();

        var bookIDOrdinal = reader.GetOrdinal("PK");
        var bookTitleOrdinal = reader.GetOrdinal("title");
        var firstNameOrdinal = reader.GetOrdinal("first_name");
        var lastNameOrdinal = reader.GetOrdinal("last_name");

        var bookDTO = new BookAuthor()
        {
            id = reader.GetInt32(bookIDOrdinal),
            title = reader.GetString(bookTitleOrdinal),
            Author = new AuthorDTO()
            {
                firstName = reader.GetString(firstNameOrdinal),
                lastName = reader.GetString(lastNameOrdinal)
            }
        };

        return bookDTO;
    }

    public async Task AddBookWithAuthor(BookAuthor newBookAuthor)
    {
      var query =
         @"BEGIN TRANSACTION;
        DECLARE @BookID INT;
        INSERT INTO books (title)
        VALUES (@title);
        SET @BookID = SCOPE_IDENTITY();
        INSERT INTO authors (first_name, last_name) VALUES (@firstName, @lastName)
        INSERT INTO books_authors (FK_book, FK_author)
        VALUES (@BookID, SCOPE_IDENTITY());
        COMMIT TRANSACTION;";   
      
      using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
      using SqlCommand command = new SqlCommand();

      command.Connection = connection;
      command.CommandText = query;
      command.Parameters.AddWithValue("@title", newBookAuthor.title);
      command.Parameters.AddWithValue("@firstName", newBookAuthor.Author.firstName);
      command.Parameters.AddWithValue("@lastName", newBookAuthor.Author.lastName);

      await connection.OpenAsync();
      await command.ExecuteNonQueryAsync();
    }
}