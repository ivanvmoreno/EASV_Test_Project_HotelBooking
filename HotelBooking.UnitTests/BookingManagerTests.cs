using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.TestData;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
	public class BookingManagerTests
	{
		private IBookingManager bookingManager;
		private Mock<IRepository<Booking>> bookingRepository;
		private Mock<IRepository<Room>> roomRepository;

		public BookingManagerTests()
		{
			bookingRepository = new Mock<IRepository<Booking>>();
			bookingRepository.Setup(x => x.GetAll()).Returns(BookingRepositoryData.bookings);
			bookingRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(BookingRepositoryData.bookings[1]);

			roomRepository = new Mock<IRepository<Room>>();
			roomRepository.Setup(x => x.GetAll()).Returns(RoomRepositoryData.rooms);

			bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
		}

		[Fact]
		public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
		{
			DateTime date = DateTime.Today;
			Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(date, date));
		}

		[Theory]
		[InlineData(1, 0)]
		[InlineData(-1, 0)]
		public void FindAvailableRoom_CheckDates_ThrowsArgumentException(int start, int end)
		{
			DateTime startDate = DateTime.Today.AddDays(start);
			DateTime endDate = DateTime.Today.AddDays(end);
			Assert.Throws<ArgumentException>(() => bookingManager.FindAvailableRoom(startDate, endDate));
		}

		[Fact]
		public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
		{
			DateTime date = DateTime.Today.AddDays(1);
			int roomId = bookingManager.FindAvailableRoom(date, date);
			Assert.NotEqual(-1, roomId);
		}

		[Fact]
		public void FindAvailableRoom_RoomNotAvailable_RoomIdMinusOne()
		{
			DateTime date = DateTime.Today.AddDays(15);
			int roomId = bookingManager.FindAvailableRoom(date, date);
			Assert.Equal(-1, roomId);
		}

		[Fact]
		public void FindAvailableRoom_CallsOnce_BookingRepositoryGetAll()
		{
			DateTime date = DateTime.Today.AddDays(15);
			bookingManager.FindAvailableRoom(date, date);
			bookingRepository.Verify(x => x.GetAll(), Times.Once());
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_AvailableRoom_ReturnsTrue(Booking booking)
		{
			Assert.True(bookingManager.CreateBooking(booking));
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingNotAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_AvailableRoom_ReturnsFalse(Booking booking)
		{
			Assert.False(bookingManager.CreateBooking(booking));
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_AvailableRoom_UpdatesBookingRoomId(Booking booking)
		{
			int originalRoomId = booking.RoomId;
			bookingManager.CreateBooking(booking);
			Assert.NotEqual(booking.RoomId, originalRoomId);
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingNotAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_AvailableRoom_SameBookingRoomId(Booking booking)
		{
			int originalRoomId = booking.RoomId;
			bookingManager.CreateBooking(booking);
			Assert.Equal(booking.RoomId, originalRoomId);
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_AvailableRoom_BookingIsActiveTrue(Booking booking)
		{
			bookingManager.CreateBooking(booking);
			Assert.True(booking.IsActive);
		}

		[Theory]
		[MemberData(nameof(CreateBookingData.BookingNotAvailableDate), MemberType = typeof(CreateBookingData))]
		public void CreateBooking_NotAvailableRoom_BookingIsActiveFalse(Booking booking)
		{
			bookingManager.CreateBooking(booking);
			Assert.False(booking.IsActive);
		}

		[Fact]
		public void GetFullyOccupiedDates_StartDateOlderThanEndDate_ThrowsArgumentException()
		{
			DateTime date = DateTime.Today;
			Assert.Throws<ArgumentException>(() => bookingManager.GetFullyOccupiedDates(date.AddDays(5), date.AddDays(1)));
		}

		[Theory]
		[InlineData(10, 15, 6)]
		[InlineData(5, 10, 1)]
		public void GetFullyOccupiedDates_FullyOccupiedPeriod_ReturnListOfEqualLength(int start, int end, int fullyOccupiedDays)
		{
			DateTime startDate = DateTime.Today.AddDays(start);
			DateTime endDate = DateTime.Today.AddDays(end);
			List<DateTime> period = bookingManager.GetFullyOccupiedDates(startDate, endDate);
			Assert.Equal(fullyOccupiedDays, period.Count);
		}

		[Fact]
		public void GetFullyOccupiedDates_CallsOnce_BookingRepositoryGetAll()
		{
			DateTime startDate = DateTime.Today.AddDays(10);
			DateTime endDate = DateTime.Today.AddDays(15);
			bookingManager.GetFullyOccupiedDates(startDate, endDate);
			bookingRepository.Verify(x => x.GetAll(), Times.Once());
		}
		
		[Fact]
		public void GetFullyOccupiedDates_BookingRepositoryGetAll_NoBookings()
		{
			Mock<IRepository<Room>> _roomRepository = new Mock<IRepository<Room>>();
			_roomRepository.Setup(x => x.GetAll()).Returns(new List<Room>());
			IBookingManager _bookingManager = new BookingManager(bookingRepository.Object, _roomRepository.Object);
			DateTime startDate = DateTime.Today.AddDays(10);
			DateTime endDate = DateTime.Today.AddDays(15);
			List<DateTime> datesList = _bookingManager.GetFullyOccupiedDates(startDate, endDate);
			Assert.Empty(datesList);
		}
	}
}
