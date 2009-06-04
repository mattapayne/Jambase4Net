using System;

namespace Jambase4Net
{
    public interface IVenue : IJambaseObject
    {
        String Name { get; }
        String City { get; }
        String State { get; }
        String ZipCode { get; }
    }
}
