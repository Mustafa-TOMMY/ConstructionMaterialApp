using ConstructionMaterial.DAL.Infra;
using ConstructionMaterial.DAL.Models;
using ConstructionMaterial.DAL.Models.Enum;
using ConstructionMaterial.BLL.DTOs;
using ConstructionMaterial.BLL.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionMaterial.BLL
{
    public class OrderService : IOrderService
    {
        private readonly IMyAppRepo _myAppRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="myAppRepo">The repository layer dependency used to load and save data.</param>
        public OrderService(IMyAppRepo myAppRepo)
        {
            _myAppRepo = myAppRepo;
        }

        public void AddOrder(OrderDto order)
        {
            var data = _myAppRepo.LoadFromJson();
            var orderMapper = new Order()
            {
                Id = data.Orders.Any()
                    ? data.Orders.Max(m => m.Id) + 1
                    : 1,
                OrderNumber = order.OrderNumber,
                MaterialName = order.MaterialName,
                Category = (MaterialType)Enum.Parse(typeof(MaterialType), order.Category),
                ElementType = (ElementType)Enum.Parse(typeof(ElementType), order.ElementType),
                Quantity = order.Quantity,
                Unit = order.Unit,
                UnitPrice = order.UnitPrice,
                Date = order.Date,
                Status = order.Status
            };
            data.Orders.Add(orderMapper);
            _myAppRepo.SaveToJson(data);
        }

        public List<OrderDto> GetAllOrders()
        {
            var data = _myAppRepo.LoadFromJson();
            var orders = data.Orders;
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                MaterialName = o.MaterialName,
                Category = o.Category.ToString(),
                ElementType = o.ElementType.ToString(),
                Quantity = o.Quantity,
                Unit = o.Unit,
                UnitPrice = o.UnitPrice,
                Total = o.Total,
                Date = o.Date,
                Status = o.Status
            }).ToList();
        }

        public OrderDto GetOrderById(int orderId)
        {
            var data = _myAppRepo.LoadFromJson();
            var order = data.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                var orderMapper = new OrderDto()
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    MaterialName = order.MaterialName,
                    Category = order.Category.ToString(),
                    ElementType = order.ElementType.ToString(),
                    Quantity = order.Quantity,
                    Unit = order.Unit,
                    UnitPrice = order.UnitPrice,
                    Total = order.Total,
                    Date = order.Date,
                    Status = order.Status
                };
                return orderMapper;
            }
            else
            {
                throw new Exception($"Order with ID '{orderId}' not found.");
            }
        }

        public void RemoveOrder(int orderId)
        {
            var data = _myAppRepo.LoadFromJson();
            var orderToRemove = data.Orders.FirstOrDefault(o => o.Id == orderId);
            if (orderToRemove == null)
            {
                throw new Exception($"Order with ID '{orderId}' not found.");
            }
            data.Orders.Remove(orderToRemove);
            _myAppRepo.SaveToJson(data);
        }

        public void UpdateOrder(OrderDto order)
        {
            var data = _myAppRepo.LoadFromJson();
            var existingOrder = data.Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID '{order.Id}' not found.");
            }

            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.MaterialName = order.MaterialName;
            existingOrder.Category = (MaterialType)Enum.Parse(typeof(MaterialType), order.Category);
            existingOrder.ElementType = (ElementType)Enum.Parse(typeof(ElementType), order.ElementType);
            existingOrder.Quantity = order.Quantity;
            existingOrder.Unit = order.Unit;
            existingOrder.UnitPrice = order.UnitPrice;
            existingOrder.Date = order.Date;
            existingOrder.Status = order.Status;

            _myAppRepo.SaveToJson(data);
        }
    }
}
