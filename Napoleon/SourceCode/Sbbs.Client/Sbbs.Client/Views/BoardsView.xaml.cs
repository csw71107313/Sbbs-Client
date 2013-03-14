/************************************************
 * FileName: BoardsView.xaml.cs
 * Document-related:
 * Module: Sbbb.Client
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-06-2013
 *************************************************/

using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Sbbs.Core;

namespace Sbbs.Client
{
    public partial class BoardsView
    {
        BoardsViewModel m_BoardsViewModel;

        BoardsViewModel BoardsViewModel
        {
            get { return m_BoardsViewModel ?? (m_BoardsViewModel = this.DataContext as BoardsViewModel); }
        }

        public BoardsView()
        {
            InitializeComponent();
        }

        private void LongListSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                BoardModel board = (e.AddedItems[0]as Microsoft.Phone.Controls.LongListSelector.LongListSelectorItem).Item as BoardModel;

                if (BoardsViewModel.BoardSelectedCommand.CanExecute(null))
                {
                    BoardsViewModel.BoardSelectedCommand.Execute(board);
                }

                // 清除选择，否则同样的项目无法点击第二次
                (sender as LongListSelector).SelectedItem = null;
            }
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                BoardModel topic = e.AddedItems[0] as BoardModel;

                if (BoardsViewModel.BoardSelectedCommand.CanExecute(null))
                {
                    BoardsViewModel.BoardSelectedCommand.Execute(topic);
                }

                // 清除选择，否则同样的项目无法点击第二次
                (sender as ListBox).SelectedIndex = -1;
            }
        }
    }
}
