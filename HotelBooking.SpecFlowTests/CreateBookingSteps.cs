using System;
using TechTalk.SpecFlow;
using HotelBooking.Core;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace HotelBooking.SpecFlowTests
{
    [Binding]
    public class CreateBookingSteps
    {
        Booking booking;
        List<DateTime> occupiedRange = new List<DateTime>();
        Boolean result;
        private Mock<BookingManager> bookingManager;
        private Mock<IRepository<Booking>> bookingRepository;
        private Mock<IRepository<Room>> roomRepository;

        public CreateBookingSteps()
        {
            bookingRepository = new Mock<IRepository<Booking>>();
            roomRepository = new Mock<IRepository<Room>>();
            bookingManager = new Mock<BookingManager>(bookingRepository.Object, roomRepository.Object);
            bookingManager.CallBase = true;
            bookingManager.Setup(b => b.FindAvailableRoom(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((DateTime startDate, DateTime endDate) => (occupiedRange.Contains(startDate) || occupiedRange.Contains(endDate)) ? 1 : -1);
        }

        [Given(@"a (.*) and (.*)")]
        public void GivenAAnd(DateTime startDate, DateTime endDate)
        {
            booking = new Booking();
            booking.StartDate = startDate;
            booking.EndDate = endDate;
        }

        [Given(@"a range of occupied dates")]
        public void AndARangeOfOccupiedDates(Table table)
        {
            foreach (var row in table.Rows) 
            {
                DateTime endDate = DateTime.Parse(row[1]);
                for (DateTime date = DateTime.Parse(row[0]); date <= endDate; date.AddDays(1))
                {
                    if (!occupiedRange.Contains(date))
                        occupiedRange.Add(date);
                }
            }
        }

        [When(@"creating the booking")]
        public void WhenCreatingTheBooking()
        {
            result = bookingManager.Object.CreateBooking(booking);
        }

        [Then(@"a new active booking is created")]
        public void ThenANewActiveBookingIsCreated()
        {
            Assert.True(result);
        }

        [Then(@"no new booking is created")]
        public void ThenNoNewBookingIsCreated()
        {
            Assert.False(result);
        }
    }
}