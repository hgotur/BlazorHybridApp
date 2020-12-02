using BlazorRedux;
using FirstBlazorHybridApp.redux.slices;

namespace FirstBlazorHybridApp.redux
{
    public class State
    {
        // state
        public CounterSlice CounterState { get; set; } = new CounterSlice();
        public GameSlice GameState { get; set; } = new GameSlice();
        
        public static State RootReducer(State state, IAction action)
        {
            return new State
            {
                CounterState = CounterSlice.CounterReducer(state.CounterState, action),
                GameState = GameSlice.GameReducer(state.GameState , action)
            };
        }
    }
}
