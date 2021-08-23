namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
        
type Barrier(game:Game, x:float32, y:float32) = 
    inherit DrawableGameComponent(game)

    // Declare the spritebatch
    let mutable spriteBatch : SpriteBatch = null

    // Barrier variables
    let mutable _barrierHits = 0
    let mutable spriteImage : Texture2D = null
    let mutable _barrierPositionX = x
    let mutable _barrierPositionY = y
    let mutable _spriteActive = true

    // Collision rectangle
    let mutable CollisionRect = new Rectangle(0, 0, 0, 0)

    // Sprite counter variables
    let mutable spriteCounter = 1
    let mutable spriteCountDuration = 1.0
    let mutable spriteTime = 0.0
       
    // Get/Set for the barrier hits varriable
    member this.barrierHits
        with get () = _barrierHits
        and set (value) = _barrierHits <- value

    // Get/Set for the collision rectangle
    member this.rect
        with get () = CollisionRect
        and set (value) = CollisionRect <- value

    // Get/Set for the spite position X
    member this.spritePositionX
        with get () = _barrierPositionX
        and set (value) = _barrierPositionX <- value

    // Get/Set for the spite position Y
    member this.spritePositionY
        with get () = _barrierPositionY
        and set (value) = _barrierPositionY <- value

    // Get/Set for the spite active boolean
    member this.spriteActive
        with get () = _spriteActive
        and set (value) = _spriteActive <- value

    override x.Initialize() =
        
        // Initialises the sprite batch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        // Loads the barrier texture
        spriteImage <- game.Content.Load(@"Backgrounds\Barrier")

        _barrierHits <- 0
        _spriteActive <- true

        base.Initialize()

    override x.Update(gameTime) = 
        
        // Switches the barrier textures depending on the value of the barrier hits variable
        if _barrierHits = 0 then spriteImage <- game.Content.Load(@"Backgrounds\Barrier")
        elif _barrierHits = 1 then spriteImage <- game.Content.Load(@"Backgrounds\Barrier_Crack_1")
        elif _barrierHits = 2 then spriteImage <- game.Content.Load(@"Backgrounds\Barrier_Crack_2")

        // If the barrier has been hit 3 times, it is made in-active
        if _barrierHits = 3 then _spriteActive <- false

        base.Update(gameTime)

    override x.Draw(gameTime) =
        
        spriteBatch.Begin()

        // Draws the barrier if it is active
        if _spriteActive = true then spriteBatch.Draw(spriteImage, Vector2(_barrierPositionX, _barrierPositionY), Color.White)
            
        // Convert barrier position to int
        let mutable x = (int _barrierPositionX)
        let mutable y = (int _barrierPositionY)

        // Creates the collision rectangle
        CollisionRect.X <- x
        CollisionRect.Y <- y
        CollisionRect.Width <- spriteImage.Width
        CollisionRect.Height <- spriteImage.Height

        spriteBatch.End()

        base.Draw(gameTime)