﻿using ItStore.Infrastructure;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace It_Store.Models.DataFolder
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
