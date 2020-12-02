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
        public GameStatus GameStatus { get; private set; } = new GameStatus();
        public Metadata Metadata { get; private set; } = new Metadata();

        // actions
        public class AddTeam : IAction {
            public string Name { get; private set; }

            public AddTeam(string name)
            {
                Name = name;
            }
        }

        // reducer
        public static GameSlice GameReducer(GameSlice oldState, IAction action)
        {
            switch (action)
            {
                case AddTeam a:
                    GameSlice newState = (GameSlice)oldState.MemberwiseClone();
                    newState.Teams.Add(new Team(oldState.Metadata, a.Name));
                    return newState;
                default:
                    return oldState;
            }
        }
    }
}
