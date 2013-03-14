/************************************************
 * FileName: BoardModel.cs
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
    [DataContract(Name = "board")]
    public class BoardModel
    {
        [DataMember(Name = "name")]
        public string EnglishName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "unread")]
        public bool Unread { get; set; }

        [DataMember(Name = "users")]
        public uint Users { get; set; }

        [DataMember(Name = "count")]
        public uint Count { get; set; }

        [DataMember(Name = "leaf")]
        public bool Leaf { get; set; }

        [DataMember(Name = "boards")]
        public ObservableCollection<BoardModel> Boards { get; set; }
    }

    public class BoardsGroupModel : ObservableCollection<BoardModel>
    {
        public BoardsGroupModel(string name)
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
