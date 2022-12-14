using System;
using System.Web;

namespace HW05
{
    /// <summary>
    /// Interface defining methods for informing about progress
    /// </summary>
    public interface IProgressHandler
    {
        /// <summary>
        /// Updates actual value of progress
        /// </summary>
        /// <param name="progress">Actual value of progress in interval [0; 100]</param>
        void UpdateProgress(float progress);
    }
}
