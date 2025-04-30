using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser
{
    public class ListUserCommand : IRequest<ListUserResult>
    {
        public ListUserCommand(int page, int size, string order) 
        {
            Page = page;
            PageSize = size;
            Order = order;
        }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Order { get; set; }
    }
}
