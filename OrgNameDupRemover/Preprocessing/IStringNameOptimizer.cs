namespace OrgNameDupRemover.Preprocessing
{
    /// <summary>
    /// An operation which results in an string that would be matched better
    /// with other forms of the same concept
    /// </summary>
    public interface IStringNameOptimizer
    {
        string Optimize(string target);
    }
}
