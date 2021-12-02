using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Shesh_Besh
{
    /// <summary>
    /// Interaction logic for StartGame.xaml
    /// </summary>
    public partial class StartGame : Window
    {
        static Model3D[] OutR = new Model3D[15];
        static Model3D[] OutG = new Model3D[15];
        static Model3D[] RedPlayer = new Model3D[18];                                                               // מערכים של אוביקטים מסוג מודלי של כל הכלים
        static Model3D[] GrayPlayer = new Model3D[18];
        static Model3D[] EatRed = new Model3D[15];
        static Model3D[] EatGray = new Model3D[15];
        static Model3D md;
        static int firsrT = 1, NCountOutSideR = 15, NCountOutSideG = 15;
        static bool fDice = false, sDice = false, isCople = false, NoPossibleM = false, NowOEat;
        static Col[] AllOfCol = new Col[26]; //AllOfCol[0] ,AllOfCol[25]- not using (for eat)
        static string turn = "gray"; static int f, s, redS, grayS, Count = 4, prev = 1, col, countEatR = 0, countEatG = 0;
        static Random reg = new Random();
        static Model3DGroup temp;
        static double beginTime=0;
        static DispatcherTimer tmAnim;
        static Kube D1, D2;
        static int[] colArG = new int[] { 1, 1, 12, 12, 12, 12, 12, 17, 17, 17, 19, 19, 19, 19, 19 };
        static int[] colArR = new int[] { 24, 24, 13, 13, 13, 13, 13, 8, 8, 8, 6, 6, 6, 6, 6 };
        static bool isKeyDown = true;
        // static Label L1 = new Label(), L2 = new Label(), L3 = new Label();
        // static Label[] L = new Label[25];
        static bool ifMove;

        public StartGame()
        {
            InitializeComponent();
        }
        private void on_OpenScreen(object sender, RoutedEventArgs e)
        {
            Transform3DGroup tGroup = new Transform3DGroup();                                                                                       // יצירת כל הכלים והוספתם לסצנה
            Transform3DGroup v = new Transform3DGroup();

            tmAnim = new DispatcherTimer();
            tmAnim.Interval = TimeSpan.FromSeconds(.8);
            tmAnim.Tick += TmAnim_Tick;

            temp = ((Model3DGroup)FindResource("Dice")).Clone();
            Scene.Children.Add(temp);
            tGroup.Children.Add(new TranslateTransform3D(-10, 5, 3));
            tGroup.Children.Add(new ScaleTransform3D(0.3, 0.3, 0.3));
            temp.Transform = tGroup;
            D1 = new Kube(temp, "D1", tGroup);

            temp = ((Model3DGroup)FindResource("Dice")).Clone();
            Scene.Children.Add(temp);
            v.Children.Add(new TranslateTransform3D(-13, 5, 3));
            v.Children.Add(new ScaleTransform3D(0.3, 0.3, 0.3));
            temp.Transform = v;
            D2 = new Kube(temp, "D2",v);
           
            for (int i = 0; i < 18; i++)
            {

                GrayPlayer[i] = (new GeometryModel3D((MeshGeometry3D)this.Resources["sesbes"], (Material)this.Resources["gray"])).Clone();
                RedPlayer[i] = (new GeometryModel3D((MeshGeometry3D)this.Resources["sesbes"], (Material)this.Resources["red"])).Clone();

                Scene.Children.Add(GrayPlayer[i]);
                Scene.Children.Add(RedPlayer[i]);

            }

            //colArR = new int[15] { 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 4, 2, 2, 2, 1 };
            //colArG = new int[15] { 19, 19, 19, 19, 20, 20, 20, 20, 23, 23, 24, 24, 21, 21, 22 };

            //colArR = new int[15] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 22 };
            //colArG = new int[15] { 19, 19, 19, 19, 20, 20, 20, 20, 23, 23, 24, 24, 21, 21, 21 };

            //colArR = new int[15] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 2, 2, 6, 5 };
            //colArG = new int[15] { 19, 19, 19, 19, 20, 20, 20, 20, 23, 23, 22, 22, 3, 3, 3 };

            //colArR = new int[15] { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19 };
            //colArG = new int[15] { 21, 21, 21, 21, 20, 20, 20, 20, 23, 23, 23, 23, 23, 24, 24 };


            for (int i = 0; i < 15; i++)
            {
                tGroup = new Transform3DGroup();
                //  tGroup.Children.Add(new TranslateTransform3D(Xto(colArG[i]), 0, Zto(colArG[i], CountOfThisCol(i, "G"))));
                tGroup.Children.Add(new TranslateTransform3D(20, 0, 20));
                tGroup.Children.Add(new ScaleTransform3D(.8, 1.5, .8));
                GrayPlayer[i].Transform = tGroup;

                if (AllOfCol[colArG[i]] == null)
                    AllOfCol[colArG[i]] = new Col(GrayPlayer[i], colArG[i], "gray");
                else
                    AllOfCol[colArG[i]].Add(GrayPlayer[i]);

                tGroup = new Transform3DGroup();
                //   tGroup.Children.Add(new TranslateTransform3D(Xto(colArR[i]), 0, Zto(colArR[i], CountOfThisCol(i, "R"))));
                tGroup.Children.Add(new TranslateTransform3D(20, 0, 20));
                tGroup.Children.Add(new ScaleTransform3D(.8, 1.5, .8));
                RedPlayer[i].Transform = tGroup;

                if (AllOfCol[colArR[i]] == null)
                    AllOfCol[colArR[i]] = new Col(RedPlayer[i], colArR[i], "red");
                else
                    AllOfCol[colArR[i]].Add(RedPlayer[i]);

            }



            for (int i = 0,be = 015; i < 15; i++ ,be++)
            {

                DoubleAnimation xAnim = new DoubleAnimation();
                DoubleAnimation zAnim = new DoubleAnimation();

                xAnim.To = Xto(colArG[i]);
                xAnim.Duration = TimeSpan.FromSeconds(.2);
                xAnim.BeginTime = TimeSpan.FromSeconds(be * 0.2);

                zAnim.To = Zto(colArG[i], CountOfThisCol(i, "G"));
                zAnim.Duration = TimeSpan.FromSeconds(.2);
                xAnim.BeginTime = TimeSpan.FromSeconds(be *.2);

                ((Transform3DGroup)GrayPlayer[i].Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetXProperty, xAnim);

                ((Transform3DGroup)GrayPlayer[i].Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetZProperty, zAnim);

                xAnim.To = Xto(colArR[i]);
                zAnim.To = Zto(colArR[i], CountOfThisCol(i, "R"));
                ((Transform3DGroup)RedPlayer[i].Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetXProperty, xAnim);

                ((Transform3DGroup)RedPlayer[i].Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetZProperty, zAnim);



                //MooveXZ(GrayPlayer[i], Xto(colArG[i]), Zto(colArG[i], CountOfThisCol(i, "G")));

            }

            DoubleAnimation dAnim = new DoubleAnimation();
            dAnim.To = 0;
            dAnim.BeginTime = TimeSpan.FromSeconds(.6);
            dAnim.Duration = TimeSpan.FromSeconds(15 *.2);
            ro1.BeginAnimation(AxisAngleRotation3D.AngleProperty, dAnim);

            

            for (int i = 15; i < 18; i++)
            {
                tGroup = new Transform3DGroup();
                tGroup.Children.Add(new TranslateTransform3D(20, 0.5, Zto(1, 0)));
                tGroup.Children.Add(new ScaleTransform3D(.8, 1.5, .8));
                GrayPlayer[i].Transform = tGroup;

                tGroup = new Transform3DGroup();
                tGroup.Children.Add(new TranslateTransform3D(20, 0.5, Zto(1, 0)));
                tGroup.Children.Add(new ScaleTransform3D(.8, 1.5, .8));
                RedPlayer[i].Transform = tGroup;

            }

            dAnim.BeginTime = TimeSpan.FromSeconds(.6 + 15*.2);
            dAnim.To = 65;
            cam.BeginAnimation(PerspectiveCamera.FieldOfViewProperty, dAnim);

            OutsideG.Content = "Press U,D to\nchange the\nviewfinder";
            Outside.Content = "Press Enter \n to rool the\n dice";

           

            DoubleAnimation oAnim = new DoubleAnimation();
            oAnim.To = 1;
            oAnim.Duration = TimeSpan.FromSeconds(3);
            oAnim.Completed += OAnim_Completed;
            Outside.BeginAnimation(OpacityProperty, oAnim);

            OutsideG.BeginAnimation(OpacityProperty, oAnim);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            DoubleAnimation dAnim = new DoubleAnimation();
            dAnim.To = 10;
            dAnim.Duration = TimeSpan.FromSeconds(2);
            if (e.Key == Key.U)
                rotate.BeginAnimation(AxisAngleRotation3D.AngleProperty, dAnim);
            if (e.Key == Key.D)
            {
                dAnim.To = 0;
                rotate.BeginAnimation(AxisAngleRotation3D.AngleProperty, dAnim);
            }
            if (e.Key == Key.Enter)
            {
                if (firsrT == 1 || firsrT == 2)
                {
                    if (firsrT == 1)
                    {
                        redS = roolKube(D1);
                        numbers.Content = redS;
                    }
                    else
                    {
                        grayS = roolKube(D2);
                        numbers.Content = grayS;

                        if (redS == grayS)
                        {
                            MessageBox.Show("equal value - rool again ", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                            firsrT = 1;
                            curent.Content = "Red";
                            curent.Foreground = new SolidColorBrush(Colors.Red);
                            return;
                        }
                        else
                        {
                            if (redS > grayS)
                                turn = "red";
                            MessageBox.Show(turn + " start", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                            firsrT = -1;
                            curent.Content = (turn == "red") ? "Red" : "Gray";
                            if (turn == "gray")
                                curent.Foreground = new SolidColorBrush(Colors.Gray);
                            else
                                curent.Foreground = new SolidColorBrush(Colors.Red);
                            numbers.Content = "";
                            return;

                        }
                    }
                    firsrT++;
                    curent.Content = "Gray";
                    curent.Foreground = new SolidColorBrush(Colors.Gray);

                }
                else if (isKeyDown)
                {
                    int ft = roolKube(D1), st = roolKube(D2);
                    f = Math.Min(ft, st); s = Math.Max(ft, st);
                    numbers.Content = f + "," + s;
                    isKeyDown = false;
                    if (f == s) { Count = 8; isCople = true; }
                    ifHaveSteps();
                    if (NoPossibleM)
                    {
                        isKeyDown = true;
                        MessageBox.Show("There are no possible moves", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                        Count = 4; fDice = false; sDice = false; numbers.Content = ""; f = 0; s = 0;
                        if (turn == "red")
                        { turn = "gray"; curent.Content = "Gray"; curent.Foreground = new SolidColorBrush(Colors.Gray); }
                        else
                        { turn = "red"; curent.Content = "Red"; curent.Foreground = new SolidColorBrush(Colors.Red); }
                        NoPossibleM = false;
                    }
                }
            }
           Array.ForEach(colArG, Console.Write);

        }
        private void TmAnim_Tick(object sender, EventArgs e)
        {
            if (turn == "gray")
                EatMRed();
            else
                EatMgray();
            Move(3, "null");
            tmAnim.Stop();
            if (Count == 0 || NoPossibleM)
            {
                if (NoPossibleM)
                    MessageBox.Show("There are no possible moves", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                Count = 4; fDice = false; sDice = false; numbers.Content = ""; f = 0; s = 0;
                if (turn == "red")
                { turn = "gray"; curent.Content = "Gray"; curent.Foreground = new SolidColorBrush(Colors.Gray); }
                else
                { turn = "red"; curent.Content = "Red"; curent.Foreground = new SolidColorBrush(Colors.Red); }
                isKeyDown = true;
                NoPossibleM = false;
            }
        }

        private void onMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(MainViewport3D);
            RayMeshGeometry3DHitTestResult MeshHitResult = (RayMeshGeometry3DHitTestResult)VisualTreeHelper.HitTest(MainViewport3D, location);
            bool con = false;
            col = WhishCol(MeshHitResult);  //העמודה בלוח שנלחצה
            NowOEat = false;
            if (numbers.Content != null && firsrT == -1 && f != 0)
            {
                if (turn == "red" && countEatR != 0)
                {
                    if (Count % 2 == 0 && col == 0)
                    {
                        MooveY(EatRed[countEatR - 1], 0.5, 0);
                        md = EatRed[countEatR - 1];
                        Count--; prev = col; return;
                    }
                    else
                    {

                        if (prev == col || prev == 25 && col == 0)
                        {
                            MooveY(EatRed[countEatR - 1], 0.1, 0);
                            Count++;
                            return;
                        }

                        else if (Count % 2 != 0)
                        { RedOut(); }



                    }

                }
                if (turn == "gray" && countEatG != 0)
                {
                    if (Count % 2 == 0 && col == 0)
                    {
                        MooveY(EatGray[countEatG - 1], 0.5, 0);
                        md = EatGray[countEatG - 1];
                        Count--; prev = col; return;
                    }
                    else
                    {

                        if (prev == col)
                        {
                            MooveY(EatGray[countEatG - 1], 0.1, 0);
                            Count++;
                            return;
                        }

                        else if (Count % 2 != 0)
                        {
                            GrayOut();
                        }
                    }
                }

                if (AllOfCol[col] != null && Count % 2 == 0 && Count != 0)
                    if (AllOfCol[col].GetColor() == turn)
                    {
                        if (turn == "red")
                            con = (countEatR == 0);
                        else
                            con = (countEatG == 0);
                    }
                if (con && prev != 25 && prev != 0 && Count != 0 || con && !NowOEat)
                {
                    MooveY(AllOfCol[col].GetModel3D(), 0.5, 0);
                    prev = col;
                    md = AllOfCol[prev].GetModel3D();
                    Count--; return;
                }
                bool con1 = false;
                if (Count % 2 != 0 && Count != 0)
                {
                    if (turn == "red")
                        con1 = (countEatR == 0);
                    else
                        con1 = (countEatG == 0);

                    if (prev == col && con1)
                    {
                        MooveY(AllOfCol[col].GetModel3D(), 0, 0);
                        Count++;
                        return;
                    }
                    else if (con1)
                    {
                        if (turn == "red")
                            redM();
                        else
                            grayM();
                    }
                }



                if (Count == 0 || NoPossibleM)
                {
                    if (NoPossibleM)
                        MessageBox.Show("There are no possible moves", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
                    Count = 4; fDice = false; sDice = false; numbers.Content = ""; f = 0; s = 0;
                    if (turn == "red")
                    { turn = "gray"; curent.Content = "Gray"; curent.Foreground = new SolidColorBrush(Colors.Gray); }
                    else
                    { turn = "red"; curent.Content = "Red"; curent.Foreground = new SolidColorBrush(Colors.Red); }
                    isKeyDown = true;
                    NoPossibleM = false;
                }
            }
        }
        static void ifHaveSteps()
        {
            bool Iff = true, Ifs = true, Have = true;

            if (turn == "gray")
            {
                if (countEatG != 0)
                {
                    if (!fDice)
                    {
                        if (AllOfCol[f] != null)
                            Iff = (AllOfCol[f].GetColor() == turn || AllOfCol[f].GetColor() != turn && AllOfCol[f].GetCountOfThisCol() == 1);
                        Have = Iff;
                    }
                    if (!sDice)
                        if (AllOfCol[s] != null)
                        {
                            Ifs = (AllOfCol[s].GetColor() == turn || AllOfCol[s].GetColor() != turn && AllOfCol[s].GetCountOfThisCol() == 1);
                            Have = Ifs;
                        }

                    if (!fDice && !sDice)
                        Have = (Iff || Ifs);

                    NoPossibleM = !Have; return;
                }
                else
                {
                    for (int i = 1; i < 24; i++)
                    {
                        while (ColGorRorNull(i) != "G")
                        {
                            if (i < 24)
                                i++;
                        }
                        Iff = true; Ifs = true;
                        if (!fDice)
                        {
                            if (i + f < 25)
                                if (AllOfCol[i + f] != null)
                                    Iff = (AllOfCol[i + f].GetColor() == turn
                            || AllOfCol[i + f].GetColor() != turn && AllOfCol[i + f].GetCountOfThisCol() == 1);
                            Have = Iff;
                        }

                        if (!sDice)
                        {
                            if (i + s < 25)
                                if (AllOfCol[i + s] != null)
                                    Ifs = (AllOfCol[i + s].GetColor() == turn
                             || AllOfCol[i + s].GetColor() != turn && AllOfCol[i + s].GetCountOfThisCol() == 1);
                            Have = Ifs;
                        }

                        if (!fDice && !sDice)
                            Have = (Iff || Ifs);

                        if (Have)
                            return;
                    }
                }
                NoPossibleM = !Have; return;
            }

            else
            {
                if (countEatR != 0)
                {

                    if (!fDice)
                    {
                        if (AllOfCol[25 - f] != null)
                            Iff = (AllOfCol[25 - f].GetColor() == turn || AllOfCol[25 - f].GetColor() != turn && AllOfCol[25 - f].GetCountOfThisCol() == 1);
                        Have = Iff;
                    }
                    if (!sDice)
                        if (AllOfCol[25 - s] != null)
                        {
                            Ifs = (AllOfCol[25 - s].GetColor() == turn || AllOfCol[25 - s].GetColor() != turn && AllOfCol[25 - s].GetCountOfThisCol() == 1);
                            Have = Ifs;
                        }
                    if (!fDice && !sDice)
                        Have = (Iff || Ifs);

                    NoPossibleM = !Have; return;
                }
                else
                {
                    for (int i = 24; i > 0; i--)
                    {
                        while (ColGorRorNull(i) != "R")
                        {
                            if (i > 0)
                                i--;
                        }
                        Iff = true; Ifs = true;
                        if (!fDice)
                        {
                            if (i - f > 0)
                                if (AllOfCol[i - f] != null)
                                    Iff = (AllOfCol[i - f].GetColor() == turn
                                || AllOfCol[i - f].GetColor() != turn && AllOfCol[i - f].GetCountOfThisCol() == 1);
                            Have = Iff;
                        }
                        if (!sDice)
                        {
                            if (i - s > 0)
                                if (AllOfCol[i - s] != null)
                                    Ifs = (AllOfCol[i - s].GetColor() == turn
                                   || AllOfCol[i - s].GetColor() != turn && AllOfCol[i - s].GetCountOfThisCol() == 1);
                            Have = Ifs;
                        }

                        if (!fDice && !sDice)
                            Have = (Iff || Ifs);

                        if (Have)
                            return;
                    }

                    NoPossibleM = !Have; return;
                }
            }
        }


        static void grayM()
        {
            bool con = false;int tempCol = col;
            if (AllOfCol[col] == null)
            {
                if (prev + f == col && !fDice)
                {
                    Move(1, "null");
                    return;
                }
                else if (prev + s == col && !sDice)
                {
                    Move(2, "null"); return;
                }
                else if (prev + s + f == col && !fDice && !sDice)
                {
                    if (AllOfCol[prev + f] != null)
                    {
                        if (AllOfCol[prev + f].GetColor() == "gray" ||
                              (AllOfCol[prev + f].GetColor() == "red" && (AllOfCol[prev + f].GetCountOfThisCol() == 1)))
                            con = true;
                    }
                    else
                        con = true;

                    if (AllOfCol[prev + s] != null)
                    {
                        if (AllOfCol[prev + s].GetColor() == "gray" ||
                             (AllOfCol[prev + s].GetColor() == "red" && (AllOfCol[prev + s].GetCountOfThisCol() == 1)))
                            con = true;
                    }
                    else
                        con = true;
                    if (con)
                    {
                        if (ColGorRorNull(prev + f) != "" && ColGorRorNull(prev + s) != "")
                        {
                            if (AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() == 1 &&
                               AllOfCol[prev + s].GetColor() != turn 
                               || AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() == 1 &&
                               AllOfCol[prev + s].GetColor() != turn)
                            {
                                col = f + prev;
                                EatMRed();
                                col = tempCol;
                            }
                            else if (AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() != 1 &&
                                AllOfCol[prev + s].GetColor() != turn && AllOfCol[prev + s].GetCountOfThisCol() == 1)
                            {
                                col = s + prev;
                                EatMRed();
                                col = tempCol;
                            }
                        }
                            Move(3, "null"); return;
                    }
                }

            }
            else
            {
                if (prev + f == col)
                    if (!fDice && AllOfCol[col].GetColor() == "gray" ||
                        AllOfCol[col].GetColor() == "red" && AllOfCol[col].GetCountOfThisCol() == 1)
                    {
                        if (AllOfCol[col].GetColor() == "red")
                        {
                            EatMRed();
                            Move(1, "null"); return;
                        }
                        Move(1, ""); return;
                    }
                if (prev + s == col)
                    if (!sDice && AllOfCol[col].GetColor() == "gray" ||
                          AllOfCol[col].GetColor() == "red" && AllOfCol[col].GetCountOfThisCol() == 1)
                    {
                        if (AllOfCol[col].GetColor() == "red")
                        {
                            EatMRed();
                            Move(2, "null"); return;
                        }

                        Move(2, ""); return;
                    }

                if (prev + s + f == col)
                    if (!fDice && !sDice && AllOfCol[col].GetColor() == "gray" ||
                            AllOfCol[col].GetColor() == "red" && AllOfCol[col].GetCountOfThisCol() == 1)

                    {
                        if (AllOfCol[prev + f] != null)
                        {
                            if (AllOfCol[prev + f].GetColor() == "gray" ||
                                  (AllOfCol[prev + f].GetColor() == "red" && (AllOfCol[prev + f].GetCountOfThisCol() == 1)))
                                con = true;
                        }
                        else
                            con = true;

                        if (AllOfCol[prev + s] != null)
                        {
                            if (AllOfCol[prev + s].GetColor() == "gray" ||
                                 (AllOfCol[prev + s].GetColor() == "red" && (AllOfCol[prev + s].GetCountOfThisCol() == 1)))
                                con = true;
                        }
                        else
                            con = true;
                        if (con)
                        {
                            if (ColGorRorNull(prev + f) != "" && ColGorRorNull(prev + s) != "")
                            {
                                if (AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() == 1 &&
                                AllOfCol[prev + s].GetColor() != turn 
                                || AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() == 1 &&
                                AllOfCol[prev + s].GetColor() != turn )
                                {
                                    col = f + prev;
                                    EatMRed();
                                    col = tempCol;
                                }
                                else if (AllOfCol[prev + f].GetColor() != turn && AllOfCol[prev + f].GetCountOfThisCol() != 1 &&
                                    AllOfCol[prev + s].GetColor() != turn && AllOfCol[prev + s].GetCountOfThisCol() == 1)
                                {
                                    col = s + prev;
                                    EatMRed();
                                    col = tempCol;
                                }
                            }
                            if (AllOfCol[col].GetColor() == "red")
                            {
                                 tmAnim.Start(); return;

                            }
                            
                                Move(3, ""); return;
                        }
                    }
            }
        }
        static void redM()
        {
            bool con = false; int tempCol = col;
            if (AllOfCol[col] == null)
            {
                if (prev - f == col && !fDice)
                {
                    Move(1, "null"); return;
                }

                else if (prev - s == col && !sDice)
                {
                    Move(2, "null"); return;
                }

                else if (prev - s - f == col && !fDice && !sDice)
                {
                    if (AllOfCol[prev - f] != null)
                    {
                        if (AllOfCol[prev - f].GetColor() == "red" ||
                           (AllOfCol[prev - f].GetColor() == "gray" && (AllOfCol[prev - f].GetCountOfThisCol() == 1)))
                            con = true;
                    }
                    else
                        con = true;
                    if (AllOfCol[prev - s] != null)
                    {
                        if (AllOfCol[prev - s].GetColor() == "red" ||
                            (AllOfCol[prev - s].GetColor() == "gray" && (AllOfCol[prev - s].GetCountOfThisCol() == 1)))
                            con = true;
                    }
                    else
                        con = true;
                    if (con)
                    {
                        
                        if (ColGorRorNull(prev - f) != "" && ColGorRorNull(prev - s) != "")
                        {
                            if (AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() == 1 &&
                               AllOfCol[prev - s].GetColor() != turn 
                               || AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() == 1 &&
                               AllOfCol[prev - s].GetColor() != turn)
                            {
                                col = f - prev;
                                EatMgray();
                                col = tempCol;
                            }
                            else if (AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() != 1 &&
                                AllOfCol[prev - s].GetColor() != turn && AllOfCol[prev - s].GetCountOfThisCol() == 1)
                            {
                                col = s - prev;
                                EatMgray();
                                col = tempCol;
                            }
                        }
                        Move(3, "null"); return;
                    }
                }
            }
            else
            {
                if (prev - f == col)

                    if (!fDice && AllOfCol[col].GetColor() == "red" ||
                    AllOfCol[col].GetColor() == "gray" && AllOfCol[col].GetCountOfThisCol() == 1)
                    {
                        if (AllOfCol[col].GetColor() == "gray")
                        {
                            EatMgray();
                            Move(1, "null"); return;
                        }

                        Move(1, ""); return;
                    }

                if (prev - s == col)

                    if (!sDice && AllOfCol[col].GetColor() == "red" ||
                    AllOfCol[col].GetColor() == "gray" && AllOfCol[col].GetCountOfThisCol() == 1)
                    {
                        if (AllOfCol[col].GetColor() == "gray")
                        {
                            EatMgray();
                            Move(2, "null"); return;
                        }

                        Move(2, ""); return;
                    }


                if (prev - s - f == col)
                    if (!fDice && !sDice && AllOfCol[col].GetColor() == "red" ||
                            AllOfCol[col].GetColor() == "gray" && AllOfCol[col].GetCountOfThisCol() == 1)

                    {
                        if (AllOfCol[prev - f] != null)
                        {
                            if (AllOfCol[prev - f].GetColor() == "red" ||
                               (AllOfCol[prev - f].GetColor() == "gray" && (AllOfCol[prev - f].GetCountOfThisCol() == 1)))
                                con = true;
                        }
                        else
                            con = true;
                        if (AllOfCol[prev - s] != null)
                        {
                            if (AllOfCol[prev - s].GetColor() == "red" ||
                                (AllOfCol[prev - s].GetColor() == "gray" && (AllOfCol[prev - s].GetCountOfThisCol() == 1)))
                                con = true;
                        }
                        else
                            con = true;
                        if (con)
                        {
                            if (ColGorRorNull(prev - f) != "" && ColGorRorNull(prev - s) != "")
                            {
                                if (AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() == 1 &&
                                   AllOfCol[prev - s].GetColor() != turn
                                   || AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() == 1 &&
                                   AllOfCol[prev - s].GetColor() != turn)
                                {
                                    col = prev - f;
                                    EatMgray();
                                    col = tempCol;
                                }
                                else if (AllOfCol[prev - f].GetColor() != turn && AllOfCol[prev - f].GetCountOfThisCol() != 1 &&
                                    AllOfCol[prev - s].GetColor() != turn && AllOfCol[prev - s].GetCountOfThisCol() == 1)
                                {
                                    col = prev - s;
                                    EatMgray();
                                    col = tempCol;
                                }
                            }

                            if (AllOfCol[col].GetColor() == "gray")
                            {
                                tmAnim.Start(); return;
                            }

                            Move(3, ""); return;
                        }
                    }
            }


        }
        static void Move(int x, string s)
        {
            if (s == "null")
            {
                MooveXZ(AllOfCol[prev].GetModel3D(), Xto(col), Zto(col, 0));
                MooveY(AllOfCol[prev].GetModel3D(), 0, 1.5);
                AllOfCol[col] = new Col(AllOfCol[prev].PopModel3D(), col, turn); ifMove = true;
            }
            else
            {
                MooveXZ(AllOfCol[prev].GetModel3D(), Xto(col), Zto(col, AllOfCol[col].GetCountOfThisCol()));
                MooveY(AllOfCol[prev].GetModel3D(), 0, 1.5);
                AllOfCol[col].Add(AllOfCol[prev].PopModel3D()); ifMove = true;
            }

            //  MooveY(md, 0, 1.5);
            if (AllOfCol[prev].GetCountOfThisCol() == 0)
                AllOfCol[prev] = null;

            switch (x)
            {
                case 1:
                    fDice = true; Count--;
                    break;
                case 2:
                    sDice = true; Count--;
                    break;
                default:
                    fDice = true; sDice = true; Count -= 3;
                    break;
            }

            if (isCople && fDice && sDice)
            { isCople = false; fDice = false; sDice = false; }

            if (Count != 0 && countEatG == 0 && countEatR == 0)
                ifHaveSteps();


        }
        static void EatMgray()
        {
            DoubleAnimation yAnim1 = new DoubleAnimation(), xAnim = new DoubleAnimation()
           , zAnim = new DoubleAnimation();

            yAnim1.To = 0;
            yAnim1.Duration = TimeSpan.FromSeconds(.75);
            xAnim.To = 0;
            xAnim.Duration = TimeSpan.FromSeconds(.75);
            zAnim.To = 0;
            zAnim.Duration = TimeSpan.FromSeconds(.75);
            xAnim.Completed += XAnim_Completed;

            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleYProperty, yAnim1);
            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleXProperty, xAnim);
            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleZProperty, zAnim);

            EatGray[countEatG] = AllOfCol[col].PopModel3D();
            countEatG++;
        }
        static void EatMRed()
        {
            DoubleAnimation yAnim = new DoubleAnimation(), xAnim = new DoubleAnimation()
                , zAnim = new DoubleAnimation();

            yAnim.To = 0;
            yAnim.Duration = TimeSpan.FromSeconds(.75);
            xAnim.To = 0;
            xAnim.Duration = TimeSpan.FromSeconds(.75);
            zAnim.To = 0;
            zAnim.Duration = TimeSpan.FromSeconds(.75);
            zAnim.Completed += ZAnim_Completed;

            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleYProperty, yAnim);
            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleXProperty, xAnim);
            ((Transform3DGroup)AllOfCol[col].GetModel3D().Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleZProperty, zAnim);

            EatRed[countEatR] = AllOfCol[col].PopModel3D();
            countEatR++;
        }
        private static void XAnim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation scalAnimY = new DoubleAnimation(), scalAnimXZ = new DoubleAnimation();
            scalAnimY.To = 1.5;
            scalAnimY.Duration = TimeSpan.FromSeconds(.5);
            scalAnimXZ.To = 0.8;
            scalAnimXZ.Duration = TimeSpan.FromSeconds(.5);

            ((Transform3DGroup)EatGray[countEatG - 1].Transform).Children[0] = new TranslateTransform3D(0.025, 0.1, .75 + (countEatG - 1) * 1.5);
            ((Transform3DGroup)EatGray[countEatG - 1].Transform).Children[1] = new ScaleTransform3D(0, 0, 0, 0, 0, 0);

            ((Transform3DGroup)EatGray[countEatG - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleYProperty, scalAnimY);
            ((Transform3DGroup)EatGray[countEatG - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleXProperty, scalAnimXZ);
            ((Transform3DGroup)EatGray[countEatG - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleZProperty, scalAnimXZ);

        }
        private static void ZAnim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation scalAnimY = new DoubleAnimation(), scalAnimXZ = new DoubleAnimation();
            scalAnimY.To = 1.5;
            scalAnimY.Duration = TimeSpan.FromSeconds(.5);
            scalAnimXZ.To = 0.8;
            scalAnimXZ.Duration = TimeSpan.FromSeconds(.5);


            ((Transform3DGroup)EatRed[countEatR - 1].Transform).Children[0] = new TranslateTransform3D(0.025, 0.1, -.75 - (countEatR - 1) * 1.5);
            ((Transform3DGroup)EatRed[countEatR - 1].Transform).Children[1] = new ScaleTransform3D(0, 0, 0, 0, 0, 0);


            ((Transform3DGroup)EatRed[countEatR - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleYProperty, scalAnimY);
            ((Transform3DGroup)EatRed[countEatR - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleXProperty, scalAnimXZ);
            ((Transform3DGroup)EatRed[countEatR - 1].Transform).Children[1].BeginAnimation(ScaleTransform3D.ScaleZProperty, scalAnimXZ);

        }
        static void GrayOut()
        {

            if (col > 0 && col < 7)
            {
                ifMove = false;
                AllOfCol[0] = new Col(EatGray[countEatG - 1], 0, "gray");
                grayM();
                if (ifMove)
                {
                    EatGray[countEatG - 1] = null;
                    countEatG--; NowOEat = true;
                    if (Count != 0)
                        ifHaveSteps();
                }
            }
        }
        static void RedOut()
        {
            if (col > 18 && col < 25)
            {
                ifMove = false;
                prev = 25;
                AllOfCol[25] = new Col(EatRed[countEatR - 1], 25, "red");
                redM();
                if (ifMove)
                {
                    EatRed[countEatR - 1] = null; countEatR--; NowOEat = true;
                    if (Count != 0)
                        ifHaveSteps();
                }
            }

        }

        private void Outside_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool outs = false;
            if (isCople && fDice && sDice)
            { isCople = false; fDice = false; sDice = false; }

            if (isFinish() && Count % 2 != 0)
            {
                if (turn == "red")
                {
                    if (!fDice && !sDice)
                    {
                        if (col == f || col == LastItem() && f > LastItem())
                        {
                            Count--; fDice = true; outs = true;
                        }

                        else if (col == s || col == LastItem() && s > LastItem())
                        {
                            Count--; sDice = true; outs = true;
                        }
                        else if (col == s + f)
                        {
                            Count -= 3; sDice = true; fDice = true; outs = true;
                        }

                    }

                    else if (!fDice)

                    {
                        if (col == f || col == LastItem() && f > LastItem())
                        {
                            Count--; fDice = true; outs = true;
                        }
                    }

                    else if (!sDice)
                        if (col == s || col == LastItem() && s > LastItem())
                        {
                            Count--; sDice = true; outs = true;
                        }

                    if (outs)
                    {
                        MooveXZ(AllOfCol[prev].GetModel3D(), 15, Zto(1, 0));
                        OutR[15 - NCountOutSideR] = AllOfCol[col].PopModel3D();
                        NCountOutSideR--;
                        if (AllOfCol[col].GetCountOfThisCol() == 0)
                            AllOfCol[col] = null;

                        if (isCople && fDice && sDice)
                        { isCople = false; fDice = false; sDice = false; }


                        if (Count == 0)
                        {
                            Count = 4; fDice = false; sDice = false; numbers.Content = ""; f = 0; s = 0;
                            if (turn == "red")
                            { turn = "gray"; curent.Content = "Gray"; curent.Foreground = new SolidColorBrush(Colors.Gray); }
                            else
                            { turn = "red"; curent.Content = "Red"; curent.Foreground = new SolidColorBrush(Colors.Red); }


                            isKeyDown = true;

                        }

                    }
                }

            }
            if (NCountOutSideR == 0)
                WinR();

        }
        private void OutsideG_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool outs = false;
            if (isCople && fDice && sDice)
            { isCople = false; fDice = false; sDice = false; }

            if (isFinish() && Count % 2 != 0)
            {
                if (turn == "gray")
                {
                    if (!fDice && !sDice)
                    {
                        if (col == 25 - f || col == LastItem() && 25 - f < LastItem())
                        {
                            Count--; fDice = true; outs = true;
                        }

                        else if (col == 25 - s || col == LastItem() && 25 - s < LastItem())
                        {
                            Count--; sDice = true; outs = true;
                        }
                        else if (col == 25 - s - f)
                        {
                            Count -= 3; sDice = true; fDice = true; outs = true;
                        }

                    }

                    else if (!fDice)

                    {
                        if (col == 25 - f || col == LastItem() && 25 - f < LastItem())
                        {
                            Count--; fDice = true; outs = true;
                        }
                    }

                    else if (!sDice)
                        if (col == 25 - s || col == LastItem() && 25 - s < LastItem())
                        {
                            Count--; sDice = true; outs = true;
                        }

                    if (outs)
                    {
                        MooveXZ(AllOfCol[prev].GetModel3D(), 25, Zto(24, 0));
                        OutR[15 - NCountOutSideG] = AllOfCol[col].PopModel3D();
                        NCountOutSideG--;
                        if (AllOfCol[col].GetCountOfThisCol() == 0)
                            AllOfCol[col] = null;

                        if (isCople && fDice && sDice)
                        { isCople = false; fDice = false; sDice = false; }

                        if (Count == 0)
                        {
                            Count = 4; fDice = false; sDice = false; numbers.Content = ""; f = 0; s = 0;
                            if (turn == "red")
                            { turn = "gray"; curent.Content = "Gray"; curent.Foreground = new SolidColorBrush(Colors.Gray); }
                            else
                            { turn = "red"; curent.Content = "Red"; curent.Foreground = new SolidColorBrush(Colors.Red); }
                            isKeyDown = true;

                        }

                    }
                }

            }
            if (NCountOutSideG == 0)
                WinG();
        }

        private void OAnim_Completed(object sender, EventArgs e)
        {
            DoubleAnimation oA = new DoubleAnimation();
            oA.To = 0;
            oA.BeginTime = TimeSpan.FromSeconds(5);
            oA.Duration = TimeSpan.FromSeconds(4);
            oA.Completed += OA_Completed;
            Outside.BeginAnimation(OpacityProperty, oA);
            OutsideG.BeginAnimation(OpacityProperty, oA);
        }

        private void OA_Completed(object sender, EventArgs e)
        {
            OutsideG.Content = "Gray:\nPress in\nthis area\nwhen finish";
            Outside.Content = "Red:\nPress in\nthis area\nwhen finish";
            DoubleAnimation oAnim = new DoubleAnimation();
            oAnim.Completed += OAnim_Completed1;
            oAnim.To = 1;
            oAnim.BeginTime = TimeSpan.FromSeconds(3);
            oAnim.Duration = TimeSpan.FromSeconds(4);
            Outside.BeginAnimation(OpacityProperty, oAnim);
            OutsideG.BeginAnimation(OpacityProperty, oAnim);
        }

        private void OAnim_Completed1(object sender, EventArgs e)
        {
            DoubleAnimation oA = new DoubleAnimation();
            oA.To = 0;
            oA.BeginTime = TimeSpan.FromSeconds(3);
            oA.Duration = TimeSpan.FromSeconds(4);
            OutsideG.BeginAnimation(OpacityProperty, oA);
            Outside.BeginAnimation(OpacityProperty, oA);
        }

        static string ColGorRorNull(int k )
        {
            string retSt = "";

            if (AllOfCol[k] != null)
                retSt = (AllOfCol[k].GetColor() == "gray") ? "G": "R" ;

            return retSt;
        }
        static double Zto(int col, int countofshes)
        {
            double zto;
            if (col > 12)
                zto = -7.1 + countofshes % 5;
            else
                zto = 7.1 - countofshes % 5;
            return zto;
        }
        static double Xto(int col)
        {
            double xto = 0;
            if (col <= 6)
                xto = 7.97 - col * 1.12;
            else if (col >= 19)
                xto = (col - 18) * 1.12 + 0.13;
            else if (col > 6 && col < 13)
                xto = ((col - 6) * 1.12 + 0.13) * -1;
            else
                xto = (7.97 - (col - 12) * 1.12) * -1;
            return xto;
        }
        static int WhishCol(RayMeshGeometry3DHitTestResult MeshHitResult)
        {
            double Col = 0;

            if (MeshHitResult.PointHit.X > 0.5 || MeshHitResult.PointHit.X < -0.5)
            {
                if (MeshHitResult.PointHit.X < 0)
                {
                    if (MeshHitResult.PointHit.Z < 0)
                        Col = 12 + Math.Floor(Math.Abs((6.9 + MeshHitResult.PointHit.X) / 0.9));

                    else
                        Col = 6 + Math.Floor(1 + Math.Abs((0.62 + MeshHitResult.PointHit.X) / 0.9));

                }
                else
                {
                    if (MeshHitResult.PointHit.Z < 0)
                        Col = 18 + Math.Floor(1 + (MeshHitResult.PointHit.X - 0.62) / 0.9);

                    else
                        Col = Math.Floor((6.9 - MeshHitResult.PointHit.X) / 0.9);

                }
            }
            return (int)Col;
        }
      
        static void WinR()
        {
            MessageBox.Show("Red win", "Winer!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            MooveXZ(RedPlayer[0], -6.85-.5+2, -2.1);
            MooveXZ(RedPlayer[1], -6.85+2, -1.1);
            MooveXZ(RedPlayer[2], -6.85+.5+2, -0.1);
            MooveXZ(RedPlayer[3], -6.85 + 1.5-.33+2, -1.1);
            MooveXZ(RedPlayer[4], -6.85 + 2+2, -0.1);
            MooveXZ(RedPlayer[5], -6.85 + 2.5+2, -1.1);
            MooveXZ(RedPlayer[6], -6.85 + 3+2, -2.1);
            MooveXZ(RedPlayer[7], -6.85 + 5+2, -0.1);
            MooveXZ(RedPlayer[8], -6.85 + 5+2, -1.1);
            MooveXZ(RedPlayer[9], -6.85 + 5+2, -2.1);
            MooveXZ(RedPlayer[10], -6.85 + 5+2, -3.5);
            MooveXZ(RedPlayer[11], -6.85 + 7+2, -0.1);
            MooveXZ(RedPlayer[12], -6.85 + 7+2, -1.1);
            MooveXZ(RedPlayer[13], -6.85 + 7+2, -2.1);
            MooveXZ(RedPlayer[14], -6.85 + 8+2, -2.1);
            MooveXZ(RedPlayer[15], -6.85 + 9+2, -2.1);
            MooveXZ(RedPlayer[16], -6.85 + 9+2, -1.1);
            MooveXZ(RedPlayer[17], -6.85 + 9+2, -0.1);

            for (int i = 0; i < 15; i++)
                MooveY(GrayPlayer[i], -0.3, 0);

            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.To = -60;
            yAnim.Duration = TimeSpan.FromSeconds(5);

            ((Transform3DGroup)D1.GetModel().Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetYProperty, yAnim);
            ((Transform3DGroup)D2.GetModel().Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetYProperty, yAnim);




        }
        static void WinG()
        {
            MessageBox.Show("Gray win", "Winer!!!", MessageBoxButton.OK, MessageBoxImage.Information);
            MooveXZ(GrayPlayer[0], -6.85 - .5 + 2, -2.1);
            MooveXZ(GrayPlayer[1], -6.85 + 2, -1.1);
            MooveXZ(GrayPlayer[2], -6.85 + .5 + 2, -0.1);
            MooveXZ(GrayPlayer[3], -6.85 + 1.5 - .33 + 2, -1.1);
            MooveXZ(GrayPlayer[4], -6.85 + 2 + 2, -0.1);
            MooveXZ(GrayPlayer[5], -6.85 + 2.5 + 2, -1.1);
            MooveXZ(GrayPlayer[6], -6.85 + 3 + 2, -2.1);
            MooveXZ(GrayPlayer[7], -6.85 + 5 + 2, -0.1);
            MooveXZ(GrayPlayer[8], -6.85 + 5 + 2, -1.1);
            MooveXZ(GrayPlayer[9], -6.85 + 5 + 2, -2.1);
            MooveXZ(GrayPlayer[10], -6.85 + 5 + 2, -3.5);
            MooveXZ(GrayPlayer[11], -6.85 + 7 + 2, -0.1);
            MooveXZ(GrayPlayer[12], -6.85 + 7 + 2, -1.1);
            MooveXZ(GrayPlayer[13], -6.85 + 7 + 2, -2.1);
            MooveXZ(GrayPlayer[14], -6.85 + 8 + 2, -2.1);
            MooveXZ(GrayPlayer[15], -6.85 + 9 + 2, -2.1);
            MooveXZ(GrayPlayer[16], -6.85 + 9 + 2, -1.1);
            MooveXZ(GrayPlayer[17], -6.85 + 9 + 2, -0.1);

            for (int i = 0; i < 15; i++)
                MooveY(RedPlayer[i], -0.3, 0);
            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.To = -60;
            yAnim.Duration = TimeSpan.FromSeconds(5);

            ((Transform3DGroup)D1.GetModel().Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetYProperty, yAnim);
            ((Transform3DGroup)D2.GetModel().Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetYProperty, yAnim);

        }
        static void MooveY(Model3D model, double to, double bt)
        {

            DoubleAnimation yAnim = new DoubleAnimation();
            yAnim.To = to;
            yAnim.Duration = TimeSpan.FromSeconds(1);
            yAnim.BeginTime = TimeSpan.FromSeconds(bt);

            ((Transform3DGroup)model.Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetYProperty, yAnim);
        }
        static void MooveXZ(Model3D model, double Xto, double Zto)
        {

            DoubleAnimation xAnim = new DoubleAnimation();
            DoubleAnimation zAnim = new DoubleAnimation();

            xAnim.To = Xto;
            xAnim.Duration = TimeSpan.FromSeconds(1.5);

            zAnim.To = Zto;
            zAnim.Duration = TimeSpan.FromSeconds(1.5);

            ((Transform3DGroup)model.Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetXProperty, xAnim);

            ((Transform3DGroup)model.Transform).Children[0].BeginAnimation(TranslateTransform3D.OffsetZProperty, zAnim);

        }
        static int roolKube(Kube k)
        {
            int XYZ;
            int Degree;
            string si;
            beginTime = 0;

            for (int i = 0; i < 5; i++, beginTime++)
            {
                XYZ = reg.Next(3);
                Degree = reg.Next(2);
                si = (Degree == 0) ? "+" : "-";
                switch (XYZ)
                {
                    case 0:
                        k.RotationX(si, beginTime * .25);
                        break;
                    case 1:
                        k.RotationY(si, beginTime * .25);
                        break;
                    case 2:
                        k.RotationZ(si, beginTime * .25);
                        break;
                }
            }
            return k.GetTop();
        }
        static int LastItem()
        {
            int i;
            if (turn == "red")
            {
                i = 6;
                while (ColGorRorNull(i) != "R")
                    i--;
            }
            else
            {
                i = 19;
                while (ColGorRorNull(i) != "G")
                    i++;
            }

            return i;
        }
        static int CountOfThisCol(int i, string s)
        {
            int temp = i, retval = 0;
            bool isFlag = true;
            if (s == "G")
            {
                while (isFlag && i != 0)
                {
                    isFlag = (colArG[i - 1] == colArG[i]);
                    if (isFlag)
                        i--;
                }

                while (i != temp)
                { i++; retval++; }

            }

            else
            {
                while (isFlag && i != 0)
                {
                    isFlag = (colArR[i - 1] == colArR[i]);
                    if (isFlag)
                        i--;
                }

                while (i != temp)
                { i++; retval++; }
            }

            return retval;
        }
        static bool isFinish()
        {
            int count = 0;
            if (turn == "red")
            {
                for (int i = 0; i <= 6; i++)
                    if (AllOfCol[i] != null)
                    {
                        if (AllOfCol[i].GetColor() == turn)
                            count += AllOfCol[i].GetCountOfThisCol();
                    }

                return count == NCountOutSideR;
            }
            else
            {
                for (int i = 19; i <= 24; i++)
                    if (AllOfCol[i] != null)
                    {
                        if (AllOfCol[i].GetColor() == turn)
                            count += AllOfCol[i].GetCountOfThisCol();
                    }

                return count == NCountOutSideG;
            }
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Growing.Grow_Up((TextBlock)sender);
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Growing.Grow_Down((TextBlock)sender);
        }
        private void OnBackMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
