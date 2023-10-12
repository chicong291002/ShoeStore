﻿using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SmartPhoneStore.ViewModels.Catalog.Coupons;
using SmartPhoneStore.ViewModels.Common;
using SmartPhoneStore.ViewModels.Catalog.Products;
using SmartPhoneStore.Utilities.Constants;
using SmartPhoneStore.AdminApp.ApiIntegration;

namespace ShoeStore.AdminApp.ApiIntegration.Products
{
    public class CouponApiClient : BaseApiClient, ICouponApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CouponApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateCoupon(CouponCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/coupons/", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            return await Delete($"/api/coupons/" + id);
        }

        public async Task<PagedResult<CouponViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<CouponViewModel>>(
               $"/api/coupons/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               $"&keyword={request.Keyword}&sortOption={request.SortOption}");

            return data;
        }

        public async Task<List<CouponViewModel>> GetAll()
        {
            return await GetListAsync<CouponViewModel>("/api/coupons");
        }

        public async Task<CouponViewModel> GetById(int id)
        {
            return await GetAsync<CouponViewModel>($"/api/coupons/{id}");
        }

        public async Task<bool> UpdateCoupon(CouponUpdateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/coupons/updateCoupon", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
