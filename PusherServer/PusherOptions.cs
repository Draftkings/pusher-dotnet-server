﻿using RestSharp;
using System;

namespace PusherServer
{
    /// <summary>
    /// Options to be set on the <see cref="Pusher">Pusher</see> instance.
    /// </summary>
    public class PusherOptions: IPusherOptions
    {
        private static int DEFAULT_HTTPS_PORT = 443;
        private static int DEFAULT_HTTP_PORT = 80;

        IRestClient _client;
        bool _encrypted = false;
        bool _portModified = false;
        int _port = DEFAULT_HTTP_PORT;

        /// <summary>
        /// Gets or sets a value indicating whether calls to the Pusher REST API are over HTTP or HTTPS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if encrypted; otherwise, <c>false</c>.
        /// </value>
        public bool Encrypted
        {
            get
            {
                return _encrypted;
            }
            set
            {
                _encrypted = value;
                if (_encrypted && _portModified == false)
                {
                    _port = 443;
                }
            }
        }

        /// <summary>
        /// Gets or sets the REST API port that the HTTP calls will be made to.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                _portModified = true;
            }
        }

        /// <summary>
        /// Gets or sets the rest client. Generally only expected to be used for testing.
        /// </summary>
        /// <value>
        /// The rest client.
        /// </value>
        public IRestClient RestClient
        {
            get
            {
                if (_client == null)
                {
                    _client = new RestClient(GetBaseUrl().ToString());
                }
                return _client;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("RestClient cannot be null");
                }
                _client = value;
            }
        }

        /// <inheritdoc/>
        public string HostName { get; set; }

        private static string DEFAULT_REST_API_HOST = "api.pusherapp.com";

        private Uri GetBaseUrl()
        {
            string hostName = HostName ?? DEFAULT_REST_API_HOST;

            string baseUrl = (Encrypted ? "https" : "http") + "://" +
                hostName +
                (Port == 80 ? "" : ":" + Port);
            return new Uri(baseUrl);
        }
    }
}
