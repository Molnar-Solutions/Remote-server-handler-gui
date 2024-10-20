using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.model
{
    public record struct SysteamHealthResponse(
        string osType,
        string architecture,
        long cpuUsage,
        long availableMemory,
        long totalMemory,
        long availableStorage,
        string[] logs
    );

    public record struct SystemHealthRequest(
        string userName    
    );
}
