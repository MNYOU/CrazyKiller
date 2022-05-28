using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CrazyKiller
{
    public class ViewGolems
    {
        private static List<GolemImages> golemsImages;
        private List<ViewGolem> golems;
        private static int wait;
        private int counter;

        public ViewGolems(List<Golem> golems)
        {
            golemsImages = new List<GolemImages>();
            for (var i = 0; i < 3; i++)
                golemsImages.Add(new GolemImages(i + 1));
            if (golems != null && golemsImages != null)
                golems.FirstOrDefault().Size = golemsImages.FirstOrDefault().Attacking.FirstOrDefault().Size;
            Initialize(golems);
        }

        private void Initialize(List<Golem> golems)
        {
            wait = 1;
            this.golems = new List<ViewGolem>();
            foreach (var zombie in golems)
            {
                this.golems.Add(new ViewGolem(zombie, counter % 3));
                if (counter == 2) counter = 0;
                else counter++;
            }
        }

        public void PaintGolems(Graphics graphics, List<Golem> zombies)
        {
            if (golems.Count < zombies.Count) Initialize(zombies);
            foreach (var golem in golems.OrderByDescending(x => x.IsDead))
            {
                var image = golem.GetImage();
                var pos = golem.GetPosition();
                if (golem.IsTurnLeft)
                    graphics.DrawImage(image, pos.X + image.Width, pos.Y, -image.Width, image.Height);
                else
                    graphics.DrawImage(image, pos);
            }
        }

        private class ViewGolem
        {
            private int elapsedTime;
            private readonly Golem zombie;
            private readonly Queue<Image> queueImages;
            private readonly GolemImages images;
            private GolemState previousState;
            private GolemState state;
            private Image lastImage;
            public bool IsTurnLeft { get; private set; }
            public bool IsDead => zombie.IsDead;

            public ViewGolem(Golem zombie, int index)
            {
                this.zombie = zombie;
                images = golemsImages[index];
                state = GolemState.Walking;
                previousState = state;
                queueImages = new Queue<Image>();
                elapsedTime = GameModel.Random.Next(images.Attacking.Count);
            }

            public Image GetImage()
            {
                UpdateState();
                UpdateDirection();
                if (previousState != state || queueImages.Count == 0)
                    UpdateQueue();
                lastImage = queueImages.Dequeue();
                return lastImage;
            }

            private void UpdateDirection()
            {
                var offsetX = zombie.Position.X - zombie.PreviousPosition.X;
                if (Math.Sign(offsetX) == 1 && IsTurnLeft) IsTurnLeft = false;
                else if (Math.Sign(offsetX) == -1 && !IsTurnLeft) IsTurnLeft = true;
            }

            private void UpdateState()
            {
                previousState = state;
                if (zombie.IsDead)
                    state = GolemState.Dying;
                else if (zombie.IsPenetration)
                    state = GolemState.Hurt;
                else if (zombie.IsAttack)
                    state = GolemState.Attacking;
                else
                    state = GolemState.Walking;
            }

            private void UpdateQueue()
            {
                if (IsDead && lastImage == this.images.Dying.Last())
                {
                    for (var i = 0; i < this.images.Dying.Count; i++)
                        queueImages.Enqueue(lastImage);
                    return;
                }

                queueImages.Clear();
                var images = GetImagesByState();
                for (var i = elapsedTime; i < images.Count; i++)
                for (var j = 0; j < wait; j++)
                    queueImages.Enqueue(images[i]);

                elapsedTime = 0;
            }

            private List<Image> GetImagesByState()
            {
                switch (state)
                {
                    case GolemState.Attacking:
                        return images.Attacking;
                    case GolemState.Walking:
                        return images.Walking;
                    case GolemState.Dying:
                        return images.Dying;
                    case GolemState.Hurt:
                        return images.Hurt;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            }

            public Point GetPosition()
            {
                return PointMethods.GetOffsetPosition(zombie);
            }
        }

        private class GolemImages
        {
            private int Id { get; }
            public List<Image> Walking { get; private set; }
            public List<Image> Attacking { get; private set; }
            public List<Image> Hurt { get; private set; }
            public List<Image> Dying { get; private set; }

            public GolemImages(int id)
            {
                Id = id;
                Initialize();
            }

            private void Initialize()
            {
                Dying = new List<Image>();
                Hurt = new List<Image>();
                Attacking = new List<Image>();
                Walking = new List<Image>();
                var number = GameModel.Random.Next(3) + 1;
                AddImagesForEvent(Walking, Id, GolemState.Walking);
                AddImagesForEvent(Attacking, Id, GolemState.Attacking);
                AddImagesForEvent(Hurt, Id, GolemState.Hurt);
                AddImagesForEvent(Dying, Id, GolemState.Dying);
            }

            private void AddImagesForEvent(List<Image> images, int golemNumber, GolemState state)
            {
                var folder = state + "Small";
                var path =
                    string.Join("\\", AppDomain.CurrentDomain.BaseDirectory.Split('\\').Reverse().Skip(3).Reverse()) +
                    $"\\images\\golems\\golem{golemNumber}\\PNG\\{folder}";
                images.AddRange(Directory.GetFiles(path).Select(Image.FromFile));
            }
        }
    }
}