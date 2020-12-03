using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FirstBlazorHybridApp.Game {
    public class Team
    {
        public string Name { get; set; }
        public IList<Player> ActivePlayers { get; private set; }
        public IList<Player> BenchPlayers { get; private set; }

        private Metadata metadata;
        public Team(Metadata metadata, string name)
        {
            this.metadata = metadata;
            Name = name;

            ActivePlayers = new List<Player>();
            BenchPlayers = new List<Player>();
        }

        public void Add(Player player) {
            if (!metadata.NumActivePlayers.HasValue) {
                ActivePlayers.Add(player);
            }
            else {
                if (ActivePlayers.Count == metadata.NumActivePlayers.Value) {
                    BenchPlayers.Add(player);
                }
                else {
                    ActivePlayers.Add(player);
                }
            }
        }

        public void AddActivePlayer(Player player, int index)
        {
            player.Team = this;
            ActivePlayers.Insert(index, player);
            if (metadata.NumActivePlayers.HasValue)
            {
                if (ActivePlayers.Count > metadata.NumActivePlayers.Value)
                {
                    Player end = ActivePlayers[ActivePlayers.Count - 1];
                    ActivePlayers.RemoveAt(ActivePlayers.Count - 1);
                    BenchPlayers.Insert(0, end);
                }
            }
        }

        public void AddBenchPlayer(Player player, int index)
        {
            player.Team = this;
            BenchPlayers.Insert(index, player);
        }

        public void RemoveActivePlayer(int index)
        {
            ActivePlayers.RemoveAt(index);
        }

        public void RemoveBenchPlayer(int index)
        {
            BenchPlayers.RemoveAt(index);
        }

    }

    public class Player
    {
        public string Name { get; set; }

        [JsonIgnore]
        public Team Team { get; set; }

        public Player(string name, Team team)
        {
            Name = name;
            Team = team;
        }
    }

    public class Question
    {
        private Metadata metadata;
        public TossupResult TossupResult { get; private set; }
        public List<BonusResult> BonusResults { get; private set; } = new List<BonusResult>();

        public List<Player> HeardBy { get; } = new List<Player>();
        public Player? AnsweredByPlayer { get; private set; }
        public Team? AnsweredByTeam { get; private set; }
        public List<Player> NegsByPlayer { get; } = new List<Player>();
        public List<Team> NegsByTeam { get; } = new List<Team>();

        public Question(Metadata metadata)
        {
            this.metadata = metadata;
        }

        private void AddHeardBy(IList<Team> teams)
        {
            foreach (Team team in teams)
            {
                HeardBy.AddRange(team.ActivePlayers);
            }
        }

        public void AwardTossup(Player player, IList<Team> teams, TossupResult tossupResult)
        {
            TossupResult = tossupResult;
            if (tossupResult != TossupResult.NO_ANSWER)
            {
                AnsweredByPlayer = player;
                AnsweredByTeam = player.Team;
            }

            AddHeardBy(teams);
        }

        public void AwardNeg(Player player)
        {
            NegsByPlayer.Add(player);
            NegsByTeam.Add(player.Team);
        }

        public void AwardNoAnswer(IList<Team> teams)
        {
            TossupResult = TossupResult.NO_ANSWER;
            AddHeardBy(teams);
        }

        public void AwardBonus(BonusResult bonusResult)
        {
            if (TossupResult == TossupResult.NO_ANSWER)
            {
                throw new Exception("Can't award bonus if the tossup hasn't been answered.");
            }

            if (BonusResults.Count >= this.metadata.NumBonusPerTossup)
            {
                throw new Exception($"Can't award more than {this.metadata.NumBonusPerTossup} bonuses per tossup.");
            }

            BonusResults.Add(bonusResult);
        }
    }
}