﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pricing.Domain.Dtos
{
    public class UpdateTarrifRequestDto
    {
        public string ProductCode { get; set; }
        public IList<PriceDto> Prices { get; set; }
    }
}
