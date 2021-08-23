namespace SpaceInvaders
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

// GameStates
type GameState =
    | GameStart = 0
    | GamePlay = 1
    | GameOver = 2
    | GameWon = 3

// Action Key states
type ActionKeyState =
    | KeyUp = 0
    | KeyDown = 1

// Sprite manager game component
type SpriteManager(game:Game)  =
    inherit DrawableGameComponent(game)
    
    // SpriteBatch
    let mutable spriteBatch : SpriteBatch = null

    // Random class, used for the enemy fire position
    let rnd = System.Random()
    let mutable randomX = 0

    // Game timer
    let GameTimer = new GameTimer(game)

    // Background sprites
    let startBackgroundSprite = new Background(game, "Start Background")
    let gameOverBackgroundSprite = new Background(game, "Game Over Background")
    let gameWonBackgroundSprite = new Background(game, "Game Won Background")

    // Player
    let Player = new Player(game)
    let mutable finalScoreCalculated = false

    // Player projectile
    let PlayerProjectile = new Projectile(game, 0.0f, 0.0f, "Player Projectile")
    let mutable triggerDown = false

    // Barrier array
    let barrierArray : Barrier[] = Array.zeroCreate 3;

    // Creteas a 2 dimensional array of enemies and a 2 dimensional array of enemy projectiles
    let enemyArray : Enemy[,] = Array2D.zeroCreate 3 8;
    let enemyProjectileArray : Projectile[,] = Array2D.zeroCreate 3 8;

    // Sets the current game state
    let mutable currentGameState : GameState = GameState.GameStart

    // Sets the action key state
    let mutable previousKeyState : ActionKeyState = ActionKeyState.KeyUp

    // Fonts
    let mutable scoreFont: SpriteFont = null

    // Initialise method
    override x.Initialize() =

        // Initialise the spriteBatch
        spriteBatch <- new SpriteBatch(game.GraphicsDevice)
        
        // Initialise the background sprites
        startBackgroundSprite.Initialize()
        gameOverBackgroundSprite.Initialize()
        gameWonBackgroundSprite.Initialize()

        // Initialise the game timer
        GameTimer.Initialize()

        // Initialise the player and the projectile
        Player.Initialize()
        PlayerProjectile.Initialize()

        // Initialise the barrier sprites
        let mutable barrierCount = 0
        let mutable barrierX = 100.0f
        for c = 1 to 3 do
            barrierArray.[barrierCount] <- new Barrier(game, barrierX, 620.0f)
            barrierArray.[barrierCount].Initialize()
            barrierCount <- barrierCount + 1
            if barrierCount = 1 then barrierX <- 450.0f
            elif barrierCount = 2 then barrierX <- 800.0f

        // Initialises the enemies and the enemy projectiles
        let mutable enemyX = 100.0f
        let mutable enemyY = 100.0f
        let mutable counter1 = 0
        let mutable counter2 = 0
        for c = 1 to 24 do
            // Generates a new random number between 1 and 1024
            randomX <- rnd.Next(1, game.GraphicsDevice.Viewport.Width)

            // Converts the random number to a float32 value
            let rand = (float32)randomX

            // Creates the enemies for each row
            if counter1 = 0 then enemyArray.[counter1,counter2] <- new Enemy(game, "Enemy1", enemyX, enemyY, Vector2(rand,enemyY))
            elif counter1 = 1 then enemyArray.[counter1,counter2] <- new Enemy(game, "Enemy2", enemyX, enemyY, Vector2(rand,enemyY))
            elif counter1 = 2 then enemyArray.[counter1,counter2] <- new Enemy(game, "Enemy3", enemyX, enemyY, Vector2(rand,enemyY))
            enemyArray.[counter1,counter2].Initialize()
            enemyProjectileArray.[counter1,counter2] <- new Projectile(game, 0.0f, 0.0f, "Enemy Projectile")
            enemyProjectileArray.[counter1,counter2].Initialize()
            counter2 <- counter2 + 1
            enemyX <- enemyX + 100.0f

            // When the end of a row is reached, the variables are incremented
            if c = 8 then
                counter1 <- 1
                counter2 <- 0
                enemyX <- 100.0f
                enemyY <- 200.0f
            if c = 16 then
                counter1 <- 2
                counter2 <- 0
                enemyX <- 100.0f
                enemyY <- 300.0f

        base.Initialize()

    // Load method
    override x.LoadContent() =

        // Loads the fonts
        scoreFont <- game.Content.Load(@"Font/gameFont")

        base.LoadContent()

    // Update method
    override x.Update(gameTime) = 
        
        // Updates the game timer
        GameTimer.Update (gameTime)

        // Updates the start background sprite
        if currentGameState = GameState.GameStart then
            startBackgroundSprite.Update(gameTime)

            // The Xbox 360 start button or keyboard enter key are used to start the game
            if GamePad.GetState(PlayerIndex.One).Buttons.Start = ButtonState.Pressed && previousKeyState = ActionKeyState.KeyUp || 
                Keyboard.GetState().IsKeyDown(Keys.Enter) && previousKeyState = ActionKeyState.KeyUp then
                
                    // When a new game is started, the player is initialised
                    Player.Initialize()
                    finalScoreCalculated <- false
                
                    // The game timer is initialised
                    GameTimer.Initialize()
                
                    // The sprite manager is initialised
                    x.Initialize()
                
                    // The barrier sprites are initialised and updated
                    let mutable barrierCount = 0
                    for c = 1 to 3 do
                         barrierArray.[barrierCount].Initialize()
                         barrierArray.[barrierCount].Update(gameTime)
                         barrierCount <- barrierCount + 1

                    // Sets the gamestate
                    currentGameState <- GameState.GamePlay
                
        // If the game is playing
        if currentGameState = GameState.GamePlay then
            
            // Updates the player
            Player.Update (gameTime)

            // Loop counter variables
            let mutable counter1 = 0
            let mutable counter2 = 0
            for a = 1 to 24 do

                // Updates the enemies
                enemyArray.[counter1,counter2].Update(gameTime)

                // If any of the enemy sprites reach the right screen border, another loop is used to loop through the entire array
                if enemyArray.[counter1,counter2].spriteDirection = 2 && enemyArray.[counter1,counter2].spritePositionX >= 956.0f && enemyArray.[counter1,counter2].spriteActive = true then
                    let mutable counter3 = 0
                    let mutable counter4 = 0
                    for b = 1 to 24 do
                        // Increments the sprites Y position and switches the current direction
                        enemyArray.[counter3,counter4].spritePositionY <- enemyArray.[counter3,counter4].spritePositionY + 50.0f
                        enemyArray.[counter3,counter4].spriteDirection <- 1

                        // Generates a new random number between 1 and 1024
                        randomX <- rnd.Next(1, game.GraphicsDevice.Viewport.Width)

                        // Converts the random number to a float32
                        let rand = (float32)randomX

                        // Sets the random number as the enemy sprites firePosition X 
                        enemyArray.[counter3,counter4].firePosition <- Vector2(rand, enemyArray.[counter3,counter4].spritePositionY)
                        counter4 <- counter4 + 1

                        // When the end of a row is reached, the variables are incremented
                        if b = 8 then
                            counter3 <- 1
                            counter4 <- 0
                        if b = 16 then
                            counter3 <- 2
                            counter4 <- 0
                // If any of the enemy sprites reach the left screen border, another loop is used to loop through the entire array
                if enemyArray.[counter1,counter2].spriteDirection = 1 && enemyArray.[counter1,counter2].spritePositionX <= 0.0f && enemyArray.[counter1,counter2].spriteActive = true then
                    let mutable counter3 = 0
                    let mutable counter4 = 0
                    for t = 1 to 24 do
                        // Increments the sprites Y position and switches the current direction
                        enemyArray.[counter3,counter4].spritePositionY <- enemyArray.[counter3,counter4].spritePositionY + 50.0f
                        enemyArray.[counter3,counter4].spriteDirection <- 2

                        // Generates a new random number between 1 and 1024
                        randomX <- rnd.Next(1, game.GraphicsDevice.Viewport.Width)

                        // Converts the random number to a float32
                        let rand = (float32)randomX

                        // Sets the random number as the enemy sprites firePosition X 
                        enemyArray.[counter3,counter4].firePosition <- Vector2(rand, enemyArray.[counter3,counter4].spritePositionY)
                        counter4 <- counter4 + 1

                        // When the end of a row is reached, the variables are incremented
                        if t = 8 then
                            counter3 <- 1
                            counter4 <- 0   
                        if t = 16 then
                            counter3 <- 2
                            counter4 <- 0     
                            
                // If any of the enemy sprites reach the barriers the game is over
                if enemyArray.[counter1,counter2].rect.Intersects barrierArray.[0].rect && enemyArray.[counter1,counter2].spriteActive = true && barrierArray.[0].spriteActive = true || 
                    enemyArray.[counter1,counter2].rect.Intersects barrierArray.[1].rect && enemyArray.[counter1,counter2].spriteActive = true && barrierArray.[1].spriteActive = true || 
                    enemyArray.[counter1,counter2].rect.Intersects barrierArray.[2].rect && enemyArray.[counter1,counter2].spriteActive = true && barrierArray.[2].spriteActive = true then currentGameState <- GameState.GameOver

                // If the enemy sprites position is equal to the enemy sprites random fire posistion
                // A new enemy projectile is created
                if enemyArray.[counter1,counter2].spritePositionX = enemyArray.[counter1,counter2].firePosition.X && enemyArray.[counter1,counter2].spriteActive = true then
                    enemyProjectileArray.[counter1,counter2] <- new Projectile(game, enemyArray.[counter1,counter2].spritePositionX, enemyArray.[counter1,counter2].spritePositionY, "Enemy Projectile")
                    enemyProjectileArray.[counter1,counter2].Initialize()
                    enemyProjectileArray.[counter1,counter2].spriteActive <- true
            
                // If the projectile is active, it is updated
                if enemyProjectileArray.[counter1,counter2].spriteActive = true then enemyProjectileArray.[counter1,counter2].Update(gameTime)
                    
                // Once the projectile goes out of the lower screen bounds, the sprite is made in-active
                if enemyProjectileArray.[counter1,counter2].spritePositionY > 768.00f then enemyProjectileArray.[counter1,counter2].spriteActive <- false  
                           
                counter2 <- counter2 + 1
                // When the end of a row is reached, the variables are incremented
                if a = 8 then
                    counter1 <- 1
                    counter2 <- 0
                if a = 16 then
                    counter1 <- 2
                    counter2 <- 0         
    
            // Prevents the player from shooting through the barriers
            if PlayerProjectile.rect.Intersects barrierArray.[0].rect && barrierArray.[0].spriteActive = true || 
                PlayerProjectile.rect.Intersects barrierArray.[1].rect  && barrierArray.[1].spriteActive = true || 
                PlayerProjectile.rect.Intersects barrierArray.[2].rect  && barrierArray.[2].spriteActive = true then PlayerProjectile.spriteActive <- false
                       
            // If the space bar or right trigger button are pressed, a projectile is created
            if currentGameState = GameState.GamePlay && GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f && PlayerProjectile.spriteActive = false && triggerDown = false || 
                currentGameState = GameState.GamePlay && Keyboard.GetState().IsKeyDown(Keys.Space) && PlayerProjectile.spriteActive = false && triggerDown = false then
                    PlayerProjectile.spritePositionX <- Player.playerPositionX + 20.0f
                    PlayerProjectile.spritePositionY <- Player.playerPositionY - 20.0f
                    PlayerProjectile.spriteActive <- true

            // This records the previous state of the right trigger button and spacebar key
            if GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f || Keyboard.GetState().IsKeyDown(Keys.Space) then triggerDown <- true
            else triggerDown <- false

            // Updates the player projectile
            if PlayerProjectile.spriteActive = true then PlayerProjectile.Update(gameTime)
        
            // Loop to check if the player projectile has hit an enemy or a barrier
            let mutable deadEnemies = 0
            counter1 <- 0
            counter2 <- 0
            for k = 1 to 24 do

                // If the player projectile hits an enemy, the enemy is made in-active and the projectile is made in-active
                if PlayerProjectile.rect.Intersects enemyArray.[counter1,counter2].rect && enemyArray.[counter1,counter2].spriteActive = true && PlayerProjectile.spriteActive = true then
                    enemyArray.[counter1,counter2].spriteActive <- false
                    PlayerProjectile.spriteActive <- false
                    Player.playerScore <- Player.playerScore + 10

                // If an enemy is in-active, the variable is incremented
                if enemyArray.[counter1,counter2].spriteActive = false then deadEnemies <- deadEnemies + 1

                // If an enemy projectile hits the player, the enemys projectile is made in-active and the players lives are decremented
                if enemyProjectileArray.[counter1,counter2].rect.Intersects Player.rect && enemyProjectileArray.[counter1,counter2].spriteActive = true then
                    enemyProjectileArray.[counter1,counter2].spriteActive <- false
                    // The players lives are decremented
                    Player.playerLives <- Player.playerLives - 1

                // If the enemy projectile hits any of the barrier sprites
                if enemyProjectileArray.[counter1,counter2].rect.Intersects barrierArray.[0].rect && barrierArray.[0].spriteActive = true && enemyProjectileArray.[counter1,counter2].spriteActive = true then
                    enemyProjectileArray.[counter1,counter2].spriteActive <- false
                    // The barrier 1 hits variable is incremented
                    barrierArray.[0].barrierHits <- barrierArray.[0].barrierHits + 1
                    barrierArray.[0].Update(gameTime)
                elif enemyProjectileArray.[counter1,counter2].rect.Intersects barrierArray.[1].rect && barrierArray.[1].spriteActive = true && enemyProjectileArray.[counter1,counter2].spriteActive = true  then
                    enemyProjectileArray.[counter1,counter2].spriteActive <- false
                    // The barrier 2 hits variable is incremented
                    barrierArray.[1].barrierHits <- barrierArray.[1].barrierHits + 1
                    barrierArray.[1].Update(gameTime)
                elif enemyProjectileArray.[counter1,counter2].rect.Intersects barrierArray.[2].rect && barrierArray.[2].spriteActive = true && enemyProjectileArray.[counter1,counter2].spriteActive = true  then
                    enemyProjectileArray.[counter1,counter2].spriteActive <- false
                    // The barrier 3 hits variable is incremented
                    barrierArray.[2].barrierHits <- barrierArray.[2].barrierHits + 1
                    barrierArray.[2].Update(gameTime)

                counter2 <- counter2 + 1
                if k = 8 then
                    counter1 <- 1
                    counter2 <- 0
                if k = 16 then
                    counter1 <- 2
                    counter2 <- 0

            // If the deadEnemies variable = 24, the player has won the game
            if deadEnemies = 24 then currentGameState <- GameState.GameWon

            // If the player has no lives left, the game is over
            if Player.playerLives = 0 then currentGameState <- GameState.GameOver

        // If the game state = game over
        if currentGameState = GameState.GameOver then
            // Updates the game over screen
            gameOverBackgroundSprite.Update(gameTime)

            // If the enter key or start button are pressed, the gamestate is set to game start
            if Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start = ButtonState.Pressed then currentGameState <- GameState.GameStart

        // If the game state = game won
        if currentGameState = GameState.GameWon then
            // Updates the game won screen
            gameWonBackgroundSprite.Update(gameTime)

            // Calculates the final score
            if finalScoreCalculated = false then

                // The final score is determined by the players lives, the time taken to win and the condition of the barrier sprites
                Player.playerFinalScore <- Player.playerScore

                // Adds points to the final score depending on the amount of lives the player had remaining
                Player.playerFinalScore <- Player.playerFinalScore + Player.playerLives * 100

                // Deducts the time taken to complete the game from the players final score
                Player.playerFinalScore <- Player.playerFinalScore - GameTimer.counter

                // Deducts points from the final score depending on the condition of the 3 barrier sprites
                let mutable barrierCounter = 0
                for c = 1 to 3 do
                    Player.playerFinalScore <- Player.playerFinalScore - barrierArray.[barrierCounter].barrierHits * 10
                    barrierCounter <- barrierCounter + 1
        
                finalScoreCalculated <- true

            // If the enter key or start button are pressed, the gamestate is set to game start
            if Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start = ButtonState.Pressed then currentGameState <- GameState.GameStart
        
        // Records the previous state of the action key (Enter key or start button)
        if Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.Start = ButtonState.Pressed then previousKeyState <- ActionKeyState.KeyDown
        else previousKeyState <- ActionKeyState.KeyUp

        base.Update(gameTime)

    // Draw method
    override x.Draw(gameTime) =

        // Clears the screen
        game.GraphicsDevice.Clear(Color.Black)

        spriteBatch.Begin()

        // If gamestate = game start, the start background sprite is drawn
        if currentGameState = GameState.GameStart then startBackgroundSprite.Draw(gameTime)

        // If gamestate = game play
        if currentGameState = GameState.GamePlay then
            // Draws score font
            spriteBatch.DrawString(scoreFont, "Score", Vector2(30.f,10.f), Color.White) 
            spriteBatch.DrawString(scoreFont, Player.playerScore.ToString(), Vector2(110.f,10.f), Color.White)

            // Draw player lives font
            spriteBatch.DrawString(scoreFont, "Lives", Vector2(890.f,10.f), Color.White) 
            spriteBatch.DrawString(scoreFont, Player.playerLives.ToString(), Vector2(960.f,10.f), Color.White)

            // Draw the game timer
            GameTimer.Draw gameTime

            // Draws the player
            Player.Draw gameTime

            // Draws all of the enemies and the enemy projectiles
            let mutable counter1 = 0
            let mutable counter2 = 0
            for c = 1 to 24 do
                enemyArray.[counter1,counter2].Draw (gameTime)
                enemyProjectileArray.[counter1,counter2].Draw(gameTime)
                counter2 <- counter2 + 1
                if c = 8 then
                    counter1 <- 1
                    counter2 <- 0
                if c = 16 then
                    counter1 <- 2
                    counter2 <- 0
                     
            // Draws the barrier sprites
            let mutable barrierCounter = 0
            for c = 1 to 3 do
                barrierArray.[barrierCounter].Draw(gameTime)
                barrierCounter <- barrierCounter + 1

            // If the player projectile is active, it is drawn
            if PlayerProjectile.spriteActive = true then PlayerProjectile.Draw(gameTime)
        
        // If gamestate = game over, the game over background sprite is drawn
        if currentGameState = GameState.GameOver then gameOverBackgroundSprite.Draw(gameTime)

        // If gamestate = game won, the game won background sprite and final score are drawn
        if currentGameState = GameState.GameWon then
            gameWonBackgroundSprite.Draw(gameTime)
            spriteBatch.DrawString(scoreFont, "Your final score is " + Player.playerFinalScore.ToString(), Vector2(370.f,400.f), Color.White)

        spriteBatch.End()

        base.Draw(gameTime)