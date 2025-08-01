using Domain.Constants;
using Domain.Entities.Identity;
using Domain.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public static class AppLogger
    {
        public static void WriteLog<T>(this ILogger<T> logger, string message, object? logTypeParam = null, int userId = 0, LogType logType = LogType.Info, TimeSpan ElapsedTime = default)
        {
            logger.LogCritical("{Date}\t{Time}\t{UserId}\t{Method}\t{Path}\t{LogParam}\t{ElapsedTime}",
                PersianCalendarService.Now.ToString("yyyy/MM/dd"),
                DateTime.Now.ToString("HH:mm:ss"),
                userId.ToString(),
                logType.ToString(),
                message,
                JsonSerializer.Serialize(logTypeParam),
                ElapsedTime.ToString(@"dd\.hh\:mm\:ss\.fff"));
        }
    }
}