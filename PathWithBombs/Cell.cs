
namespace Labirint
{
    class Cell
    {
        public int Step { get; set; } = 0;
        public CellType CellType { get; set; } = CellType.Empty;
        public int X { get; }
        public int Y { get; }
        public int CountOfBomb { get; }
        public Cell()
        {
                
        }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Cell(int x, int y, int countOfBomb)
        {
            X = x;
            Y = y;
            CountOfBomb = countOfBomb;
        }
        
    }
}
