using System;
using Xunit;

namespace ExtendedGeoCoordinate.Tests
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

        [Theory]
        [InlineData(-181)]
        [InlineData(8851)]
        public void Ctor_AltitudeOutOfBounds_ThrowsException(double alt)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(11, 11, alt));
        }

        [Fact]
        public void IsGoodPosition_ValidCoordinates_True()
        {
            Assert.True(new GeoCoordinate(11, 11).IsGoodPosition);
        }

        [Fact]
        public void IsGoodPosition_InValidLat_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, 11).IsGoodPosition);
        }

        [Fact]
        public void IsGoodPosition_InValidLon_False()
        {
            Assert.False(new GeoCoordinate(11, double.NaN).IsGoodPosition);
        }

        [Fact]
        public void IsGoodPosition_InValidCoordinates_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, double.NaN).IsGoodPosition);
        }

        [Fact]
        public void Is3DPosition_ValidCoordinates_True()
        {
            Assert.True(new GeoCoordinate(11, 11, 11).Is3DPosition);
        }

        [Fact]
        public void Is3DPosition_InValidLat_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, 11, 11).Is3DPosition);
        }

        [Fact]
        public void Is3DPosition_InValidLon_False()
        {
            Assert.False(new GeoCoordinate(11, double.NaN, 11).Is3DPosition);
        }

        [Fact]
        public void Is3DPosition_InValidAlt_False()
        {
            Assert.False(new GeoCoordinate(11, 11, double.NaN).Is3DPosition);
        }

        [Fact]
        public void Is3DPosition_InValidCoordinates_False()
        {
            Assert.False(new GeoCoordinate(double.NaN, double.NaN, double.NaN).Is3DPosition);
        }

        [Fact]
        public void Equals_SecondObjectSameValues_True()
        {
            Assert.Equal(new GeoCoordinate(11, 11), new GeoCoordinate(11, 11));
        }

        [Fact]
        public void Equals_SecondObjectDiffValues_True()
        {
            Assert.NotEqual(new GeoCoordinate(11, 11), new GeoCoordinate(12, 11));
        }

        [Fact]
        public void Equals_SameObject_True()
        {
            var x = new GeoCoordinate(11, 11);
            Assert.Equal(x, x);
        }

        [Fact]
        public void EqualityOperator_SecondObjectSameValues_True()
        {
            Assert.True(new GeoCoordinate(11, 11) == new GeoCoordinate(11, 11));
        }

        [Fact]
        public void EqualityOperator_SecondObjectDifferentValues_True()
        {
            Assert.False(new GeoCoordinate(11, 11) == new GeoCoordinate(11, 12));
        }

        [Fact]
        public void NotEqualOperator_SecondObjectSameValues_True()
        {
            Assert.False(new GeoCoordinate(11, 11) != new GeoCoordinate(11, 11));
        }

        [Fact]
        public void NotEqualOperator_SecondObjectDiffValues_True()
        {
            Assert.True(new GeoCoordinate(11, 11) != new GeoCoordinate(12, 11));
        }

        [Fact]
        public void GeoCoordinate_GetDistanceToUsingHaversine_ReturnsExpectedDistance()
        {
            var start = new GeoCoordinate(1, 1);
            var end = new GeoCoordinate(5, 5);
            var distance = start.GetDistanceTo(end, DistanceFormula.Haversine);
            var expected = 62851.816846125;

            Assert.Equal(expected, distance, 9);
        }

        [Fact]
        public void GeoCoordinate_GetDistanceToUsingSphericalLawOfCosinus_ReturnsExpectedDistance()
        {
            var start = new GeoCoordinate(1, 1);
            var end = new GeoCoordinate(5, 5);
            var distance = start.GetDistanceTo(end, DistanceFormula.SphericalLawOfCosinus);
            var expected = 62851.816846125;

            Assert.Equal(expected, distance, 9);
        }

        [Fact]
        public void GeoCoordinate_GetDistanceToUsingVicenty_ReturnsExpectedDistance()
        {
            var start = new GeoCoordinate(1, 1);
            var end = new GeoCoordinate(5, 5);
            var distance = start.GetDistanceTo(end, DistanceFormula.Vicenty);
            var expected = 62642.77580421;

            Assert.Equal(expected, distance, 9);
        }

        [Fact]
        public void IsUnknown_WithUnknown_True()
        {
            Assert.True(new GeoCoordinate().IsUnknown);
        }

        [Fact]
        public void IsUnknown_With2DPosition_False()
        {
            Assert.False(new GeoCoordinate(11, 11).IsUnknown);
        }
    }
}
