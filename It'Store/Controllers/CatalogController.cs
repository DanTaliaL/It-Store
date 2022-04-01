﻿using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItStore.Controllers
{
    public class CatalogController : Controller
    {
        private DataContext Data { get; set; }
        public CatalogController(DataContext DC)
        {
            Data = DC;
        }
        public int PageSize = 8;

        public IActionResult Catalog(string category, string searchstring, int productPage = 1)
        {
            ViewBag.SearchingString = searchstring;
            ViewBag.SelectedCategory = category;           
            return View( new ProductsListViewModel
            {
                
                Products = Data.Products
                .Where(q => category == null || q.Categories == category)
                .Where(q=> searchstring==null || q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.SEO.Contains(searchstring) || q.Price.ToString() == searchstring)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null?
                    Data.Products.Count():
                    Data.Products
                    .Where(q=>q.Categories==category && q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.SEO.Contains(searchstring) || q.Price.ToString() == searchstring) //не вернвый расчет поиска и категорий
                    .Count()
                },
                CurrentCategory = category
            });
        }
    }
}
