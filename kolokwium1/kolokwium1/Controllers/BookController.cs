using kolokwium1.Models.DTOs;
using kolokwium1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    [HttpGet]
    [Route("api/books/{id}/authors")]
    public async Task<IActionResult> GetAnimal(int id)
    {

        var bookAuthor = await _bookRepository.GetAuthorByID(id);
            
        return Ok(bookAuthor);
    }
    [HttpPost("api/books")]
    public async Task<IActionResult> AddProductToWarehouse([FromBody] BookAuthor newBookAuthor)
    {
        try
        {
            await _bookRepository.AddBookWithAuthor(newBookAuthor);
            return Ok("Book added.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}