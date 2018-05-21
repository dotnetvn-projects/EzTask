using AutoMapper;
using EzTask.Entity.Data;
using EzTask.Entity.Framework;
using EzTask.Framework.Common;
using EzTask.Interfaces.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Business.Infrastructures
{
    public class EzTaskMapperProfile : Profile
    {
        public EzTaskMapperProfile()
        {
            PhraseMapper();
        }
        private void PhraseMapper()
        {
            //Map phrase entity to phrase model
            CreateMap<Phrase, IPhraseModel>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToEnum<PhraseStatus>()));

            //Map phrase model to phrase entity
            CreateMap<IPhraseModel, Phrase>()
                .ForMember(c => c.Status, t => t.MapFrom(z => z.Status.ToInt16<PhraseStatus>()));
        }
    }
}
