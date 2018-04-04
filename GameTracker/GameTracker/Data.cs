using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    public static class Data
    {
        public static List<TrackedGame> trackedGamesList;

        static Data()
        {
            // Initialize list of existing tracked games.
            trackedGamesList = new List<TrackedGame>();

            string tempPath = "/GameCovers/Crysis2.jpg";
            string tempProgressNote = "Beat the game, but did not unlock 100% of unlockables yet.";
            string tempRatingNote = "I really enjoyed this game, especially the graphics and the soundtrack. The story was also pretty good but still has a little wiggle room for improvement.";
            string tempGameInfo = "\nRelease Date: 2011-08-29\nGenre(s): Shooter, Action\nDeveloper(s): Crytek\nPublisher(s): Electronic Arts\nPlatform(s): PC, PS3, XBOX 360\nGameplay Mode(s): Single Player, Multiplayer\n\nDescription:\nSequel to one of the greatest PC shooters ever, Crysis 2 offers console players their first taste of Crytek's unique shooter gameplay. Featuring futuristic war, gorgeous destruction and the chance to kick alien butt on the grandest stage of all, New York City, Crysis 2 is destined at the least to equal its predecessor, if not surpass it. Additional features include: challenging AI enemies in the single player campaign, 12-player support online, new and improved upgradable Nanosuit 2 technology and more.\n";

            TrackedGame crysis2 = new TrackedGame(tempPath, "Crysis 2", ProgressStatus.Completed, tempProgressNote, 3, 9.4, tempRatingNote, tempGameInfo);

            tempPath = "/GameCovers/GearsOfWar4.jpg";
            tempProgressNote = "Heard good things about this game and will play it during the Summer of 2018.";
            tempRatingNote = "";
            tempGameInfo = "\nRelease Date: 2016-10-11\nGenre(s): Shooter\nDeveloper(s): The Coalition\nPublisher(s): Microsoft Studios\nPlatform(s): XBOX One, PC\nGameplay Mode(s): Single Player, Multiplayer, Co-Operative\n\nDescription:\nA new saga begins for one of the most acclaimed video game franchises in history. After narrowly escaping an attack on their village, JD Fenix and his friends, Kait and Del, must rescue the ones they love and discover the source of a monstrous new enemy.\n";

            TrackedGame gearsOfWar4 = new TrackedGame(tempPath, "Gears Of War 4", ProgressStatus.PlanToPlay, tempProgressNote, 0, -1, tempRatingNote, tempGameInfo);

            tempPath = "/GameCovers/TheLastOfUs.jpg";
            tempProgressNote = "Halfway done, really need to find time to finish this one !!!";
            tempRatingNote = "Out of this world game, not many like it !";
            tempGameInfo = "\nRelease Date: 2013-06-14\nGenre(s): Shooter, Adventure\nDeveloper(s): Naughty Dog\nPublisher(s): Sony Computer Entertainment\nPlatform(s): PS3, PS4\nGameplay Mode(s): Single Player, Multiplayer\n\nDescription:\nTwenty years after a mutated fungus started turning people all over the world into deadly zombies, humans become an endangered species. Joel, a Texan in his forties with the \"emotional range of a teaspoon\" (to quote Hermione from Harry Potter), finds himself responsible with the safety of a fourteen year old girl named Ellie whom he must smuggle to a militia group called the Fireflies. And as if the infected aren't enough of a hassle, they also have to deal with the authorities who wouldn't let them leave the quarantine zone, as well as other survivors capable of killing anyone who might have something useful in their backpacks.\n";

            TrackedGame theLastOfUs = new TrackedGame(tempPath, "The Last Of Us", ProgressStatus.InProgress, tempProgressNote, 0, 9.5, tempRatingNote, tempGameInfo);

            trackedGamesList.Add(crysis2);
            trackedGamesList.Add(gearsOfWar4);
            trackedGamesList.Add(theLastOfUs);
        }
    }
}
