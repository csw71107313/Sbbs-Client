/************************************************
 * FileName: IApplicationBarService.cs
 * Document-related:
 * Module: Sbbb.Controls
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-06-2013
 *************************************************/

namespace Sbbs.Controls
{
    public interface IApplicationBarService
    {
        /// <summary>
        /// A work around for 
        /// 如何在 Windows Phone 应用程序的一个控件中使用不同的应用程序栏 
        /// </summary>
        bool IsApplicationBarVisible { set; get; }
    }
}
