﻿using Hydra.Cms.Core.Domain;
using Hydra.Cms.Core.Interfaces;
using Hydra.Cms.Core.Models;
using Hydra.Infrastructure.Data.Extension;
using Hydra.Kernel.Extensions;
using Hydra.Kernel.Interfaces.Data;
using Hydra.Kernel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Cms.Api.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IQueryRepository _queryRepository;
        private readonly ICommandRepository _commandRepository;
        public SubscribeService(IQueryRepository queryRepository, ICommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <returns></returns>
        public async Task<Result<PaginatedList<SubscribeModel>>> GetList(GridDataBound dataGrid)
        {
            var result = new Result<PaginatedList<SubscribeModel>>();

            var list = await (from subscribe in _queryRepository.Table<Subscribe>().Include(x => x.SubscribeLabel)
                              select new SubscribeModel()
                              {
                                  Id = subscribe.Id,
                                  Email = subscribe.Email,
                                  InsertDate = subscribe.InsertDate,
                                  SubscribeLabelId = subscribe.SubscribeLabelId,
                                  SubscribeLabeTitle = subscribe.SubscribeLabel.Title,
                              }).OrderByDescending(x => x.Id).ToPaginatedListAsync(dataGrid);

            result.Data = list;

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<SubscribeModel>> GetById(long id)
        {
            var result = new Result<SubscribeModel>();
            var subscribe = await _queryRepository.Table<Subscribe>().Include(x => x.SubscribeLabel).FirstAsync(x => x.Id == id);

            var subscribeModel = new SubscribeModel()
            {
                Id = subscribe.Id,
                Email = subscribe.Email,
                InsertDate = subscribe.InsertDate,
                SubscribeLabelId = subscribe.SubscribeLabelId,
                SubscribeLabeTitle = subscribe.SubscribeLabel.Title,
            };
            result.Data = subscribeModel;

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribeModel"></param>
        /// <returns></returns>
        public async Task<Result<SubscribeModel>> Add(SubscribeModel subscribeModel)
        {
            var result = new Result<SubscribeModel>();
            try
            {
                var isExist = await _queryRepository.Table<Subscribe>().AnyAsync(x => x.Email == subscribeModel.Email);
                if (isExist)
                {
                    result.Status = ResultStatusEnum.ItsDuplicate;
                    result.Message = "The Email already exist";
                    result.Errors.Add(new Error(nameof(subscribeModel.Email), "The Email already exist"));
                    return result;
                }
                var subscribe = new Subscribe()
                {
                    SubscribeLabelId = subscribeModel.SubscribeLabelId,
                    Email = subscribeModel.Email,
                    InsertDate = DateTime.Now
                };

                await _commandRepository.InsertAsync(subscribe);
                await _commandRepository.SaveChangesAsync();

                subscribeModel.Id = subscribe.Id;

                result.Data = subscribeModel;

                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = ResultStatusEnum.ExceptionThrowed;
                return result;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribeModel"></param>
        /// <returns></returns>
        public async Task<Result<SubscribeModel>> Update(SubscribeModel subscribeModel)
        {
            var result = new Result<SubscribeModel>();
            try
            {
                var subscribe = await _queryRepository.Table<Subscribe>().FirstOrDefaultAsync(x => x.Id == subscribeModel.Id);
                if (subscribe is null)
                {
                    result.Status = ResultStatusEnum.NotFound;
                    result.Message = "The Subscribe not found";
                    return result;
                }
                var isExist = await _queryRepository.Table<Subscribe>().AnyAsync(x => x.Id != subscribeModel.Id && x.Email == subscribeModel.Email);
                if (isExist)
                {
                    result.Status = ResultStatusEnum.ItsDuplicate;
                    result.Message = "The Email already exist";
                    result.Errors.Add(new Error(nameof(subscribeModel.Email), "The Email already exist"));
                    return result;
                }

                subscribe.SubscribeLabelId = subscribeModel.SubscribeLabelId;
                subscribe.Email = subscribeModel.Email;

                _commandRepository.UpdateAsync(subscribe);
                await _commandRepository.SaveChangesAsync();

                result.Data = subscribeModel;

                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = ResultStatusEnum.ExceptionThrowed;
                return result;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> Delete(long id)
        {
            var result = new Result();
            var subscribe = await _queryRepository.Table<Subscribe>().FirstOrDefaultAsync(x => x.Id == id);
            if (subscribe is null)
            {
                result.Status = ResultStatusEnum.NotFound;
                result.Message = "The Subscribe not found";
                return result;
            }

            _commandRepository.DeleteAsync(subscribe);
            await _commandRepository.SaveChangesAsync();

            return result;
        }

    }
}