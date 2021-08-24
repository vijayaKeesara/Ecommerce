using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Ecommerce.DA
{
    public partial interface IConfig
    {
        /// <summary>
        /// Gets a section name to load configuration
        /// </summary>
        [JsonIgnore]
        string Name => GetType().Name;
    }
    public partial class CacheConfig : IConfig
    {
        /// <summary>
        /// Gets or sets the default cache time in minutes
        /// </summary>
        public int DefaultCacheTime { get; set; } = 60;

        /// <summary>
        /// Gets or sets the short term cache time in minutes
        /// </summary>
        public int ShortTermCacheTime { get; set; } = 3;
    }
}
