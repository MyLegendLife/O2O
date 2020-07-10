using Newtonsoft.Json.Linq;
using O2O.DTO;
using O2O.Model;
using System;
using System.Collections.Generic;

namespace O2O.IService
{
    public interface IOrderService : IServiceSupport
    {
        void Add(OrderEntity entity);

        void Update(OrderEntity entity);

        void Update(OrderEntity entity, Dictionary<string, object> dic);

        void UpdateState(string orderId, int state);

        bool IsExist(string orderId);

        void SetBuy(string orderId);

        OrderEntity Get(string id);

        OrderEntity Get(Guid id);

        OrderEntity GetByOrderId(string orderId);

        List<OrderDTO> GetOrders(string userId, string shopNo, JObject query);

        List<OrderDtlDTO> GetOrderDtls(string orderId);
    }
}
