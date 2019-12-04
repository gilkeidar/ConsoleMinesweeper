# Console Minesweeper
Console Minesweeper (Text-based) game written in C#.

## To run the game

Go to this path: bin\Release and run the Minesweeper.exe file.

## Rules

Open a square by writing its coordinate similar to chess notation (letter + number) (e.g. b3). In the square will either be nothing (no bomb and no numbers), a number, or a bomb. If there's a number, it means that of the surrounding squares there are exactly that many squares that contains bombs (the number is from 1-8, and if there isn't a number, it means it's a 0 and thus none of the surrounding squares have bombs in them). 

To mark a square as containing a bomb, write "s(coordinate)" (e.g. sb3)
