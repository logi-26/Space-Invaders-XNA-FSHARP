namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
 
type Game1() as this =
    inherit Game()

    // Declares the graphics device and sets the graphics profile
    let graphics = new GraphicsDeviceManager(this)
    do graphics.GraphicsProfile <- GraphicsProfile.Reach // Reach was used because my laptop did not support HiDef mode

    // Spritebatch
    let mutable spriteBatch : SpriteBatch = null

    // Sets the content directory
    do this.Content.RootDirectory <- "Content"

    // Sprite manager
    let SpriteManager = new SpriteManager(this)
    
    override game.Initialize() =

        // Initialize the spritebatch
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)

        // Sets the viewport size
        graphics.PreferredBackBufferHeight <- 768
        graphics.PreferredBackBufferWidth <- 1024
        graphics.IsFullScreen <- true
        graphics.ApplyChanges()

        // Initialise the sprite manager
        SpriteManager.Initialize()

        base.Initialize()

    override game.LoadContent() =

        base.LoadContent()

    override game.Update gameTime =

        // Allows the game to exit using the Esc key on a keyboard or the back button on the xbox 360 controller
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed) || Keyboard.GetState().IsKeyDown(Keys.Escape) then this.Exit()

        // Update the sprite manager
        SpriteManager.Update(gameTime)

        base.Update gameTime
 
    override game.Draw gameTime =
            
        spriteBatch.Begin()

        // Draws the sprite manager game component
        SpriteManager.Draw(gameTime)

        spriteBatch.End()
        base.Draw gameTime

// Run game
module Services =
    let game = new Game1()
    try game.Run()
    finally game.Dispose()