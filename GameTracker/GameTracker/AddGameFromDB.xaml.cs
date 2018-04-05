using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameTracker
{
    /// <summary>
    /// Interaction logic for AddGameFromDB.xaml
    /// </summary>
    public partial class AddGameFromDB : Window
    {
        #region Initialization
        public AddGameFromDB()
        {
            InitializeComponent();

            // Defaults disables.
            btnSearchDB.IsEnabled = false;
            btnClearDBSearch.IsEnabled = false;
            txtNumResultsFound.Content = "";
            btnAdd.IsEnabled = false;
        }
        #endregion

        #region Listeners

        // Ensures search button is disabled when search box is empty.
        private void txtDatabaseSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtDatabaseSearch.Text))
            {
                btnSearchDB.IsEnabled = false;
            }
            else
            {
                btnSearchDB.IsEnabled = true;
            }
        }

        private void btnSearchDB_Click(object sender, RoutedEventArgs e)
        {
            searchDatabase(txtDatabaseSearch.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!isAlreadyTracked())
            {
                addGame();
                MessageBoxResult result;
                result = MessageBox.Show("The game has been added to your tracked games list.", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                MessageBoxResult result;
                result = MessageBox.Show("It appears the game is already in your tracked games list.", "Already Tracked", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnClearDBSearch_Click(object sender, RoutedEventArgs e)
        {
            clearSearch();
        }

        // Refresh UI with the selected game.
        private void lstDBSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAdd.IsEnabled = true;
            TrackedGame selected = this.getSelectedResult();

            if (selected != null)
            {
                refreshResultInfo(selected);
            }
        }

        #endregion

        private void searchDatabase(string input)
        {
            clearSearch();
            txtDatabaseSearch.Text = input;

            int numResults = 0;

            // Will always search the entire dummy database, as this is just a prototype to show how searching affects the UI.
            foreach (TrackedGame t in Data.dummyGamesDatabase)
            {
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = t.gameTitle;
                listItem.Name = t.listId;
                lstDBSearchResults.Items.Add(listItem);
                numResults++;
            }

            txtNumResultsFound.Content = "Results Found: " + numResults;
            btnClearDBSearch.IsEnabled = true;
        }

        // Obtains the TrackedGame from the dummy online database.
        private TrackedGame getSelectedResult()
        {
            ListBoxItem lbi = (lstDBSearchResults.SelectedItem as ListBoxItem);

            if (lbi == null)
            {
                return null;
            }

            string selectedGameId = lbi.Name;

            TrackedGame selectedGame = null;

            // Obtain the game from the dummy database using the listID of the selected game.
            foreach (TrackedGame t in Data.dummyGamesDatabase)
            {
                if (t.listId == selectedGameId)
                {
                    selectedGame = t;
                    break;
                }
            }

            return selectedGame;
        }

        // Refresh result info section.
        private void refreshResultInfo(TrackedGame game)
        {
            txtResultInformation.Text = game.gameInformation;
            imgResult.Source = new BitmapImage(new Uri(game.imagePath, UriKind.Relative));
            scrollResultInfo.ScrollToTop();
        }

        // Clears the search and search results.
        private void clearSearch()
        {
            // Clear user search input.
            txtDatabaseSearch.Text = "";
            // Remove all search results.
            lstDBSearchResults.Items.Clear();
            txtNumResultsFound.Content = "Results Found: -";
            btnClearDBSearch.IsEnabled = false;

            txtResultInformation.Text = "Perform a search and select an item to add from the results.";
            imgResult.Source = new BitmapImage(new Uri("/Resources/NoImage.png", UriKind.Relative));
        }

        // Checks if the game being added is already in the tracked games list.
        public bool isAlreadyTracked()
        {
            bool result = false;

            TrackedGame toCheck = getSelectedResult();

            foreach(TrackedGame t in Data.trackedGamesList)
            {
                if (toCheck.listId == t.listId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // Adds the game to the tracked games list (both UI and Data.trackedGamesList)
        public void addGame()
        {
            TrackedGame t = getSelectedResult();

            Data.trackedGamesList.Add(new TrackedGame(t.imagePath, t.gameTitle,t.progressStatus,t.progressNote,t.numberOfPlaythroughs,t.myRating,t.ratingNote,t.gameInformation));

            ListBoxItem item = new ListBoxItem();
            item.Content = t.gameTitle;
            item.Name = t.listId;

            MainWindow.mainWindow.lstTrackedGames.Items.Add(item);
            MainWindow.mainWindow.txtSortingInfo.Content = "None (not supported)";
            int count = MainWindow.mainWindow.lstTrackedGames.Items.Count;
            MainWindow.mainWindow.txtTrackedGamesCount.Content = "Displaying " + count + " out of " + count + " Tracked Games";
        }
    }
}
