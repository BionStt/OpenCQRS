using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }

    public class CreateProduct2 : DomainCommand<Product>
    {
        public string Title { get; set; }
    }
}
