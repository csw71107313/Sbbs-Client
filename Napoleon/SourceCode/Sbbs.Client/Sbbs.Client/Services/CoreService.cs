/************************************************
 * FileName: CoreService.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using Sbbs.Core;

namespace Sbbs.Client
{
    public class CoreService
    {
        private static Service m_Instance = null;
        public static Service Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Service();

                return m_Instance;
            }
        }
    }
}
