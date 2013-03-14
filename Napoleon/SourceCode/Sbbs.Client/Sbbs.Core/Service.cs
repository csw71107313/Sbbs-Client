/************************************************
 * FileName: BoardModel.cs
 * Document-related:
 * Module: Sbbs.Core
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-03-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using TopicCollection = System.Collections.ObjectModel.ObservableCollection<Sbbs.Core.TopicModel>;
using BoardCollection = System.Collections.ObjectModel.ObservableCollection<Sbbs.Core.BoardModel>;

namespace Sbbs.Core
{
    /// <summary>
    /// 回调函数参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ServiceArg<T>
    {
        public Action<T, bool, string> Callback;
    }

    public class Service
    {
        #region [Constants]

        /// <summary>
        /// API前缀和后缀
        /// </summary>
        private const string APIBASE = "http://bbs.seu.edu.cn/napi/";
        private const string APIPOST = ".json";

        //默认版面模式
        private int m_BoardMode = 2;

        #endregion

        #region [Properties]

        /// <summary>
        /// 用户认证Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 版面模式
        /// </summary>
        public int BoardMode
        {
            get { return m_BoardMode; }
            set
            {
                if (value != m_BoardMode)
                {
                    m_BoardMode = value;
                }
            }
        }

        #endregion

        #region [Public Methods]
      
        /// <summary>
        /// 登录换取Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="callback"></param>
        public void Login(string userName, string password, Action<string, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "token" + APIPOST 
                                      + "?user=" + HttpUtility.UrlEncode(userName)
                                      + "&pass=" + HttpUtility.UrlEncode(password));

            wc.DownloadStringCompleted += DownloadedAndParse<string, TokenResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<string>() { Callback = callback });
        }

        /// <summary>
        /// 全部版面
        /// </summary>
        /// <param name="callback"></param>
        public void GetAllSections(Action<BoardCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "sections" + APIPOST 
                                      + "?up=1&token=" + HttpUtility.UrlEncode(Token));

            wc.DownloadStringCompleted += DownloadedAndParse<BoardCollection, BoardsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<BoardCollection>() { Callback = callback });
        }

        /// <summary>
        /// 获取十大
        /// </summary>
        /// <param name="callback"></param>
        public void GetTopTen(Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "hot/topten" + APIPOST);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        /// <summary>
        /// 获取收藏夹
        /// </summary>
        /// <param name="callback"></param>
        public void GetFavorites(Action<BoardCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "fav/list" + APIPOST 
                                      + "?up=1&token=" + HttpUtility.UrlEncode(Token));

            wc.DownloadStringCompleted += DownloadedAndParse<BoardCollection, BoardsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<BoardCollection>() { Callback = callback });
        }

        /// <summary>
        /// 获取版面
        /// </summary>
        /// <param name="board"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="callback"></param>
        public void GetBoard(string board, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            GetBoard(board, BoardMode, start, limit, callback);
        }

        /// <summary>
        /// 版面标记已读
        /// </summary>
        /// <param name="board"></param>
        public void MarkBoardAsRead(string board)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "board/" + board 
                                      + "/markread" + APIPOST
                                      + "?token=" + HttpUtility.UrlEncode(Token));

            wc.DownloadStringAsync(uri);
        }

        /// <summary>
        /// 获取话题
        /// </summary>
        /// <param name="board"></param>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="callback"></param>
        public void GetTopic(string board, int id, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "topic/" + board + "/" + id + APIPOST 
                                      + "?token=" + HttpUtility.UrlEncode(Token) 
                                      + "&start=" + start
                                      + "&limit=" + limit);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        /// <summary>
        /// 首页热点
        /// </summary>
        /// <param name="callback"></param>
        public void GetHotTopics(Action<ObservableCollection<HotTopicsModel>, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "hot/topics" + APIPOST);

            wc.DownloadStringCompleted += DownloadedAndParse<ObservableCollection<HotTopicsModel>, HotTopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<ObservableCollection<HotTopicsModel>>() { Callback = callback });
        }

        /// <summary>
        /// 热门版面
        /// </summary>
        /// <param name="callback"></param>
        public void GetHotBoards(Action<BoardCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "hot/boards" + APIPOST);

            wc.DownloadStringCompleted += DownloadedAndParse<BoardCollection, BoardsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<BoardCollection>() { Callback = callback });
        }

        /// <summary>
        /// 发帖
        /// </summary>
        /// <param name="board"></param>
        /// <param name="reid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="callback"></param>
        public void PostTopic(string board, int reid, string title, string content, Action<TopicModel, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "topic/post" + APIPOST 
                                      + "?type=2&token=" + HttpUtility.UrlEncode(Token) 
                                      + "&board=" + board 
                                      + "&reid=" + reid
                                      + "&title=" + HttpUtility.UrlEncode(title) 
                                      + "&content=" + HttpUtility.UrlEncode(content));

            wc.DownloadStringCompleted += DownloadedAndParse<TopicModel, TopicResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicModel>() { Callback = callback });
        }

        /// <summary>
        /// 邮箱内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="callback"></param>
        public void GetMailBox(int type, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "mailbox/get" + APIPOST 
                                      + "?token=" + HttpUtility.UrlEncode(Token)
                                      + "&start=" + start 
                                      + "&limit=" + limit 
                                      + "&type=" + type);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, MailsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        /// <summary>
        /// 单封邮件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void GetMail(int type, int id, Action<TopicModel, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "mail/get" + APIPOST 
                                      + "?token=" + HttpUtility.UrlEncode(Token) 
                                      + "&type=" + type 
                                      + "&id=" + id);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicModel, MailResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicModel>() { Callback = callback });
        }

        /// <summary>
        /// 写邮件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="callback"></param>
        public void SendMail(string user, string title, string content, Action<TopicModel, bool, string> callback)
        {
            SendMail(user, title, content, 0, callback);
        }

        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void DeleteMail(int type, int id, Action<int, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "mail/delete" + APIPOST 
                                      + "?token=" + HttpUtility.UrlEncode(Token) 
                                      + "&type=" + type 
                                      + "&id=" + id);

            wc.DownloadStringCompleted += DownloadedAndParse<int, ResultResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<int>() { Callback = callback });
        }
        
        #endregion

        #region [Private Methods]
        
        /// <summary>
        /// 获取版面
        /// </summary>
        /// <param name="board"></param>
        /// <param name="mode"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="callback"></param>
        private void GetBoard(string board, int mode, int start, int limit, Action<TopicCollection, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "board/" + board + APIPOST
                                      + "?token=" + HttpUtility.UrlEncode(Token)
                                      + "&start=" + start
                                      + "&limit=" + limit
                                      + "&mode=" + mode);

            wc.DownloadStringCompleted += DownloadedAndParse<TopicCollection, TopicsResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicCollection>() { Callback = callback });
        }

        /// <summary>
        /// 写邮件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="reid"></param>
        /// <param name="callback"></param>
        private void SendMail(string user, string title, string content, int reid, Action<TopicModel, bool, string> callback)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(APIBASE + "mail/send" + APIPOST
                                      + "?token=" + HttpUtility.UrlEncode(Token)
                                      + "&user=" + user
                                      + "&reid=" + reid
                                      + "&title=" + HttpUtility.UrlEncode(title)
                                      + "&content=" + HttpUtility.UrlEncode(content));

            wc.DownloadStringCompleted += DownloadedAndParse<TopicModel, MailResponse>;
            wc.DownloadStringAsync(uri, new ServiceArg<TopicModel>() { Callback = callback });
        }

        #endregion

        #region [Handlers]

        /// <summary>
        /// 下载完成后分析JSON数据然后调用回调函数
        /// </summary>
        /// <typeparam name="C">C为返回类型，比如TopicCollection</typeparam>
        /// <typeparam name="R">R为JSON的Response类型</typeparam>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadedAndParse<C, R>(object sender, DownloadStringCompletedEventArgs e)
            where R : Response, IResponse<C>
        {
            // 用户传入参数
            ServiceArg<C> arg = (ServiceArg<C>)e.UserState;
            // 检查网络错误
            if (e.Error != null)
            {
                arg.Callback(default(C), false, "网络连接错误");
                return;
            }

            // 读取下载的字符串
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(e.Result)))
            {
                // 转换成JSON
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(R));
                R result = serializer.ReadObject(stream) as R;
                if (result.Error != null)
                {
                    // result.error表示有错误
                    arg.Callback(default(C), true, result.Error);
                }
                else
                {
                    arg.Callback(result.Root, true, null);
                }
            }
        }

        #endregion
    }
}
