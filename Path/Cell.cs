
namespace Labirint
{
    class Cell
    {
        public int Step { get; set; } = 0; // Число шагов от старта до текущей клетки
        public CellType CellType { get; set; } = CellType.Empty; 
        public int X { get; }  //Координаты клетки в лабиринте
        public int Y { get; }
         

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        
    }
}
