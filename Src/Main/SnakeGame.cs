using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Src.Classes;
using System;
using System.Runtime.CompilerServices;

namespace Snake
{
    public class SnakeGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Body Body;
        private Food Food;
        private Random rnd;
        private Boolean up, down, left, right;
        private const int snakeSize = 10;
        private const int gameHeight = 50;
        private const int gameWidth = 100;
        public SnakeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            up = down = left = right = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = gameHeight * snakeSize;
            graphics.PreferredBackBufferWidth = gameWidth * snakeSize;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            rnd = new Random();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Body = new Body(this, GraphicsDevice, spriteBatch, snakeSize);
            Food = new Food(this, GraphicsDevice, spriteBatch, snakeSize);
            font = Content.Load<SpriteFont>("font");

            this.Components.Add(Body);
            this.Components.Add(Food);
        }

        public void setFoodLocation()
        {
            Food.posX = rnd.Next(0, GraphicsDevice.Viewport.Width / snakeSize) * snakeSize;
            Food.posY = rnd.Next(0, GraphicsDevice.Viewport.Height / snakeSize) * snakeSize;
            Food.active = true;
        }

        public void CheckSnakeFood()
        {
            if (Body.posX == Food.posX && Body.posY == Food.posY)
            {
                Body.score++;
                Food.active = false;
                Body.AddTail();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!Food.active)
            {
                setFoodLocation();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && up)
            {
                Body.dirX = 0;
                Body.dirY = -1;
                down = false;
                up = true;
                left = true;
                right = true;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Down) && down)
            {
                Body.dirX = 0;
                Body.dirY = 1;
                down = true;
                up = false;
                left = true;
                right = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && left)
            {
                Body.dirX = -1;
                Body.dirY = 0;
                down = true;
                up = true;
                left = true;
                right = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && right)
            {
                Body.dirX = 1;
                Body.dirY = 0;
                down = true;
                up = true;
                left = false;
                right = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && !Body.run)
            {
                Body.run = true;
                Body.resetSnake();
            }


            CheckSnakeFood();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(51,51,51));

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score "+Body.score, new Vector2(snakeSize), Color.Gray);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
