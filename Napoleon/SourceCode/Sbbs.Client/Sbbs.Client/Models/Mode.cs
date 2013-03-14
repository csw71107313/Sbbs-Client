/************************************************
 * FileName: Mode.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 03-08-2013
 *************************************************/

namespace Sbbs.Client
{
    public class Mode
    {
        public int Id
        {
            get; private set;
        }

        public string Description
        {
            get; private set;
        }

        public Mode(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public Mode()
        {
            
        }
    }
}
