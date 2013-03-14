/************************************************
 * FileName: IsolatedStorageFacade.cs
 * Document-related:
 * Module: Sbbs.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-09-2013
 *************************************************/

using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using Sbbs.Core;

namespace Sbbs.Client
{
    public class IsolatedStorageFacade : IIsolatedStorageFacade
    {
        /// <summary>
        /// Get the Top Ten topics from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Top Ten topics are not in isolated storage.</exception>
        /// <returns>ObservableCollection of TopicModel objects</returns>
        public ObservableCollection<TopicModel> GetTopTen()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.TopTenKey))
            {
                throw new InvalidOperationException("The Top Ten topics are not in isolated storage.");
            }

            var topTenTopics =
                (ObservableCollection<TopicModel>) IsolatedStorageSettings.ApplicationSettings[Constants.TopTenKey];
            return topTenTopics;
        }

        /// <summary>
        /// Saves the Top Ten topics to isolated storage.
        /// </summary>
        /// <param name="topTenTopics">The Top Ten topics</param>
        /// <exception cref="ArgumentNullException">If the Top Ten topics argument is null</exception>
        public void SaveTopTen(ObservableCollection<TopicModel> topTenTopics)
        {
            if (topTenTopics == null)
            {
                throw new ArgumentNullException("topTenTopics");
            }

            IsolatedStorageSettings.ApplicationSettings[Constants.TopTenKey] = topTenTopics;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Get all the sections from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If all the sections are not in isolated storage.</exception>
        /// <returns>ObservableCollection of BoardModel objects</returns>
        public ObservableCollection<BoardModel> GetAllSections()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.SectionsKey))
            {
                throw new InvalidOperationException("All the sections are not in isolated storage.");
            }

            var sections = 
                (ObservableCollection<BoardModel>)IsolatedStorageSettings.ApplicationSettings[Constants.SectionsKey];
            return sections;
        }

        /// <summary>
        /// Saves all the sections to isolated storage.
        /// </summary>
        /// <param name="sections">All the sections</param>
        /// <exception cref="ArgumentNullException">If the sections argument is null</exception>
        public void SaveAllSections(ObservableCollection<BoardModel> sections)
        {
            if (sections == null)
            {
                throw new ArgumentNullException("sections");
            }

            IsolatedStorageSettings.ApplicationSettings[Constants.SectionsKey] = sections;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Get all the Favorite boards from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If all the Favorite boards are not in isolated storage.</exception>
        /// <returns>ObservableCollection of BoardModel objects</returns>
        public ObservableCollection<BoardModel> GetFavorites()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.FavoritesKey))
            {
                throw new InvalidOperationException("All the Favorite boards are not in isolated storage.");
            }

            var favorites =
                (ObservableCollection<BoardModel>) IsolatedStorageSettings.ApplicationSettings[Constants.FavoritesKey];
            return favorites;
        }

        /// <summary>
        /// Saves all the Favorite boards to isolated storage.
        /// </summary>
        /// <param name="favorites">All the favorite boards</param>
        public void SaveFavorites(ObservableCollection<BoardModel> favorites)
        {
            IsolatedStorageSettings.ApplicationSettings[Constants.FavoritesKey] = favorites;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Get the user Token from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the user Token is not in isolated storage.</exception>
        /// <returns>The user Token</returns>
        public string GetToken()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.TokenKey))
            {
                throw new InvalidOperationException("The user Token is not in isolated storage.");
            }

            var token = (string) IsolatedStorageSettings.ApplicationSettings[Constants.TokenKey];
            return token;
        }

        /// <summary>
        /// Saves the user Token to isolated storage.
        /// </summary>
        /// <param name="token">The user Token</param>
        public void SaveToken(string token)
        {
            IsolatedStorageSettings.ApplicationSettings[Constants.TokenKey] = token;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        /// <summary>
        /// Get the user name from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the user name is not in isolated storage.</exception>
        /// <returns>The user name</returns>
        public string GetUsername()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.UsernameKey))
            {
                throw new InvalidOperationException("The user name is not in isolated storage.");
            }

            var username = (string)IsolatedStorageSettings.ApplicationSettings[Constants.UsernameKey];
            return username;
        }

        /// <summary>
        /// Saves the user name to isolated storage.
        /// </summary>
        /// <param name="username">The user name</param>
        public void SaveUsername(string username)
        {
            IsolatedStorageSettings.ApplicationSettings[Constants.UsernameKey] = username;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Get the Board Mode from isolated storage.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Board Mode is not in isolated storage.</exception>
        /// <returns>The Board Mode</returns>
        public int GetBoardMode()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Constants.BoardModeKey))
            {
                throw new InvalidOperationException("The Board Mode is not in isolated storage.");
            }

            var boardMode = (int) IsolatedStorageSettings.ApplicationSettings[Constants.BoardModeKey];
            return boardMode;
        }

        /// <summary>
        /// Saves the Board Mode to isolated storage.
        /// </summary>
        /// <param name="boardMode">The Board Mode</param>
        /// <exception cref="ArgumentOutOfRangeException ">If the boardMode'id argument is not between 0 and 2 as a Integer</exception>
        public void SaveBoardMode(int boardMode)
        {
            if (boardMode < 0 || boardMode > 2)
            {
                throw new ArgumentOutOfRangeException("boardMode");
            }

            IsolatedStorageSettings.ApplicationSettings[Constants.BoardModeKey] = boardMode;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        /// <summary>
        /// Get the board description from isolated storage.
        /// </summary>
        /// <param name="englishName">The english name of the board</param>
        /// <exception cref="InvalidOperationException">If the board description is not in isolated storage.</exception>
        /// <returns>The description of the board</returns>
        public string GetBoardDescriptionByName(string englishName)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(englishName))
            {
                throw new InvalidOperationException("The board description is not in isolated storage.");
            }

            var description = (string)IsolatedStorageSettings.ApplicationSettings[englishName];
            return description;
        }

        /// <summary>
        /// Saves the board description to isolated storage.
        /// </summary>
        /// <param name="englishName">The english name of the board</param>
        /// <param name="description">The description of the board</param>
        public void SaveBoardDescription(string englishName, string description)
        {
            IsolatedStorageSettings.ApplicationSettings[englishName] = description;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}
