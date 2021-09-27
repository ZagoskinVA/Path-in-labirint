using System;
using System.Collections.Generic;
using System.IO;
namespace Labirint
{
    class Labirint
    {
        private Queue<Cell> queue; 
        private Cell[,] field;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Labirint(string path)
        {
            queue = new Queue<Cell>();
            LoadField(path);
        }
        public void GenerateWall(int count) 
        {
            var rnd = new Random();
            if (count < 0)
                throw new ArgumentException();  
            for (int i = 0; i < count; i++)
            {
                var x = rnd.Next(0,Width-1);
                var y = rnd.Next(0, Height-1);
                field[x, y].CellType = CellType.Wall;
            }
        }

        public int SearchPath() // Поиск кратчайшего пути с помощью поиска в ширину
        {
            
            var cell = queue.Dequeue();
            while (cell.CellType != CellType.Finish)
            {
                var x = cell.X;
                var y = cell.Y;
                var step = cell.Step;
                AddCell(x+1,y,step);
                AddCell(x, y + 1, step);  // Проверка всех клеток связанной с текущей
                AddCell(x - 1, y, step);   
                AddCell(x, y - 1, step);
                if (queue.Count == 0)
                    return -1;            // Не существует пути из старта в финиш
                cell = queue.Dequeue();
            }
            BuildPath(cell); 
            return cell.Step; // Выводим кол-во шагов до финиша

        }

        private void AddCell(int i, int j , int step)
        {
            if (i >= Width || j >= Height || i < 0 || j < 0 
                || field[i,j].CellType == CellType.Start   // Проверка условий на выход за границы 
                || field[i,j].CellType == CellType.Wall)   
            {
                return;
            }
            if (field[i, j].Step == 0) // Если клетка не помечена добавляем в очередь
            {
                field[i, j].Step = step + 1;
                queue.Enqueue(field[i,j]);
            }
                
        }
        private void BuildPath(Cell finalCell)
        {
            var cell = finalCell;
            while (cell.CellType != CellType.Start)
            {
                cell = MarkCell(cell);
            }
            
        }
        private Cell MarkCell(Cell cell) // Помечаем клетки в кратчайшем пути
        {
            var x = cell.X;
            var y = cell.Y;
            var step = cell.Step;
            bool isNextCell;
            isNextCell = IsNextCell(x + 1, y, step);
            if (isNextCell)
                return field[x + 1, y];
            isNextCell = IsNextCell(x, y + 1, step);
            if (isNextCell)
                return field[x, y + 1];
            isNextCell = IsNextCell(x - 1, y, step);
            if (isNextCell)
                return field[x - 1, y];
            isNextCell = IsNextCell(x, y - 1, step);
            return field[x, y - 1];
        }
        private bool IsNextCell(int i, int j, int step)
        {

            if (i >= Width || j >= Height || i < 0 || j < 0)
            {
                return false;
            }
            var cell = field[i, j];
            if (cell.CellType == CellType.Wall)
                return false;
            if (cell.CellType == CellType.Start)
                return true;
            if (cell.Step == step - 1)
            {
                cell.CellType = CellType.Path;
                return true;
            }
            return false;           
        }
        private void LoadField(string path) // Загрузка лабиринта из txt файла
        {
            string labirint;
            using (var sr = new StreamReader(path))
            {
                var str = sr.ReadLine().Split(' ');
                Height = int.Parse(str[0]);
                Width = int.Parse(str[1]);
                for (int i = 0; i < Height; i++) 
                {
                    str = sr.ReadLine();
                    for (int j = 0; j < Width; j++) 
                    {
                        field[i, j] = new Cell(i, j);
                        switch (str[j]) 
                        {
                            case '-':
                                field[i, j].CellType = CellType.Empty;
                                break;
                            case 'x':
                                field[i, j].CellType = CellType.Wall;
                                break;
                        }
                    }
                }
                str = sr.ReadLine().Split(' ');
                field[int.Parse(str[0]), int.Parse(str[1])].CellType = CellType.Start;
                queue.Enqueue(field[int.Parse(str[0]), int.Parse(str[1])]);
                str = sr.ReadLine().Split(' ');
                field[int.Parse(str[0]), int.Parse(str[1])].CellType = CellType.Finish;
            }
        }

    }
}
