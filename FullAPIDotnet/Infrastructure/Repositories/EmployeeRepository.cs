using FullAPIDotnet.Domain.DTOs;
using FullAPIDotnet.Domain.Model;

namespace FullAPIDotnet.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    // chama o context para ter acesso ao banco
    private readonly ConnectionContext _context = new ConnectionContext();

    // private readonly ConnectionContext _context;
    // public EmployeeRepository(ConnectionContext context)
    // {
    //     _context = context;
    // }

    public void Add(Employee employee)
    {
        // atraves do Add, já é criado o novo registro na tabela;
        _context.Employee.Add(employee);

        // é necessario executar o SaveChange para toda alteração no banco
        _context.SaveChanges();
    }

    public List<EmployeeDto> Get(int pageNumber, int pageQuantity)
    {
        return _context.Employee
            // skip pula determinada quantidade de registros
            .Skip(pageNumber * pageQuantity)
            // take limita a quantidade de registros retornados
            .Take(pageQuantity)
            .Select(
                employee => new EmployeeDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Photo = employee.Photo
                }
            )
            .ToList();
    }

    public Employee? Get(Guid id)
    {
        return _context.Employee.Find(id);
    }
}