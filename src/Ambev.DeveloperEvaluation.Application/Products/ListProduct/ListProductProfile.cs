//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;

//namespace Ambev.DeveloperEvaluation.Application.Products.ListProduct
//{
//    public class ListProductProfile
//    {
//        /// <summary>
//        /// Initializes the mappings for ListProduct feature
//        /// </summary>
//        public ListProductProfile()
//        {
//            CreateMap<ListProductRequest, ListProductCommand>()
//                .ConstructUsing(request => new ListProductCommand(request.Page, request.PageSize));
//        }
//    }
//}
