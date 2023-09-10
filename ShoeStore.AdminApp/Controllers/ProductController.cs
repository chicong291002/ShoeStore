﻿using Microsoft.AspNetCore.Mvc;
using ShoeStore.AdminApp.Services.Products;
using ShoeStore.AdminApp.Services.Roles;
using ShoeStore.AdminApp.Services.Users;
using ShoeStore.Application.Catalog.Products.DTOS;
using ShoeStore.Application.Common;
using ShoeStore.Application.System.Users.DTOS;

namespace ShoeStore.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 2)
        {
            //default parameters
            /*            var session = HttpContext.Session.GetString("Token");*/
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _productApiClient.GetAllProductsPaging(request);
            ViewBag.keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data); // ra duoc pageUser
        }
    }
}
