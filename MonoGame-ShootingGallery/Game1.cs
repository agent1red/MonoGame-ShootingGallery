using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame_ShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Declare content variables 

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;
        SpriteFont gameFont;

        static Random r = new Random();
        static int minValue = 20;
        static int maxValue = 400; 

        Vector2 targetPosition = new Vector2(r.Next(minValue, maxValue), r.Next(minValue, maxValue));
        const int targetRadius = 45;
        MouseState mState;
        bool mReleased = true;
        int score = 0;
        double timer = 10;




        public Game1() 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here. Every single frame this will be updated. 

            //*** Timer settings ***//

            

            if (timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (timer < 0)
            {
                timer = 0;
            }
            //*** Timer end ***//


            //*** Mouse states used for game ***//
            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                float mouseTargetDistance = Vector2.Distance(targetPosition, mState.Position.ToVector2());
                if (mouseTargetDistance < targetRadius && timer > 0)
                {
                    score++;
                    targetPosition.X = r.Next(0, _graphics.PreferredBackBufferWidth);
                    targetPosition.Y = r.Next(0, _graphics.PreferredBackBufferHeight);
                }
             
                mReleased = false;
            }

            if(mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            //*** Mouse states end ***//




            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            if (timer > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
            }
            
            _spriteBatch.DrawString(gameFont, $"The Score: {score}", new Vector2(100,0), Color.White);
            _spriteBatch.DrawString(gameFont, $"Timer: {Math.Ceiling(timer)}", new Vector2(350,0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}