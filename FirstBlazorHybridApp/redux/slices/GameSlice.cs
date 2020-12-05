using System.Collections.Generic;
using BlazorRedux;
using FirstBlazorHybridApp.Game;

namespace FirstBlazorHybridApp.redux.slices
{
    public class GameSlice
    {
        // state
        public List<Question> Questions { get; private set; } = new List<Question>();
        public List<Team> Teams { get; private set; } = new List<Team>();
        public GameProgress GameProgress { get; private set; } = new GameProgress();
        public Metadata Metadata { get; private set; } = new Metadata();

        public int GetScore(Team team)
        {
            int score = 0;
            foreach (var question in Questions)
            {
                score += question.GetPtsEarned(team);
            }

            return score;
        }

        // actions
        public class AddPlayer : IAction {
            public string Name { get; private set; }
            public Team Team { get; private set; }

            public AddPlayer(string name, Team team)
            {
                Name = name;
                Team = team;
            }
        }

        public class StartGame : IAction
        {
            public Metadata Metadata { get; private set; }

            public StartGame(Metadata metadata)
            {
                Metadata = metadata;
            }
        }

        public class AwardTossup : IAction
        {
            public Player Player { get; private set; }

            public TossupResult TossupResult { get; private set; }

            public AwardTossup(Player player, TossupResult tossupResult)
            {
                Player = player;
                TossupResult = tossupResult;
            }
        }

        public class AwardNeg : IAction
        {
            public Player Player { get; private set; }

            public AwardNeg(Player player)
            {
                Player = player;
            }
        }

        // reducer
        public static GameSlice GameReducer(GameSlice oldState, IAction action)
        {
            GameSlice newState = (GameSlice)oldState.MemberwiseClone();
            switch (action)
            {
                case AddPlayer a:
                    {
                        // TODO: HARSHA: revisit this, don't think we're copying state properly
                        Team? team = newState.Teams.Find((team) => team.Equals(a.Team));
                        team?.Add(new Player(a.Name, team));
                        return newState;
                    }
                case StartGame a:
                    {
                        newState.Metadata = a.Metadata;
                        newState.GameProgress.Status = GameStatus.IN_PROGESS;
                        for (int i =  0; i < a.Metadata.NumTeams; ++i) {
                            newState.Teams.Add(new Team(a.Metadata, $"Team {i + 1}"));
                        }
                        newState.Questions.Add(new Question(a.Metadata));
                        return newState;
                    }
                case AwardTossup a:
                    {
                        newState.Questions[newState.Questions.Count - 1].AwardTossup(a.Player, newState.Teams, a.TossupResult);
                        return newState;
                    }
                case AwardNeg a:
                    {
                        newState.Questions[newState.Questions.Count - 1].AwardNeg(a.Player);
                        return newState;
                    }
                default:
                    return oldState;
            }
        }
    }
}
