using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Dto.UserDto
{
    public class ListUserResultDto
    {
        public ListUserResultDto(int total, IEnumerable<Entities.User> users) 
        {
            Total = total;
            Users = users;
        }
        public int Total { get; set; }
        public IEnumerable<Entities.User> Users { get; set; } = new List<Entities.User>();
    }
}
