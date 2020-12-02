using BlazorRedux;

namespace FirstBlazorHybridApp.redux.slices
{
    public class CounterSlice
    {
        // state
        public int Counter { get; set; } = 0;

        // actions
        public class IncrementByOne : IAction { }

        // reducer
        public static CounterSlice CounterReducer(CounterSlice counterState, IAction action)
        {
            switch(action)
            {
                case IncrementByOne _:
                    return new CounterSlice { Counter = counterState.Counter + 1 };
                default:
                    return counterState;
            }
        }
    }
}
