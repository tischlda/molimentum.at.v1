﻿using System;

namespace LumiSoft.Net.Dns.Client
{
    /// <summary>
    /// DNS client exception.
    /// </summary>
    public class DNS_ClientException : Exception
    {
        private RCODE m_RCode;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="rcode">DNS server returned error code.</param>
        public DNS_ClientException(RCODE rcode) : base("Dns error: " + rcode + ".")
        {
            m_RCode = rcode;
        }


        #region Properties implementation

        /// <summary>
        /// Gets DNS server returned error code.
        /// </summary>
        public RCODE ErrorCode
        {
            get{ return m_RCode; }
        }

        #endregion

    }
}
