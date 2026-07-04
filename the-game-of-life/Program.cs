using System.Numerics;
using Raylib_cs;

namespace the_game_of_life
{
    internal class Program
    {
        static int height = 600;
        static int width = 800;
        static int cellSize = 40;
        static double sleepTimer = 0;
        static int[,] map = new int[(int)(width/cellSize), (int)(height/cellSize)];
        static bool started = false;
        static async Task Main(string[] args)
        {
            Raylib.InitWindow(width, height, "The game of life");
            Raylib.SetTargetFPS(60);

            double timer = 0;

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                for(int i = cellSize; i < width; i += cellSize)
                {
                    Raylib.DrawLine(i, 0, i, height, Color.Black);
                }
                for(int i = cellSize; i < height; i += cellSize)
                {
                    Raylib.DrawLine(0, i, width, i, Color.Black);
                }

                DrawMap(); 
                if(started){
                    if(sleepTimer > 0)
                    {
                        sleepTimer -= Raylib.GetFrameTime();
                    }
                    else
                    {
                        SimulationFrame(); 
                        sleepTimer = 0.5;
                    }
                }

                Vector2 mousePos = Raylib.GetMousePosition();
                Raylib.DrawRectangle((int)(mousePos.X/cellSize)*cellSize, (int)(mousePos.Y/cellSize)*cellSize, cellSize, cellSize, Color.Gray);

                if (!started)
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.Left))
                    {
                        map[(int)(mousePos.X / cellSize), (int)(mousePos.Y / cellSize)] = 1;
                    }else if (Raylib.IsMouseButtonDown(MouseButton.Right))
                    {
                        map[(int)(mousePos.X / cellSize), (int)(mousePos.Y / cellSize)] = 0;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.Space))
                    {
                        int[,] temp = new int[(int)(width/cellSize), (int)(height/cellSize)];
                        map = temp;
                    }
                    if(Raylib.IsKeyDown(KeyboardKey.S))
                    { 
                        started = true;
                        timer = 2;
                    }
                }
                if(timer > 0)
                {
                    ShowText("Simulation started", 40);
                    timer -= Raylib.GetFrameTime();
                }
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        static void ShowText(string text, int fontSize)
        {
            int textLength = Raylib.MeasureText(text, fontSize);
            Raylib.DrawText(text, (width - textLength)/2, (height - fontSize)/2, fontSize, Color.Red);
        }
        static void DrawMap()
        {
            for(int i = 0; i < map.GetLength(0); i++)
                {
                    for(int y = 0; y < map.GetLength(1); y++)
                    {
                        if(map[i,y] == 1)
                        {
                            Raylib.DrawRectangle(i * cellSize, y * cellSize, cellSize, cellSize, Color.Blue);
                        }
                    }
                }
        }
        static void SimulationFrame()
        {
            int[,] tempMap = new int[map.GetLength(0), map.GetLength(1)];
            Vector2[] neighbours =
            {
                new Vector2(-1, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(0, -1),
                new Vector2(0, 1),
                new Vector2(1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1)
            };

            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    int neighboursAlive = 0;
                    foreach(Vector2 vec in neighbours)
                    {
                        if(vec.X + x >= 0 && vec.X + x < map.GetLength(0) && vec.Y + y >= 0 && vec.Y + y < map.GetLength(1))
                        {
                            if(map[x + (int)vec.X, y + (int)vec.Y] == 1)
                            {
                                neighboursAlive++;
                            }
                        }
                    }
                    if((neighboursAlive == 2 || neighboursAlive == 3) && map[x, y] == 1)
                        {
                            tempMap[x, y] = 1;
                        }else if(neighboursAlive == 3 && map[x, y] == 0)
                        {
                            tempMap[x, y] = 1;
                        }
                        else
                        {
                            tempMap[x, y] = 0;
                        }
                }
            }
            map = tempMap;

        }
    }
}
