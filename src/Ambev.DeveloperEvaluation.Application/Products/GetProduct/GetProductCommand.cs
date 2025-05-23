﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public record GetProductCommand : IRequest<GetProductResult>
    {
        /// <summary>
        /// The unique identifier of the Product to retrieve
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new instance of GetProductCommand
        /// </summary>
        /// <param name="id">The ID of the Product to retrieve</param>
        public GetProductCommand(int id)
        {
            Id = id;
        }
    }
}
