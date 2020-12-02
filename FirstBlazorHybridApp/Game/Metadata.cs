namespace FirstBlazorHybridApp.Game {
    public class Metadata {
        public int? NumActivePlayers { get; set; } = 4;
        public int NumTeams { get; set; } = 2;

        public int TossupWeight { get; set; } = 10;
        public int? PowerWeight { get; set; } = 15;
        public int? NegWeight { get; set; } = -5;
        public int NumBonusPerTossup { get; set; } = 3;
        public int BonusWeight { get; set; } = 10;
        public bool BounceBacks { get; set; } = false;
    }

    public enum TossupResult
    {
        NO_ANSWER,
        NORMAL,
        POWER
    }

    public enum BonusResult
    {
        NO_ANSWER,
        ANSWERED,
        BOUNCE_BACK
    }

    public class GameStatus
    {
        bool IsInProgress { get; set; } = false;
        int CurrentQuestion { get; set; } = 0;
        bool IsInBonus { get; set; } = false;
    }
}