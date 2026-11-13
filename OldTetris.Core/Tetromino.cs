using System;
using System.Collections.Generic;
using System.Text;

namespace OldTetris.Core
{
    // Defines the different Tetris pieces (shape, color and rotation)
    public class Tetromino 
    {
        public int[,] Shape { get; }
        public int Color { get; }   // 1-7, used as index in the color list

        public int Rows => Shape.GetLength(0);
        public int Cols => Shape.GetLength(1);

        private Tetromino(int[,] shape, int color)
        {
            Shape = shape;
            Color = color;
        }

        // The 7 Tetris pieces. Each shape is [row, col], 1 = filled
        // https://harddrop.com/wiki/Tetromino
        private static readonly int[][,] shapes =
        {
            // (I:) straight line
            // ■ ■ ■ ■
            new int[,] { { 1, 1, 1, 1 } },

            // (O:) square
            // ■ ■
            // ■ ■
            new int[,] { { 1, 1 }, { 1, 1 } },

            // (T:) T shape
            // · ■ ·
            // ■ ■ ■
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },

            // (S:) S shape
            // · ■ ■
            // ■ ■ ·
            new int[,] { { 0, 1, 1 }, { 1, 1, 0 } },

            // (Z:) Z shape, mirror of S
            // ■ ■ ·
            // · ■ ■
            new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },

            // (J:) J shape
            // ■ · ·
            // ■ ■ ■
            new int[,] { { 1, 0, 0 }, { 1, 1, 1 } },

            // (L:) L shape, mirror of J
            // · · ■
            // ■ ■ ■
            new int[,] { { 0, 0, 1 }, { 1, 1, 1 } },
        };

        // Creates a random piece
        public static Tetromino CreateRandom()
        {
            int index = Random.Shared.Next(shapes.Length);
            return new Tetromino(shapes[index], index + 1);
        }

        // Returns a new piece rotated 90 degrees clockwise
        public Tetromino Rotated()
        {
            int[,] rotated = new int[Cols, Rows];

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    rotated[c, Rows - 1 - r] = Shape[r, c];

            return new Tetromino(rotated, Color);
        }

        public bool IsFilled(int r, int c) => Shape[r, c] != 0;
    }
}