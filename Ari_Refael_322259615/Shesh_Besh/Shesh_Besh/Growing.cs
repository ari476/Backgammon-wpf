using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace Shesh_Besh
{
    class Growing
    {
        private static DoubleAnimation anim = new DoubleAnimation();
        private static double OrigFontSize, MaxFontSize;


        public static void Grow_Up(TextBlock tb)
        {
            //אירוע זה מגדיל את התווית הנבחרת כאשר העכבר עליו
            OrigFontSize = tb.FontSize;
            MaxFontSize = OrigFontSize + 5;

            anim.From = tb.FontSize;
            anim.To = MaxFontSize;
            anim.Duration = TimeSpan.FromSeconds(.5);
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }

        public static void Grow_Down(TextBlock tb)
        {//אירוע זה מקטין את התווית הנבחרת כאשר העכבר עזב אותו
            anim.From = tb.FontSize;
            anim.To = OrigFontSize;
            anim.Duration = TimeSpan.FromSeconds(.5);
            tb.BeginAnimation(TextBlock.FontSizeProperty, anim);
        }
    }
}
