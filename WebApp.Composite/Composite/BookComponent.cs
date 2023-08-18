namespace WebApp.Composite.Composite
{
    public class BookComponent : IComponent
    {
        public BookComponent(string name, int id)
        {
            Name = name;
            Id = id;

        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int Count()
        {
            return 1;
        }

        public string Display()
        {
            return $"<li class='list-group-item'>{Name}</li>";
        }
    }
}
