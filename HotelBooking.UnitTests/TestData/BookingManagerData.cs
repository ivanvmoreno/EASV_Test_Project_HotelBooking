using System;
using HotelBooking.Core;
using System.Collections.Generic;
using System.Collections;

namespace HotelBooking.UnitTests.TestData
{
    public class CreateBookingData
    {
        public static IEnumerable<object[]> BookingAvailableDate()
        {
            DateTime date = DateTime.Today.AddDays(1);
            Booking booking = new Booking();
            booking.StartDate = date;
            booking.EndDate = date;
            yield return new object[] { booking };
        }

        public static IEnumerable<object[]> BookingNotAvailableDate()
        {
            DateTime date = DateTime.Today.AddDays(15);
            Booking booking = new Booking();
            booking.StartDate = date;
            booking.EndDate = date;
            yield return new object[] { booking };
        }
    }

    public class GetFullyOccupiedDatesData
    {
        
    }
}
