namespace HypergraphDiplom.Domain.Helpers.Guarding;

/// <summary>
/// 
/// </summary>
public sealed class GuardantException:
    Exception
{

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public GuardantException(
        string message,
        Exception? innerException = null) : base(message, innerException)
    {

    }

    #endregion

}