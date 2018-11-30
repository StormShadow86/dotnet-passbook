﻿using System;
using System.Collections.Generic;
using Passbook.Generator;

namespace Passbook.Web
{
	public abstract class PassProvider : IDisposable
	{
        /// <summary>
        /// Passtype identifier supported by this <see cref="T:PassProvider" />
        /// </summary>
        public abstract string PassTypeIdentifier { get; }

        /// <summary>
        /// Indicates where the <paramref name="passTypeIndentifier" /> specified is supposed by this <see cref="PassProvider" /> 
        /// </summary>
        /// <returns><c>true</c>, if pass type is supported, <c>false</c> otherwise.</returns>
        /// <param name="passTypeIdentifier">Pass type identifier.</param>
		public bool SupportsPassType(string passTypeIdentifier)
        {
            return String.Equals(this.PassTypeIdentifier, passTypeIdentifier, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates wether passes generated by this <see cref="PassProvider" /> should use Passbook Webservice for dynamic updates 
        /// </summary>
        /// <returns><c>true</c> if this instance generates dynamically updating passes; otherwise, <c>false</c>.</returns>
        public abstract bool IsUpdating();

        /// <summary>
        /// Registers a device pass for future dynamic updates
        /// </summary>
        /// <param name="deviceLibraryIdentifier">Device library identifier</param>
        /// <param name="passTypeIdentifier">Pass type identifier</param>
        /// <param name="serialNumber">Serial number</param>
        /// <param name="pushToken">Push token</param>
        /// <remarks>>Only required for dynamic updating passes</remarks>
		public virtual void RegisterDevicePass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Retrieve the list of passes registered for a device which have been updated.
        /// </summary>
        /// <returns>List of pass serialNumbers</returns>
        /// <param name="deviceLibraryIdentifier">Device library identifier</param>
        /// <param name="passTypeIdentifier">Pass type identifier</param>
        /// <param name="updatedSince">Return only passes updated since (UTC datetime stamp)</param>
        /// <remarks>>Only required for dynamic updating passes</remarks>
		public virtual List<string> GetDevicePasses(string deviceLibraryIdentifier, string passTypeIdentifier, DateTime updatedSince)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Prepares the <see cref="T:Passbook.Generator.PassGeneratorRequest" /> for pass generation.
        /// </summary>
        /// <returns><see cref="T:Passbook.Generator.PassGeneratorRequest" /></returns>
        /// <param name="passTypeIdentifier">Pass type identifier</param>
        /// <param name="serialNumber">Pass Serial number</param>
		public virtual PassGeneratorRequest GetPass(string serialNumber)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Unregisters the device pass
        /// </summary>
        /// <param name="deviceLibraryIdentifier">Device library identifier</param>
        /// <param name="passTypeIdentifier">Pass type identifier</param>
        /// <param name="serialNumber">Serial number</param>
        /// <remarks>>Only required for dynamic updating passes</remarks>
		public virtual void UnregisterDevicePass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Releases all resource used by the <see cref="Passbook.Web.PassProvider"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Passbook.Web.PassProvider"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="Passbook.Web.PassProvider"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="Passbook.Web.PassProvider"/> so the garbage collector can reclaim the memory that the
        /// <see cref="Passbook.Web.PassProvider"/> was occupying.</remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Passbook.Web.PassProvider"/> is reclaimed by garbage collection.
        /// </summary>
		~PassProvider() 
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

        /// <summary>
        /// Releases all resource used by the <see cref="Passbook.Web.PassProvider"/> object.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
		}
	}
}

