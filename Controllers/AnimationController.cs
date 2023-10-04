using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Floppy_Plane_WPF.GameObjects;

namespace Floppy_Plane_WPF.Controllers
{
    internal class AnimationController
    {
        private Player Player { get; }
        private List<Enemy> Enemies { get; }
        private int EnemyCount { get; set; }
        private Canvas Frame { get; }
        private Grid Ui { get; }
        private Grid GameOverScreen { get; }
        private Random Random { get; }

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

        public AnimationController(Player player, Canvas canvas, Grid gameUi, Grid gameOverScreen, List<Enemy> enemies)
        {
            Player = player;
            Frame = canvas;
            Ui = gameUi;
            GameOverScreen = gameOverScreen;
            Enemies = enemies;

            CanReSpawn = true;

            Random = new Random();
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

            Player.SetToStartPosition();
            SetLevel(1);

            Started = true;
            FrameUpdateTimer.Start();
            EnemyTimer.Start();
            LevelTimer.Start();

            GameOverScreen.Visibility = Visibility.Hidden;
        }

        private void Timer_PlayerMove(object? sender, EventArgs e)
        {
            Player.MovePlayer();
            // Move enemies and check if any have left the screen
            var toRemove = Enemies.FindAll(enemy =>
            {
                enemy.Move();
                return enemy.X <= 0 + 25 - Enemy.BaseWidth;
            });

            // Remove the enemies that have left the screen
            toRemove.ForEach(enemy =>
            {
                Frame.Children.Remove(enemy.Sprite);
                Enemies.Remove(enemy);
                Enemies.TrimExcess();
            });
        }

        private void Timer_AttemptSpawnEnemy()
        {
            // Has a random chance to spawn an enemy on every tick
            if (Random.Next(25) != 10 && Enemies.Count != 0) return;
            int yPos = Random.Next((int)(Frame.ActualHeight - Enemy.BaseHeight));
            if (!IsSafeDistance(yPos)) return;
            Invoke(() => Enemies.Add(new Enemy(Frame, ++EnemyCount, yPos, Level, SpeedIncreaseValue, ShowHitBoxes)));
        }

        private void Timer_CheckCollision()
        {
            // Check if player has lost
            if (!EnemyHit() && !Player.HitFloor) return;

            // Stop timers
            FrameUpdateTimer.Stop();
            EnemyTimer.Stop();
            LevelTimer.Stop();

            DeathDelayTimer.Start();
            CanReSpawn = false;

            Started = false;

            Invoke(() =>
            {
                // Clear items on screen
                Enemies.Clear();
                Frame.Children.Clear();
                // Show Game Over screen
                GameOverScreen.Visibility = Visibility.Visible;
            });
        }

        private void Timer_DeathDelay(object? sender, EventArgs e)
        {
            DeathDelayTimer.Stop();
            CanReSpawn = true;
        }

        private bool IsSafeDistance(int yPosition)
        {
            for (var i = 1; i < 2; i++)
            {
                if (i > Enemies.Count) break;
                var toTest = Enemies[^i];
                double xDistance = Frame.ActualWidth - toTest.X;
                double yDistance = yPosition - toTest.Y;
                double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                if (distance < SafeDistance) return false;
            }

            return true;
        }

        private bool EnemyHit()
        {
            Rect player = new(Player.X, Player.Y, Player.Sprite.ActualWidth, Player.Sprite.ActualHeight);
            foreach (var enemy in Enemies)
            {
                Rect en = new(enemy.X, enemy.Y, enemy.Sprite.ActualWidth, enemy.Sprite.ActualHeight);
                if (player.IntersectsWith(en)) return true;
            }
            return false;
        }

        private void SetLevel(int level)
        {
            Level = level;
            var label = Ui.Children[1];
            if (label is Label { Name: "LevelIndicator" } levelIndicator) levelIndicator.Content = $"{level}";
            else throw new Exception("Could not find level indicator label");
        }

        public void SetLevelUpTime(int time)
        {
            LevelTimer.Interval = TimeSpan.FromSeconds(time);
        }
    }
}
