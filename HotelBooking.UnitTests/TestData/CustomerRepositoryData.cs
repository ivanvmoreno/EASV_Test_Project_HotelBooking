using System;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.TestData
{
    public class CustomerRepositoryData
    {
        public static Customer[] customers =
        {
            new Customer { Email = "bp@gmail.com", Id = 1, Name = "Bent Pedersen" },
            new Customer { Email = "hc@gmail.com" , Id = 2, Name = "Hans Christiansen" }
        };
    }
}
