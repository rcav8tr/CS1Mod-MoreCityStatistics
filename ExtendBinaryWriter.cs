using System;
using System.IO;

namespace MoreCityStatistics
{
    /// <summary>
    /// extend BinaryWriter to write date/time and nullable values
    /// for date/time, write the year, month, day, hour, minute, and second
    /// for nullables, always write both the flag and the value, write zero if the value is null
    /// </summary>
    public static class ExtendBinaryWriter
    {
        /// <summary>
        /// Writes a date/time to the current stream and advances the stream position by 24 bytes.
        /// </summary>
        public static void Write(this BinaryWriter writer, DateTime value)
        {
            writer.Write(value.Year);
            writer.Write(value.Month);
            writer.Write(value.Day);
            writer.Write(value.Hour);
            writer.Write(value.Minute);
            writer.Write(value.Second);
        }

        /// <summary>
        /// Writes a nullable four-byte signed integer to the current stream and advances the stream position by five bytes.
        /// </summary>
        public static void Write(this BinaryWriter writer, int? value)
        {
            writer.Write(value.HasValue);
            writer.Write(value ?? 0);
        }

        /// <summary>
        /// Writes a nullable four-byte unsigned integer to the current stream and advances the stream position by five bytes.
        /// </summary>
        public static void Write(this BinaryWriter writer, uint? value)
        {
            writer.Write(value.HasValue);
            writer.Write(value ?? 0);
        }

        /// <summary>
        /// Writes a nullable eight-byte signed integer to the current stream and advances the stream position by nine bytes.
        /// </summary>
        public static void Write(this BinaryWriter writer, long? value)
        {
            writer.Write(value.HasValue);
            writer.Write(value ?? 0);
        }

        /// <summary>
        /// Writes a nullable four-byte floating-point value to the current stream and advances the stream position by five bytes.
        /// </summary>
        public static void Write(this BinaryWriter writer, float? value)
        {
            writer.Write(value.HasValue);
            writer.Write(value ?? 0);
        }
    }
}
