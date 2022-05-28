using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    public class ViewPlayer
    {
        private readonly Player player;
        private Dictionary<PlayerState, (Image, Size)> images;
        private (Image, Size) runAndShoot;
        private (Image, Size) running;
        private (Image, Size) shooting;
        private (Image, Size) hurt;
        private (Image, Size) blink;
        private (Image, Size) reloading;
        private PlayerState state;
        private bool isTurnLeft;
        private int counter;
        private int Wait { get; }
        private Point Pos => PointMethods.GetOffsetPosition(player);

        public ViewPlayer(Player player)
        {
            this.player = player;
            Wait = 2;
            Initialise();
        }

        private void Initialise()
        {
            runAndShoot = GetImageWithSize("shooting_while_run", 6);
            running = GetImageWithSize("run", 6);
            shooting = GetImageWithSize("shooting_while_stand", 3);
            hurt = GetImageWithSize("hurting", 1);
            blink = GetImageWithSize("blinking", 3);
            reloading = GetImageWithSize("reloading_while_stand", 7);
            GetImageWithSize("stand", 1);

            player.Size = hurt.Item2;
            state = PlayerState.Stand;
        }

        public void Paint(Graphics graphics)
        {
            UpdateState();
            UpdateDirection();
            var (image, imageSize) = GetImagesByState();
            var spritesCount = image.Width / imageSize.Width;
            if (isTurnLeft) counter--;
            else counter++;
            if (counter / Wait > spritesCount || counter == -1)
            {
                if (isTurnLeft) counter = Wait * spritesCount - 1;
                else counter = 0;
            }

            var rec = new Rectangle(
                imageSize.Width * (counter / Wait) == 0 ? 0 : imageSize.Width * (counter / Wait - 1), 0,
                imageSize.Width, imageSize.Height);
            graphics.DrawImage(image, Pos.X, Pos.Y, rec, GraphicsUnit.Pixel);
        }

        private (Image, Size) GetImagesByState()
        {
            if (images is null)
                images = new Dictionary<PlayerState, (Image, Size)>
                {
                    [PlayerState.Attack] = shooting,
                    [PlayerState.AttackAndRun] = runAndShoot,
                    [PlayerState.Run] = running,
                    [PlayerState.Reloading] = reloading,
                    [PlayerState.Stand] = blink,
                    [PlayerState.Hurt] = hurt
                };

            var image = images[state];
            if (!isTurnLeft) return image;
            var rotateImage = (Image) image.Item1.Clone();
            rotateImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return (rotateImage, image.Item2);
        }

        private void UpdateState()
        {
            if (player.Position != player.PreviousPosition)
                state = player.Gun.MouseIsClick && player.Gun.FiredAmmunition != player.Gun.Ammunition
                    ? PlayerState.AttackAndRun
                    : PlayerState.Run;
            else if (player.Gun.FiredAmmunition == player.Gun.Ammunition) state = PlayerState.Reloading;
            else if (player.Gun.MouseIsClick) state = PlayerState.Attack;
            else if (player.IsAttacked) state = PlayerState.Hurt;
            else state = PlayerState.Stand;
        }

        private void UpdateDirection()
        {
            var offsetX = player.Position.X - player.PreviousPosition.X;
            var mouseOffsetX = player.Gun.Position.X - player.Position.X;
            if (player.Gun.MouseIsClick)
            {
                if (Math.Sign(mouseOffsetX) == 1) isTurnLeft = false;
                else if (Math.Sign(mouseOffsetX) == -1) isTurnLeft = true;
            }
            else if (Math.Sign(mouseOffsetX) == 1 && isTurnLeft && state != PlayerState.Run)
                isTurnLeft = false;
            else if (Math.Sign(mouseOffsetX) == -1 && !isTurnLeft && state != PlayerState.Run)
                isTurnLeft = true;
            else if (Math.Sign(offsetX) == 1 && isTurnLeft)
                isTurnLeft = false;
            else if (Math.Sign(offsetX) == -1 && !isTurnLeft)
                isTurnLeft = true;
        }

        private static (Image, Size) GetImageWithSize(string even, int imagesCount)
        {
            var builder = new StringBuilder();
            builder.Append(even);
            builder[0] = char.ToUpper(even[0]);
            even = builder.ToString();
            var path =
                string.Join("\\", AppDomain.CurrentDomain.BaseDirectory.Split('\\').Reverse().Skip(3).Reverse()) +
                $"\\images\\player";
            var names = Directory.GetFiles(path);
            foreach (var fileName in names.Select(x => x.Split('\\').Last()))
                if (fileName.Split('.').First() == even || fileName.Split(' ').First() == even)
                {
                    var image = Image.FromFile(path + '\\' + fileName);
                    return (image, new Size(image.Width / imagesCount, image.Height));
                }

            return (null, Size.Empty);
        }
    }
}