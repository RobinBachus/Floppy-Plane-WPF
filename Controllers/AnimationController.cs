using Floppy_Plane_WPF.AudioUtils;
using Floppy_Plane_WPF.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Floppy_Plane_WPF.Controllers
{
    internal class AnimationController
    {
        private readonly Player _player;
        private readonly List<Enemy> _enemies;
        private readonly Canvas _frame;
        private readonly Grid _ui;
        private readonly Grid _gameOverScreen;
        private readonly Random _random;
        private readonly MusicController _musicController;

        private int EnemyCount { get; set; }

        public bool CanReSpawn { get; private set; }

        public bool Started { get; set; }
        public int Level { get; set; }
        public int SpeedIncreaseValue { get; set; }
        public bool ShowHitBoxes { get; set; }
        public int SafeDistance { get; set; }

        /// <summary>
        /// Timer for the animations
        /// </summary>
        private DispatcherTimer FrameUpdateTimer { get; }

        /// <summary>
        /// Timer for enemy interactions such as spawning and collision detection
        /// </summary>
        private DispatcherTimer EnemyTimer { get; }

        /// <summary>
        /// Timer for increasing the level
        /// </summary>
        private DispatcherTimer LevelTimer { get; }

        /// <summary>
        /// Allows for a short period of time between dying and being able to restart to avoid accidental restarts after dying
        /// </summary>
        private DispatcherTimer DeathDelayTimer { get; }

        /// <summary>
        /// Execute an <see cref="Action"/> on the <see cref="Dispatcher"/> Thread.
        /// Used to manipulate object properties from within multi-threaded functions.
        /// </summary>
        /// <param name="callback">The <see cref="Action"/> to execute</param>
        private static void Invoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);

        public AnimationController(Player player, Canvas canvas, Grid gameUi, Grid gameOverScreen, List<Enemy> enemies, MusicController musicController)
        {
            _player = player;
            _frame = canvas;
            _ui = gameUi;
            _gameOverScreen = gameOverScreen;
            _enemies = enemies;
            _musicController = musicController;

            CanReSpawn = true;

            _random = new Random();
            Started = false;
            Level = 1;
            SpeedIncreaseValue = 3;
            ShowHitBoxes = false;
            SafeDistance = 300;

            FrameUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            EnemyTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1) };
            LevelTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            DeathDelayTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.5) };

            FrameUpdateTimer.Tick += Timer_PlayerMove;
            EnemyTimer.Tick += delegate { Task.Run(Timer_AttemptSpawnEnemy); };
            EnemyTimer.Tick += delegate { Task.Run(Timer_CheckCollision); };
            LevelTimer.Tick += delegate { Task.Run(() => Invoke(() => SetLevel(++Level))); };
            DeathDelayTimer.Tick += Timer_DeathDelay;
        }

        public void StartPlayerAnimation()
        {
            _player.SetToStartPosition();
            SetLevel(1);

            Started = true;
            FrameUpdateTimer.Start();
            EnemyTimer.Start();
            LevelTimer.Start();

            _player.GraphicsController.StartSound();

            _gameOverScreen.Visibility = Visibility.Hidden;
        }

        private void Timer_PlayerMove(object? sender, EventArgs e)
        {
            _player.MovePlayer();
            // Move enemies and check if any have left the screen
            var toRemove = _enemies.FindAll(enemy =>
            {
                enemy.Move();
                return enemy.X <= 0 + 25 - Enemy.BaseWidth;
            });

            // Remove the enemies that have left the screen
            toRemove.ForEach(enemy =>
            {
                _frame.Children.Remove(enemy.Sprite);
                _enemies.Remove(enemy);
                _enemies.TrimExcess();
            });
        }

        private void Timer_AttemptSpawnEnemy()
        {
            // Has a random chance to spawn an enemy on every tick
            if (_random.Next(25) != 10 && _enemies.Count != 0) return;
            int yPos = _random.Next((int)(_frame.ActualHeight - Enemy.BaseHeight));
            if (!IsSafeDistance(yPos)) return;
            Invoke(() => _enemies.Add(new Enemy(_frame, ++EnemyCount, yPos, Level, SpeedIncreaseValue, ShowHitBoxes)));
        }

        private void Timer_CheckCollision()
        {
            // Check if player has lost
            if (!EnemyHit() && !_player.HitFloor) return;

            // Stop timers
            FrameUpdateTimer.Stop();
            EnemyTimer.Stop();
            LevelTimer.Stop();

            DeathDelayTimer.Start();
            CanReSpawn = false;

            Started = false;

            Invoke(() =>
            {
                // Stop music and player audio
                _musicController.Stop();
                _player.GraphicsController.PlayDeathSound();
                // Clear items on screen
                _enemies.Clear();
                _frame.Children.Clear();
                // Show Game Over screen
                _gameOverScreen.Visibility = Visibility.Visible;
            });
        }

        private void Timer_DeathDelay(object? sender, EventArgs e)
        {
            DeathDelayTimer.Stop();
            CanReSpawn = true;
        }

        /// <summary>
        /// Checks if an enemy spawn position is far away enough from the last 2 enemies.
        /// This helps to keep distance between the enemies.
        /// </summary>
        /// <param name="yPosition">The position to test</param>
        /// <returns>True if <paramref name="yPosition"/> is far enough from other enemies, else false</returns>
        private bool IsSafeDistance(int yPosition)
        {
            for (var i = 1; i < 2; i++)
            {
                if (i > _enemies.Count) break;
                var toTest = _enemies[^i];
                double xDistance = _frame.ActualWidth - toTest.X;
                double yDistance = yPosition - toTest.Y;
                double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                if (distance < SafeDistance) return false;
            }

            return true;
        }

        private bool EnemyHit()
        {
            Rect player = new(_player.X, _player.Y, _player.Sprite.ActualWidth, _player.Sprite.ActualHeight);
            return _enemies
                .Select(enemy => new Rect(enemy.X, enemy.Y, enemy.Sprite.ActualWidth, enemy.Sprite.ActualHeight))
                .Any(en => player.IntersectsWith(en));
        }

        private void SetLevel(int level)
        {
            Level = level;
            if (_ui.Children[1] is Label { Name: "LevelIndicator" } levelIndicator) levelIndicator.Content = $"{level}";
            else throw new Exception("Could not find level indicator label");
        }

        public void SetLevelUpTime(int time)
        {
            LevelTimer.Interval = TimeSpan.FromSeconds(time);
        }
    }
}