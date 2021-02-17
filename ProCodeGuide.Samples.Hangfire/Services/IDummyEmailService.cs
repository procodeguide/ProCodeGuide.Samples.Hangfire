using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCodeGuide.Samples.Hangfire.Services
{
    public interface IDummyEmailService
    {
        void SendEmail(string backGroundJobType, string startTime);
    }
}
