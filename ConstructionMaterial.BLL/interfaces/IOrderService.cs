using ConstructionMaterial.BLL.DTOs;
using System;
using System.Collections.Generic;

namespace ConstructionMaterial.BLL.interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves all stored orders mapped into a list of Data Transfer Objects (DTOs).
        /// </summary>
        /// <returns>A list of <see cref="OrderDto"/> representing all available orders.</returns>
        List<OrderDto> GetAllOrders();

        /// <summary>
        /// Retrieves a specific order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <returns>A <see cref="OrderDto"/> containing the matching order's data.</returns>
        /// <exception cref="Exception">Thrown when no order matches the provided identifier.</exception>
        OrderDto GetOrderById(int orderId);

        /// <summary>
        /// Adds a new order to the data store by mapping a Data Transfer Object (DTO) to the core domain model.
        /// </summary>
        /// <param name="order">The order data transfer object containing the details to add.</param>
        void AddOrder(OrderDto order);

        /// <summary>
        /// Updates the attributes of an existing order based on the provided Data Transfer Object.
        /// </summary>
        /// <param name="order">The order data transfer object containing updated values and a valid ID.</param>
        /// <exception cref="Exception">Thrown when no order matches the provided DTO's identifier.</exception>
        void UpdateOrder(OrderDto order);

        /// <summary>
        /// Removes an order from the data store based on its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to be removed.</param>
        /// <exception cref="Exception">Thrown when no order matches the provided identifier.</exception>
        void RemoveOrder(int orderId);
    }
}
