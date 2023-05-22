

using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace SnakeGame
{
    
    // Class for the direction movements of our snake
    public class Direction
    {
        //Directions our snake will be moving
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1,0);
        public readonly static Direction Down = new Direction(1,0);

        //an integer that represents the offset in the row direction for the current direction.
        public int RowOffset { get; }

        //an integer that represents the offset in the column direction for the current direction
        public int ColOffset { get; }
        //Constructor
        private Direction (int rowOffset,int colOffset)
        {
            RowOffset = rowOffset;
            ColOffset = colOffset;
        }
        /*A method that returns a new Direction object representing the opposite direction 
         to the current direction, this is done by negating the RowOffset and ColOffset properties
         of the current object.*/
        public Direction Opposite()
        {
            return new Direction(-RowOffset,-ColOffset);  

        }
        /*an overridden method that takes an object as an argument and returns a boolean 
         indicating whether the object is equal to the current Direction object. 
         Two Direction objects are considered equal if they have the same RowOffset and ColOffset properties*/
        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }

        /*an overridden method that returns a hash code for the current Direction object
        /based on its RowOffset and ColOffset properties.*/
        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColOffset);
        }

        /*an overloaded operator that takes two Direction objects as arguments and returns
         a boolean indicating whether they are equal. 
         This operator uses the EqualityComparer class to compare the two objects.*/
        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }
        /*an overloaded operator that takes two Direction objects as arguments and returns 
         a boolean indicating whether they are not equal. 
          This operator simply negates the result of the operator == method*/
        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}
