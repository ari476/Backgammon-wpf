using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Shesh_Besh
{
    class Kube
    {
        Model3DGroup Dice;
        int front = 6, left = 5, bottum = 4, top = 3, right = 2, back = 1;
        string Name;
        AxisAngleRotation3D ax3d;
        DoubleAnimation xAnim = new DoubleAnimation();
        Transform3DGroup Tg = new Transform3DGroup();

        public Kube(Model3DGroup Dice, string Name, Transform3DGroup Tg)
        {
            this.Dice = Dice;
            this.Name = Name;
            xAnim.Duration = TimeSpan.FromSeconds(.25);
            this.Tg = Tg;
        }
        public int GetTop() { return this.top; }
        public Model3DGroup GetModel() { return this.Dice; }
        public void RotationX(string si, double bt)
        {
            int tempFront = front, tempTop = top, tempBack = back, tempBottum = bottum;
            xAnim.To = (si == "+") ? 90 : -90;
            xAnim.BeginTime = TimeSpan.FromSeconds(bt);
            ax3d = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);

            if (Name == "D1")
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3, 1.5, .9)));
            else
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3.91, 1.5, .89)));

            ax3d.BeginAnimation(AxisAngleRotation3D.AngleProperty, xAnim);

            if(si =="+")
            {
                front = tempTop;
                bottum = tempFront;
                back = tempBottum;
                top = tempBack;
            }
            else
            {
                front = tempBottum;
                bottum = tempBack;
                back = tempTop;
                top = tempFront;
            }
        }
        public void RotationY(string si ,double bt)
        {
            int tempFront = front, tempBack = back, tempLeft = left, tempRight = right;
            xAnim.To = (si == "+") ? 90 : -90;
            xAnim.BeginTime = TimeSpan.FromSeconds(bt);
            ax3d = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);

            if (Name == "D1")
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3, 1.785, 0.9)));
            else
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3.91, 1.796, .89)));

            ax3d.BeginAnimation(AxisAngleRotation3D.AngleProperty, xAnim);

            if (si == "+")
            {
                front = tempLeft;
                right = tempFront;
                back = tempRight;
                left = tempBack;
            }
            else
            {
                front = tempRight;
                right = tempBack;
                back = tempLeft;
                left = tempFront;
            }
        }
        public void RotationZ(string si, double bt)
        {
            int tempTop = top, tempBottum = bottum, tempLeft = left, tempRight = right;
            xAnim.To = (si == "+") ? 90 : -90;
            xAnim.BeginTime = TimeSpan.FromSeconds(bt);
            ax3d = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);

            if (Name == "D1")
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3, 1.5, 2)));
            else
                Tg.Children.Add(new RotateTransform3D(ax3d, new Point3D(-3.91, 1.5, 2)));

            ax3d.BeginAnimation(AxisAngleRotation3D.AngleProperty, xAnim);

            if (si == "+")
            {
                left = tempTop;
                bottum = tempLeft;
                right = tempBottum;
                top = tempRight;
            }
            else
            {
                right = tempTop;
                bottum = tempRight;
                left = tempBottum;
                top = tempLeft;

            }
        }

    }
}
