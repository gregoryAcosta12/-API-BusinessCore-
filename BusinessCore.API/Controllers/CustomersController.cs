using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Customer;
using BusinessCore.Application.DTOs.Customers;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(new ApiResponseDto<IEnumerable<CustomerResponseDto>>(customers, "Clientes obtenidos exitosamente"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<CustomerResponseDto>(customer, "Cliente obtenido exitosamente"));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetByUserId(int userId)
        {
            var customer = await _customerService.GetByUserIdAsync(userId);
            return Ok(new ApiResponseDto<CustomerResponseDto>(customer, "Cliente obtenido exitosamente"));
        }

        [HttpGet("taxid/{taxId}")]
        public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> GetByTaxId(string taxId)
        {
            var customer = await _customerService.GetByTaxIdAsync(taxId);
            return Ok(new ApiResponseDto<CustomerResponseDto>(customer, "Cliente obtenido exitosamente"));
        }

        [HttpGet("with-balance")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<CustomerResponseDto>>>> GetWithBalance()
        {
            var customers = await _customerService.GetCustomersWithBalanceAsync();
            return Ok(new ApiResponseDto<IEnumerable<CustomerResponseDto>>(customers, "Clientes con saldo obtenidos exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<CustomerResponseDto>>>> GetPaged([FromQuery] CustomerFilterDto filter)
        {
            var result = await _customerService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<CustomerResponseDto>>(result, "Clientes paginados obtenidos exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> Create([FromBody] CustomerCreateDto createDto)
        {
            var customer = await _customerService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id },
                new ApiResponseDto<CustomerResponseDto>(customer, "Cliente creado exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<CustomerResponseDto>>> Update(int id, [FromBody] CustomerUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<CustomerResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var customer = await _customerService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<CustomerResponseDto>(customer, "Cliente actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Cliente eliminado exitosamente"));
        }

        [HttpGet("total-balance")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalBalance()
        {
            var balance = await _customerService.GetTotalCreditBalanceAsync();
            return Ok(new ApiResponseDto<decimal>(balance, "Saldo total de crédito obtenido exitosamente"));
        }

        [HttpGet("{id}/balance")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetBalance(int id)
        {
            var balance = await _customerService.GetCustomerBalanceAsync(id);
            return Ok(new ApiResponseDto<decimal>(balance, "Saldo del cliente obtenido exitosamente"));
        }

        [HttpPatch("{id}/credit-limit")]
        public async Task<ActionResult<ApiResponseDto<bool>>> UpdateCreditLimit(int id, [FromBody] decimal limit)
        {
            var result = await _customerService.UpdateCreditLimitAsync(id, limit);
            return Ok(new ApiResponseDto<bool>(result, "Límite de crédito actualizado exitosamente"));
        }

        [HttpPost("{id}/adjust-balance")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AdjustBalance(int id, [FromBody] AdjustBalanceDto adjustDto)
        {
            var result = await _customerService.AdjustBalanceAsync(id, adjustDto.Amount, adjustDto.Reason);
            return Ok(new ApiResponseDto<bool>(result, "Saldo ajustado exitosamente"));
        }

        [HttpPost("{id}/pay-balance")]
        public async Task<ActionResult<ApiResponseDto<bool>>> PayBalance(int id, [FromBody] decimal amount)
        {
            var result = await _customerService.PayBalanceAsync(id, amount);
            return Ok(new ApiResponseDto<bool>(result, "Pago de saldo realizado exitosamente"));
        }
    }

    public class AdjustBalanceDto
    {
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}