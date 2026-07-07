using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Invoice;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

uusing Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Invoices;
using BusinessCore.Application.Interfaces;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> GetById(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura obtenida exitosamente"));
        }

        [HttpGet("number/{invoiceNumber}")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> GetByInvoiceNumber(string invoiceNumber)
        {
            var invoice = await _invoiceService.GetByInvoiceNumberAsync(invoiceNumber);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura obtenida exitosamente"));
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<InvoiceResponseDto>>>> GetOrderInvoices(int orderId)
        {
            var invoices = await _invoiceService.GetOrderInvoiceAsync(orderId);
            return Ok(new ApiResponseDto<IEnumerable<InvoiceResponseDto>>(invoices, "Facturas de la orden obtenidas exitosamente"));
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<InvoiceResponseDto>>>> GetCustomerInvoices(int customerId)
        {
            var invoices = await _invoiceService.GetCustomerInvoicesAsync(customerId);
            return Ok(new ApiResponseDto<IEnumerable<InvoiceResponseDto>>(invoices, "Facturas del cliente obtenidas exitosamente"));
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<InvoiceResponseDto>>>> GetOverdue()
        {
            var invoices = await _invoiceService.GetOverdueInvoicesAsync();
            return Ok(new ApiResponseDto<IEnumerable<InvoiceResponseDto>>(invoices, "Facturas vencidas obtenidas exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<InvoiceResponseDto>>>> GetPaged([FromQuery] InvoiceFilterDto filter)
        {
            var result = await _invoiceService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<InvoiceResponseDto>>(result, "Facturas paginadas obtenidas exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> Create([FromBody] InvoiceCreateDto createDto)
        {
            var invoice = await _invoiceService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = invoice.Id },
                new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura creada exitosamente"));
        }

        [HttpPost("generate-from-order/{orderId}")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> GenerateFromOrder(int orderId)
        {
            var invoice = await _invoiceService.GenerateInvoiceFromOrderAsync(orderId);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura generada desde orden exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> Update(int id, [FromBody] InvoiceUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<InvoiceResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var invoice = await _invoiceService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura actualizada exitosamente"));
        }

        [HttpPost("{id}/send")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> SendInvoice(int id)
        {
            var invoice = await _invoiceService.SendInvoiceAsync(id);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura enviada exitosamente"));
        }

        [HttpPost("{id}/mark-paid")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> MarkAsPaid(int id, [FromBody] decimal amount)
        {
            var invoice = await _invoiceService.MarkAsPaidAsync(id, amount);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura marcada como pagada exitosamente"));
        }

        [HttpPost("{id}/mark-overdue")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> MarkAsOverdue(int id)
        {
            var invoice = await _invoiceService.MarkAsOverdueAsync(id);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura marcada como vencida exitosamente"));
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ApiResponseDto<InvoiceResponseDto>>> CancelInvoice(int id, [FromBody] string reason)
        {
            var invoice = await _invoiceService.CancelInvoiceAsync(id, reason);
            return Ok(new ApiResponseDto<InvoiceResponseDto>(invoice, "Factura cancelada exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _invoiceService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Factura eliminada exitosamente"));
        }

        [HttpGet("{id}/pdf")]
        public async Task<ActionResult<ApiResponseDto<string>>> GeneratePdf(int id)
        {
            var pdfUrl = await _invoiceService.GenerateInvoicePdfAsync(id);
            return Ok(new ApiResponseDto<string>(pdfUrl, "PDF de factura generado exitosamente"));
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponseDto<InvoiceStatisticsDto>>> GetStatistics(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stats = await _invoiceService.GetInvoiceStatisticsAsync(startDate, endDate);
            return Ok(new ApiResponseDto<InvoiceStatisticsDto>(stats, "Estadísticas de facturas obtenidas exitosamente"));
        }

        [HttpGet("total-outstanding")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalOutstanding()
        {
            var total = await _invoiceService.GetTotalOutstandingAsync();
            return Ok(new ApiResponseDto<decimal>(total, "Total pendiente de cobro obtenido exitosamente"));
        }
    }
}