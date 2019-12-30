using BraunauMobil.VeloBasar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BraunauMobil.VeloBasar.Tests.JsonUtilsTests
{
    public class GetLineConfig
    {
        [Fact]
        public void Empty()
        {
            var config = JsonUtils.GetLineConfig(Array.Empty<ChartDataPoint>(), "Label");
            Assert.Equal("{\"type\":\"line\",\"data\":{\"labels\":[],\"datasets\":[{\"label\":\"Label\",\"fill\":false,\"data\":[],\"backgroundColor\":\"rgb(0, 0, 0)\",\"borderColor\":\"rgb(0, 0, 0)\"}]}}", config);
        }
    }
}
