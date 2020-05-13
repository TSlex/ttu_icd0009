namespace WebApp.Areas.Admin.Models
{
    /// <summary>
    /// Error
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Request id for view
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}