using FullAPIDotnet.Domain.DTOs;

namespace FullAPIDotnet.Domain.Model;

public interface IEmployeeRepository
{
    void Add(Employee employee);

    List<EmployeeDto> Get(int pageNumber, int pageQuantity);

    Employee? Get(Guid id);
}