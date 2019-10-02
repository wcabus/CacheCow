﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CacheCow.Server
{
    /// <summary>
    /// Default impl where it tries to cast as ICacheResource and if successful, calls the method
    /// </summary>
    public class DefaultTimedETagExtractor : ITimedETagExtractor
    {
        private readonly ISerialiser _serialiser;
        private readonly IHasher _hasher;

        public DefaultTimedETagExtractor(ISerialiser serialiser, IHasher hasher)
        {
            _serialiser = serialiser;
            _hasher = hasher;
        }

        public TimedEntityTagHeaderValue Extract(object viewModel)
        {
            var resource = viewModel as ICacheResource;
            if (resource != null)
                return resource.GetTimedETag();

            return new TimedEntityTagHeaderValue(_hasher.ComputeHash(_serialiser.Serialise(viewModel)));
        }
    }


    public class DefaultTimedETagExtractor<TViewModel> : DefaultTimedETagExtractor, ITimedETagExtractor<TViewModel>
    {
        public DefaultTimedETagExtractor(ISerialiser serialiser, IHasher hasher) : base(serialiser, hasher)
        {
        }

        public TimedEntityTagHeaderValue Extract(TViewModel viewModel)
        {
            return base.Extract(viewModel);
        }
    }

}
