using Newtonsoft.Json.Linq;
using O2O.DTO;
using O2O.Model;
using O2O.Model.Entities;
using System;
using System.Collections.Generic;

namespace O2O.IService
{
    public interface IOrderService : IServiceSupport
    {
        void Add(OrderEntity entity);

        void Update(OrderEntity entity);

        void UpdateDtl(IEnumerable<OrderDtlEntity> entity);

        void Update(OrderEntity entity, Dictionary<string, object> dic);

        void UpdateState(string orderId, int state);

        void UpdateEntityFields(OrderEntity entity, List<string> fileds);

        bool IsExist(string orderId);

        void SetBuy(string orderId, int buyState);

        OrderEntity Get(string id);

        OrderEntity Get(Guid id);

        int GetCount(string userId);

        OrderEntity GetByOrderId(string orderId);

        //OrderInfoDTO GetInfoByOrderId(string orderId);

        List<OrderDTO> GetOrders(string userId, string shopNo, JObject query);

        List<OrderDtlDTO> GetOrderDtls(string orderId);

        int[] GetMissOrder(string userId, DateTime dateTime, string shopNo, int takeType);

        Dictionary<string, int[]> GetMissOrder(
          string userId,
          DateTime dateTime,
          int takeType);

        List<OrderDtlGroupDto> GetOrderProdList(
          string userId,
          string text);

        void UpdateProdNoMapInfo(string userId, string prodNo, string prodName, string prodNoNew);

        void DeleteProdNoMapInfo(string userId, string prodNo, string prodName);
    }
}
