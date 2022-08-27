namespace Bookstore.Domain.Entities;

public class Author
{
    public long Id { get; set; }
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
    public List<Book> Books { get; set; } = default!;
}