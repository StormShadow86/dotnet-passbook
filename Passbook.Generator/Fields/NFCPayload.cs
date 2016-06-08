using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Fields
{
    public class NFCPayload
    {
        /// <summary>
        /// Required. The payload to be transmitted to the Apple Pay terminal.
        /// Must be 64 bytes or less. Message longer than 64 bytes are truncated by the system.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Optional. Public encryption key used by the Value Added Services protocol.
        /// Use a Base64 encoded X.509 SubjectPublicKeyInfo structure containing a ECDH public key for group P256.
        /// </summary>
        public string EncryptionPublicKey { get; set; }
    }
}
