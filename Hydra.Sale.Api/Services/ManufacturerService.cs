﻿using Hydra.Infrastructure.Data.Extension;
using Hydra.Kernel.Extensions;
using Hydra.Kernel.Interfaces.Data;
using Hydra.Kernel.Models;
using Hydra.Sale.Core.Domain;
using Hydra.Sale.Core.Interfaces;
using Hydra.Sale.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Sale.Api.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IQueryRepository _queryRepository;
        private readonly ICommandRepository _commandRepository;
        public ManufacturerService(IQueryRepository queryRepository, ICommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <returns></returns>
        public async Task<Result<PaginatedList<ManufacturerModel>>> GetList(GridDataBound dataGrid)
        {
            var result = new Result<PaginatedList<ManufacturerModel>>();

            var list = await (from manufacturer in _queryRepository.Table<Manufacturer>()
                              select new ManufacturerModel()
                              {
                                  Id = manufacturer.Id,
                                  Name = manufacturer.Name,
                                  MetaKeywords = manufacturer.MetaKeywords,
                                  MetaTitle = manufacturer.MetaTitle,
                                  Description = manufacturer.Description,
                                  MetaDescription = manufacturer.MetaDescription,
                                  PictureId = manufacturer.PictureId,
                                  Published = manufacturer.Published,
                                  Deleted = manufacturer.Deleted,
                                  DisplayOrder = manufacturer.DisplayOrder,
                                  CreatedOnUtc = manufacturer.CreatedOnUtc,
                                  UpdatedOnUtc = manufacturer.UpdatedOnUtc,
                                  //ProductManufacturers = manufacturer.ProductManufacturers,
                                  //Discounts = manufacturer.Discounts,

                              }).OrderByDescending(x => x.Id).ToPaginatedListAsync(dataGrid);

            result.Data = list;

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<ManufacturerModel>> GetById(int id)
        {
            var result = new Result<ManufacturerModel>();
            var manufacturer = await _queryRepository.Table<Manufacturer>().FirstOrDefaultAsync(x => x.Id == id);

            var manufacturerModel = new ManufacturerModel()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                MetaKeywords = manufacturer.MetaKeywords,
                MetaTitle = manufacturer.MetaTitle,
                Description = manufacturer.Description,
                MetaDescription = manufacturer.MetaDescription,
                PictureId = manufacturer.PictureId,
                Published = manufacturer.Published,
                Deleted = manufacturer.Deleted,
                DisplayOrder = manufacturer.DisplayOrder,
                CreatedOnUtc = manufacturer.CreatedOnUtc,
                UpdatedOnUtc = manufacturer.UpdatedOnUtc,
                //ProductManufacturers = manufacturer.ProductManufacturers,
                //Discounts = manufacturer.Discounts,

            };
            result.Data = manufacturerModel;

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="manufacturerModel"></param>
        /// <returns></returns>
        public async Task<Result<ManufacturerModel>> Add(ManufacturerModel manufacturerModel)
        {
            var result = new Result<ManufacturerModel>();
            try
            {
                bool isExist = await _queryRepository.Table<Manufacturer>().AnyAsync(x => x.Id == manufacturerModel.Id);
                if (isExist)
                {
                    result.Status = ResultStatusEnum.ItsDuplicate;
                    result.Message = "The Id already exist";
                    result.Errors.Add(new Error(nameof(manufacturerModel.Id), "The Id already exist"));
                    return result;
                }
                var manufacturer = new Manufacturer()
                {
                    Name = manufacturerModel.Name,
                    MetaKeywords = manufacturerModel.MetaKeywords,
                    MetaTitle = manufacturerModel.MetaTitle,
                    Description = manufacturerModel.Description,
                    MetaDescription = manufacturerModel.MetaDescription,
                    PictureId = manufacturerModel.PictureId,
                    Published = manufacturerModel.Published,
                    Deleted = manufacturerModel.Deleted,
                    DisplayOrder = manufacturerModel.DisplayOrder,
                    CreatedOnUtc = manufacturerModel.CreatedOnUtc,
                    UpdatedOnUtc = manufacturerModel.UpdatedOnUtc,
                    //ProductManufacturers = manufacturerModel.ProductManufacturers,
                    //Discounts = manufacturerModel.Discounts,

                };

                await _commandRepository.InsertAsync(manufacturer);
                await _commandRepository.SaveChangesAsync();

                manufacturerModel.Id = manufacturer.Id;

                result.Data = manufacturerModel;

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
        /// <param name="manufacturerModel"></param>
        /// <returns></returns>
        public async Task<Result<ManufacturerModel>> Update(ManufacturerModel manufacturerModel)
        {
            var result = new Result<ManufacturerModel>();
            try
            {
                var manufacturer = await _queryRepository.Table<Manufacturer>().FirstAsync(x => x.Id == manufacturerModel.Id);
                if (manufacturer is null)
                {
                    result.Status = ResultStatusEnum.NotFound;
                    result.Message = "The Manufacturer not found";
                    return result;
                }
                bool isExist = await _queryRepository.Table<Manufacturer>().AnyAsync(x => x.Id != manufacturerModel.Id);
                if (isExist)
                {
                    result.Status = ResultStatusEnum.ItsDuplicate;
                    result.Message = "The Id already exist";
                    result.Errors.Add(new Error(nameof(manufacturerModel.Id), "The Id already exist"));
                    return result;
                }
                manufacturer.Name = manufacturerModel.Name;
                manufacturer.MetaKeywords = manufacturerModel.MetaKeywords;
                manufacturer.MetaTitle = manufacturerModel.MetaTitle;
                manufacturer.Description = manufacturerModel.Description;
                manufacturer.MetaDescription = manufacturerModel.MetaDescription;
                manufacturer.PictureId = manufacturerModel.PictureId;
                manufacturer.Published = manufacturerModel.Published;
                manufacturer.Deleted = manufacturerModel.Deleted;
                manufacturer.DisplayOrder = manufacturerModel.DisplayOrder;
                manufacturer.CreatedOnUtc = manufacturerModel.CreatedOnUtc;
                manufacturer.UpdatedOnUtc = manufacturerModel.UpdatedOnUtc;
                //manufacturer.ProductManufacturers = manufacturerModel.ProductManufacturers;
                //manufacturer.Discounts = manufacturerModel.Discounts;

                _commandRepository.UpdateAsync(manufacturer);
                await _commandRepository.SaveChangesAsync();

                result.Data = manufacturerModel;

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
        public async Task<Result> Delete(int id)
        {
            var result = new Result();
            var manufacturer = await _queryRepository.Table<Manufacturer>().FirstOrDefaultAsync(x => x.Id == id);
            if (manufacturer is null)
            {
                result.Status = ResultStatusEnum.NotFound;
                result.Message = "The Manufacturer not found";
                return result;
            }

            _commandRepository.DeleteAsync(manufacturer);
            await _commandRepository.SaveChangesAsync();

            return result;
        }

    }
}