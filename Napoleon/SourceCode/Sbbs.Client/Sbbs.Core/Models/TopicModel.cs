/************************************************
 * FileName: TopicModel.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Runtime.Serialization;

namespace Sbbs.Core
{
    [DataContract(Name = "topic")]
    public class TopicModel
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 文章正文
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }

        [DataMember(Name = "board")]
        public string Board { get; set; }

        [DataMember(Name = "quote")]
        public string Quote { get; set; }

        [DataMember(Name = "quoter")]
        public string Quoter { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "reid")]
        public int Reid { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "replies")]
        public int Replies { get; set; }

        [DataMember(Name = "read")]
        public int Read { get; set; }

        [DataMember(Name = "unread")]
        public bool Unread { get; set; }

        [DataMember(Name = "top")]
        public bool Top { get; set; }

        [DataMember(Name = "mark")]
        public bool Mark { get; set; }
    }
}
