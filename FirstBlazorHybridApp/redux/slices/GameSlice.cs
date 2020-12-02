using BlazorRedux;
using System.Collections.Generic;

namespace FirstBlazorHybridApp.redux.slices
{
    public class Team
    {
        public string Name { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public Team Team { get; set; }
    }

    public class Question
    {
        public int Tossup { get; set; }
        public IEnumerable<int> Bonus { get; set; }
        public Player? player { get; set; }
    }

    public class GameSlice
    {
        // state
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();

        // actions
        public class NextQuestion : IAction { }

        // reducer
        public static GameSlice GameReducer(GameSlice gameState, IAction action)
        {
            switch (action)
            {
                case NextQuestion _:
                    return gameState;
                default:
                    return gameState;
            }
        }
    }
}
