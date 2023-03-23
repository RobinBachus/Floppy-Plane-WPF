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
        private Player Player { get; set; }
        private List<Enemy> Enemies { get; set; }
        private int EnemyCount { get; set; }
        private Canvas Frame { get; set; }
        private readonly Random Random = new();


        // Timer for the animations
        private DispatcherTimer FrameUpdateTimer { get; }
        // Timer for spawning enemies
        private DispatcherTimer EnemySpawnTimer { get; }
        // Timer for spawning enemies
        private DispatcherTimer CollisionTimer { get; }

        public AnimationController(Player player, Canvas canvas, List<Enemy> enemies)
        {
            Player = player;
            Frame = canvas;
            Enemies = enemies;

            FrameUpdateTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            EnemySpawnTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            CollisionTimer = new()
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };

            FrameUpdateTimer.Start();
            EnemySpawnTimer.Start();
            CollisionTimer.Start();
        }

        public void StartPlayerAnimation()
        {
            FrameUpdateTimer.Tick += Timer_PlayerMove;
            EnemySpawnTimer.Tick += (sender, args) => Task.Run(() => Timer_AttemptSpawnEnemy(sender, args));
            CollisionTimer.Tick += (sender, args) => Task.Run(() => Timer_CheckCollision(sender, args));
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
                        Enemies.Add(new(Frame, ++EnemyCount, yPos));
                    });
                }
            }
        }

        private void Timer_CheckCollision(object? sender, EventArgs e)
        {

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
                if (distance < 400) return false;
            }

            return true;
        }
    }
}
