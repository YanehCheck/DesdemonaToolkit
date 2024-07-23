using YanehCheck.EpicGamesUtils.BL.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.BL.Facades;

public class ItemFacade(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<ItemEntity, ItemFullModel> mapper) : Facade<ItemEntity, ItemFullModel, ItemEntityMapper>(unitOfWorkFactory, mapper) {
    
}   