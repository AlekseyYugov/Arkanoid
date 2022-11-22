using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Arkanoid
{
    internal class Program
    {
        static RenderWindow window;

        static Texture ballTexture;
        static Texture stickTexture;
        static Texture blockTexture;

        static Sprite stick;
        static Sprite[] blocks;    

        static int[] arrayOfRandomNumbers = new int[81]; // массив случайных чисел
        static int blockQuantity = 0;// количество блоков
        static int level = 0;
        static int attempt = 3; //попытки

        static Ball ball;
        public static void SetStartPosition(int level)
        {
            int index = 0;
            int size = 0;
            int indent = 0; //отступ
            if (level == 1) {size = 5; indent = 82;}
            if (level == 2) {size = 7; indent = 38;}
            if (level == 3) {size = 9; indent = 15;}
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    blocks[index] = new Sprite(blockTexture);
                    blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + indent) + 100,
                                                          y * (blocks[index].TextureRect.Height + 15) + indent);
                    index++;
                }
            }
            stick.Position = new Vector2f(400, 500);
            ball.sprite.Position = new Vector2f(375, 400);// положение мяча
        }

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(800, 600), "Arkanoid");
            window.SetFramerateLimit(60);

            window.Closed += Window_Closed;

            ballTexture = new Texture("Ball.png");
            stickTexture = new Texture("Stick.png");
            blockTexture = new Texture("Block.png");

            Random random = new Random();

            for (int i = 0; i < arrayOfRandomNumbers.Length; i++)
            {
                arrayOfRandomNumbers[i] = random.Next(1, 3); 
            }

            while (window.IsOpen)
            {
                ball = new Ball(ballTexture);
                stick = new Sprite(stickTexture);
                level++;
                if (level == 1)
                {
                    attempt = 3;
                    blockQuantity = 25;
                    blocks = new Sprite[blockQuantity];
                    for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);
                    SetStartPosition(level);
                }
                if (level == 2)
                {
                    attempt = 3;
                    blockQuantity = 49;
                    blocks = new Sprite[blockQuantity];
                    for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);
                    SetStartPosition(level);
                }
                if (level == 3)
                {
                    attempt = 3;
                    blockQuantity = 81;
                    blocks = new Sprite[blockQuantity];
                    for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);
                    SetStartPosition(level);
                }
                if (level == 4)
                {
                    window.Close();
                }

                while (window.IsOpen)
                {
                    window.Clear();
                    window.DispatchEvents();

                    if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
                    {
                        ball.Start(8, new Vector2f(0, -1));
                    }
                    ball.Move(new Vector2i(0, 0), new Vector2i(780, 580));

                    ball.CheckCollision(stick, "Stick");
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        if (ball.CheckCollision(blocks[i], "Block")) //удаление блоков
                        {
                            if (arrayOfRandomNumbers[i] < 2)
                            {
                                blocks[i].Position = new Vector2f(1000, 1000);
                                blockQuantity--;
                                break;
                            }   
                            else 
                            { 
                                arrayOfRandomNumbers[i] = 1;
                                break;
                            }      
                        }
                    }

                    stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.TextureRect.Width * 0.5f, stick.Position.Y);

                    window.Draw(ball.sprite);
                    
                    if(ball.sprite.Position.Y > 600)
                    {
                        attempt--;
                        if(attempt == 0) window.Close();
                        ball.sprite.Position = new Vector2f(375, 400);
                        ball.speed = 0;
                        ball.direction.X = 0;
                        ball.direction.Y = 0;
                        if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
                        {
                            ball.Start(8, new Vector2f(0, -1));
                        }
                    }

                    window.Draw(stick);
                    for (int i = 0; i < blocks.Length; i++)
                    {
                        window.Draw(blocks[i]);
                    }
                    if (blockQuantity == 0)
                    {
                        break;
                    }
                    window.Display();
                }
            }
        }
        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }

    }
}
