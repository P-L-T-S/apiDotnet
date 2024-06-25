// model representa a tabela do banco
// os atributos da classe representam uma coluna

namespace FullAPIDotnet.Domain.Model;

public class Employee
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    public string? Photo { get; private set; }

    public Employee()
    {
    }

    public Employee(string name, int age, string photo)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Age = age;
        Photo = photo;
    }
}