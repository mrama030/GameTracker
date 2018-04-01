using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Maximize the window by default.
            this.WindowState = WindowState.Maximized;
            // On close event, warning message is used if some changes are unsaved.   
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);

            lstTrackedGames.SelectedItem = itemCrysis2;
            btnSearch.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnClearFilters.IsEnabled = false;
            btnClearSearch.IsEnabled = false;
        }

        private void lstTrackedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            refreshGameSection(lbi.Name);
        }

        public void refreshGameSection(string listSelectedGameId)
        {
            TrackedGame game = null;

            if (Data.trackedGamesList == null)
            {
                return;
            }

            foreach (TrackedGame t in Data.trackedGamesList)
            {
                if (t.listId == listSelectedGameId)
                {
                    game = t;
                    break;
                }
            }

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


            }
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            /*
            TrackedGame t = null;

            foreach(TrackedGame temp in Data.trackedGamesList)
            {
                if (temp.listId == this.lstTrackedGames.SelectedItem.ToString())
                {
                    t = temp;
                    break;
                }
            }
            */

            /*if (t.hasUnsavedChanges)
            {
                // Display warning message.
            }*/
        }

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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sliderRating_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }
    }
}
