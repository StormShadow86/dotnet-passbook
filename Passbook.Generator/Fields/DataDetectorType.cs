using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passbook.Generator.Fields
{
	[Flags]
	public enum DataDetectorTypes
	{
		/// <summary>
		/// Do not detect any data types
		/// </summary>
		None = 1,
		/// <summary>
		/// Automatically detect phone numbers
		/// </summary>
		PKDataDetectorTypePhoneNumber = 1,
		/// <summary>
		/// Automatically detect links
		/// </summary>
		PKDataDetectorTypeLink = 2,
		/// <summary>
		/// Automatically detect addresses
		/// </summary>
		PKDataDetectorTypeAddress = 4,
		/// <summary>
		/// Automatically detect calendar events
		/// </summary>
		PKDataDetectorTypeCalendarEvent = 8
	}
}
