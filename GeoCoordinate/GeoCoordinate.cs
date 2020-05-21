using System;

namespace StandardGIS
{
    /// <summary>
    /// Represents a geographical location that is determined by latitude and longitude coordinates.
    /// May also include, altitude, accuracy, speed and course information
    /// </summary>

    public class GeoCoordinate
    {
        ///<summary>
        /// An empty instance of GeoCoordinate class that has unknown position
        ///</summary>
        public static readonly GeoCoordinate Unknown = new GeoCoordinate();

        public double Latitude { get; }
        public double Longitude { get; }
        public double Altitude { get; }

        /// <summary>
        /// Initializes a new instance of GeoCoordinate that has no data fields set.
        /// </summary>
        public GeoCoordinate() : this(double.NaN, double.NaN)
        {

        }

        /// <summary>
        /// Initializes a new instance of GeoCoordinate that represents a 2D position.
        /// </summary>
        /// <param name="latitude">The latitude of the location. May range from -90.0 to 90.0. </param>
        /// <param name="longitude">The longitude of the location. May range from -180.0 to 180.0.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Latitude or longitude is out of range.</exception>
        public GeoCoordinate(double latitude, double longitude) : this(latitude, longitude, double.NaN)
        {
        }

        /// <summary>
        /// Initializes a new instance of GeoCoordinate that represents a 3D position.
        /// </summary>
        /// <param name="latitude">The latitude of the location. May range from -90.0 to 90.0. </param>
        /// <param name="longitude">The longitude of the location. May range from -180.0 to 180.0.</param>
        /// <param name="altitude">The altitude in meters. May be negative, 0, positive, or Double.NaN, if unknown.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Latitude or longitude is out of range.</exception>
        public GeoCoordinate(double latitude, double longitude, double altitude)
        {
            if (latitude > 90.0 || latitude < -90.0)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Argument must be in range of -90 to 90");
            }

            if (longitude> 180.0 || longitude < -180.0)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Argument must be in range of -90 to 90");
            }

            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        ///<summary>
        /// Checks if current instance of GeoCoordinate has a valid position
        ///</summary>
        public bool HasPosition() => !double.IsNaN(Latitude) && !double.IsNaN(Longitude);

        ///<summary>
        /// Checks if current instance of GeoCoordinate has a valid 3D position
        ///</summary>
        public bool Has3DPosition() => HasPosition() && !double.IsNaN(Altitude);
    }
}