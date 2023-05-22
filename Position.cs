
using System;
using System.Collections.Generic;

namespace SnakeGame
{
    //Class for the position of the snake in the grid
    public class Position
    {
        public int Row { get; }
        public int Col { get; }
        //Constructor  that take row and col
        public Position(int row,int col)
        {
            Row = row;
            Col = col;
        }
        /*The Translate method takes a Direction object and returns a new Position object
        that is translated by the row and column offsets specified in the Direction object.*/
        public Position Translate (Direction dir)
        {
            return new Position(Row + dir.RowOffset,Col + dir.ColOffset);

        }
        /*The Equals method is used to compare Position objects for equality. 
         * It returns true if the Row and Col properties of both objects are equal. */
        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }
        /*The GetHashCode method is overridden to provide a hash code for the Position object
        based on its Row and Col properties.*/
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }
        /*The == operator is used to compare Position objects for equality.
        It returns true if the Row and Col properties of both objects are equal.*/
        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }
        /*The != operator is used to compare Position objects for inequality.
        It returns true if the Row and Col properties of both objects are not equal*/
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
