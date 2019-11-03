using EzTask.Entity.Data;
using EzTask.Framework.ImageHandler;
using EzTask.Framework.IO;
using EzTask.Framework.Security;
using EzTask.Interface;
using EzTask.Model;
using EzTask.Model.Enum;
using EzTask.Plugin.MessageService.Data.Email;
using EzTask.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EzTask.Business
{
    public class AccountBusiness : BusinessCore
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly IWebEnvironment _hostEnvironment;
        private readonly IMessageCenter _mesageCenter;

        public AccountBusiness(ImageProcessor imageProcessor,
           UnitOfWork unitOfWork, IMessageCenter mesageCenter, IWebEnvironment hostEnvironment) : base(unitOfWork)
        {
            _imageProcessor = imageProcessor;
            _hostEnvironment = hostEnvironment;
            _mesageCenter = mesageCenter;
        }

        /// <summary>
        /// Register a new account to EzTask
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountModel>> RegisterNew(AccountModel model)
        {
            ResultModel<AccountModel> result = new ResultModel<AccountModel>
            {
                Status = ActionStatus.Failed
            };

            var account = model.ToEntity();

            if (account.Id < 1)
            {
                account.CreatedDate = DateTime.Now;
            }

            account.PasswordHash = Cryptography.GetHashString(account.AccountName);
            account.Password = Encrypt.Do(account.Password,
                account.PasswordHash);

            account.UpdatedDate = DateTime.Now;
            account.AccountStatus = (int)AccountStatus.Active;

            UnitOfWork.AccountRepository.Add(account);
            int insertedRecord = await UnitOfWork.CommitAsync();

            if (insertedRecord > 0)
            {
                result.Status = ActionStatus.Ok;
                result.Data = account.ToModel();
            }

            return result;
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResultModel<AccountInfoModel>> UpdateAccount(AccountInfoModel model)
        {
            ResultModel<AccountInfoModel> result = new ResultModel<AccountInfoModel>
            {
                Status = ActionStatus.NotFound
            };

            var entity = model.ToEntity();

            var accountInfo = await UnitOfWork
                .AccountInfoRepository
                .GetAsync(c => c.Id == model.AccountInfoId);

            if (accountInfo != null)
            {
                accountInfo.Update(entity);

                int updateRecord = await UnitOfWork.CommitAsync();
                if (updateRecord > 0)
                {
                    result.Status = ActionStatus.Ok;
                    result.Data = accountInfo.ToModel();
                }
            }
            return result;
        }

        /// <summary>
        /// Update password
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<AccountModel>> UpdatePassword(string accountName, string password, 
            string newPassword, bool isRecoverMode = false)
        {
            ResultModel<AccountModel> result = new ResultModel<AccountModel>
            {
                Status = ActionStatus.NotFound
            };

            var account = await UnitOfWork
                .AccountRepository
                .GetAsync(c => c.AccountName == accountName);

            if (account != null)
            {
                var oldPassword = Encrypt.Do(password, account.PasswordHash);

                if(isRecoverMode)
                {
                    oldPassword = account.Password;
                }

                if (oldPassword == account.Password)
                {
                    account.Password = Encrypt.Do(newPassword, account.PasswordHash);

                    UnitOfWork.AccountRepository.Update(account);

                    int updateRecord = await UnitOfWork.CommitAsync();
                    if (updateRecord > 0)
                    {
                        result.Status = ActionStatus.Ok;
                        result.Data = account.ToModel();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Update avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> UpdateAvatar(int accountId, Stream stream)
        {
            ResultModel<bool> result = new ResultModel<bool>
            {
                Status = ActionStatus.Failed
            };

            var accountInfo = await UnitOfWork
                .AccountInfoRepository
                .GetAsync(c => c.AccountId == accountId);

            if (accountInfo != null)
            {
                byte[] bytes = await stream.ConvertStreamToBytes();
                byte[] imageData = await _imageProcessor.CreateNewSize(bytes, 250, 250);
                stream.Dispose();

                accountInfo.DisplayImage = imageData;

                int iResult = await UnitOfWork.CommitAsync();
                if (iResult > 0)
                {
                    result.Status = ActionStatus.Ok;
                    result.Data = true;
                }
            }
            else
            {
                result.Status = ActionStatus.NotFound;
            }
            return result;
        }

        /// <summary>
        /// Get account information
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountInfo(int accountId)
        {
            var data = await UnitOfWork.AccountInfoRepository
                .Entity.Include(c => c.Account)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            return data.ToModel();
        }

        /// <summary>
        /// Get account by account name and password
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccount(string accountName, string password)
        {
            var accountInfo = await UnitOfWork.AccountInfoRepository
                .Entity.Include(c => c.Account).AsNoTracking()
                .FirstOrDefaultAsync(c => c.Account.AccountName == accountName && c.Account.Password == password);

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Get account by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountByEmail(string email)
        {
            var accountInfo = await UnitOfWork.AccountInfoRepository
                .Entity.Include(c => c.Account)
                .Where(c => c.Email == email)
                .Select(x => new AccountInfo
                {
                    DisplayName = x.DisplayName,
                    AccountId = x.AccountId,
                    Account = new Account { AccountName = x.Account.AccountName }
                }).FirstOrDefaultAsync();

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> Login(AccountModel model)
        {
            string hash = Cryptography.GetHashString(model.AccountName);
            model.Password = Encrypt.Do(model.Password, hash);

            AccountInfoModel account = await GetAccount(model.AccountName, model.Password);
            return account;
        }

        /// <summary>
        /// Get account by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<AccountModel> GetAccount(string accountName)
        {
            var data = await UnitOfWork.AccountRepository.GetAsync(c => c.AccountName == accountName);
            return data.ToModel();
        }

        /// <summary>
        /// Get account info by account name
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public async Task<AccountInfoModel> GetAccountInfo(string accountName)
        {
            var accountInfo = await UnitOfWork.AccountInfoRepository
                        .Entity
                        .Include(c => c.Account)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Account.AccountName == accountName);

            return accountInfo.ToModel();
        }

        /// <summary>
        /// Load avatar
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<byte[]> LoadAvatar(int accountId)
        {
            var data = await UnitOfWork.AccountInfoRepository.GetAsync(c =>
                    c.AccountId == accountId, allowTracking: false);

            return data.DisplayImage;
        }

        /// <summary>
        /// Get list account
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="manageUserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AccountModel>> GetAccountList(int page, int pageSize,
            int manageUserId)
        {
            var data = await UnitOfWork.AccountRepository
                .Entity
                .Where(c => c.ManageAccountId == manageUserId)
                .Skip(pageSize * page - pageSize)
                .Take(pageSize)
                .ToListAsync();

            return data.ToModels();
        }

        /// <summary>
        /// Send recover password email
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public void SendRecoverLink(string email, string name, string title, Guid activeCode)
        {
            string emailTemplateUrl = _hostEnvironment.GetRootContentUrl()
                      + "/resources/templates/password_recover.html";

            string emailContent = StreamIO.ReadFile(emailTemplateUrl);
            emailContent = emailContent.Replace("{UserName}", name);
            emailContent = emailContent.Replace("{Url}", "http://localhost:52767/change-password.html?code=" + activeCode).ToString();

            _mesageCenter.Push(new EmailMessage
            {
                Content = emailContent,
                Title = title,
                To = email
            });
        }

        /// <summary>
        /// Create new recover account session
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ResultModel<RecoverSessionModel>> CreateRecoverSession(string email)
        {
            ResultModel<RecoverSessionModel> result = new ResultModel<RecoverSessionModel>();
            var account = await GetAccountByEmail(email);
            if (account == null)
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {

                var newCode = Guid.NewGuid();
                var session = new RecoverSession
                {
                    AccountId = account.AccountId,
                    ExpiredTime = DateTime.Now.AddMinutes(15),
                    Code = newCode
                };

                var existItems = await UnitOfWork.RecoverSessionRepository
                    .GetManyAsync(c => c.AccountId == account.AccountId);

                if(existItems.Any())
                {
                    UnitOfWork.RecoverSessionRepository.DeleteRange(existItems);
                }

                UnitOfWork.RecoverSessionRepository.Add(session);

                var iResult = await UnitOfWork.CommitAsync();
                if (iResult > 0)
                {
                    session.Account = new Account
                    {
                        Id = account.AccountId,
                        AccountName = account.AccountName,
                        AccountInfo = new AccountInfo { DisplayName = account.DisplayName }
                    };

                    result.Status = ActionStatus.Ok;
                    result.Data = session.ToModel();
                }
            }
            return result;
        }

        /// <summary>
        /// get recover session by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResultModel<RecoverSessionModel>> GetRecoverSession(string code)
        {
            ResultModel<RecoverSessionModel> result = new ResultModel<RecoverSessionModel>();

            if (string.IsNullOrEmpty(code))
                return result;

            var data = await UnitOfWork.RecoverSessionRepository
                .Entity.Include(c => c.Account)
                .Where(c => c.Code == new Guid(code))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(data == null)
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {
                result.Status = ActionStatus.Ok;
                result.Data = data.ToModel();
            }

            return result;
        }

        /// <summary>
        /// delete recover session by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResultModel<bool>> DeleteRecoverSession(string code)
        {
            ResultModel<bool> result = new ResultModel<bool>();

            if (string.IsNullOrEmpty(code))
                return result;

            var data = await UnitOfWork
                .RecoverSessionRepository
                .GetAsync(c => c.Code == new Guid(code), allowTracking: false);

            if (data == null)
            {
                result.Status = ActionStatus.NotFound;
            }
            else
            {
                result.Status = ActionStatus.Ok;
                UnitOfWork.RecoverSessionRepository.Delete(data);
                await UnitOfWork.CommitAsync();
            }

            return result;
        }
    }
}