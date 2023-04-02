using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Floppy_Plane_WPF
{
    internal class AnimationController
    {
        private Player Player { get; }
        private List<Enemy> Enemies { get; }
        private int EnemyCount { get; set; }
        private Canvas Frame { get; }
        private Grid UI { get; }
        private Grid GameOverScreen { get; }
        private Random Random { get; }

        public bool Started { get; set; }
        public int Level { get; set; }
        public int SpeedIncreaseValue { get; set; }
        public bool ShowHitBoxes { get; set; }
        public bool CanRespawn { get; private set; }

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
        /// </summary> */
        private DispatcherTimer DeathDelayTimer { get; }

        public AnimationController(Player player, Canvas canvas, Grid GameUI,  Grid gameOverScreen , List<Enemy> enemies)
        {
            Player = player;
            Frame = canvas;
            UI = GameUI;
            GameOverScreen = gameOverScreen;
            Enemies = enemies;

            Random = new Random();
            Started = false;
            Level = 1;
            SpeedIncreaseValue = 3;
            ShowHitBoxes = false;
            CanRespawn = true;

            FrameUpdateTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            EnemyTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            LevelTimer = new()
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            DeathDelayTimer = new()
            {
                Interval= TimeSpan.FromSeconds(.5)
            };

            FrameUpdateTimer.Tick += Timer_PlayerMove;
            EnemyTimer.Tick += (sender, args) => Task.Run(() => Timer_AttemptSpawnEnemy(sender, args));
            EnemyTimer.Tick += (sender, args) => Task.Run(() => Timer_CheckCollision(sender, args));
            LevelTimer.Tick += (sender, args) => Task.Run(() => Timer_Levelup(sender, args));
            DeathDelayTimer.Tick += Timer_DeathDelay;
        }

        public void StartPlayerAnimation()
        {
            
            Player.SetToStartingPosition();
            SetLevel(1);

            Started = true;
            FrameUpdateTimer.Start();
            EnemyTimer.Start();
            LevelTimer.Start();

            GameOverScreen.Visibility = Visibility.Hidden;
        }

        void Timer_PlayerMove(object? sender, EventArgs e)
        {
            Player.MovePlayer();
            // Move enemies and check if any have left the screen
            List<Enemy> toRemove = Enemies.FindAll((enemy) => {
                enemy.Move();
                return enemy.X <= 0 +25 - Enemy.BaseWidth; 
            });

            // Remove the enemies that have left the screen
            toRemove.ForEach(enemy => {
                Frame.Children.Remove(enemy.Sprite);
                Enemies.Remove(enemy);
                Enemies.TrimExcess();
            });
        }

        private void Timer_AttemptSpawnEnemy(object? sender, EventArgs e) 
        {
            // Has a random chance to spawn an enemy on every tick
            if (Random.Next(25) == 10 || Enemies.Count == 0)
            {
                int yPos = Random.Next((int)(Frame.ActualHeight - Enemy.BaseHeight));
                if (IsSafeDistace(yPos))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Enemies.Add(new(Frame, ++EnemyCount, yPos, Level, SpeedIncreaseValue, ShowHitBoxes));
                    });
                }
            }
        }

        private void Timer_CheckCollision(object? sender, EventArgs e)
        {
            // Check if player has lost
            if (EnemyHit() || Player.HitFloor)
            {
                // Stop timers
                FrameUpdateTimer.Stop();
                EnemyTimer.Stop();
                LevelTimer.Stop();

                DeathDelayTimer.Start();
                CanRespawn = false;

                Started = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Clear items on screen
                    Enemies.Clear();
                    Frame.Children.Clear();
                    // Show Game Over screen
                    GameOverScreen.Visibility = Visibility.Visible;
                });
            }
        }

        private void Timer_Levelup(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SetLevel(++Level);
            });

        }

        private void Timer_DeathDelay(object? sender, EventArgs e)
        {
            DeathDelayTimer.Stop();
            CanRespawn = true;
        }

        private bool IsSafeDistace(int yPosition)
        {
            for (int i = 1; i < 2; i++)
            {
                if (i > Enemies.Count) break;
                Enemy toTest = Enemies[^i];
                double xDistance = Frame.ActualWidth - toTest.X;
                double yDistance = yPosition - toTest.Y;
                double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                if (distance < 300) return false;
            }

            return true;
        }

        private bool EnemyHit()
        {
            Rect player = new(Player.X, Player.Y, Player.Sprite.ActualWidth, Player.Sprite.ActualHeight);
            foreach (Enemy enemy in Enemies)
            {
                Rect en = new(enemy.X, enemy.Y, enemy.Sprite.ActualWidth, enemy.Sprite.ActualHeight);
                if (player.IntersectsWith(en)) return true;
            }
            return false;
        }

        private void SetLevel(int level)
        {
            Level = level;
            UIElement _label = UI.Children[1];
            if (_label is Label label && label.Name == "LevelIndicator") label.Content = $"{level}";
        }

        public void SetLevelUpTime(int time)
        {
            LevelTimer.Interval = TimeSpan.FromSeconds(time);
        }
    }
}
