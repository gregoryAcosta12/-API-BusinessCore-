using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Payment;
using BusinessCore.Application.DTOs.Payments;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Pago obtenido exitosamente"));
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> GetByTransactionId(string transactionId)
        {
            var payment = await _paymentService.GetByTransactionIdAsync(transactionId);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Pago obtenido exitosamente"));
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<PaymentResponseDto>>>> GetOrderPayments(int orderId)
        {
            var payments = await _paymentService.GetOrderPaymentsAsync(orderId);
            return Ok(new ApiResponseDto<IEnumerable<PaymentResponseDto>>(payments, "Pagos de la orden obtenidos exitosamente"));
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<PaymentResponseDto>>>> GetByStatus(PaymentStatus status)
        {
            var payments = await _paymentService.GetPaymentsByStatusAsync(status);
            return Ok(new ApiResponseDto<IEnumerable<PaymentResponseDto>>(payments, $"Pagos con estado {status} obtenidos exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<PaymentResponseDto>>>> GetPaged([FromQuery] PaymentFilterDto filter)
        {
            var result = await _paymentService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<PaymentResponseDto>>(result, "Pagos paginados obtenidos exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> Create([FromBody] PaymentCreateDto createDto)
        {
            var payment = await _paymentService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id },
                new ApiResponseDto<PaymentResponseDto>(payment, "Pago creado exitosamente"));
        }

        [HttpPost("process")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> ProcessPayment([FromBody] PaymentProcessDto processDto)
        {
            var payment = await _paymentService.ProcessPaymentAsync(processDto);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Pago procesado exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> Update(int id, [FromBody] PaymentUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<PaymentResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var payment = await _paymentService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Pago actualizado exitosamente"));
        }

        [HttpPost("{id}/confirm")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> ConfirmPayment(int id)
        {
            var payment = await _paymentService.ConfirmPaymentAsync(id);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Pago confirmado exitosamente"));
        }

        [HttpPost("{id}/refund")]
        public async Task<ActionResult<ApiResponseDto<PaymentResponseDto>>> RefundPayment(
            int id, [FromBody] RefundRequestDto refundRequest)
        {
            var payment = await _paymentService.RefundPaymentAsync(id, refundRequest.Amount, refundRequest.Reason);
            return Ok(new ApiResponseDto<PaymentResponseDto>(payment, "Reembolso realizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _paymentService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Pago eliminado exitosamente"));
        }

        [HttpGet("order/{orderId}/total")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalPayments(int orderId)
        {
            var total = await _paymentService.GetTotalPaymentsAsync(orderId);
            return Ok(new ApiResponseDto<decimal>(total, "Total de pagos obtenido exitosamente"));
        }

        [HttpGet("order/{orderId}/paid")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> GetTotalPaid(int orderId)
        {
            var total = await _paymentService.GetTotalPaidAsync(orderId);
            return Ok(new ApiResponseDto<decimal>(total, "Total pagado obtenido exitosamente"));
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponseDto<PaymentSummaryDto>>> GetSummary(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _paymentService.GetPaymentSummaryAsync(startDate, endDate);
            return Ok(new ApiResponseDto<PaymentSummaryDto>(summary, "Resumen de pagos obtenido exitosamente"));
        }
    }

    public class RefundRequestDto
    {
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}