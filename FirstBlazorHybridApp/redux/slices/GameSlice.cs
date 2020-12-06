using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BlazorRedux;
using FirstBlazorHybridApp.Game;

namespace FirstBlazorHybridApp.redux.slices
{
    public class GameSlice
    {
        // state
        public List<Question> Questions { get; private set; } = new List<Question>();
        public List<Team> Teams { get; private set; } = new List<Team>();
        public GameStatus Status { get; private set; } = GameStatus.NOT_STARTED;
        public Metadata Metadata { get; private set; } = new Metadata();

        [JsonIgnore]
        public Question CurrentQuestion 
        { 
            get
            {
                if (Questions.Count == 0)
                {
                    throw new Exception("There is no current question.");
                }

                return Questions[Questions.Count - 1];
            } 
        }

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
        public class AwardPower : IAction
        {
            public Player Player { get; private set; }

            public AwardPower(Player player)
            {
                Player = player;
            }
        }

        public class AwardTossup : IAction
        {
            public Player Player { get; private set; }

            public AwardTossup(Player player)
            {
                Player = player;
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

        public class AwardBonus : IAction
        {
            public bool IsCorrect { get; private set; }

            public AwardBonus(bool isCorrect)
            {
                IsCorrect = isCorrect;
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
                        newState.Status = GameStatus.IN_PROGESS;
                        for (int i =  0; i < a.Metadata.NumTeams; ++i) {
                            newState.Teams.Add(new Team(a.Metadata, $"Team {i + 1}"));
                        }
                        newState.Questions.Add(new Question(a.Metadata));
                        return newState;
                    }
                case AwardPower a:
                    {
                        newState.CurrentQuestion.AwardPower(a.Player, newState.Teams);
                        return newState;
                    }
                case AwardTossup a:
                    {
                        newState.CurrentQuestion.AwardTossup(a.Player, newState.Teams);
                        return newState;
                    }
                case AwardNeg a:
                    {
                        newState.CurrentQuestion.AwardNeg(a.Player);
                        return newState;
                    }
                case AwardBonus a:
                    {
                        newState.CurrentQuestion.AwardBonus(a.IsCorrect);
                        if (newState.CurrentQuestion.QuestionStatus == QuestionStatus.COMPLETE)
                        {
                            newState.Questions.Add(new Question(newState.Metadata));
                        }
                        return newState;
                    }
                default:
                    return oldState;
            }
        }
    }
}
