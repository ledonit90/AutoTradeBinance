using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Binances.Helper
{
    public class HashHelper
    {
        #region Hmac Functions
        public string HmacSha256(string secretKey, string Message)
        {
            var ascii = new ASCIIEncoding();
            var secretKeyByte = ascii.GetBytes(secretKey);
            var messageByte = ascii.GetBytes(Message);
            HMACSHA256 hmac = new HMACSHA256(secretKeyByte);
            var signatureByte = hmac.ComputeHash(messageByte);
            return ByteToString(signatureByte);
        }
        #endregion

        #region Encoding Helpers
        public string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
        #endregion
    }
}
