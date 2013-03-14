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
    /// ��������Ӧͨ�������һ���ֶ���Ϊ����ֵ���÷���ֵ�ֶ���ͨ��������
    /// �����ӦDataContract������Ҫ�Լ��ֶ������ĸ��ֶ��Ƿ���ֵ�ֶ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponse<T>
    {
        T Root { get; }
    }

    /// <summary>
    /// ��������Ӧһ����������ֶ�
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
    /// �������⼯��
    /// �������ͣ� ʮ�󣬰���
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
    /// ���ص�������
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
    /// �����ʼ�����
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
    /// ���ص����ʼ�
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
    /// ���ذ��漯��
    /// �������ͣ� �ղؼ�
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
    /// ������ҳ�ȵ�
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
    /// �����û���֤Token
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
    /// ����int���
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
