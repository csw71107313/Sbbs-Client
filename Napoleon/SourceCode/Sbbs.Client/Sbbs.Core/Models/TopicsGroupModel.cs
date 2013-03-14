/************************************************
 * FileName: TopicsGroupModel.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Sbbs.Core
{
    [DataContract]
    public class HotTopicsModel 
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "topics")]
        public ObservableCollection<TopicModel> Topics { get; set; }
    }

    public class TopicsGroupModel : ObservableCollection<TopicModel>
    {
        public TopicsGroupModel(string name)
        {
            Title = name;
        }

        public string Title { get; set; }

        public bool HasItems
        {
            get
            {
                return Count != 0;
            }

            private set
            {
            }
        }
    }
}
