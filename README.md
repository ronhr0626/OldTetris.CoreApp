# OldTetris

Tetris built in C# with Windows Forms (.NET 10).
Kodehode – C# Fordypning, Case 1.

![Architecture](docs/OldTetris-architecture.png)

## Features

- The 7 classic pieces (I, O, T, S, Z, J, L) with rotation
- Clearing full rows
- Score, lines and levels (faster every 10 lines)
- Next piece preview
- Pause and restart
- Top 10 high score list with names, saved to file

## How to play

| Key | Action |
|---|---|
| ← / A | Move left |
| → / D | Move right |
| ↓ / S | Move down |
| ↑ / W | Rotate |
| Space | Drop |
| P | Pause |
| R | Restart |

## Scoring

| Lines cleared at once | Points |
|---|---|
| 1 | 100 |
| 2 | 300 |
| 3 | 500 |
| 4 (Tetris) | 800 |

## Project structure

```
OldTetris.CoreApp
├── OldTetris              Windows Forms App (.exe) – window, drawing, keyboard
│   ├── Form1.cs           the conductor: timer, keys, connects everything
│   ├── Rendrer.cs         draws the board, score, next piece and top 10
│   ├── NameDialog.cs      popup that asks for the player's name
│   └── Program.cs         starts the app
│
├── OldTetris.Core         Class Library (.dll) – pure game logic, no WinForms
│   ├── TetrisGame.cs      game rules: falling piece, moves, pause, game over
│   ├── Board.cs           the grid, collisions, clearing full rows
│   ├── Tetromino.cs       the 7 pieces and rotation
│   ├── ScoreManager.cs    points, lines and level
│   └── HighScore.cs       top 10 list, saved to file
│
└── docs                   architecture diagram (.png + .excalidraw)
```

The game logic lives in its own Class Library, so it can be reused in other front-ends (WPF, Avalonia, console, Web API) without changes.

## How to run

```
dotnet run --project OldTetris
```

Or open `OldTetris.CoreApp.slnx` in Visual Studio, set **OldTetris** as startup project and press **F5**.