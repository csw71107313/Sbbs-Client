/************************************************
 * FileName: Response.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Sbbs.Core
{
    /// <summary>
    /// 服务器响应通常会包括一个字段作为返回值，该返回值字段名通常不定，
    /// 因此响应DataContract子类需要自己手动定义哪个字段是返回值字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponse<T>
    {
        T Root { get; }
    }

    /// <summary>
    /// 服务器响应一定会包含的字段
    /// </summary>
    [DataContract]
    [KnownType(typeof(TopicsResponse))]
    [KnownType(typeof(BoardsResponse))]
    [KnownType(typeof(TokenResponse))]
    public class Response
    {
        [DataMember(Name = "success")]
        public bool Success;

        [DataMember(Name = "error")]
        public string Error;

        [DataMember(Name = "time")]
        public int Time;

        [DataMember(Name = "cost")]
        public int Cost;
    }

    /// <summary>
    /// 返回主题集合
    /// 符合类型： 十大，版面
    /// </summary>
    [DataContract]
    public class TopicsResponse : Response, IResponse<ObservableCollection<TopicModel>>
    {
        [DataMember(Name = "topics")]
        public ObservableCollection<TopicModel> m_Topics;

        public ObservableCollection<TopicModel> Root
        {
            get
            {
                return m_Topics;
            }
        }
    }

    /// <summary>
    /// 返回单个主题
    /// </summary>
    [DataContract]
    public class TopicResponse : Response, IResponse<TopicModel>
    {
        [DataMember(Name = "topic")]
        public TopicModel m_Topics;

        public TopicModel Root
        {
            get
            {
                return m_Topics;
            }
        }
    }

    /// <summary>
    /// 返回邮件集合
    /// </summary>
    [DataContract]
    public class MailsResponse : Response, IResponse<ObservableCollection<TopicModel>>
    {
        [DataMember(Name = "mails")]
        public ObservableCollection<TopicModel> m_Mails;

        public ObservableCollection<TopicModel> Root
        {
            get
            {
                return m_Mails;
            }
        }
    }

    /// <summary>
    /// 返回单封邮件
    /// </summary>
    [DataContract]
    public class MailResponse : Response, IResponse<TopicModel>
    {
        [DataMember(Name = "mail")]
        public TopicModel m_Mails;

        public TopicModel Root
        {
            get
            {
                return m_Mails;
            }
        }
    }

    /// <summary>
    /// 返回版面集合
    /// 符合类型： 收藏夹
    /// </summary>
    [DataContract]
    public class BoardsResponse : Response, IResponse<ObservableCollection<BoardModel>>
    {
        [DataMember(Name = "boards")]
        public ObservableCollection<BoardModel> m_Boards;

        public ObservableCollection<BoardModel> Root
        {
            get
            {
                return m_Boards;
            }
        }
    }

    /// <summary>
    /// 返回首页热点
    /// </summary>
    [DataContract]
    public class HotTopicsResponse : Response, IResponse<ObservableCollection<HotTopicsModel>>
    {
        [DataMember(Name = "topics")]
        public ObservableCollection<HotTopicsModel> m_Boards;

        public ObservableCollection<HotTopicsModel> Root
        {
            get
            {
                return m_Boards;
            }
        }
    }

    /// <summary>
    /// 返回用户认证Token
    /// </summary>
    [DataContract]
    public class TokenResponse : Response, IResponse<string>
    {
        [DataMember(Name = "token")]
        public string Token;

        [DataMember(Name = "id")]
        public string Id;

        [DataMember(Name = "name")]
        public string Name;

        public string Root
        {
            get
            {
                return Token;
            }
        }
    }

    /// <summary>
    /// 返回int结果
    /// </summary>
    [DataContract]
    public class ResultResponse : Response, IResponse<int>
    {
        [DataMember(Name = "result")]
        public int Result;

        public int Root
        {
            get
            {
                return Result;
            }
        }
    }
}
