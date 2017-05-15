namespace SquareFillXamarin.Builders
{
    public interface ISquareView
    {
        void MoveSquare(int newX, int newY);
        SquareFillPoint Centre();
    }
}