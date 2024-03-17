using System;
using System.Net;

namespace VirtualPetSchool.Http {
    internal class HttpResponse {
        private readonly HttpStatusCode _statusCode;
        public HttpStatusCode StatusCode { get { return _statusCode; } }

        private readonly object _data;
        public object Data { get { return _data; } }

        public HttpResponse(HttpStatusCode statusCode, object data) {
            this._statusCode = statusCode;
            this._data = data;
        }
    }
}
