/************************************************
 * FileName: IIsolatedStorageFacade.cs
 * Document-related:
 * Module: Sbbs.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-09-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using Sbbs.Core;

namespace Sbbs.Client
{
    public interface IIsolatedStorageFacade
    {
        /// <summary>
        /// Get the Top Ten topics from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Top Ten topics are not in isolated storage.</exception>
        /// <returns>ObservableCollection of TopicModel objects</returns>
        ObservableCollection<TopicModel> GetTopTen();

        /// <summary>
        /// Saves the Top Ten topics to isolated storage.
        /// </summary>
        /// <param name="topTenTopics">The Top Ten topics</param>
        /// <exception cref="ArgumentNullException">If the Top Ten topics argument is null</exception>
        void SaveTopTen(ObservableCollection<TopicModel> topTenTopics);

        /// <summary>
        /// Get all the sections from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If all the sections are not in isolated storage.</exception>
        /// <returns>ObservableCollection of BoardModel objects</returns>
        ObservableCollection<BoardModel> GetAllSections();

        /// <summary>
        /// Saves all the sections to isolated storage.
        /// </summary>
        /// <param name="sections">All the sections</param>
        /// <exception cref="ArgumentNullException">If the sections argument is null</exception>
        void SaveAllSections(ObservableCollection<BoardModel> sections);

        /// <summary>
        /// Get all the Favorite boards from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If all the Favorite boards are not in isolated storage.</exception>
        /// <returns>ObservableCollection of BoardModel objects</returns>
        ObservableCollection<BoardModel> GetFavorites();

        /// <summary>
        /// Saves all the Favorite boards to isolated storage.
        /// </summary>
        /// <param name="favorites">All the favorite boards</param>
        void SaveFavorites(ObservableCollection<BoardModel> favorites);

        /// <summary>
        /// Get the user Token from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the user Token is not in isolated storage.</exception>
        /// <returns>The user Token</returns>
        string GetToken();

        /// <summary>
        /// Saves the user Token to isolated storage.
        /// </summary>
        /// <param name="token">The user Token</param>
        void SaveToken(string token);

        /// <summary>
        /// Get the user name from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the user name is not in isolated storage.</exception>
        /// <returns>The user name</returns>
        string GetUsername();

        /// <summary>
        /// Saves the user name to isolated storage.
        /// </summary>
        /// <param name="username">The user name</param>
        void SaveUsername(string username);

        /// <summary>
        /// Get the Board Mode from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Board Mode is not in isolated storage.</exception>
        /// <returns>The Board Mode</returns>
        int GetBoardMode();

        /// <summary>
        /// Saves the Board Mode to isolated storage.
        /// </summary>
        /// <param name="boardMode">The Board Mode</param>
        /// <exception cref="ArgumentOutOfRangeException ">If the boardMode'id argument is not between 0 and 2 as a Integer</exception>
        void SaveBoardMode(int boardMode);

        /// <summary>
        /// Get the board description from isolated storage.
        /// </summary>
        /// <param name="englishName">The english name of the board</param>
        /// <exception cref="InvalidOperationException">If the board description is not in isolated storage.</exception>
        /// <returns>The description of the board</returns>
        string GetBoardDescriptionByName(string englishName);

        /// <summary>
        /// Saves the board description to isolated storage.
        /// </summary>
        /// <param name="englishName">The english name of the board</param>
        /// <param name="description">The description of the board</param>
        void SaveBoardDescription(string englishName, string description);
    }
}
