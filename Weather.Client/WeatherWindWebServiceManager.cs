using System;

namespace Weather.Client
{
    public abstract class WeatherWindWebServiceManager
    {

        public abstract (double Speed, double Direction, string Trend) Get(DateTime startDate);


        public virtual double FormatWindSpeed(double speed)
        {
            if (speed < 0)
                throw new ArgumentException("Wind speed cannot be negative.");

            //en yakın ondalığa yuvarlamak için;
            return Math.Round(speed, 1);
        }
        public virtual string FormatWindDirection(double degrees)
        {
            if (degrees < 0 || degrees >= 360)
                throw new ArgumentException("Wind direction must be between 0 and 359 degrees.");

            return degrees switch
            {
                < 45 => "North",
                < 135 => "East",
                < 225 => "South",
                < 315 => "West",
                _ => "North"
            };
        }

        public virtual string FormatWindOutput(double speed, double direction, string trend)
        {
            return $"{speed} km/h, {FormatWindDirection(direction)}, Trend: {trend}";
        }
    }
}