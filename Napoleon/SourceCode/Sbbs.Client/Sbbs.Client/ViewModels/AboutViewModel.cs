/************************************************
 * FileName: AboutViewModel.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-09-2013
 *************************************************/

using Microsoft.Practices.Prism.ViewModel;

namespace Sbbs.Client
{
    public class AboutViewModel : NotificationObject
    {
        #region [Fields]

        private static readonly string SBBSURI = "http://bbs.seu.edu.cn/bbsqry.php?userid=ND";
        private static readonly string WEIBOURI = "http://weibo.cn/ndisfool";

        #endregion

        #region [Properties]

        public string SbbsUri
        {
            get
            {
                return SBBSURI;
            }
        }

        public string WeiboUri
        {
            get
            {
                return WEIBOURI;
            }
        }
        
        #endregion
    }
}
