/************************************************
 * FileName: IModeService.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

namespace Sbbs.Client
{
    public interface IModeService
    {
        /// <summary>
        /// Get the mode settings.
        /// </summary>
        ModeSettings ModeSettings { get; }
    }
}
