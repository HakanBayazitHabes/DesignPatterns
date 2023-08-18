using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace WebApp.Composite.Composite
{
    public class BookComposit : IComponent
    {
        public BookComposit(string name, int id)
        {
            Name = name;
            Id = id;
            _components = new List<IComponent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        private List<IComponent> _components;


        public IReadOnlyCollection<IComponent> Components => _components;
        public void Add(IComponent component)
        {
            _components.Add(component);
        }

        public void Remove(IComponent component)
        {
            _components.Remove(component);
        }

        public int Count()
        {
            return _components.Sum(x => x.Count());
        }

        public string Display()
        {
            var sb = new StringBuilder();
            sb.Append($"<div class='text-primary my-1'><a href='#' class='menu'> {Name} ({Count()})  </a></div>");

            if (!_components.Any()) return sb.ToString();

            sb.Append("<ul class='list-group list-group-flush ms-3'>");

            foreach (var item in _components)
            {
                sb.Append(item.Display());
            }

            sb.Append("</ul>");
            return sb.ToString();

        }

        public List<SelectListItem> GetSelectListItems(string line)
        {
            var list = new List<SelectListItem> { new($"{line}{Name}", Id.ToString()) };

            if (_components.Any(x => x is BookComposit))
            {
                line += " - ";
            }


            _components.ForEach(x =>
            {
                if (x is BookComposit bookComposit)
                {
                    list.AddRange(bookComposit.GetSelectListItems(line));
                }
            });

            return list;

        }
    }
}
