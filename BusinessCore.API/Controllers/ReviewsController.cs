using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Review;
using BusinessCore.Application.DTOs.Reviews;
using BusinessCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ReviewResponseDto>>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            return Ok(new ApiResponseDto<ReviewResponseDto>(review, "Reseña obtenida exitosamente"));
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReviewResponseDto>>>> GetProductReviews(int productId)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId);
            return Ok(new ApiResponseDto<IEnumerable<ReviewResponseDto>>(reviews, "Reseñas del producto obtenidas exitosamente"));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReviewResponseDto>>>> GetUserReviews(int userId)
        {
            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            return Ok(new ApiResponseDto<IEnumerable<ReviewResponseDto>>(reviews, "Reseñas del usuario obtenidas exitosamente"));
        }

        [HttpGet("pending")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReviewResponseDto>>>> GetPending()
        {
            var reviews = await _reviewService.GetPendingReviewsAsync();
            return Ok(new ApiResponseDto<IEnumerable<ReviewResponseDto>>(reviews, "Reseñas pendientes obtenidas exitosamente"));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponseDto<PagedResultDto<ReviewResponseDto>>>> GetPaged([FromQuery] ReviewFilterDto filter)
        {
            var result = await _reviewService.GetPagedAsync(filter);
            return Ok(new ApiResponseDto<PagedResultDto<ReviewResponseDto>>(result, "Reseñas paginadas obtenidas exitosamente"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<ReviewResponseDto>>> Create([FromBody] ReviewCreateDto createDto)
        {
            var review = await _reviewService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = review.Id },
                new ApiResponseDto<ReviewResponseDto>(review, "Reseña creada exitosamente"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<ReviewResponseDto>>> Update(int id, [FromBody] ReviewUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest(new ApiResponseDto<ReviewResponseDto>("El ID de la URL no coincide con el ID del objeto", 400));

            var review = await _reviewService.UpdateAsync(updateDto);
            return Ok(new ApiResponseDto<ReviewResponseDto>(review, "Reseña actualizada exitosamente"));
        }

        [HttpPost("{id}/approve")]
        public async Task<ActionResult<ApiResponseDto<ReviewResponseDto>>> Approve(int id)
        {
            var review = await _reviewService.ApproveReviewAsync(id);
            return Ok(new ApiResponseDto<ReviewResponseDto>(review, "Reseña aprobada exitosamente"));
        }

        [HttpPost("{id}/reject")]
        public async Task<ActionResult<ApiResponseDto<ReviewResponseDto>>> Reject(int id, [FromBody] string reason)
        {
            var review = await _reviewService.RejectReviewAsync(id, reason);
            return Ok(new ApiResponseDto<ReviewResponseDto>(review, "Reseña rechazada exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(int id)
        {
            var result = await _reviewService.DeleteAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Reseña eliminada exitosamente"));
        }

        [HttpGet("product/{productId}/average-rating")]
        public async Task<ActionResult<ApiResponseDto<double>>> GetAverageRating(int productId)
        {
            var average = await _reviewService.GetAverageRatingAsync(productId);
            return Ok(new ApiResponseDto<double>(average, "Calificación promedio obtenida exitosamente"));
        }

        [HttpGet("product/{productId}/review-count")]
        public async Task<ActionResult<ApiResponseDto<int>>> GetReviewCount(int productId)
        {
            var count = await _reviewService.GetReviewCountAsync(productId);
            return Ok(new ApiResponseDto<int>(count, "Conteo de reseñas obtenido exitosamente"));
        }

        [HttpGet("product/{productId}/rating-distribution")]
        public async Task<ActionResult<ApiResponseDto<RatingDistributionDto>>> GetRatingDistribution(int productId)
        {
            var distribution = await _reviewService.GetRatingDistributionAsync(productId);
            return Ok(new ApiResponseDto<RatingDistributionDto>(distribution, "Distribución de calificaciones obtenida exitosamente"));
        }

        [HttpGet("product/{productId}/top/{count}")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<ReviewResponseDto>>>> GetTopReviews(int productId, int count)
        {
            var reviews = await _reviewService.GetTopReviewsAsync(productId, count);
            return Ok(new ApiResponseDto<IEnumerable<ReviewResponseDto>>(reviews, "Mejores reseñas obtenidas exitosamente"));
        }

        [HttpPost("{id}/report")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ReportReview(int id, [FromBody] string reason)
        {
            var result = await _reviewService.ReportReviewAsync(id, reason);
            return Ok(new ApiResponseDto<bool>(result, "Reseña reportada exitosamente"));
        }

        [HttpPost("{id}/mark-verified")]
        public async Task<ActionResult<ApiResponseDto<bool>>> MarkVerified(int id)
        {
            var result = await _reviewService.MarkAsVerifiedPurchaseAsync(id);
            return Ok(new ApiResponseDto<bool>(result, "Reseña marcada como compra verificada exitosamente"));
        }
    }
}