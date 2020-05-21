using System;

namespace StandardGIS
{
    /// <summary>
    /// Represents a geographical location that is determined by latitude and longitude coordinates.
    /// May also include, altitude, accuracy, speed and course information
    /// </summary>

    public class GeoCoordinate : IEquatable<GeoCoordinate>
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

            if (longitude > 180.0 || longitude < -180.0)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Argument must be in range of -180 to 180");
            }

            if (altitude > 8850 || altitude < -153.0)
            {
                throw new ArgumentOutOfRangeException(nameof(altitude), "Argument must be in range of -153 to 8850");
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


        /// <summary>
        ///     Determines if the GeoCoordinate object is equivalent to the parameter, based solely on latitude and longitude.
        /// </summary>
        /// <returns>
        ///     true if the GeoCoordinate objects are equal; otherwise, false.
        /// </returns>
        /// <param name="other">The GeoCoordinate object to compare to the calling object.</param>
        public bool Equals(GeoCoordinate other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (!Latitude.Equals(other.Latitude))
            {
                return false;
            }

            return Longitude.Equals(other.Longitude);
        }

        /// <summary>
        ///     Determines whether two GeoCoordinate objects refer to the same two dimensional location
        /// </summary>
        /// <returns>
        ///     true, if the GeoCoordinate objects are determined to be equivalent; otherwise, false.
        /// </returns>
        /// <param name="left">The first GeoCoordinate to compare.</param>
        /// <param name="right">The second GeoCoordinate to compare.</param>
        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two GeoCoordinate objects correspond to different two dimensional locations.
        /// </summary>
        /// <returns>
        ///     true, if the GeoCoordinate objects are determined to be different; otherwise, false.
        /// </returns>
        /// <param name="left">The first GeoCoordinate to compare.</param>
        /// <param name="right">The second GeoCoordinate to compare.</param>
        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Returns a string that contains the latitude and longitude.
        /// </summary>
        /// <returns>
        ///     A string that contains the latitude longitude and altitude, separated by a comma.
        /// </returns>
        public override string ToString()
        {
            if (this == Unknown)
            {
                return "Unknown";
            }

            return $"{Latitude.ToString("00.000000")}, {Longitude.ToString("00.000000")}, {Altitude.ToString("00.00")}";
        }

        /// <summary>
        ///     Serves as a hash function for the GeoCoordinate.
        /// </summary>
        /// <returns>
        ///     A hash code for the current GeoCoordinate.
        /// </returns>
        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }
    }
}