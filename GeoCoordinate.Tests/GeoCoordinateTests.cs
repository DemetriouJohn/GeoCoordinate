using System;
using Xunit;

namespace StandardGIS.Tests
{
    public class GeoCoordinateTests
    {
        [Theory]
        [InlineData(-91)]
        [InlineData(91)]
        public void Ctor_LatitudeOutOfBounds_ThrowsException(double lat)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(lat, 11));
        }

        [Theory]
        [InlineData(-181)]
        [InlineData(181)]
        public void Ctor_LongitudeOutOfBounds_ThrowsException(double lon)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(11, lon));
        }

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

        [Fact]
        public void Has3DPosition_ValidCoordinates_True()
        {
            Assert.True(new GeoCoordinate(11, 11, 11).Has3DPosition());
        }

        [Fact]
        public void Has3DPosition_InValidLat_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, 11, 11).Has3DPosition());
        }

        [Fact]
        public void Has3DPosition_InValidLon_False()
        {
            Assert.False(new GeoCoordinate(11, double.NaN, 11).Has3DPosition());
        }

        [Fact]
        public void Has3DPosition_InValidAlt_False()
        {
            Assert.False(new GeoCoordinate(11, 11, double.NaN).Has3DPosition());
        }

        [Fact]
        public void Has3DPosition_InValidCoordinates_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, double.NaN, double.NaN).Has3DPosition());
        }
    }
}
