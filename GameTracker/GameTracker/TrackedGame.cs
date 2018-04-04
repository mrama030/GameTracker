using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    public enum ProgressStatus { Completed, InProgress, PlanToPlay }

    public class TrackedGame
    {
        public string listId;
        public string imagePath;
        public string gameTitle;
        //public bool hasUnsavedChanges; 
        public ProgressStatus progressStatus;
        public string progressNote;
        public int numberOfPlaythroughs;
        public double myRating;
        public string ratingNote;
        public string gameInformation;

        // Constructor
        public TrackedGame(string imagePath, string gameTitle, ProgressStatus progressStatus, string progressNote, int numberOfPlaythroughs, double myRating, string ratingNote, string gameInformation)
        {
            this.listId = "item" + gameTitle.Replace(" ", "");
            // Images must be within the project's folder.
            this.imagePath = imagePath;
            this.gameTitle = gameTitle;
            //this.hasUnsavedChanges = false;
            this.progressStatus = progressStatus;
            this.progressNote = progressNote;
            this.numberOfPlaythroughs = numberOfPlaythroughs;
            this.myRating = myRating;
            this.ratingNote = ratingNote;
            this.gameInformation = gameInformation;
        }
    }
}
