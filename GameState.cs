

using System;
using System.Collections.Generic;

namespace SnakeGame
{
    //Defines the current state of the game
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool Gameover { get; private set; }

        //Variable for the buffer that temporaliy hold the directions
        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();

        //Linked list to store all positions occupied by the snake
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        //Random object to place the food in the grid randomly
        private readonly Random random = new Random();

        // Constructor to intialise our fields
        public GameState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right; //First direction when game starts
            AddSnake(); //adding snake
            AddFood(); //adding Food
        }
        //Method to add the snake in our grid
        private void AddSnake()
        {
            //Snake will start in center of rows taking 3 positions 
            int startPos = Rows / 2;
            for(int i = 1; i <= 3; i++)
            {
                Grid[startPos, i] = GridValue.Snake; //Giving the snake the 3pos
                //Adding the  occupied snake positions to our list
                snakePositions.AddFirst(new Position(startPos,i));

            }

        }
        //Method to get/return all empty positions in the grid
        private IEnumerable<Position> EmptyPositions()
        {
            //Loop through the whole grid
            for(int i=0; i<Rows; i++)
            {
                for(int j=0; j<Cols; j++)
                {
                    //Checking if the grid is empty
                    if (Grid[i,j] == GridValue.Empty)
                    {
                        //returning the empty grid position
                        yield return new Position(i,j);
                    }
                }
            }

        }

        // Method to add food in the game
        private void AddFood()
        {
            //List to have all our empty positions
            List<Position> emptyPos = new List<Position>(EmptyPositions());
            //Game end if no more empty positions
            if(emptyPos.Count == 0)
            {
                return;
            }
            //Food placing in the grid randomyly
            Position pos = emptyPos[random.Next(emptyPos.Count)];
            Grid[pos.Row,pos.Col]=GridValue.Food;
        }
        // Method to return postion of the snake head
        public Position HeadPosition()
        {
            return snakePositions.First.Value; //First value in our linkedList is the head position
        }
        //Method to return position of the snake Tail
        public Position TailPosition()
        {
            return snakePositions.Last.Value;  //Last value in our linkedList is our tail position
        }
        //Method that returns all snake positions
        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        //Methods to modify the snake
        
        //Method to add the head
        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos); //add it the beginning
            Grid[pos.Row, pos.Col] =GridValue.Snake;

        }
        //Method to remove the tail
        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }
        //Method to get our last pre-determined snake direction
        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0)
            {
                return Dir;
            }
            return dirChanges.Last.Value;
        }
        //Method to determine if our snake can change direction
        //Returns true if our direction can be added to the buffer
        private bool CanChangeDirection(Direction newDir)
        {
            //if we 2 directions stored in the buffer
            if (dirChanges.Count == 2)
            {
                return false;
            }
           //if there is space we get the last predetermined position
           //Return true if the last direction is different from the new one and not opposites 
           Direction lastDir = GetLastDirection();
            return newDir !=lastDir && newDir != lastDir.Opposite();

        }
        //Methods to modify the game state.
        //1.Method to change snake direction
        public void ChangeDirection (Direction dir)
        {
            //if direction can change
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }
        //2.Method to move the snake
        //check if snake is outside the grid
        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0||pos.Row >= Rows || pos.Col<0||pos.Col>= Cols;
        }
        //Method to idetenfify what the snake will hit
        private GridValue WillHit (Position newHeadPos)
        {
            //if our snake goes outside  the grid
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }
            //If new head position is the same as tail position
            //We move the tail so the position becomes empty and the head takes it
            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }
            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        //Move method to move the snake one step in the current direction
        public void Move()
        {
            //Checking if there is a direction change in the buffer
            if(dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst(); //we remove it from the buffer
            }

            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);
            //if snake hits the outside game ends
            if(hit == GridValue.Outside || hit == GridValue.Snake)
            {
                Gameover =true;
            }
            //If snake hits empty position we remove tail and add head to the new position
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            //if snake hits food
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++; //we increment the score
                AddFood(); //add food to another position
            }
        }
    }
}
