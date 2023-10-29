using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake.Src.Classes
{
    class Body : DrawableGameComponent
    {
        public int score { get; set; } = 0;
        public Boolean run { get; set; }
        public int posX { get; set; }
        public int posY { get; set; }
        public int dirX { get; set; } = 1;
        public int dirY { get; set; } = 0;
        private int size = 0;
        private int williSecondsSiceLastUpdate = 0;
        private int oldPosX = 0;
        private int oldPosY = 0;
        private const int updateInterval = 33;

        private List<Rectangle> tailList;
        private SpriteBatch SpriteBatch;
        private Texture2D pixel;
        private GraphicsDevice graphics;
        public Body(Game game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, int size) : base(game)
        {
            this.SpriteBatch = spriteBatch;
            this.size = size;
            this.graphics = GraphicsDevice;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });


            posX = graphics.Viewport.Width / 2;
            posY = graphics.Viewport.Height / 2;

            tailList = new List<Rectangle>();
            tailList.Add(new Rectangle(posX, posY, size, size));
        }

        public override void Update(GameTime gameTime)
        {
            williSecondsSiceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if (williSecondsSiceLastUpdate >= updateInterval && run)
            {
                williSecondsSiceLastUpdate = 0;

                oldPosX = posX;
                oldPosY = posY;

                posX += dirX * size;
                posY += dirY * size;
                
                if (posY == -size || posY == graphics.Viewport.Height || posX == -size || posX == graphics.Viewport.Width)
                {
                    run = false;
                    posX = oldPosX;
                    posY = oldPosY;
                    return;
                }
                
                if (tailList.Count >1)
                {
                    for(int i = tailList.Count - 1; i > 0; i--)
                    {
                        if (posX == tailList[i].X && posY == tailList[i].Y)
                        {
                            run = false;
                            posX = oldPosX;
                            posY = oldPosY;
                            return;
                        }
                        tailList[i] = new Rectangle(tailList[i - 1].X, tailList[i - 1].Y, size, size);
                    }
                }
            }
            tailList[0] = new Rectangle(posX, posY, size, size);
            base.Update(gameTime);
        }

        public void AddTail()
        {
            tailList.Add(new Rectangle(posX,posY,size,size));
        }

        public void resetSnake()
        {
            tailList.Clear();
            score = 0;
            posX = graphics.Viewport.Width / 2;
            posY = graphics.Viewport.Height / 2;

            tailList.Add(new Rectangle(posX, posY, size, size));
            run = true;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            if (run)
            {
                foreach (var item in tailList)
                {
                    SpriteBatch.Draw(pixel, new Rectangle(item.X - 1, item.Y - 1, size + 2, size + 2), Color.Gray);
                    SpriteBatch.Draw(pixel, item, Color.White);
                }
            }
            else
            {
                foreach (var item in tailList)
                {
                    SpriteBatch.Draw(pixel, new Rectangle(item.X - 1,item.Y - 1,size + 2,size + 2),Color.Gray);
                    SpriteBatch.Draw(pixel, item, Color.Red);
                }
            }

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
