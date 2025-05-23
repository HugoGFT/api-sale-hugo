﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for ListProduct operation
        /// </summary>
        public GetProductProfile()
        {
            CreateMap<Product, GetProductResult>();
        }
    }
}
