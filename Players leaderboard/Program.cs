using System;
using System.Collections.Generic;

namespace Players_leaderboard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new()
            {
                new("Manetaan", 44, 448),
                new("Inylingo", 66, 890),
                new("Zanenada", 36, 900),
                new("Hinolewh", 52, 801),
                new("Frtliach", 14, 94),
                new("Dellsein", 3, 115),
                new("Ininnezu", 2, 71),
                new("Vedyorsi", 53, 217),
                new("Asaeneli", 41, 559),
                new("Vaumerri", 61, 364),
                new("Ndeddith", 4, 174),
                new("Juariane", 80, 588),
                new("Elerkhuc", 32, 155),
                new("Kelavora", 79, 653),
                new("Heguynon", 79, 614),
                new("Pikyryan", 17, 285),
                new("Barorien", 91, 818),
            };

            Database database = new(players, 3);
            DatabaseView view = new(database);
            view.ShowLeaderboards();
            Console.ReadKey();
        }
    }

    public class DatabaseView
    {
        private readonly Database _database;

        public DatabaseView(Database database)
        {
            ArgumentNullException.ThrowIfNull(database);

            _database = database;
        }

        public void ShowLeaderboards()
        {
            int levelLeaderboardPosition = 0;
            int powerLeaderboardPosition = 40;

            ShowLeaderboard(_database.GetLevelLeaderboard(), levelLeaderboardPosition);
            ShowLeaderboard(_database.GetPowerLeaderboard(), powerLeaderboardPosition);
        }

        private void ShowPlayer(Player player)
        {
            Console.WriteLine($"{player.Name}  {player.Level}  {player.Power}");
        }

        private void ShowLeaderboard(List<Player> players, int leftOffset)
        {
            for (int i = 0; i < players.Count; i++)
            {
                Console.SetCursorPosition(leftOffset, i);
                ShowPlayer(players[i]);
            }
        }
    }

    public class Database
    {
        private readonly List<Player> _players;
        private readonly Leaderboard _levelLeaderboard;
        private readonly Leaderboard _powerLeaderboard;

        public Database(List<Player> players, int playersInLeaderboard)
        {
            ArgumentNullException.ThrowIfNull(players);

            _players = players;
            _levelLeaderboard = new(player => player.Level, playersInLeaderboard);
            _powerLeaderboard = new(player => player.Power, playersInLeaderboard);
        }

        public List<Player> GetLevelLeaderboard()
        {
            return _levelLeaderboard.OrderPlayers(_players);
        }

        public List<Player> GetPowerLeaderboard()
        {
            return _powerLeaderboard.OrderPlayers(_players);
        }
    }

    public class Leaderboard
    {
        private readonly Func<Player, int> _filter;
        private readonly int _maxPlayers;

        public Leaderboard(Func<Player, int> filter, int maxPlayers)
        {
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxPlayers);

            _filter = filter;
            _maxPlayers = maxPlayers;
        }

        public List<Player> OrderPlayers(List<Player> players)
        {
            var sortedPlayers = players.OrderByDescending(_filter).Take(_maxPlayers).ToList();
            return sortedPlayers;
        }
    }

    public class Player
    {
        public Player(string name, int level, int power)
        {
            Name = name;
            Level = level;
            Power = power;
        }

        public string Name { get; }

        public int Level { get; }

        public int Power { get; }
    }
}
