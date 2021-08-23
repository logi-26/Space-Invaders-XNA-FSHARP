namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
        
type Projectile(game:Game, x:float32, y:float32, projectile:string) = 
    inherit DrawableGameComponent(game)

    // Declare the spritebatch
    let mutable spriteBatch : SpriteBatch = null

    // Projectile variables
    let mutable spriteImage : Texture2D = null
    let mutable _spritePositionX = x
    let mutable _spritePositionY = y
    let mutable _spriteActive = true
    let projectileType = projectile 
    let nl = System.Nullable<_>()

    // Collision rectangle
    let mutable CollisionRect = new Rectangle(0, 0, 0, 0)
       
    // Get/Set for the collision rectangle
    member this.rect
        with get () = CollisionRect
        and set (value) = CollisionRect <- value

    // Get/Set for the projectile position X
    member this.spritePositionX
        with get () = _spritePositionX
        and set (value) = _spritePositionX <- value

    // Get/Set for the projectile position Y
    member this.spritePositionY
        with get () = _spritePositionY
        and set (value) = _spritePositionY <- value

    // Get/Set for the projectile active boolean
    member this.spriteActive
        with get () = _spriteActive
        and set (value) = _spriteActive <- value

    override x.Initialize() =
        
        // Initialises the sprite batch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)

        // Loads the projectile texture
        if projectileType = "Enemy Projectile" then spriteImage <- game.Content.Load(@"Enemies\Alien_Projectile1")
        else spriteImage <- game.Content.Load(@"Player\Spaceship_Projectile1")
            
        _spriteActive <- false

        base.Initialize()

    override x.Update(gameTime) = 

        // If the projectile is active
        if _spriteActive = true then

            // If it is an enemy projectile
            if projectileType = "Enemy Projectile" then
                _spritePositionY <- _spritePositionY + 10.0f

                // If the projectile Y position is greater than the lower screen bounds, the projectile is made in-active
                if _spritePositionY > (float32)game.GraphicsDevice.Viewport.Height then _spriteActive <- false
            // If it is a player projectile
            else  
                _spritePositionY <- _spritePositionY - 10.0f

                // If the projectile Y position is greater than the upper screen bounds, the projectile is made in-active
                if _spritePositionY < 0.0f then _spriteActive <- false

        base.Update(gameTime)

    override x.Draw(gameTime) =
        
        spriteBatch.Begin()

        // Draws the sprite if it is active
        if _spriteActive = true then spriteBatch.Draw(spriteImage, Vector2(_spritePositionX, _spritePositionY), nl, Color.White, 0.0f, Vector2(0.0f,0.0f), 1.5f, SpriteEffects.None, 1.0f)
            
        // Convert sprite position to int
        let mutable x = (int _spritePositionX)
        let mutable y = (int _spritePositionY)

        // Creates the collision rectangle 1.5 times the size of the projectile sprite image
        // This is because the projectile sprite image is drawn with a scale of 1.5
        CollisionRect.X <- x
        CollisionRect.Y <- y
        CollisionRect.Width <- spriteImage.Width + spriteImage.Width / 2
        CollisionRect.Height <- spriteImage.Height + spriteImage.Height / 2

        spriteBatch.End()

        base.Draw(gameTime)