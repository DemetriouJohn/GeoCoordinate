using System;

namespace ExtendedGeoCoordinate
{
    /// <summary>
    /// Represents a geographical location that is determined by latitude and longitude coordinates.
    /// May also include, altitude, accuracy, speed and course information
    /// </summary>

    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        private const float EarthRadius = 637100.0f;

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
        ///     Determines if the GeoCoordinate object is equivalent to the parameter, based solely on 2D position
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

        /// <summary>
        ///     Returns the distance between the latitude and longitude coordinates that are specified by this GeoCoordinate and
        ///     another specified GeoCoordinate.
        /// </summary>
        /// <returns>
        ///     The distance between the two coordinates, in meters.
        /// </returns>
        /// <param name="other">The GeoCoordinate for the location to calculate the distance to.</param>
        /// <param name="Formula">The formula to use to calculate distance</param>
        public double GetDistanceTo(GeoCoordinate other, DistanceFormula distanceFormula)
        {
            if (double.IsNaN(Latitude) || double.IsNaN(Longitude) || double.IsNaN(other.Latitude) ||
                double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number");
            }

            switch (distanceFormula)
            {
                case DistanceFormula.Haversine:
                    return GetDistanceHaversine(other);
                case DistanceFormula.SphericalLawOfCosinus:
                    return GetDistanceSphericalLawOfCosines(other);
                case DistanceFormula.Vicenty:
                    return GetDistanceVicenty(other);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets a value that indicates whether the GeoCoordinate does not contain either latitude or longitude data.
        /// </summary>
        /// <returns>
        ///     true if the GeoCoordinate does not contain latitude or longitude data; otherwise, false.
        /// </returns>
        public bool IsUnknown => Equals(Unknown);

        /// <summary>
        ///     Determines if a specified GeoCoordinate is equal to the current GeoCoordinate, based solely on 2D position
        /// </summary>
        /// <returns>
        ///     true, if the GeoCoordinate objects are equal; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare the GeoCoordinate to.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as GeoCoordinate);
        }

        private double GetDistanceVicenty(GeoCoordinate other)
        {
            double flat = 1 / 298.257223563;    //flattening of earth
            double rpol = (1 - flat) * EarthRadius;

            double sinSigma = 0;
            double cosSigma = 0;
            double sigma = 0;
            double sin_alpha = 0;
            double cosSqAlpha = 0;
            double cos2sigma = 0;
            double C = 0;
            double lam_pre = 0;

            // convert to radians
            var latp = Math.PI * Latitude / 180.0;
            var latc = Math.PI * other.Latitude / 180.0;
            var longp = Math.PI * Longitude / 180.0;
            var longc = Math.PI * other.Longitude / 180.0;

            double u1 = Math.Atan((1 - flat) * Math.Tan(latc));
            double u2 = Math.Atan((1 - flat) * Math.Tan(latp));

            double lon = longp - longc;
            double lam = lon;
            double tol = Math.Pow(10f, -12f); // iteration tolerance
            double diff = 1;
            while (Math.Abs(diff) > tol)
            {
                sinSigma = Math.Sqrt(Math.Pow((Math.Cos(u2) * Math.Sin(lam)), 2) + Math.Pow(Math.Cos(u1) * Math.Sin(u2) - Math.Sin(u1) * Math.Cos(u2) * Math.Cos(lam), 2));
                cosSigma = Math.Sin(u1) * Math.Sin(u2) + Math.Cos(u1) * Math.Cos(u2) * Math.Cos(lam);
                sigma = Math.Atan(sinSigma / cosSigma);
                sin_alpha = (Math.Cos(u1) * Math.Cos(u2) * Math.Sin(lam)) / sinSigma;
                cosSqAlpha = 1 - Math.Pow(sin_alpha, 2);
                cos2sigma = cosSigma - ((2 * Math.Sin(u1) * Math.Sin(u2)) / cosSqAlpha);
                C = (flat / 16) * cosSqAlpha * (4 + flat * (4 - 3 * cosSqAlpha));
                lam_pre = lam;
                lam = lon + (1 - C) * flat * sin_alpha * (sigma + C * sinSigma * (cos2sigma + C * cosSigma * (2 * Math.Pow(cos2sigma, 2) - 1)));
                diff = Math.Abs(lam_pre - lam);
            }

            double usq = cosSqAlpha * ((Math.Pow(EarthRadius, 2) - Math.Pow(rpol, 2)) / Math.Pow(rpol, 2));
            double A = 1 + (usq / 16384) * (4096 + usq * (-768 + usq * (320 - 175 * usq)));
            double B = (usq / 1024) * (256 + usq * (-128 + usq * (74 - 47 * usq)));
            double delta_sig = B * sinSigma * (cos2sigma + 0.25 * B * (cosSigma * (-1 + 2 * Math.Pow(cos2sigma, 2)) -
                                                                 (1 / 6) * B * cos2sigma * (-3 + 4 * Math.Pow(sinSigma, 2)) *
                                                                 (-3 + 4 * Math.Pow(cos2sigma, 2))));
            double dis = rpol * A * (sigma - delta_sig);
            double azi1 = Math.Atan2((Math.Cos(u2) * Math.Sin(lam)), (Math.Cos(u1) * Math.Sin(u2) - Math.Sin(u1) * Math.Cos(u2) * Math.Cos(lam)));

            return dis;
        }

        private double GetDistanceHaversine(GeoCoordinate other)
        {
            var d1 = Latitude * Math.PI / 180.0;
            var num1 = Longitude * Math.PI / 180.0;
            var d2 = other.Latitude * Math.PI / 180.0;
            var num2 = other.Longitude * Math.PI / 180.0 - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return EarthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private double GetDistanceSphericalLawOfCosines(GeoCoordinate other)
        {
            var f1 = Latitude * Math.PI / 180;
            var f2 = other.Latitude * Math.PI / 180;
            var dl = (other.Longitude - Longitude) * Math.PI / 180;
            return Math.Acos(Math.Sin(f1) * Math.Sin(f2) + Math.Cos(f1) * Math.Cos(f2) * Math.Cos(dl)) * EarthRadius;
        }
    }
}