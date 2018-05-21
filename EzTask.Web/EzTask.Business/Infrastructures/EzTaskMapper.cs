using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Interfaces.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Business.Infrastructures
{
    public static class EzTaskMapper
    {
        private static IMapper _mapper;
        public static void Config(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Phrase Mapper
        public static IPhraseModel MapToModel(this Phrase entity)
        {
            if (entity == null)
                return null;

            return _mapper.Map<IPhraseModel>(entity);
        }

        public static Phrase MapToEntity(this IPhraseModel model)
        {
            if (model == null)
                return null;

            return _mapper.Map<Phrase>(model);
        }

        public static IEnumerable<IPhraseModel> MapToModels(this IEnumerable<Phrase> entity)
        {
            return _mapper.Map<IEnumerable<IPhraseModel>>(entity);
        }
        #endregion

    }
}
