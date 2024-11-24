﻿using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;
using Talabt.Core.Order_Aggregate;
using Talabt.Core.Repositories;
using Talabt.Core.Services;

namespace Talabt.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IBasketRepository basketRepository,IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if(Basket is null) return null;
            var ShippingPrice = 0M;
            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            if(Basket.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                    if(item.Price != Product.Price)
                    {
                        item.Price = Product.Price;
                    }
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"}
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100
                };
                 paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId,Options);
                 Basket.PaymentIntentId = paymentIntent.Id;
                 Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }
    }
}
