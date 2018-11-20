using Microsoft.Extensions.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.Infrastructure
{
    public class CDriveHasMoreThan1GbFreeHealthCheck : IHealthCheck
    {
        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            long freeSpaceinGb = GetTotalFreeSpaceInGb(@"C:\");
            CheckStatus status = freeSpaceinGb > 1 ? CheckStatus.Healthy : CheckStatus.Unhealthy;

            return new ValueTask<IHealthCheckResult>(HealthCheckResult.FromStatus(status, $"Free Space in GB: {freeSpaceinGb}"));

        }

        private long GetTotalFreeSpaceInGb(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalFreeSpace / 1024 / 1024 / 1024;
                }
            }
            throw new ArgumentException($"Invalid Drive Name {driveName}");
        }
    }
}
