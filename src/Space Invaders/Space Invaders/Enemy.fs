namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
        
type Enemy(game:Game, enemyType:string, x:float32, y:float32, firePosition:Vector2) = 
    inherit DrawableGameComponent(game)

    // Declare the spritebatch
    let mutable spriteBatch : SpriteBatch = null

    // Enemy variables
    let mutable spriteImage : Texture2D = null
    let mutable spriteType = enemyType
    let mutable _spritePositionX = x
    let mutable _spritePositionY = y
    let mutable spriteSpeed = 1.0f
    let mutable _spriteDirection = 2 // 1 = left, 2 = right
    let mutable _spriteActive = true
    let mutable _firePosition = firePosition

    // Collision rectangle
    let mutable CollisionRect = new Rectangle(0, 0, 0, 0)

    // Enemy sprite counter variables
    let mutable spriteCounter = 1
    let mutable spriteCountDuration = 1.0
    let mutable spriteTime = 0.0
       
    // Get/Set for the random fire position
    member this.firePosition
        with get () = _firePosition
        and set (value) = _firePosition <- value

    // Get/Set for the collision rectangle
    member this.rect
        with get () = CollisionRect
        and set (value) = CollisionRect <- value

    // Get/Set for the spite position X
    member this.spritePositionX
        with get () = _spritePositionX
        and set (value) = _spritePositionX <- value

    // Get/Set for the spite position Y
    member this.spritePositionY
        with get () = _spritePositionY
        and set (value) = _spritePositionY <- value

    // Get/Set for the spite direction
    member this.spriteDirection
        with get () = _spriteDirection
        and set (value) = _spriteDirection <- value

    // Get/Set for the spite active boolean
    member this.spriteActive
        with get () = _spriteActive
        and set (value) = _spriteActive <- value

    override x.Initialize() =
            
        // initialises the sprite batch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        // Loads the enemy sprite images
        if spriteType = "Enemy1" then spriteImage <- game.Content.Load(@"Enemies\Alien_3.1")
        elif spriteType = "Enemy2" then spriteImage <- game.Content.Load(@"Enemies\Alien_1.1")
        elif spriteType = "Enemy3" then spriteImage <- game.Content.Load(@"Enemies\Alien_2.1")

        base.Initialize()
        
    // Update method
    override x.Update(gameTime) = 
            
        // This is a timer for the sprite images, every second the sprite images are swapped to give the illusion of animation
        spriteTime <- spriteTime + (float)gameTime.ElapsedGameTime.TotalSeconds
        if spriteTime >= spriteCountDuration then
            spriteCounter <- spriteCounter + 1
            spriteTime <- spriteTime - spriteCountDuration

        // If the sprite counter variable is odd, the first sprite images are loaded
        if spriteCounter % 2 = 1 then
            if spriteType = "Enemy1" then spriteImage <- game.Content.Load(@"Enemies\Alien_3.1")
            elif spriteType = "Enemy2" then spriteImage <- game.Content.Load(@"Enemies\Alien_1.1")
            elif spriteType = "Enemy3" then spriteImage <- game.Content.Load(@"Enemies\Alien_2.1")

        // If the sprite counter variable is even, the second sprite images are loaded
        if spriteCounter % 2 = 0 then
            if spriteType = "Enemy1" then spriteImage <- game.Content.Load(@"Enemies\Alien_3.2")
            elif spriteType = "Enemy2" then spriteImage <- game.Content.Load(@"Enemies\Alien_1.2")
            elif spriteType = "Enemy3" then spriteImage <- game.Content.Load(@"Enemies\Alien_2.2")
           
        // Moves the enemy sprites across the screen from left to right
        if spriteType = "Enemy1" && _spriteDirection = 2 && _spritePositionX < (float32)game.GraphicsDevice.Viewport.Width - (float32)spriteImage.Width || 
            spriteType = "Enemy2" && _spriteDirection = 2 && _spritePositionX < (float32)game.GraphicsDevice.Viewport.Width - (float32)spriteImage.Width || 
            spriteType = "Enemy3" && _spriteDirection = 2 && _spritePositionX < (float32)game.GraphicsDevice.Viewport.Width - (float32)spriteImage.Width then _spritePositionX <- _spritePositionX + spriteSpeed
        elif (spriteType = "Enemy1"  && _spriteDirection = 1 && _spritePositionX > 0.0f || 
                spriteType = "Enemy2" && _spriteDirection = 1 && _spritePositionX > 0.0f || 
                spriteType = "Enemy3" && _spriteDirection = 1 && _spritePositionX > 0.0f) then _spritePositionX <- _spritePositionX - spriteSpeed

        base.Update(gameTime)
        
    override x.Draw(gameTime) =
        
        spriteBatch.Begin()

        // Draws the sprite if it is active
        if _spriteActive = true then spriteBatch.Draw(spriteImage, Vector2(_spritePositionX, _spritePositionY), Color.White)
            
        // Convert sprite position to int
        let mutable x = (int _spritePositionX)
        let mutable y = (int _spritePositionY)

        // Creates the collision rectangle
        CollisionRect.X <- x
        CollisionRect.Y <- y
        CollisionRect.Width <- spriteImage.Width
        CollisionRect.Height <- spriteImage.Height

        spriteBatch.End()
        
        base.Draw(gameTime)
