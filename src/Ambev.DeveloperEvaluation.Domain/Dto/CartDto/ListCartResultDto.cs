﻿namespace Ambev.DeveloperEvaluation.Domain.Dto.CartDto
{
    public class ListCartResultDto
    {
        public ListCartResultDto() { }
        public ListCartResultDto(int totalItems, int totalPages, int currentPage, IEnumerable<Entities.Cart> Carts)
        {
            TotalItems = totalItems;
            Data = Carts;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }
        public int TotalItems { get; set; }
        public IEnumerable<Entities.Cart> Data { get; set; } = new List<Entities.Cart>();
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
