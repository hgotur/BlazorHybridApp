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
        public class AddTeam : IAction {
            public string Name { get; private set; }

            public AddTeam(string name)
            {
                Name = name;
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
                case AddTeam a:
                    {
                        newState.Teams.Add(new Team(oldState.Metadata, a.Name));
                        return newState;
                    }
                case StartGame a:
                    {
                        newState.Metadata = a.Metadata;
                        newState.GameProgress.Status = GameStatus.IN_PROGESS;
                        return newState;
                    }
                default:
                    return oldState;
            }
        }
    }
}
