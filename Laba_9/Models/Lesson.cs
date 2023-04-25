using System;

namespace Laba_9.Models
{
    public class Lesson //: IEquatable<Lesson>
    {
        public int id { get; set; }
        public string lesson_name { get; set; }
        public string description { get; set; }
        //public bool Equals(Lesson other)
        //{
        //    //Check whether the compared object is null.
        //    if (Object.ReferenceEquals(other, null)) return false;
        //    //Check whether the compared object references the same data.
        //    if (Object.ReferenceEquals(this, other)) return true;
        //    //Check whether the products' properties are equal.
        //    return this.id == other.id;
        //}
        //// If Equals() returns true for a pair of objects
        //// then GetHashCode() must return the same value for these objects.
        //public override int GetHashCode()
        //{
        //    //Calculate the hash code for the product.
        //    return id;
        //}
    }
}