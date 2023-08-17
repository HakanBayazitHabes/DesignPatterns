using System.Collections;
namespace WebApp.Composite.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public ICollection<Book> Books;
    }
}