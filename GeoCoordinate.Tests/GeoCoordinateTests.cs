using System;
using Xunit;

namespace StandardGIS.Tests
{
    public class GeoCoordinateTests
    {
        [Fact]
        public void HasPosition_ValidCoordinates_True()
        {
            Assert.True(new GeoCoordinate(11, 11).HasPosition());
        }

        [Fact]
        public void HasPosition_InValidLat_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, 11).HasPosition());
        }

        [Fact]
        public void HasPosition_InValidLon_False()
        {
            Assert.False(new GeoCoordinate(11, double.NaN).HasPosition());
        }

        [Fact]
        public void HasPosition_InValidCoordinates_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, double.NaN).HasPosition());
        }
    }
}
