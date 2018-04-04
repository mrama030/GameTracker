using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Mohamed Ali Ramadan (7688825)

namespace GameTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListBoxItem previousSelectedGame;

        //public static TrackedGame unsavedGameInstance;

        #region Initialization

        // Initial startup and startup settings.
        public MainWindow()
        {
            InitializeComponent();

            // Maximize the window by default.
            this.WindowState = WindowState.Maximized;

            // On close event, warning message is used if some changes are unsaved.   
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            // Disable some buttons.
            btnSearch.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnClearFilters.IsEnabled = false;
            btnClearSearch.IsEnabled = false;

            // Select Crysis 2 as startup game. (First list item.)
            lstTrackedGames.SelectedItem = itemCrysis2;
        }

        #endregion

        #region Methods for changing game selection and refreshing UI.

        // Gets the entity in the dummy database representing the currently selected game.
        private TrackedGame getSelectedGame()
        {
            ListBoxItem lbi = (lstTrackedGames.SelectedItem as ListBoxItem);

            if (lbi == null)
            {
                return null;
            }

            string selectedGameId = lbi.Name;

            TrackedGame selectedGame = null;

            // Obtain the game from the dummy database using the listID of the selected game.
            foreach (TrackedGame t in Data.trackedGamesList)
            {
                if (t.listId == selectedGameId)
                {
                    selectedGame = t;
                    break;
                }
            }

            return selectedGame;
        }

        // Detects a change of which game is selected in the list and displays the newly selected game.
        private void lstTrackedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check for unsaved changes first.
            if (btnSave.IsEnabled == true)
            {
                // Required to avoid recursive warning messages.
                if (e.AddedItems[0] as ListBoxItem == previousSelectedGame)
                {
                    return;
                }

                MessageBoxResult result;
                result = MessageBox.Show("Discard changes and switch games?", "Unsaved Changes Detected", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                #region Resets Selection + Code Required So Changes Are Not Lost On Game Switch

                // User does not want to discard changes.
                if (result != MessageBoxResult.Yes)
                {
                    ListBoxItem removedItem = e.RemovedItems[0] as ListBoxItem;

                    TrackedGame toRestore = getUnsavedGameInstance(removedItem);

                    // Reset the selection to the game before game switch attempt.
                    lstTrackedGames.SelectedValue = e.RemovedItems[0];

                    // The UI will be refreshed to the game data that is in the dummy database,
                    // and will lost the user's changes at this point.

                    // Restore changes to UI using refresh function and unsaved TrackedGame instance.
                    refreshGameSection(toRestore);

                    // Enable the save button (as it is disabled on a refresh).
                    enableSaveForGame();

                    return;
                }

                #endregion
            }

            TrackedGame selected = getSelectedGame();

            if (selected == null)
            {
                return;
            }

            refreshGameSection(selected);
        }

        // Refreshes the right side of the UI with the selected game's information.
        public void refreshGameSection(TrackedGame game)
        {
            if (game == null)
            {
                return;
            }
            else
            {
                txtTitle.Text = game.gameTitle;
                txtGameInformation.Text = game.gameInformation;
                imgGameCover.Source = new BitmapImage(new Uri(game.imagePath, UriKind.Relative));
                txtPlaythroughs.Text = game.numberOfPlaythroughs.ToString();
                txtProgressNote.Text = game.progressNote;

                if (String.IsNullOrWhiteSpace(game.ratingNote))
                {
                    txtRatingNote.Text = "None";
                }
                else
                {
                    txtRatingNote.Text = game.ratingNote;
                }

                if (game.myRating >= 0)
                {
                    txtRating.Text = game.myRating.ToString();
                    sliderRating.Value = game.myRating;
                }
                else
                {
                    sliderRating.Value = 0.0;
                    txtRating.Text = "None";
                }

                // Games are always created with Plan To Play status, so this attribute is never null.
                if (game.progressStatus == ProgressStatus.Completed)
                {
                    cmbProgressStatus.SelectedItem = itemCompleted;
                }
                else if (game.progressStatus == ProgressStatus.InProgress)
                {
                    cmbProgressStatus.SelectedItem = itemInProgress;
                }
                else if (game.progressStatus == ProgressStatus.PlanToPlay)
                {
                    cmbProgressStatus.SelectedItem = itemPlanToPlay;
                }

                disableSaveForGame();
            }
        }

        /// <summary>
        /// Called when the user decides not to discard changes on game switch.
        /// Creates a TrackedGame object representing the unsaved changes of the user,
        /// By using static attributes from the dummy database (ex: image path) and 
        /// attributes from the UI (for the user modifiable attributes).
        /// </summary>
        /// <param name="unsavedSelection"></param>
        /// <returns>Temporary TrackedGame object used to refresh the UI back to the user's changes.</returns>        
        private TrackedGame getUnsavedGameInstance(ListBoxItem unsavedSelection)
        {
            string unsavedGameId = unsavedSelection.Name;

            // Tracked Game object
            TrackedGame unsavedGame = null;

            // Obtain the game from the dummy database using the listID of the selected game.
            foreach (TrackedGame t in Data.trackedGamesList)
            {
                if (t.listId == unsavedGameId)
                {
                    unsavedGame = t;
                    break;
                }
            }

            // Update the TrackedGame object with the unsaved changes of the user.

            if (txtRating.Text == "None")
            {
                unsavedGame.myRating = -1;
            }
            else
            {
                unsavedGame.myRating = Double.Parse(txtRating.Text);
            }

            unsavedGame.numberOfPlaythroughs = Int32.Parse(txtPlaythroughs.Text);
            unsavedGame.progressNote = txtProgressNote.Text;
            unsavedGame.ratingNote = txtRatingNote.Text;

            if (cmbProgressStatus.SelectedItem == itemCompleted)
            {
                unsavedGame.progressStatus = ProgressStatus.Completed;
            }
            else if (cmbProgressStatus.SelectedItem == itemInProgress)
            {
                unsavedGame.progressStatus = ProgressStatus.InProgress;
            }
            else if (cmbProgressStatus.SelectedItem == itemPlanToPlay)
            {
                unsavedGame.progressStatus = ProgressStatus.PlanToPlay;
            }

            return unsavedGame;
        }

        #endregion

        #region Application Exit Messages

        // Prevents closing application if changes are pending. Asks user if he wants to discard and close application or cancel.
        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result;

            if (btnSave.IsEnabled == true)
            {
                result = MessageBox.Show("Discard changes and exit?", "Unsaved Changes Detected", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            }
            else
            {
                result = MessageBox.Show("Are you sure you want to leave?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            }


            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Events that detect changes to save.

        private void txtPlaythroughs_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Only do this if the game was in "No Changes Detected" state.
            if (btnSave.IsEnabled == false)
            {
                enableSaveForGame();
            }
        }

        private void cmbProgressStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only do this if the game was in "No Changes Detected" state.
            if (btnSave.IsEnabled == false)
            {
                enableSaveForGame();
            }
        }

        private void txtProgressNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Only do this if the game was in "No Changes Detected" state.
            if (btnSave.IsEnabled == false)
            {
                enableSaveForGame();
            }
        }

        private void txtRatingNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Only do this if the game was in "No Changes Detected" state.
            if (btnSave.IsEnabled == false)
            {
                enableSaveForGame();
            }
        }

        private void sliderRating_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Only do this if the game was in "No Changes Detected" state.
            if (btnSave.IsEnabled == false)
            {
                enableSaveForGame();
            }
        }

        #endregion

        #region Methods to permit saving.

        private void disableSaveForGame()
        {
            btnSave.IsEnabled = false;
            imgSave.Source = new BitmapImage(new Uri("/Resources/DisabledSave.png", UriKind.Relative));
        }

        // Indicates in the dummy database that the selected game now has changes pending to be saved.
        private void enableSaveForGame()
        {
            btnSave.IsEnabled = true;
            btnSave.BorderThickness = new Thickness(1, 1, 1, 1);
            btnSave.Background = Brushes.White;
            imgSave.Source = new BitmapImage(new Uri("/Resources/EnabledSave.png", UriKind.Relative));
            previousSelectedGame = lstTrackedGames.SelectedItem as ListBoxItem;
        }

        // Updates dummy database with the changes.
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem lbi = (lstTrackedGames.SelectedItem as ListBoxItem);

            if (lbi == null)
            {
                return;
            }

            string selectedGameId = lbi.Name;

            // Obtain the game from the dummy database using the listID of the selected game.
            foreach (TrackedGame t in Data.trackedGamesList)
            {
                if (t.listId == selectedGameId) // Game to update with changes to save.
                {
                    if (txtRating.Text == "None")
                    {
                        t.myRating = -1;
                    }
                    else
                    {
                        t.myRating = Double.Parse(txtRating.Text);
                    }

                    t.numberOfPlaythroughs = Int32.Parse(txtPlaythroughs.Text);
                    t.progressNote = txtProgressNote.Text;
                    t.ratingNote = txtRatingNote.Text;

                    if (cmbProgressStatus.SelectedItem == itemCompleted)
                    {
                        t.progressStatus = ProgressStatus.Completed;
                    }
                    else if (cmbProgressStatus.SelectedItem == itemInProgress)
                    {
                        t.progressStatus = ProgressStatus.InProgress;
                    }
                    else if (cmbProgressStatus.SelectedItem == itemPlanToPlay)
                    {
                        t.progressStatus = ProgressStatus.PlanToPlay;
                    }

                    break;
                }
            }

            disableSaveForGame();
        }

        #endregion

        #region User Input Validation Methods

        // Intercept user text input to the number of playthroughs text box and only change text if it is a positive integer character.
        private void txtPlaythroughs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (sender as TextBox);

            string currentText = textBox.Text;

            if (String.IsNullOrWhiteSpace(currentText))
            {
                e.Handled = IsZeroToNine(e.Text);
            }
            else if (Int32.Parse(currentText) == 0) // There is a 0 in the text box.
            {
                e.Handled = false;

                int result;
                if (!int.TryParse(e.Text, out result)) // User typed in a digit.
                {
                    return;
                }
                else
                {
                    txtPlaythroughs.Text = ""; //Erase contents and replace with typed in digit. 
                }   
            }
            else
            {
                e.Handled = IsZeroToNine(e.Text);
            }
        }

        // Is the user input character between zero and nine.
        private static bool IsZeroToNine(string t)
        {
            Regex reg = new Regex("[^0-9]"); //regex that matches disallowed text
            return reg.IsMatch(t);
        }

        // Is the user input character between one and nine.
        private static bool IsOneToNine(string t)
        {
            Regex reg = new Regex("[^1-9]"); //regex that matches disallowed text
            return reg.IsMatch(t);
        }

        #endregion

        #region Other Non-Implemented

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAddGameFromDB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAddGameManually_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
