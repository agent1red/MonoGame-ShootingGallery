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
        Texture2D restartButtonSprite;
        SpriteFont gameFont;

        static Random r = new Random();
        static int minValue = 20;
        static int maxValue = 400; 

        Vector2 targetPosition = new Vector2(r.Next(minValue, maxValue), r.Next(minValue, maxValue));

        Vector2 startButtonPosition; 
        const int targetRadius = 45;
        MouseState mState;
        bool mReleased = true;
        int score = 0;
        double timer = 10;
        bool gameEnd = false;
        int clickCount; 




        public Game1() 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
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
            restartButtonSprite = Content.Load<Texture2D>("playbutton");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {

            startButtonPosition.X = (_graphics.PreferredBackBufferWidth / 2) - 100;
            startButtonPosition.Y = (_graphics.PreferredBackBufferHeight / 2) + targetRadius;

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
                float distanceToRestartButton = Vector2.Distance(startButtonPosition, mState.Position.ToVector2());
                if (mouseTargetDistance < targetRadius && timer > 0)
                {
                    score++;
                    clickCount++;

                    if (clickCount == 3)
                    {
                        timer += 3;
                        clickCount = 0;  
                    }

                    targetPosition.X = r.Next(0, _graphics.PreferredBackBufferWidth);
                    targetPosition.Y = r.Next(0, _graphics.PreferredBackBufferHeight);
                }

                if (gameEnd && distanceToRestartButton < 45) // using 45 as a radius for simplicity, adjust as needed.
                {
                    RestartGame();
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

            startButtonPosition.X = (_graphics.PreferredBackBufferWidth / 2) - 100; 
            startButtonPosition.Y = (_graphics.PreferredBackBufferHeight / 2) + targetRadius; 

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            if (timer > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
                _spriteBatch.DrawString(gameFont, $"Target hits: {score}", new Vector2(100, 0), Color.White);
                _spriteBatch.DrawString(gameFont, $"Time Left: {Math.Ceiling(timer)}", new Vector2(350, 0), Color.White);
            }
            else
            {
                gameEnd = true; 
                _spriteBatch.DrawString(gameFont, $"Game Over, Your New Score is: {score}", new Vector2((_graphics.PreferredBackBufferWidth / 2) - 250, _graphics.PreferredBackBufferHeight / 2), Color.White);
                _spriteBatch.Draw(restartButtonSprite, new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 90, 50),  Color.Red);
            }
            

            _spriteBatch.Draw(crosshairsSprite, new Vector2(mState.X-25, mState.Y-25), Color.White);
            
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RestartGame()
        {
            score = 0;
            clickCount = 0;
            timer = 10;
            gameEnd = false;
            targetPosition = new Vector2(r.Next(minValue, maxValue), r.Next(minValue, maxValue));
        }

    }
}