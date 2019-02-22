namespace Lykke.Service.ConfirmationCodes.Contract.Models
{
    /// <summary>
    /// call status
    /// </summary>
    public enum CallLimitStatus
    {
        /// <summary>
        /// Call is allowed
        /// </summary>
        Allowed,
        
        /// <summary>
        /// Timeout between calls
        /// </summary>
        CallTimeout,
        
        /// <summary>
        /// Reached maximum number of calls
        /// </summary>
        LimitExceed
    }
}
