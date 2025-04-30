using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Dto.User;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser
{
    public class ListUserHandler : IRequestHandler<ListUserCommand, ListUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetUserHandler
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for GetUserCommand</param>
        public ListUserHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetUserCommand request
        /// </summary>
        /// <param name="request">The GetUser command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The user details if found</returns>
        public async Task<ListUserResult> Handle(ListUserCommand request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<ListUserFilter>(request);
            var user = await _userRepository.GetByFilterAsync(filter, cancellationToken);

            return _mapper.Map<ListUserResult>(user);
        }
    }
}
