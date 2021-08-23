namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

// Game timer - Game component
type GameTimer(game:Game)  =
    inherit DrawableGameComponent(game)

    // SpriteBatch
    let mutable spriteBatch : SpriteBatch = null

    // Font
    let mutable timerFont: SpriteFont = null

    // Timer variables
    let mutable _counter = 1
    let mutable _countDuration = 1.0
    let mutable _currentTime = 0.0

    // Get/Set for the counter variable
    member this.counter
        with get () = _counter
        and set (value) = _counter <- value

    override x.Initialize() =

        // Initialise the spriteBatch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)
        
        // Loads the game timer font
        timerFont <- game.Content.Load(@"Font/timerFont")

        // Initialises the game timer variables
        _counter <- 1
        _countDuration <- 1.0
        _currentTime <- 0.0

        base.Initialize()

    override x.Update(gameTime) = 
        
        // Updates the timer
        _currentTime <- _currentTime + (float)gameTime.ElapsedGameTime.TotalSeconds
        if _currentTime >= _countDuration then
            _counter <- _counter + 1
            _currentTime <- _currentTime - _countDuration

        base.Update(gameTime)

    override x.Draw(gameTime) =

        spriteBatch.Begin()

        // Draws the game timer
        spriteBatch.DrawString(timerFont, _counter.ToString(), Vector2(490.f,10.f), Color.White)

        spriteBatch.End()

        base.Draw(gameTime)