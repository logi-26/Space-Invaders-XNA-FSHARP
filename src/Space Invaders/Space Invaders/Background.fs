namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
        
type Background(game:Game, sprite:string) = 
    inherit DrawableGameComponent(game)

    // Declare the spritebatch
    let mutable spriteBatch : SpriteBatch = null

    // Sprite variables
    let mutable spriteImage : Texture2D = null
    let mutable spriteType : string  = sprite

    // Sprite counter variables
    let mutable spriteCounter = 1
    let mutable spriteCountDuration = 1.0
    let mutable spriteTime = 0.0
       
    override x.Initialize() =
            
        // Initialises the sprite batch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        // Loads the background sprite images
        if spriteType = "Start Background" then spriteImage <- game.Content.Load(@"Backgrounds\Start_Screen_1")
        elif spriteType = "Game Over Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameEnd_Screen1")
        elif spriteType = "Game Won Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameWon_Screen1")

        base.Initialize()
        
    override x.Update(gameTime) = 

        // This is a timer for the sprite images, every second the sprite images are swapped to give the illusion of animation
        spriteTime <- spriteTime + (float)gameTime.ElapsedGameTime.TotalSeconds
        if spriteTime >= spriteCountDuration then
            spriteCounter <- spriteCounter + 1
            spriteTime <- spriteTime - spriteCountDuration

        // If the sprite counter variable is odd, the first sprite images are loaded
        if spriteCounter % 2 = 1 then
            if spriteType = "Start Background" then spriteImage <- game.Content.Load(@"Backgrounds\Start_Screen_1")
            elif spriteType = "Game Over Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameEnd_Screen1")
            elif spriteType = "Game Won Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameWon_Screen1")

        // If the sprite counter variable is even, the second sprite images are loaded
        if spriteCounter % 2 = 0 then
            if spriteType = "Start Background" then spriteImage <- game.Content.Load(@"Backgrounds\Start_Screen_2")
            elif spriteType = "Game Over Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameEnd_Screen2")
            elif spriteType = "Game Won Background" then spriteImage <- game.Content.Load(@"Backgrounds\GameWon_Screen2")

        base.Update(gameTime)

    // Draw method
    override x.Draw(gameTime) =

        spriteBatch.Begin()

        // Draws the background sprite
        spriteBatch.Draw(spriteImage, Vector2(0.0f, 0.0f), Color.White)
           
        spriteBatch.End()

        base.Draw(gameTime)