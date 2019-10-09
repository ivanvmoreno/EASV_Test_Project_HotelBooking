using System;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.TestData
{
    public class BookingRepositoryData
    {
        public static Booking[] bookings =
        {
            new Booking { Id=1, StartDate=DateTime.Today.AddDays(5), EndDate=DateTime.Today.AddDays(15), IsActive=true, CustomerId=1, RoomId=1 },
            new Booking { Id=2, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=2, RoomId=2 },
        };
    }
}
