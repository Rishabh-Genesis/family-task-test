using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Domain.DataModels.Task;

namespace WebApi.AutoMapper
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<CreateMemberCommand, Member>();
            CreateMap<UpdateMemberCommand, Member>();
            CreateMap<UpdateTaskCommand, Task>();
            CreateMap<Member, MemberVm>();
            CreateMap<CreateTaskCommand, Domain.DataModels.Task>();
            CreateMap<Task, TaskVm>();
        }
    }
}
