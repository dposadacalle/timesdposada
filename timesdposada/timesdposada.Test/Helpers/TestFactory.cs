using employees.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;
using timesdposada.Common.Models;
using timesdposada.Functions.Entities;

namespace timesdposada.Test.Helpers
{
    public class TestFactory
    {
        public static TimeEntity GetEntryEntity()
        {
            return new TimeEntity
            {
                ETag = "*",
                PartitionKey = "TIME",
                RowKey = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                IsConsolidated = false,
                Type = "0"
            };
        }
        public static DefaultHttpRequest CreateHttpRequest(Guid entryId, Time entryRequest)
        {
            string request = JsonConvert.SerializeObject(entryRequest); 
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{entryId}" 
            };
        }

        public static DefaultHttpRequest DeleteHttpRequest(Guid entryId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{entryId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Time entryRequest)
        {
            string request = JsonConvert.SerializeObject(entryRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Time GetEntryRequest()
        {
            return new Time
            {
                Date = DateTime.Now,
                Consolidate = false,
                Type = "1"
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
