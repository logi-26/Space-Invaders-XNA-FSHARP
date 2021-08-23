namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Player(game:Game)=
    inherit DrawableGameComponent(game)

    // SpriteBatch
    let mutable spriteBatch : SpriteBatch = null
    let mutable playerSprite : Texture2D = null

    // Player Variables
    let mutable _playerLives = 3
    let mutable _playerScore = 0
    let mutable _playerFinalScore = 0
    let mutable playerSpeed = 2.0f
    let mutable _playerPositionX = 490.f
    let mutable _playerPositionY = 694.f

    let mutable imageSwitch = 1
    let nl = System.Nullable<_>()

    // Collision rectangle
    let mutable CollisionRect = new Rectangle(0, 0, 0, 0)

    // Get/Set for the player lives
    member this.playerLives
        with get () = _playerLives
        and set (value) = _playerLives <- value

    // Get/Set for the player score
    member this.playerScore
        with get () = _playerScore
        and set (value) = _playerScore <- value

     // Get/Set for the player final score
    member this.playerFinalScore
        with get () = _playerFinalScore
        and set (value) = _playerFinalScore <- value

    // Get/Set for the spite position X
    member this.playerPositionX
        with get () = _playerPositionX
        and set (value) = _playerPositionX <- value

    // Get/Set for the spite position Y
    member this.playerPositionY
        with get () = _playerPositionY
        and set (value) = _playerPositionY <- value

    // Get/Set for the collision rectangle
    member this.rect
        with get () = CollisionRect
        and set (value) = CollisionRect <- value
    
    // Initialize method
    override x.Initialize() =

        // Initialize the spriteBatch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)
        
        // Loads the player texture
        playerSprite <- game.Content.Load(@"Player\Spaceship_Centre1")

        // Initialises the player values
        _playerLives <- 3
        _playerScore <- 0
        _playerFinalScore <- 0
        _playerPositionX <- 490.f

        base.Initialize()

    // Update method
    override x.Update(gameTime) = 
        
        // Allows the player to move left using a keyboard (Within the screen bounds)
        if Keyboard.GetState().IsKeyDown(Keys.Left) then
            if _playerPositionX > 0.0f then _playerPositionX <- _playerPositionX - playerSpeed

        // Allows the player to move right using a keyboard (Within the screen bounds)
        if Keyboard.GetState().IsKeyDown(Keys.Right) then
            if _playerPositionX < (float32)game.GraphicsDevice.Viewport.Width - ((float32)playerSprite.Width + (float32)playerSprite.Width / 2.0f) then _playerPositionX <- _playerPositionX + playerSpeed

        // Allows the player to move right using a Xbox 360 controller (Within the screen bounds)
        if GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.0f then
            if _playerPositionX < (float32)game.GraphicsDevice.Viewport.Width - ((float32)playerSprite.Width + (float32)playerSprite.Width / 2.0f) then _playerPositionX <- _playerPositionX + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X *3.00f

        // Allows the player to move left using a Xbox 360 controller (Within the screen bounds)
        if GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.0f then
            if _playerPositionX > 0.0f then _playerPositionX <- _playerPositionX + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X *3.00f

        // This switches the player image with every call of this method
        // This creates a fast animation effect for the spaceship thruster flames
        if imageSwitch = 1 then
            playerSprite <- game.Content.Load(@"Player\Spaceship_Centre1")
            imageSwitch <- 2
        elif imageSwitch = 2 then
            playerSprite <- game.Content.Load(@"Player\Spaceship_Centre2")
            imageSwitch <- 1

        base.Update(gameTime)

    // Draw method
    override x.Draw(gameTime) =

        spriteBatch.Begin()

        // Draws the player 
        spriteBatch.Draw(playerSprite, Vector2(_playerPositionX, _playerPositionY), nl, Color.White, 0.0f, Vector2(0.0f,0.0f), 1.5f, SpriteEffects.None, 1.0f)

        // Convert sprite position to int
        let mutable x = (int _playerPositionX)
        let mutable y = (int _playerPositionY)

        // Creates the collision rectangle 1.5 times the size of the sprite image
        // This is because the sprite image is drawn with a scale of 1.5
        CollisionRect.X <- x
        CollisionRect.Y <- y
        CollisionRect.Width <- playerSprite.Width + playerSprite.Width / 2
        CollisionRect.Height <- playerSprite.Height + playerSprite.Height / 2

        spriteBatch.End()

        base.Draw(gameTime)