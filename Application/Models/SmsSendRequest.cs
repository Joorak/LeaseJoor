// File: Models/SmsSendRequest.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.Models
{
    /// <summary>
    /// Request model for sending an SMS.
    /// </summary>
    public class SmsSendRequest
    {
        /// <summary>
        /// Mobile number to send the SMS to.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Content of the SMS.
        /// </summary>
        public string SmsContent { get; set; } = string.Empty;

        /// <summary>
        /// Priority of the SMS.
        /// </summary>
        public Priority SendPriority { get; set; }
    }
}