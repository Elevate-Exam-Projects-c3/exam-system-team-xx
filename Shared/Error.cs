
using System.Text.Json.Serialization;

namespace exam_system.Shared
{
    public class Error : IEquatable<Error>
    {
        /// <summary>
        /// Gets the error code.
        /// And Initial the property with the specified code.
        /// </summary>
        public string Code { get; }

        public string Message { get; }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Handle the case when the error is null or empty, and return a default error.
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        /// <summary>
        /// Converts an Error object to a string representation of the error code.
        /// </summary>
        public static implicit operator string(Error error) => error.Code;

        /// <summary>
        /// Defines the equality operator for comparing two Error objects.
        /// compares the Code and Message properties of the Error objects to determine equality.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Defines the inequality operator for comparing two Error objects.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Error? a, Error? b) => !(a == b);

        /// <summary>
        /// Defines the equality comparison for the Error class.
        /// </summary>
        /// <param name="other">The other Error object to compare with.</param>
        /// <returns>True if the current Error object is equal to the other Error object; otherwise, false.</returns>
        public virtual bool Equals(Error? other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code && Message == other.Message;
        }

        /// <summary>
        /// Overrides the Equals method to compare the current Error object with another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        /// <summary>
        /// Overrides the GetHashCode method to provide a hash code for the Error object based on its properties.
        /// </summary>
        /// <returns>A hash code for the current Error object.</returns>
        public override int GetHashCode() => HashCode.Combine(Code, Message);

        /// <summary>
        /// Overrides the ToString method to provide a string representation of the Error object.
        /// </summary>
        /// <returns>A string representation of the current Error object.</returns>
        public override string ToString() => Code;
    }
}