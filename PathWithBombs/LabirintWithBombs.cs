
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Labirint
{
    class LabirintWithBombs
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Cell[,,] field;
        private List<Cell> list1 = new List<Cell>();
        private List<Cell> list2 = new List<Cell>();
        private int countOfBombs;
        private Cell StartCell;
        private Cell FinishCell;
        private Cell[,] fieldForPath;
        bool isStartCell = false;

        public LabirintWithBombs(string path, int countOfBomb)
        {
            countOfBombs = countOfBomb + 1;
            LoadField(path);
        }

        private void LoadField(string path)
        {
            string str;
            using (var sr = new StreamReader(path))
            {
                str = sr.ReadToEnd();
            }
            var split = str.Split('\n');
            field = new Cell[split.Length, split[0].Length,countOfBombs];
            Width = split.Length;
            Height = split[0].Length - 1;
            
            for (int i = 0; i < split.Length; i++)
            {
                for (int j = 0; j < split[i].Length; j++)
                {
                    for (int k = 0; k < countOfBombs; k++)
                    {
                        field[i, j,k] = new Cell(i, j);
                        switch (split[i][j])
                        {
                            case '.':
                                field[i, j,k].CellType = CellType.Empty;
                                break;
                            case '@':
                                field[i, j,k].CellType = CellType.Start;
                                StartCell = new Cell(i, j);
                                break;
                            case '#':
                                field[i, j,k].CellType = CellType.Wall;
                                break;
                            case 'F':
                                field[i, j,k].CellType = CellType.Finish;
                                FinishCell = new Cell(i,j);
                                break;
                        }
                    }
                }
            }
            list1.Add(field[StartCell.X, StartCell.Y, 0]);
        }

        public LabirintWithBombs(int width, int height, int countOfBomb)
        {
            Width = width;
            Height = height;
            countOfBombs = countOfBomb;
            field = new Cell[Width, Height, countOfBombs];
        }
        private void FillField()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    for (int k = 0; k < countOfBombs; k++)
                    {
                        field[i, j, k] = new Cell(i, j, k);
                        field[0, 0, 0].CellType = CellType.Start;
                        list1.Add(field[0, 0, 0]);
                    }
                }
            }

        }

        public int FindPath()
        {
            for (int i = 0; i < countOfBombs; i++)
            {
                while (list1.Count != 0)
                {
                  

                    var cell = list1.First(b => b.Step == list1.Min(a => a.Step));
                    list1.Remove(cell);
                    var x = cell.X;
                    var y = cell.Y;
                    var step = cell.Step;
                    var useBomb = i;
                    AddCell(x + 1, y, useBomb, step);
                    AddCell(x - 1, y, useBomb, step);
                    AddCell(x, y + 1, useBomb, step);
                    AddCell(x, y - 1, useBomb, step);
                }
                list1.AddRange(list2);
            }
            var min = int.MaxValue;
            Cell minCell = null;
            int count = 0;
            for (int i = 0; i < countOfBombs; i++)
            {
                if (field[FinishCell.X, FinishCell.Y, i].Step < min
                    && field[Width - 1, Height - 1, i].Step != 0)
                {
                    min = field[Width - 1, Height - 1, i].Step;
                    minCell = field[Width - 1, Height - 1, i];
                    count = i;

                }
            }
            
            return min;
        }

        private void BuildPath( Cell minCell,int count)
        {
            
            var x = minCell.X;
            var y = minCell.Y;
            var bomb = count;
            fieldForPath = new Cell[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    fieldForPath[i,j] = new Cell();
                    fieldForPath[i, j] = field[i, j, bomb];
                }
            }
            var cell = minCell;
            Show();
            while (!isStartCell)
            {
                
                x = cell.X;
                y = cell.Y;
                var list = new List<Cell>();
                cell = FindNextCell(x+ 1, y);
                if (isStartCell)
                    return;
                if (cell != null)
                    list.Add(cell);

                cell = FindNextCell(x, y + 1);
                if (isStartCell)
                    return;
                if (cell != null)
                    list.Add(cell);
                cell = FindNextCell(x - 1, y);
                if (isStartCell)
                    return;
                if (cell != null)
                    list.Add(cell);
                cell = FindNextCell(x, y - 1);
                if (isStartCell)
                    return;
                if (cell != null)
                    list.Add(cell);
                cell = list.First(b => b.Step == list.Min(a => a.Step));
                fieldForPath[cell.X, cell.Y].CellType = CellType.Path;
            }
            

        }
        private Cell FindNextCell(int i, int j)
        {
            if (i >= Width || j >= Height || i < 0 || j < 0 )
            {
                return null;
            }
            if (fieldForPath[i, j].CellType == CellType.Path)
                return null;
            if (fieldForPath[i, j].CellType == CellType.Start)
            {
                isStartCell = true;
                return fieldForPath[i, j];
            }
            return fieldForPath[i, j];
        }
        private void AddCell(int i, int j, int useBomb,int step)
        {
            if (i >= Width || j >= Height || i < 0 || j < 0  
                || field[i, j, useBomb].CellType == CellType.Start)
            {
                return;
            }
            if (useBomb  < countOfBombs -1 && field[i, j, useBomb].CellType == CellType.Wall )
            {
                if (field[i, j, useBomb + 1].Step <= 0)
                {
                    field[i, j, useBomb + 1].CellType = CellType.Empty;
                    field[i, j, useBomb + 1].Step = step + 1;
                    list2.Add(field[i, j, useBomb + 1]);
                }
                
            }
            else
            {
                if (field[i, j, useBomb].Step <= 0)
                {
                    field[i, j, useBomb].Step = step + 1;
                    list1.Add(field[i, j, useBomb]);
                }
            }


        }
        public void Show()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    //Console.Write(field[i,j,2].Step+" ");
                    //if (fieldForPath[i, j].CellType == CellType.Empty &&
                    //    field[i, j, 0].CellType == CellType.Wall)
                    //    fieldForPath[i, j].CellType = CellType.Wall;
                    switch (field[i, j,0].CellType)
                    {
                        case CellType.Empty:
                            Console.Write(".");
                            break;
                        case CellType.Start:
                            Console.Write("@");
                            break;
                        case CellType.Wall:
                            Console.Write("#");
                            break;
                        case CellType.Finish:
                            Console.Write("F");
                            break;
                        case CellType.Path:
                            Console.Write("/");
                            break;
                    }
                }
                Console.WriteLine();
            }

        }
    }
}
