using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Dto.User;
using Ambev.DeveloperEvaluation.Domain.Dto.UserDto;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser
{
    public class ListUserProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetUser operation
        /// </summary>
        public ListUserProfile()
        {
            CreateMap<ListUserCommand, ListUserFilter>();
            CreateMap<ListUserResultDto, ListUserResult>();
            CreateMap<User, GetUserResult>();
        }
    }
}
