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
                        return newState;
                    }
                default:
                    return oldState;
            }
        }
    }
}
