using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser
{
    public class UpdateUserResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the newly created user.
        /// </summary>
        /// <value>A GUID that uniquely identifies the created user in the system.</value>
        public Guid Id { get; set; }
    }
}
