using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Media;
using NAudio;

namespace CrazyKiller
{
    public class Sounds
    {
        private string Path { get; }
        private SoundPlayer SoundPlayer { get; }

        public Sounds()
        {
            Path = string.Join("\\", AppDomain.CurrentDomain.BaseDirectory.Split('\\').Reverse().Skip(3).Reverse()) +
                   @"\view\media\";
            SoundPlayer = new SoundPlayer();
        }

        public void StartMusic()
        {
            SoundPlayer.SoundLocation = Path + "soundtrack.wav";
            SoundPlayer.Play();
        }
        public void SoundButtonClick()
        {
            SoundPlayer.SoundLocation = Path + "button.wav";
            SoundPlayer.Play();
        }

        public void Stop()
        {
            SoundPlayer.Stop();
        }
    }
}