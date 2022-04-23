using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrazyKiller
{
    static class Program // добавлена отрисовка карты и персонажа, now player can move
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var player = new Player(20, new Point(160, 60));
            var game = new GameModel(player);
            Application.Run(new GameForm(game));
        }
    }
}
