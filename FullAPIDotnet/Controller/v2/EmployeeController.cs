using Asp.Versioning;
using AutoMapper;
using FullAPIDotnet.Application.ViewModel;
using FullAPIDotnet.Domain.DTOs;
using FullAPIDotnet.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FullAPIDotnet.Controller.v2;

[ApiController]
[ApiVersion(2.0)]
[Route("api/v{version:apiVersion}/employee")]
[Authorize]
// extender ControllerBase define a classe como controller
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    // define um logger para exibir no console
    private readonly ILogger<EmployeeController> _logger;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    // define a rota como post
    [HttpPost]
    // FromForm define que o dado vai ser recebido como Form ao invés de Json
    public IActionResult Add([FromForm] EmployeeViewModel employeeViewModel)
    {
        var filePath = Path.Combine("Storage", employeeViewModel.Photo.FileName);
        var employee = new Employee(employeeViewModel.Name, employeeViewModel.Age, filePath);

        // Stream permite a manipulação de arquivo
        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        employeeViewModel.Photo.CopyTo(fileStream);

        _employeeRepository.Add(employee);

        return Ok("Success on create!");
    }

    [HttpGet]
    public IActionResult Get(int pageNumber, int pageQuantity)
    {
        var employees = _employeeRepository.Get(pageNumber, pageQuantity);

        return Ok(employees);
    }

    [HttpPost]
    [Route("{id}/download")]
    public IActionResult DownloadPhoto(Guid id)
    {
        var employee = _employeeRepository.Get(id);

        if (employee?.Photo == null)
        {
            return NotFound("There is no photo of the employee");
        }

        var dataBytes = System.IO.File.ReadAllBytes(employee.Photo);

        return File(dataBytes, "image/png");
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Search(Guid id)
    {
        var employees = _employeeRepository.Get(id);

        // faz o auto mapeamento para converter Employee para EmployeeDTO
        var employessDtos = _mapper.Map<EmployeeDto>(employees);

        return Ok(employessDtos);
    }
}