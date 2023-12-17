﻿using Bookshop.Application.Features.Dto;
using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderRequestDto : IBaseDto
    {
        public long Id { get; set; }
        public IList<long>? ItemsId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
