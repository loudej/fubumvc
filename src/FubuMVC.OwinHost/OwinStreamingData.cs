using System;
using System.IO;
using FubuMVC.Core.Http;

namespace FubuMVC.OwinHost
{
    class OwinStreamingData : IStreamingData
    {
        private readonly Stream _stream;

        public OwinStreamingData(Gate.Request req)
        {
            _stream = req.Body;
        }

        public Stream Input
        {
            get
            {
                if (_stream.CanSeek)
                {
                    _stream.Seek(0, SeekOrigin.Begin);
                }
                return _stream;
            }
        }
    }
}