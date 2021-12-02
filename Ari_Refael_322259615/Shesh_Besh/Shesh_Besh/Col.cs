using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace Shesh_Besh
{
    class Col
    {
            int CountOfThisCol = 0; // מספר הכלים בעמודה
            int NumOfThisCol;   // (1-24)  מספר העמודה 
            string Color;

            Stack<Model3D> StObjects = new Stack<Model3D>();  // כל הכלים בעמודה


            public Col(Model3D model, int numOfThiscol, string color)
            {
                CountOfThisCol++;
                this.NumOfThisCol = numOfThiscol;
                StObjects.Push(model);
                this.Color = color;
            }

            public void Add(Model3D model)
            {
                StObjects.Push(model);
                CountOfThisCol++;
            }

            public Model3D GetModel3D() { return StObjects.Peek(); }
            public string GetColor() { return this.Color; }
            public int GetCol() { return this.NumOfThisCol; }
            public int GetCountOfThisCol() { return this.CountOfThisCol; }
            public Model3D PopModel3D() { this.CountOfThisCol--; return StObjects.Pop(); }
        }
    }

