using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessCore.Application.DTOs.Common;
using BusinessCore.Application.DTOs.Inventory;
using BusinessCore.Application.Interfaces;
using BusinessCore.Domain.Entities;
using BusinessCore.Domain.Enums;
using BusinessCore.Domain.Exceptions;
using BusinessCore.Domain.Interfaces;

namespace BusinessCore.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public InventoryService(IInventoryRepository repository, IProductRepository productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<InventoryMovementResponseDto> GetByIdAsync(int id)
        {
            var movement = await _repository.GetByIdAsync(id);
            if (movement == null)
                throw new NotFoundException($"Movimiento de inventario con ID {id} no encontrado");

            return _mapper.Map<InventoryMovementResponseDto>(movement);
        }

        public async Task<IEnumerable<InventoryMovementResponseDto>> GetMovementsByProductIdAsync(int productId)
        {
            var movements = await _repository.GetMovementsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<InventoryMovementResponseDto>>(movements);
        }

        public async Task<IEnumerable<InventoryMovementResponseDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var movements = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<InventoryMovementResponseDto>>(movements);
        }

        public async Task<PagedResultDto<InventoryMovementResponseDto>> GetPagedAsync(InventoryFilterDto filter)
        {
            var query = _repository.GetAllAsync();
            var movements = await query;

            if (filter.ProductId.HasValue)
                movements = movements.Where(m => m.ProductId == filter.ProductId);

            if (filter.ProductVariantId.HasValue)
                movements = movements.Where(m => m.ProductVariantId == filter.ProductVariantId);

            if (filter.WarehouseId.HasValue)
                movements = movements.Where(m => m.SourceWarehouseId == filter.WarehouseId || m.TargetWarehouseId == filter.WarehouseId);

            if (filter.Type.HasValue)
                movements = movements.Where(m => m.Type == filter.Type);

            if (filter.StartDate.HasValue)
                movements = movements.Where(m => m.MovementDate >= filter.StartDate);

            if (filter.EndDate.HasValue)
                movements = movements.Where(m => m.MovementDate <= filter.EndDate);

            if (!string.IsNullOrEmpty(filter.Reference))
                movements = movements.Where(m => m.Reference.Contains(filter.Reference));

            var totalCount = movements.Count();
            var items = movements
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            return new PagedResultDto<InventoryMovementResponseDto>
            {
                Items = _mapper.Map<IEnumerable<InventoryMovementResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<InventoryMovementResponseDto> CreateMovementAsync(InventoryMovementCreateDto createDto)
        {
            var product = await _productRepository.GetByIdAsync(createDto.ProductId);
            if (product == null)
                throw new NotFoundException($"Producto con ID {createDto.ProductId} no encontrado");

            if (createDto.Quantity <= 0)
                throw new BadRequestException("La cantidad debe ser mayor a 0");

            var movement = _mapper.Map<InventoryMovement>(createDto);
            movement.MovementDate = DateTime.UtcNow;

            // Actualizar stock del producto
            if (createDto.Type == MovementType.Purchase || createDto.Type == MovementType.Return || createDto.Type == MovementType.Adjustment)
            {
                product.Stock += createDto.Quantity;
            }
            else if (createDto.Type == MovementType.Sale || createDto.Type == MovementType.Waste)
            {
                if (product.Stock < createDto.Quantity)
                    throw new BusinessException($"Stock insuficiente para el producto {product.Name}");

                product.Stock -= createDto.Quantity;
            }

            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);

            var created = await _repository.CreateMovementAsync(movement);
            return _mapper.Map<InventoryMovementResponseDto>(created);
        }

        public async Task<bool> DeleteMovementAsync(int id)
        {
            var movement = await _repository.GetByIdAsync(id);
            if (movement == null)
                throw new NotFoundException($"Movimiento de inventario con ID {id} no encontrado");

            // Revertir stock
            var product = await _productRepository.GetByIdAsync(movement.ProductId);
            if (product != null)
            {
                if (movement.Type == MovementType.Purchase || movement.Type == MovementType.Return || movement.Type == MovementType.Adjustment)
                    product.Stock -= movement.Quantity;
                else if (movement.Type == MovementType.Sale || movement.Type == MovementType.Waste)
                    product.Stock += movement.Quantity;

                product.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);
            }

            return true;
        }

        public async Task<bool> AdjustStockAsync(int productId, int quantity, string reason)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException($"Producto con ID {productId} no encontrado");

            var movement = new InventoryMovement
            {
                ProductId = productId,
                Type = MovementType.Adjustment,
                Quantity = Math.Abs(quantity),
                UnitCost = product.CostPrice ?? 0,
                Reference = $"AJUSTE-{DateTime.UtcNow:yyyyMMddHHmmss}",
                Notes = reason,
                MovementDate = DateTime.UtcNow
            };

            await _repository.CreateMovementAsync(movement);

            product.Stock = quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);

            return true;
        }

        public async Task<bool> TransferStockAsync(int productId, int sourceWarehouseId, int targetWarehouseId, int quantity)
        {
            return await _repository.TransferStockAsync(productId, sourceWarehouseId, targetWarehouseId, quantity);
        }

        public async Task<bool> ReceiveStockAsync(int productId, int quantity, decimal unitCost, string reference)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException($"Producto con ID {productId} no encontrado");

            var movement = new InventoryMovement
            {
                ProductId = productId,
                Type = MovementType.Purchase,
                Quantity = quantity,
                UnitCost = unitCost,
                Reference = reference ?? $"REC-{DateTime.UtcNow:yyyyMMddHHmmss}",
                MovementDate = DateTime.UtcNow
            };

            await _repository.CreateMovementAsync(movement);

            product.Stock += quantity;
            product.CostPrice = unitCost;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);

            return true;
        }

        public async Task<bool> ReleaseStockAsync(int productId, int quantity, string reference)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException($"Producto con ID {productId} no encontrado");

            if (product.Stock < quantity)
                throw new BusinessException($"Stock insuficiente para el producto {product.Name}");

            var movement = new InventoryMovement
            {
                ProductId = productId,
                Type = MovementType.Sale,
                Quantity = quantity,
                UnitCost = product.CostPrice ?? 0,
                Reference = reference ?? $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}",
                MovementDate = DateTime.UtcNow
            };

            await _repository.CreateMovementAsync(movement);

            product.Stock -= quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);

            return true;
        }

        public async Task<int> GetCurrentStockAsync(int productId)
        {
            return await _repository.GetCurrentStockAsync(productId);
        }

        public async Task<int> GetCurrentStockAsync(int productId, int warehouseId)
        {
            return await _repository.GetCurrentStockAsync(productId, warehouseId);
        }

        public async Task<IEnumerable<ProductStockDto>> GetLowStockProductsAsync(int threshold)
        {
            var products = await _productRepository.GetLowStockAsync(threshold);
            return _mapper.Map<IEnumerable<ProductStockDto>>(products);
        }

        public async Task<IEnumerable<ProductStockDto>> GetProductsWithStockAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductStockDto>>(products);
        }

        public async Task<InventoryStatisticsDto> GetInventoryStatisticsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productList = products.ToList();
            var movements = await _repository.GetMovementsByDateRangeAsync(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
            var movementList = movements.ToList();

            return new InventoryStatisticsDto
            {
                TotalProducts = productList.Count,
                TotalStock = productList.Sum(p => p.Stock),
                TotalInventoryValue = productList.Sum(p => p.Stock * (p.CostPrice ?? 0)),
                LowStockProducts = productList.Count(p => p.MinStock.HasValue && p.Stock <= p.MinStock.Value),
                OutOfStockProducts = productList.Count(p => p.Stock <= 0),
                TotalMovements = movementList.Count,
                MonthlyMovements = movementList.Count,
                MovementsByType = movementList.GroupBy(m => m.Type).ToDictionary(g => g.Key, g => g.Count())
            };
        }

        public async Task<IEnumerable<WarehouseStockDto>> GetWarehouseStockSummaryAsync()
        {
            
            return new List<WarehouseStockDto>();
        }

        public async Task<decimal> GetTotalInventoryValueAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Sum(p => p.Stock * (p.CostPrice ?? 0));
        }
    }
}