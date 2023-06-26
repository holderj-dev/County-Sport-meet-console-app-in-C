using System;
using System.Collections.Generic;
using System.Linq;

namespace League_Simulates
{
    class Team
    {
        public string Name { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points { get; set; }

        public Team(string name)
        {
            Name = name;
            Played = 0;
            Won = 0;
            Drawn = 0;
            Lost = 0;
            GoalsFor = 0;
            GoalsAgainst = 0;
            Points = 0;
        }

        public void UpdateStats(int goalsFor, int goalsAgainst, bool isWin, bool isDraw)
        {
            Played++;
            GoalsFor += goalsFor;
            GoalsAgainst += goalsAgainst;

            if (isWin)
            {
                Won++;
                Points += 3;
            }
            else if (isDraw)
            {
                Drawn++;
                Points += 1;
            }
            else
            {
                Lost++;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Team> teams = GenerateTeams();
            Team userTeam = ChooseUserTeam(teams);
            PrintStandings(teams);

            for (int round = 1; round <= 3; round++)
            {
                SimulateMatches(teams, round, userTeam);
                PrintStandings(teams);
            }

            Team winner = DetermineWinner(teams);
            Console.WriteLine($"\n{winner.Name} is the winner of the tournament!");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static List<Team> GenerateTeams()
        {
            List<Team> teams = new List<Team>();
            for (int i = 1; i <= 15; i++)
            {
                Team team = new Team("Team " + i);
                teams.Add(team);
            }
            return teams;
        }

        static Team ChooseUserTeam(List<Team> teams)
        {
            Console.WriteLine("Choose your team:");
            for (int i = 0; i < teams.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {teams[i].Name}");
            }

            int choice;
            do
            {
                Console.Write("Enter the number of your chosen team: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > teams.Count);

            return teams[choice - 1];
        }

        static void SimulateMatches(List<Team> teams, int round, Team userTeam)
        {
            Random random = new Random();

            Console.WriteLine($"\n** Round {round} Matches **");

            foreach (Team team in teams)
            {
                if (team != userTeam)
                {
                    int goalsFor = random.Next(6);
                    int goalsAgainst = random.Next(6);

                    if (goalsFor != goalsAgainst)
                    {
                        bool isWin = goalsFor > goalsAgainst;
                        bool isDraw = goalsFor == goalsAgainst;

                        team.UpdateStats(goalsFor, goalsAgainst, isWin, isDraw);

                        Console.WriteLine($"{team.Name}: {goalsFor} - {goalsAgainst}");
                    }
                }
            }

            int userGoalsFor, userGoalsAgainst;
            do
            {
                Console.Write($"Enter the goals scored by {userTeam.Name}: ");
            } while (!int.TryParse(Console.ReadLine(), out userGoalsFor) || userGoalsFor < 0);

            do
            {
                Console.Write($"Enter the goals scored against {userTeam.Name}: ");
            } while (!int.TryParse(Console.ReadLine(), out userGoalsAgainst) || userGoalsAgainst < 0);

            bool isUserWin = userGoalsFor > userGoalsAgainst;
            bool isUserDraw = userGoalsFor == userGoalsAgainst;
            userTeam.UpdateStats(userGoalsFor, userGoalsAgainst, isUserWin, isUserDraw);
        }

        static void PrintStandings(List<Team> teams)
        {
            List<Team> sortedTeams = teams.OrderByDescending(t => t.Points)
                                          .ThenByDescending(t => t.GoalDifference)
                                          .ThenByDescending(t => t.GoalsFor)
                                          .ToList();

            Console.WriteLine("\n** Current Standings **");
            Console.WriteLine("| Team\t\t| Played | Won | Drawn | Lost | Goals For | Goals Against | Goal Difference | Points |");
            Console.WriteLine("|-------------|--------|-----|-------|------|-----------|---------------|-----------------|--------|");

            foreach (Team team in sortedTeams)
            {
                Console.WriteLine($"| {team.Name}\t|   {team.Played}    |  {team.Won}  |   {team.Drawn}   |   {team.Lost}   |     {team.GoalsFor}     |       {team.GoalsAgainst}       |        {team.GoalDifference}       |   {team.Points}    |");
            }
        }

        static Team DetermineWinner(List<Team> teams)
        {
            return teams.OrderByDescending(t => t.Points)
                        .ThenByDescending(t => t.GoalDifference)
                        .ThenByDescending(t => t.GoalsFor)
                        .First();
        }
    }
}