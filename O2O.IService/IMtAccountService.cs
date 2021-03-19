using O2O.DTO.Meituan;
using System;
using System.Collections.Generic;

namespace O2O.IService
{
    public interface IMtAccountService : IServiceSupport
    {
        Mt_AccountDTO GetAccount(string userId);

        Mt_AccountDTO GetAccount(Guid id);

        List<Mt_AccountDTO> GetAccountAll();

        List<Mt_AccountDTO> GetList(string userId);

        void Add(
          string userId,
          string accountNo,
          string accountName,
          string waimaiAppId,
          string waimaiAppSecret,
          string tuangouAppKey,
          string tuangouAppSecret,
          string description);

        void Update(
          Guid id,
          string userId,
          string accountNo,
          string accountName,
          string waimaiAppId,
          string waimaiAppSecret,
          string tuangouAppKey,
          string tuangouAppSecret,
          string description);

        void Delete(Guid id);
    }
}
