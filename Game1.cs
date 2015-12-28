using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pyramid_Plunder
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //private Grid grid = new Grid(10, 14);
        private Pyramid cPyramid; // current Pyramid in play
        private Room displayRoom; // current Room to Display
        private Room testRoom = new Room(new Grid[3] {new Grid(10, 14), new Grid(10, 14), new Grid(10, 14)});
        // the Player
        private Player player = new Player("player","player", new Barrier(true), new Vector2(-1, -1), new int[2] { 1, 1 }, null, 0.9f,0.0f, new int[2] { 1, 1 });
        private Viewport viewport;
        private Texture2D wallTxt;
        private Texture2D sandStuff;
        private Texture2D heart;
        private Texture2D goldBar;
        private Texture2D EUS;
        private Texture2D EDS;
        private Texture2D ERS;
        private Texture2D ELS;
        private SpriteFont Font1; // Dialog Text Font.
        private SoundEffect ding;
        private SoundEffect musica;
        private SoundEffect musicb;
        private SoundEffectInstance musici;
        private Texture2D floorSpike;
        private Texture2D chestTx;
        private Texture2D rchestTx;
        private Texture2D skeletonTx;
        private Texture2D doorTx;
        private Texture2D doorrTx;
        private Texture2D keyTx;
        private Texture2D keygTx;
        private Texture2D pyramidTx;
        private Texture2D stairsTx;
        private Texture2D rockTx;
        private Texture2D rockigTx;
        private Texture2D holeTx;
        private Texture2D holewTx;
        private Texture2D wcrackTx;
        private Texture2D arrowTx;
        private Texture2D foottrapTx;
        private bool started = false;
        private bool finished = false; // between levels
        private bool killed = false; // played killed
        private int level = 1; // currnet Level
        private String trapbin = ""; // used to communicate floor traps with Arrows

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wallTxt = Content.Load<Texture2D>("WallBlock");
            EUS = Content.Load<Texture2D>("EUS");
            EDS = Content.Load<Texture2D>("EDS");
            ERS = Content.Load<Texture2D>("ERS");
            ELS = Content.Load<Texture2D>("ELS");
            sandStuff = Content.Load<Texture2D>("SandStuff");
            heart = Content.Load<Texture2D>("Heart");
            goldBar = Content.Load<Texture2D>("GoldBar");
            floorSpike = Content.Load<Texture2D>("FloorSpikes");
            chestTx = Content.Load<Texture2D>("Chest");
            rchestTx = Content.Load<Texture2D>("RChest");
            skeletonTx = Content.Load<Texture2D>("skeleton");
            doorTx = Content.Load<Texture2D>("doors");
            doorrTx = Content.Load<Texture2D>("doorsright");
            keyTx = Content.Load<Texture2D>("key_silver");
            keygTx = Content.Load<Texture2D>("key_gold");
            pyramidTx = Content.Load<Texture2D>("Pyramid");
            stairsTx = Content.Load<Texture2D>("DownStairs");
            rockTx = Content.Load<Texture2D>("boulder");
            rockigTx = Content.Load<Texture2D>("rockinground");
            holeTx = Content.Load<Texture2D>("hole");
            holewTx = Content.Load<Texture2D>("holewall");
            wcrackTx = Content.Load<Texture2D>("wall_block_cracked3");
            arrowTx = Content.Load<Texture2D>("Arrows");
            foottrapTx = Content.Load<Texture2D>("foottrap");
            Font1 = Content.Load<SpriteFont>("ScoreFont");
            ding = Content.Load<SoundEffect>("Ding");
            musica = Content.Load<SoundEffect>("GLA_05");
            musicb = Content.Load<SoundEffect>("GLA_04");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void StartTestRoom() // was used to debug, not used in the final production
        {
            started = true;
            Grid[] layers = testRoom.GetLayers();
            PPObject[,] walls = new PPObject[layers[1].GetObjects().GetLength(0), layers[1].GetObjects().GetLength(1)];
            for (int r = 0; r < walls.GetLength(0); r++)
            {
                for (int c = 0; c < walls.GetLength(1); c++)
                {
                    if (r == 0 || r == walls.GetLength(0) - 1 || c == 0 || c == walls.GetLength(1) - 1)
                    {
                        walls[r, c] = new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { r, c }, wallTxt, 0.5f, 0.0f);
                    }
                    else if (r == 8)
                    {
                        walls[r, c] = new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { r, c }, goldBar, 0.5f, 0.0f, 100);
                    }
                    else if (r == 5 && c == 7)
                    {
                        walls[r, c] = new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { r, c }, floorSpike, 0.5f, 0.0f, new Rectangle(32,0,32,32), 1);
                    }
                    else if (r == 5 && c == 8)
                    {
                        walls[r, c] = new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { r, c }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250);
                    }
                    else if (r == 5 && c == 9)
                    {
                        walls[r, c] = new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { r, c }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), new PPObject( "key1","key", new Barrier(false),new Vector2(),new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32)));
                    }
                    else if (r == 5 && c == 2)
                    {
                        walls[r, c] = new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { r, c }, doorTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key1",true);
                    }
                }
            }
            Grid layer1 = new Grid(walls);
            testRoom.ReplaceLayer(1, layer1);

            PPObject[,] ground = new PPObject[layers[2].GetObjects().GetLength(0), layers[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground.GetLength(0); r++)
            {
                for (int c = 0; c < ground.GetLength(1); c++)
                {

                    ground[r, c] = new PPObject("ground","ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer2 = new Grid(ground);
            testRoom.ReplaceLayer(2, layer2);
            player.SetImage(EDS, new Rectangle(0, 0, 32, 32));
            PPObject[] inv = new PPObject[24];
            inv[0] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[1] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[3] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[11] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[12] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[14] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            inv[16] = new Gold("goldbar", "gold", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, goldBar, 1.0f, 0.0f, 0);
            player.SetInv(inv);
        }

        // Check Player Event
        // handles player colliding with static Objects
        public void CPE(Player py, Room rm)
        {
            PPObject[] obs = rm.GetObjectsAtLoc(py.playerLocation[0], py.playerLocation[1]);
            for (int i = 0; i < obs.Length; i++)
            {
                if (obs[i] != null)
                {
                    if (obs[i].classname == "gold")
                    {
                        Gold tres = (Gold) obs[i];
                        if (!tres.looted)
                        {
                            tres.Collected(py);
                            ding.Play();
                            rm.GetLayers()[i].SetGridObject(null, tres.gridloc[0], tres.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "trap")
                    {
                        Trap trap = (Trap)obs[i];
                        if (!trap.triggered)
                        {
                            trap.Trigger(py);
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }
                               
                    }
                    else if (obs[i].classname == "foottrap")
                    {
                        FloorTrap trap = (FloorTrap)obs[i];
                        if (!trap.triggered)
                        {
                            trap.Trigger(py);
                            trapbin = trap.traptoactivate;
                            // play click sound
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "stair")
                    {
                        Stairs stair = (Stairs)obs[i];
                        if (!stair.on)
                        {
                            stair.Walked(py);
                            py.interpolate = 0;
                            py.moving = false;
                            displayRoom = cPyramid.GetFloor(py.floor); // swap out display room
                            py.playerLocation = stair.transportPos;
                            //py.transforing = false;
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "hiddenstairs")
                    {
                        HiddenStairs stair = (HiddenStairs)obs[i];
                        if (!stair.on && !stair.hidden)
                        {
                            stair.Walked(py);
                            py.interpolate = 0;
                            py.moving = false;
                            if (stair.name == "finalstairs")
                            {
                                // move to level select screen/ level completed
                                finished = true;
                            }
                            else
                            {
                                displayRoom = cPyramid.GetFloor(py.floor);
                                py.playerLocation = stair.transportPos;
                            }
                            //py.transforing = false;
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }

                }
            }
        }

        // Check Player Input
        // handles collecting the player's input
        public void CPI(Player py, Room rm)
        {
            if (!py.moving && !py.transforing)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.6f || Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    String output = py.MovePlayer("left", rm);
                    if (output == "ui")
                    {

                        py.SetImage(FetchPlayerImage("left", py.image), new Rectangle(0, 0, 32, 32));
                    }
                    else
                    {
                        return;
                    }
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .6f || Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    String output = py.MovePlayer("right", rm);
                    if (output == "ui")
                    {
                        py.SetImage(FetchPlayerImage("right", py.image), new Rectangle(0, 0, 32, 32));
                    }
                    else
                    {
                        return;
                    }
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .6f || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    String output = py.MovePlayer("up", rm);
                    if (output == "ui")
                    {
                        py.SetImage(FetchPlayerImage("up", py.image), new Rectangle(0, 0, 32, 32));
                    }
                    else
                    {
                        return;
                    }
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.6f || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    String output = py.MovePlayer("down", rm);
                    if (output == "ui")
                    {
                        py.SetImage(FetchPlayerImage("down", py.image), new Rectangle(0, 0, 32, 32));
                    }
                    else
                    {
                        return;
                    }
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    PlayerInteract(py,rm);
                }
            }
        }

        

        // BELLX

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q))
                this.Exit(); // always have an option to exit using Back on controller or the Q key.


            if (killed)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    trapbin = ""; // deactivate any traps
                    musici.Stop();
                    player.score = 0;
                    player.health = 3;
                    player.interpolate = 0;
                    player.moving = false;
                    started = false;
                    killed = false;
                    finished = false;
                }
            }
            else if (!started)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //StartTestRoom();
                    finished = false;
                    started = true;
                    musici = musica.CreateInstance();
                    musici.IsLooped = true;
                    musici.Volume = 0.2f;
                    musici.Play();
                    cPyramid = CreatePyramid(level);
                    displayRoom = cPyramid.GetFloor(0);
                }

                //StartTestRoom();
            }
            else if (finished)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //StartTestRoom();

                    level = level + 1;
                    if (level > 3)
                    {
                        level = 1;
                    }
                    //musica.Play(0.2f, 0.0f, 0.0f);
                    cPyramid = CreatePyramid(level);
                    displayRoom = cPyramid.GetFloor(0);
                    finished = false;
                    started = true;
                }
            }
            else
            {
                if (player != null && !player.transforing && displayRoom != null)
                {
                    if (player.health > 0)
                    {
                        CPE(player, displayRoom);
                        CPI(player, displayRoom);
                    }
                    else
                    {
                        killed = true;
                        started = false;
                        level = 1;
                    }
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        // handles Player pressing the interact key/button on objects
        public void PlayerInteract(Player py, Room rm)
        {
            int[] spot = py.playerLocation;

            // get the spot in front of the player
            if (player.direction == "left")
            {
                spot = new int[2] { spot[0], spot[1] - 1 };
            }
            else if (player.direction == "right")
            {
                spot = new int[2] { spot[0], spot[1] + 1 };
            }
            else if (player.direction == "up")
            {
                spot = new int[2] { spot[0]-1, spot[1] };
            }

            else if (player.direction == "down")
            {
                spot = new int[2] { spot[0]+1, spot[1] };
            }

            // get objects at the spot
            PPObject[] obs = rm.GetObjectsAtLoc(spot[0], spot[1]);
            // handles events
            for (int i = 0; i < obs.Length; i++)
            {
                if (obs[i] != null)
                {
                    if (obs[i].classname == "chest")
                    {
                       Chest tres = (Chest)obs[i];
                        if (!tres.looted)
                        {
                            tres.Opened(py);
                            ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, tres.gridloc[0], tres.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "finalchest")
                    {
                        Chest tres = (Chest)obs[i];
                        if (!tres.looted)
                        {
                            tres.Opened(py);
                            if (!cPyramid.fTrap)
                            {
                                cPyramid.fTrap = true;
                                cPyramid.TriggerFTrap();
                            }
                            ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, tres.gridloc[0], tres.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "door")
                    {
                        Door dor = (Door)obs[i];
                        if (dor.locked)
                        {
                            dor.Unlock(py);
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }
                    else if (obs[i].classname == "movable" && !obs[i].walkable.IsBlocked(player.direction))
                    {
                        Movable dor = (Movable)obs[i];
                        // get spot the object to be moved to
                        int[] spot2 = spot;
                        if (player.direction == "left")
                        {
                            spot2 = new int[2] { spot[0], spot[1] - 1 };
                        }
                        else if (player.direction == "right")
                        {
                            spot2 = new int[2] { spot[0], spot[1] + 1 };
                        }
                        else if (player.direction == "up")
                        {
                            spot2 = new int[2] { spot[0] - 1, spot[1] };
                        }

                        else if (player.direction == "down")
                        {
                            spot2 = new int[2] { spot[0] + 1, spot[1] };
                        }
                        bool canpush = true;
                        bool ishole = false;
                        // get object at location of object to be moved to
                        PPObject[] obs2 = rm.GetObjectsAtLoc(spot2[0], spot2[1]);
                        // check if object can be moved here
                        for (int ii = 0; ii < obs2.Length; ii++)
                        {
                            if (obs2[ii] != null && obs2[ii].classname == "hole")
                            {
                                ishole = true;
                                canpush = true;
                                break;
                            }
                            else if (obs2[ii] != null && obs2[ii].classname == "foottrap")
                            {
                                FloorTrap ft = (FloorTrap)obs2[ii];
                                ft.Trigger(player);
                                trapbin = ft.traptoactivate;
                                canpush = true;
                                break;
                            }
                            else if (obs2[ii] != null && !obs2[ii].walkable.IsBlocked(player.direction))
                            {
                                canpush = false;
                                break;
                            }
                        }
                        // if pushed onto a hole, then fill it
                        if (ishole && canpush)
                        {
                            dor.gridloc = spot2;
                            dor.SetImage(rockigTx, dor.rect);
                            dor.walkable = new Barrier(true);
                            rm.SetObjectAtLoc(1, spot2[0], spot2[1], dor);
                            rm.RemoveObjectAtLoc(1, spot[0], spot[1]);
                        }
                        // just move the object
                        else if (canpush)
                        {
                            //dor.Pushed(py);
                            dor.gridloc = spot2;
                            rm.SetObjectAtLoc(1, spot2[0], spot2[1], dor);
                            rm.RemoveObjectAtLoc(1, spot[0], spot[1]);
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }
                    /*
                    else if (obs[i].classname == "trap")
                    {
                        Trap trap = (Trap)obs[i];
                        if (!trap.triggered)
                        {
                            trap.Trigger(py);
                            //ding.Play();
                            //rm.GetLayers()[i].SetGridObject(null, trap.gridloc[0], trap.gridloc[1]);
                        }

                    }
                    */
                }
            }
        }

        public void DrawRoom(Room rm)
        {
            Grid[] layers = rm.GetLayers(); // rooms have layers, so need to loop throught layers and draw objects
            for (int l = 0; l < layers.Length; l++)
            {
                DrawObjects(viewport, layers[l],rm);
            }
            
        }

        public Texture2D FetchPlayerImage(String dir, Texture2D ima)
        {
            if (dir == "left")
            {
                return ELS;
            }
            else if (dir == "right")
            {
                return ERS;
            }
            else if (dir == "up")
            {
                return EUS;
            }
            else if (dir == "down")
            {
                return EDS;
            }
            return ima;
        }

        
        
        public void DrawScoreBar(Viewport vp, int score)
        {
            Vector2 Scale = new Vector2(20.0f / goldBar.Bounds.Width, 20.0f / goldBar.Bounds.Height);
            Vector2 origin = new Vector2(10.0f, 0.0f);

            spriteBatch.Draw(goldBar, origin, null, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(Font1, score.ToString(), origin + new Vector2(20.0f,-2.0f), Color.Gold, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.2f);
            
        }

        public void DrawHealthBar(Viewport vp, int health)
        {
            Vector2 Scale = new Vector2(20.0f / heart.Bounds.Width, 20.0f / heart.Bounds.Height);
            Vector2 origin = new Vector2(vp.Bounds.Width - 30, 0.0f);
            for (int h = 0; h < health; h++)
            {
                spriteBatch.Draw(heart, origin - new Vector2(20*h,0), null, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
            }
        }

        public void DrawObjectInInv(PPObject ob, Vector2 o, int place)
        {
            if (ob != null && ob.image != null)
            {
                if (ob.rect == null)
                {
                    Vector2 Scale = new Vector2(32.0f / ob.image.Bounds.Width, 32.0f / ob.image.Bounds.Height);
                    if (place < 12)
                    {
                        spriteBatch.Draw(ob.image, o + new Vector2(16 + (32.0f * place), 16), null, Color.White, ob.rotation, new Vector2(16, 16), Scale, SpriteEffects.None, 1.0f);

                    }
                    else
                    {
                        spriteBatch.Draw(ob.image, o + new Vector2(16+(32.0f * (place-12)), 48.0f), null, Color.White, ob.rotation, new Vector2(16, 16), Scale, SpriteEffects.None, 1.0f);
                    }
                }
                else
                {
                    Vector2 Scale = new Vector2(32.0f / ob.rect.Value.Width, 32.0f / ob.rect.Value.Height);
                    if (place < 12)
                    {
                        spriteBatch.Draw(ob.image, o + new Vector2(16+(32.0f * place), 16), ob.rect, Color.White, ob.rotation, new Vector2(16,16), Scale, SpriteEffects.None, 1.0f);

                    }
                    else
                    {
                        spriteBatch.Draw(ob.image, o + new Vector2(16+(32.0f * (place-12)), 48.0f), ob.rect, Color.White, ob.rotation, new Vector2(16, 16), Scale, SpriteEffects.None, 1.0f);

                    }
                }
            }
            Vector2 Scale2 = new Vector2(1.0f, 1.0f);
            if (place < 12)
            {
                spriteBatch.Draw(sandStuff, o + new Vector2(32.0f * place, 0.0f), new Rectangle(32,180,32,32), Color.Gray, 0.0f, Vector2.Zero, Scale2, SpriteEffects.None, .98f);
            }
            else
            {
                spriteBatch.Draw(sandStuff, o + new Vector2(32.0f * (place - 12), 32.0f), new Rectangle(32, 180, 32, 32), Color.Gray, 0.0f, Vector2.Zero, Scale2, SpriteEffects.None, .98f);
            }
        }

        public void DrawInventory(Viewport vp, Player py)
        {
            Vector2 origin = new Vector2(10.0f, vp.Bounds.Height - 67.0f);
            PPObject[] invent = py.GetInv();
            for (int i = 0; i < invent.Length; i++)
            {
                DrawObjectInInv(invent[i], origin, i);
            }
        }

        public void DrawPlayerGUI(Viewport vp, Player py)
        {
            DrawHealthBar(vp, py.health);
            DrawScoreBar(vp, py.score);
            DrawInventory(vp, player);
        }

        public void DrawPlayer(Viewport vp, Player py, Room rm)
        {
            PPObject[,] objs = rm.GetLayers()[0].GetObjects();
            int[] ppos = py.playerLocation;
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float sx = 10;
            float sy = 20;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);
            if (py != null && py.image != null)
            {
                Vector2 pos = new Vector2((ppos[1] * xsize) + sx, (ppos[0] * ysize) + sy);
                if (py.rect == null)
                {
                    Vector2 Scale = new Vector2(xsize / py.image.Bounds.Width, ysize / py.image.Bounds.Height);
                    if (!py.moving)
                    {
                        py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position + new Vector2(Scale.X / 2, Scale.Y / 2), null, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);

                }
                else
                {
                    Vector2 Scale = new Vector2(xsize / py.rect.Value.Width, ysize / py.rect.Value.Height);
                    if (!py.moving)
                    {
                        py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position+ new Vector2(Scale.X / 2, Scale.Y / 2), py.rect, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);
                }
            }
            py.transforing = false;
        }

        public void DrawEnemy(Viewport vp, Enemy py, Grid grid)
        {
            PPObject[,] objs = grid.GetObjects();
            int[] ppos = py.playerLocation;
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float sx = 10;
            float sy = 20;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);
            if (py != null && py.image != null)
            {
                Vector2 pos = new Vector2((ppos[1] * xsize) + sx, (ppos[0] * ysize) + sy);
                if (py.rect == null)
                {
                    Vector2 Scale = new Vector2(xsize / py.image.Bounds.Width, ysize / py.image.Bounds.Height);
                    if (!py.moving)
                    {
                        //py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position + new Vector2(Scale.X / 2, Scale.Y / 2), null, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);

                }
                else
                {
                    Vector2 Scale = new Vector2(xsize / py.rect.Value.Width, ysize / py.rect.Value.Height);
                    if (!py.moving)
                    {
                        //py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position + new Vector2(Scale.X / 2, Scale.Y / 2), py.rect, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);
                }
            }
        }

        public void DrawArrow(Viewport vp, Arrow py, Grid grid)
        {
            PPObject[,] objs = grid.GetObjects();
            int[] ppos = py.playerLocation;
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float sx = 10;
            float sy = 20;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);
            if (py != null && py.image != null)
            {
                Vector2 pos = new Vector2((ppos[1] * xsize) + sx, (ppos[0] * ysize) + sy);
                if (py.rect == null)
                {
                    Vector2 Scale = new Vector2(xsize / py.image.Bounds.Width, ysize / py.image.Bounds.Height);
                    if (!py.moving)
                    {
                        //py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position + new Vector2(Scale.X / 2, Scale.Y / 2), null, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);

                }
                else
                {
                    Vector2 Scale = new Vector2(xsize / py.rect.Value.Width, ysize / py.rect.Value.Height);
                    if (!py.moving)
                    {
                        //py.SetPosition(pos);
                    }
                    spriteBatch.Draw(py.image, py.position + new Vector2(Scale.X / 2, Scale.Y / 2), py.rect, Color.White, 0.0f, new Vector2(Scale.X / 2, Scale.Y / 2), Scale, SpriteEffects.None, py.depth);
                }
            }
        }

        public void DrawObjects(Viewport vp, Grid grid, Room rm)
        {
            PPObject[,] objs = grid.GetObjects();
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float sx = 10;
            float sy = 20;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);
            for (int r = 0; r < objs.GetLength(0); r++)
            {
                for (int c = 0; c < objs.GetLength(1); c++)
                {
                    
                    PPObject ob = objs[r, c];
                    if (ob != null && ob.image != null)
                    {
                        if (ob.classname == "enemy")
                        {
                            Enemy enm = (Enemy)ob;
                            if (!enm.active && cPyramid.fTrap)
                            {
                                enm.Trigger(rm);
                            }
                            else if (enm.active && cPyramid.fTrap)
                            {
                                InterPolateEnemy(enm,rm);
                                DrawEnemy(vp, enm, grid);
                            }
                        }
                        else if (ob.classname == "arrow")
                        {
                            Arrow enm = (Arrow)ob;
                            if (!enm.active && !enm.fired && trapbin == enm.name)
                            {
                                enm.Trigger(rm);
                            }
                            else if (enm.active && !enm.fired)
                            {
                                InterPolateArrow(enm, rm);
                                DrawArrow(vp, enm, grid);
                            }
                        }
                        else if (ob.classname == "hiddenstairs")
                        {
                            HiddenStairs hs = (HiddenStairs)ob;
                            if (hs.hidden && (hs.name == "finalstairs" && cPyramid.fTrap))
                            {
                                hs.Reveal();
                            }
                            else if (!hs.hidden)
                            {
                                Vector2 pos = new Vector2((c * xsize) + sx, (r * ysize) + sy);
                                if (ob.rect == null)
                                {
                                    Vector2 Scale = new Vector2(xsize / ob.image.Bounds.Width, ysize / ob.image.Bounds.Height);
                                    ob.position = pos;
                                    spriteBatch.Draw(ob.image, pos, null, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);

                                }
                                else
                                {
                                    Vector2 Scale = new Vector2(xsize / ob.rect.Value.Width, ysize / ob.rect.Value.Height);
                                    ob.position = pos;
                                    spriteBatch.Draw(ob.image, pos, ob.rect, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);
                                }
                            }
                        }
                        else if (ob.classname == "crackedwall")
                        {
                            CrackedWall enm = (CrackedWall)ob;
                            if (!enm.triggered && cPyramid.fTrap)
                            {
                                enm.Trigger(player);
                            }
                            else if (enm.triggered)
                            {
                                
                            }
                            else
                            {
                                Vector2 pos = new Vector2((c * xsize) + sx, (r * ysize) + sy);
                                if (ob.rect == null)
                                {
                                    Vector2 Scale = new Vector2(xsize / ob.image.Bounds.Width, ysize / ob.image.Bounds.Height);
                                    ob.position = pos;
                                    spriteBatch.Draw(ob.image, pos, null, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);

                                }
                                else
                                {
                                    Vector2 Scale = new Vector2(xsize / ob.rect.Value.Width, ysize / ob.rect.Value.Height);
                                    ob.position = pos;
                                    spriteBatch.Draw(ob.image, pos, ob.rect, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);
                                }
                            }
                        }
                        else
                        {
                            Vector2 pos = new Vector2((c * xsize) + sx, (r * ysize) + sy);
                            if (ob.rect == null)
                            {
                                Vector2 Scale = new Vector2(xsize / ob.image.Bounds.Width, ysize / ob.image.Bounds.Height);
                                ob.position = pos;
                                spriteBatch.Draw(ob.image, pos, null, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);

                            }
                            else
                            {
                                Vector2 Scale = new Vector2(xsize / ob.rect.Value.Width, ysize / ob.rect.Value.Height);
                                ob.position = pos;
                                spriteBatch.Draw(ob.image, pos, ob.rect, Color.White, ob.rotation, Vector2.Zero, Scale, SpriteEffects.None, ob.depth);
                            }
                        }
                    }
                }
            }
        }

        // checks for collisions between enemies and Player
        public void CheckForCollisions(Enemy ey, Player py, Room rm, Viewport vp)
        {
            PPObject[,] objs = rm.GetLayers()[2].GetObjects();
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);

            Vector2 eye = new Vector2(ey.position.X + xsize, ey.position.Y + ysize);
            Vector2 pye = new Vector2(py.position.X + xsize, py.position.Y + ysize);

            Vector2 eyc = new Vector2(ey.position.X + xsize/2, ey.position.Y + ysize/2);
            Vector2 pyc = new Vector2(py.position.X + xsize/2, py.position.Y + ysize/2);

            if (py.position.X >= eye.X ||
                pye.X <= ey.position.X ||
                py.position.Y >= eye.Y ||
                pye.Y <= ey.position.Y) // strict collisions detection, not lee way
            {
                return;
            }
            else if((eyc - pyc).Length() > 30) // throw in some lee way.
            {
                return;
            }
            else
            {
                py.health = 0;
            }

        }

        // check's collisions between Arrows and player
        public void CheckForCollisionsA(Arrow ey, Player py, Room rm, Viewport vp)
        {
            PPObject[,] objs = rm.GetLayers()[2].GetObjects();
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float xsize = vx / objs.GetLength(1);
            float ysize = vy / objs.GetLength(0);

            Vector2 eye = new Vector2(ey.position.X + xsize, ey.position.Y + ysize);
            Vector2 pye = new Vector2(py.position.X + xsize, py.position.Y + ysize);

            Vector2 eyc = new Vector2(ey.position.X + xsize / 2, ey.position.Y + ysize / 2);
            Vector2 pyc = new Vector2(py.position.X + xsize / 2, py.position.Y + ysize / 2);

            if (py.position.X >= eye.X ||
                pye.X <= ey.position.X ||
                py.position.Y >= eye.Y ||
                pye.Y <= ey.position.Y) // strict collisions detection, not lee way
            {
                return;
            }
            else if ((eyc - pyc).Length() > 30) // throw in some lee way.
            {
                return;
            }
            else
            {
                py.health = 0;
            }

        }

        // functions for interpolating dynamic objects
        public void InterPolateObject(Player py)
        {
            Vector2 distance = py.oldpos - py.newpos;
            py.interpolate = py.interpolate - 1;
            py.SetImage(FetchPlayerImage(py.direction, py.image), new Rectangle(py.interpolate * 32, 0, 32, 32));
            py.position = py.newpos + (distance * (py.interpolate / 8.0f));
            if (py.interpolate == 0)
            {
                py.position = py.newpos;
                py.oldpos = py.newpos;
                py.playerLocation = py.newLocation;
                py.moving = false;

            }
        }

        public void InterPolateEnemy(Enemy py, Room rm)
        {
            Vector2 distance = py.oldpos - py.newpos;
            py.interpolate = py.interpolate - 1;
            py.SetImage(py.image, new Rectangle(py.interpolate * 32, 0, 32, 32));
            py.position = py.newpos + (distance * (py.interpolate / 10.0f));
            CheckForCollisions(py, player,rm, viewport);
            if (py.interpolate == 0)
            {
                py.position = py.newpos;
                py.oldpos = py.newpos;
                py.playerLocation = py.newLocation;
                //py.moving = false;
                if (py.cpath+py.direction > py.path.GetLength(0) -1)
                {
                    if (!py.looppath)
                    {
                        py.direction = -1;
                    }
                    else
                    {
                        py.cpath = -1;
                    }
                }
                else if (py.cpath+py.direction < 0)
                {
                    py.direction = 1;
                }
                py.cpath = py.cpath + py.direction;
                
                py.newLocation = new int[2] { py.path[py.cpath,0], py.path[py.cpath,1] };
                py.newpos = rm.GetLayers()[2].GetGridObject(py.newLocation[0], py.newLocation[1]).position;
                py.interpolate = 10;

            }
        }

        public void InterPolateArrow(Arrow py, Room rm)
        {
            Vector2 distance = py.oldpos - py.newpos;
            py.interpolate = py.interpolate - 1;
            //py.SetImage(py.image, new Rectangle(py.interpolate * 32, 0, 32, 32));
            py.position = py.newpos + (distance * (py.interpolate / 3.0f));
            CheckForCollisionsA(py, player, rm, viewport);
            if (py.interpolate == 0)
            {
                py.position = py.newpos;
                py.oldpos = py.newpos;
                py.playerLocation = py.newLocation;
                //py.moving = false;
                if (py.cpath + py.direction > py.path.GetLength(0) - 1)
                {
                    py.fired = true;
                    py.active = false;
                    return;
                }
                else if (py.cpath + py.direction < 0)
                {
                    py.fired = true;
                    py.active = false;
                    return;
                }
                py.cpath = py.cpath + py.direction;

                py.newLocation = new int[2] { py.path[py.cpath, 0], py.path[py.cpath, 1] };
                py.newpos = rm.GetLayers()[2].GetGridObject(py.newLocation[0], py.newLocation[1]).position;
                py.interpolate = 3;

            }
        }

        public void DrawTitleScreen(Viewport vp)
        {
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float xsize = vx / 2;
            float ysize = vy / 1;
            Vector2 Scale = new Vector2(xsize / pyramidTx.Bounds.Width, ysize / pyramidTx.Bounds.Height);
            spriteBatch.Draw(pyramidTx, new Vector2(10.0f + (xsize-xsize/2),20), null, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.9f);

            Vector2 origin = new Vector2(10.0f + xsize, vy-20.0f);
            Vector2 sleng = Font1.MeasureString("Press A or Spacebar to play");
            Vector2 sleng2 = Font1.MeasureString("Pyramid Plunder!");
            spriteBatch.DrawString(Font1, "Press A or Spacebar to Play", origin + new Vector2(-sleng.X, 0), Color.Gold, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Font1, "Pyramid Plunder!", origin + new Vector2(-sleng2.X, sleng.Y*2), Color.Gold, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0.2f);

        }

        public void DrawLevelCompleteScreen(Viewport vp)
        {
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float xsize = vx / 2;
            float ysize = vy / 1;
            Vector2 Scale = new Vector2(xsize / pyramidTx.Bounds.Width, ysize / pyramidTx.Bounds.Height);
            spriteBatch.Draw(pyramidTx, new Vector2(10.0f + (xsize - xsize / 2), 20), null, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.9f);

            Vector2 origin = new Vector2(10.0f + xsize, vy - 20.0f);
            Vector2 sleng = Font1.MeasureString("Press A or Spacebar to continue");
            Vector2 sleng2 = Font1.MeasureString("Gold amount: " + player.score.ToString());
            spriteBatch.DrawString(Font1, "Press A or Spacebar to continue", origin + new Vector2(-sleng.X/2, 0), Color.Gold, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Font1, "Gold amount: " + player.score.ToString(), origin + new Vector2(-sleng2.X/2, sleng.Y), Color.Gold, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.2f);

        }

        public void DrawDeathScreen(Viewport vp)
        {
            float vx = vp.Width - 20;
            float vy = vp.Height - 90;
            float xsize = vx / 2;
            float ysize = vy / 1;
            Vector2 Scale = new Vector2(xsize / pyramidTx.Bounds.Width, ysize / pyramidTx.Bounds.Height);
            spriteBatch.Draw(pyramidTx, new Vector2(10.0f + (xsize - xsize / 2), 20), null, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.9f);

            Vector2 origin = new Vector2(10.0f + xsize, vy - 20.0f);
            Vector2 sleng = Font1.MeasureString("GAME OVER!");
            Vector2 sleng2 = Font1.MeasureString("Gold Collected: " + player.score.ToString());
            Vector2 sleng3 = Font1.MeasureString("Press A or Space to play again");
            spriteBatch.DrawString(Font1, "GAME OVER!", origin + new Vector2(-sleng.X, 0), Color.Gold, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Font1, "Gold Collected: " + player.score.ToString(), origin + new Vector2(-sleng2.X/2, sleng.Y*2), Color.Gold, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.2f);
            spriteBatch.DrawString(Font1, "Press A or Space to play again", origin + new Vector2(-sleng3.X / 2, (sleng.Y*2) + sleng2.Y), Color.Gold, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.2f);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.Begin(SpriteSortMode.FrontToBack,null);
            viewport = graphics.GraphicsDevice.Viewport;

            if (killed)
            {
                DrawDeathScreen(viewport);
            }
            else if (!started)
            {
                DrawTitleScreen(viewport);
            }
            else if (finished)
            {
                DrawLevelCompleteScreen(viewport);
            }
            else
            {
                if (displayRoom != null)
                {
                    DrawRoom(displayRoom);
                    //DrawObjects(viewport,grid);
                }

                if (player != null)
                {
                    if (player.moving)
                    {
                        InterPolateObject(player);

                    }
                    DrawPlayer(viewport, player, displayRoom);
                    DrawPlayerGUI(viewport, player);
                }
            }

            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }

        // function for creating the Levels
        public Pyramid CreatePyramid(int level)
        {
            if (level == 1)
            {
                player.playerLocation = new int[2] { 1, 1 };
                player.floor = 0;
                player.health = 3;
                player.SetInv(new PPObject[24]);
                player.SetImage(EDS, new Rectangle(0, 0, 32, 32));
                return CreatePyramid1();
            }
            else if (level == 2)
            {
                player.playerLocation = new int[2] { 1, 1 };
                player.floor = 0;
                //player.health = 3;
                player.SetInv(new PPObject[24]);
                player.SetImage(EDS, new Rectangle(0, 0, 32, 32));
                return CreatePyramid2();
            }
            else if (level == 3)
            {
                musici.Stop();
                musici.Dispose();
                musici = musicb.CreateInstance();
                musici.IsLooped = true;
                musici.Volume = 0.2f;
                musici.Play();
                player.playerLocation = new int[2] { 1, 1 };
                player.floor = 0;
                //player.health = 3;
                player.SetInv(new PPObject[24]);
                player.SetImage(EDS, new Rectangle(0, 0, 32, 32));
                return CreatePyramid3();
            }


            player.playerLocation = new int[2] { 1, 1 };
            player.floor = 0;
            //player.health = 3;
            player.SetInv(new PPObject[24]);
            player.SetImage(EDS, new Rectangle(0, 0, 32, 32));
            return CreatePyramid1();
        }

        // written out placement of every object for every room in a Pyramid
        public Pyramid CreatePyramid1()
        {
            Pyramid Level1 = new Pyramid("level1",new Room[3] {
                new Room(new Grid[3] { new Grid(6, 8), new Grid(6, 8), new Grid(6, 8) }),
                new Room(new Grid[3] { new Grid(10, 14), new Grid(10, 14), new Grid(10, 14) }),
                new Room(new Grid[3] { new Grid(10, 14), new Grid(10, 14), new Grid(10, 14) })
            });
            Level1.SetWalls(Level1.GetRooms()[0], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[1], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[2], wallTxt); // fills the outside of the room with walls

            Grid[] room1 = Level1.GetFloor(0).GetLayers();
            room1[1].SetGridObject(new HiddenStairs("finalstairs", "hiddenstairs", new Barrier(true), new Vector2(), new int[2] { 1, 6 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 3, 8 }),
                1, 1);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 2 }, goldBar, 0.5f, 0.0f, 100),
                4, 2);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 3 }, goldBar, 0.5f, 0.0f, 100),
                4, 3);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 4 }, goldBar, 0.5f, 0.0f, 100),
                4, 4);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 2 }, wallTxt, 0.5f, 0.0f),
                1, 2);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 3 }, wallTxt, 0.5f, 0.0f),
                1, 3);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 3 }, wallTxt, 0.5f, 0.0f),
                2, 3);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 4 }, wallTxt, 0.5f, 0.0f),
                1, 4);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 4 }, wallTxt, 0.5f, 0.0f),
                2, 4);
            room1[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 1, 5 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
                1, 5);
            room1[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 3, 3 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                3, 3);
            room1[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 6 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true,new int[2] { 3, 8 }),
                1, 6);
            room1[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 2, 2 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[8, 2] { { 2, 2 }, { 3, 2 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 3, 5 }, { 2, 5 } }),
              2, 2);

            PPObject[,] ground = new PPObject[room1[2].GetObjects().GetLength(0), room1[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground.GetLength(0); r++)
            {
                for (int c = 0; c < ground.GetLength(1); c++)
                {

                    ground[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer2 = new Grid(ground);
            Level1.GetFloor(0).ReplaceLayer(2, layer2);

            Grid[] room2 = Level1.GetFloor(1).GetLayers();
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 3, 7 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 2, 6 }),
                3, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 9 }, wallTxt, 0.5f, 0.0f),
                            3, 9);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 9 }, wallTxt, 0.5f, 0.0f),
                            2, 9);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 8 }, wallTxt, 0.5f, 0.0f),
                            2, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 7 }, wallTxt, 0.5f, 0.0f),
                            2, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 6 }, wallTxt, 0.5f, 0.0f),
                            2, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 6 }, wallTxt, 0.5f, 0.0f),
                            3, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 6 }, wallTxt, 0.5f, 0.0f),
                            4, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 6 }, wallTxt, 0.5f, 0.0f),
                            5, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 6 }, wallTxt, 0.5f, 0.0f),
                            6, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 7, 6 }, wallTxt, 0.5f, 0.0f),
                            7, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 8, 6 }, wallTxt, 0.5f, 0.0f),
                            8, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 10 }, wallTxt, 0.5f, 0.0f),
                            3, 10);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 12 }, wallTxt, 0.5f, 0.0f),
                            3, 12);
            room2[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 3, 11 }, doorTx, 0.5f, 0.0f,new Rectangle(0,0,32,32),"key1",true),
                            3, 11);
            room2[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 6, 12 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                new PPObject("key1", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                6, 12);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 7 }, goldBar, 0.5f, 0.0f, 100),
                            8, 7);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 8 }, goldBar, 0.5f, 0.0f, 100),
                            8, 8);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 9 }, goldBar, 0.5f, 0.0f, 100),
                            8, 9);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 10 }, goldBar, 0.5f, 0.0f, 100),
                            8, 10);
            room2[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 4, 10 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
                4, 10);
            room2[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 6, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                6, 11);
            room2[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 6, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                7, 12);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 1 }, goldBar, 0.5f, 0.0f, 100),
                2, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 1 }, goldBar, 0.5f, 0.0f, 100),
                3, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 1 }, goldBar, 0.5f, 0.0f, 100),
                4, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 5, 1 }, goldBar, 0.5f, 0.0f, 100),
                5, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 6, 1 }, goldBar, 0.5f, 0.0f, 100),
                6, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 1 }, goldBar, 0.5f, 0.0f, 100),
                7, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 1 }, goldBar, 0.5f, 0.0f, 100),
                8, 1);
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 8, 5 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 8, 6 }),
                8, 5);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 2, 1 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32),
                new int[5, 2] { { 2, 1 }, { 2, 2 }, { 2, 3 }, { 2, 4 }, { 2, 5 } }),
                2, 1);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 3, 3 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 2, false,
                new int[5, 2] { { 3, 1 }, { 3, 2 }, { 3, 3 }, { 3, 4 }, { 3, 5 } }),
                3, 3);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 4, 5 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 4, false,
                new int[5, 2] { { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 } }),
                4, 5);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 5, 2 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 1, false,
                new int[5, 2] { { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 } }),
                5, 2);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 4, 7 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, true,
                new int[12, 2] { { 4, 7 }, { 5, 7 }, { 6, 7 }, { 7, 7 }, { 8, 7 }, { 8, 8 }, { 8, 9 }, { 7, 9 }, { 6, 9 }, { 5, 9 }, { 4, 9 }, { 4, 8 } }),
                4, 7);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 8, 9 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 6, true,
                new int[12, 2] { { 4, 7 }, { 5, 7 }, { 6, 7 }, { 7, 7 }, { 8, 7 }, { 8, 8 }, { 8, 9 }, { 7, 9 }, { 6, 9 }, { 5, 9 }, { 4, 9 }, { 4, 8 } }),
                8, 9);



            PPObject[,] ground2 = new PPObject[room2[2].GetObjects().GetLength(0), room2[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground2.GetLength(0); r++)
            {
                for (int c = 0; c < ground2.GetLength(1); c++)
                {

                    ground2[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer3 = new Grid(ground2);
            Level1.GetFloor(1).ReplaceLayer(2, layer3);


            Grid[] room3 = Level1.GetFloor(2).GetLayers();
            room3[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 8, 5 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 8, 4 }),
                8, 5);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 4 }, wallTxt, 0.5f, 0.0f),
                            2, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 4 }, wallTxt, 0.5f, 0.0f),
                            3, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 4 }, wallTxt, 0.5f, 0.0f),
                            4, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 4 }, wallTxt, 0.5f, 0.0f),
                            5, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 4 }, wallTxt, 0.5f, 0.0f),
                            6, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 7, 4 }, wallTxt, 0.5f, 0.0f),
                            7, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 8, 4 }, wallTxt, 0.5f, 0.0f),
                            8, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 6 }, wallTxt, 0.5f, 0.0f),
                            1, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 6 }, wallTxt, 0.5f, 0.0f),
                            2, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 6 }, wallTxt, 0.5f, 0.0f),
                            3, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 6 }, wallTxt, 0.5f, 0.0f),
                            4, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 6 }, wallTxt, 0.5f, 0.0f),
                            6, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 7 }, wallTxt, 0.5f, 0.0f),
                            6, 7);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 8 }, wallTxt, 0.5f, 0.0f),
                            1, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 9 }, wallTxt, 0.5f, 0.0f),
                            1, 9);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 8 }, wallTxt, 0.5f, 0.0f),
                            3, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 8 }, wallTxt, 0.5f, 0.0f),
                           5, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 8 }, wallTxt, 0.5f, 0.0f),
                            6, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 9 }, wallTxt, 0.5f, 0.0f),
                            6, 9);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 10 }, wallTxt, 0.5f, 0.0f),
                            5, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 10 }, wallTxt, 0.5f, 0.0f),
                            3, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 10 }, wallTxt, 0.5f, 0.0f),
                           2, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 10 }, wallTxt, 0.5f, 0.0f),
                           1, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 11 }, wallTxt, 0.5f, 0.0f),
                           4, 11);
            room3[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 6, 5 }, doorTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key2", true),
                            6, 5);
            room3[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 4, 5 }, doorTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key3", true),
                            4, 5);
            room3[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 1, 7 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                new PPObject("key3", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                1, 7);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 3, 7 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                3, 7);
            room3[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 4, 8 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
                4, 9);
            room3[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 1, 12 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                new PPObject("key2", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                1, 12);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 2, 12 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                2, 12);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 6, 12 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                6, 12);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 7, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                7, 11);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 8, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                8, 11);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 8, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                8, 7);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 8, 11 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                7, 9);
           // room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, goldBar, 0.5f, 0.0f, 100),
          //      1, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 1 }, goldBar, 0.5f, 0.0f, 100),
                2, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 1 }, goldBar, 0.5f, 0.0f, 100),
                3, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 1 }, goldBar, 0.5f, 0.0f, 100),
                4, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 5, 1 }, goldBar, 0.5f, 0.0f, 100),
                5, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 6, 1 }, goldBar, 0.5f, 0.0f, 100),
                6, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 1 }, goldBar, 0.5f, 0.0f, 100),
                7, 1);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 1 }, goldBar, 0.5f, 0.0f, 100),
                8, 1);
            //room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 2 }, goldBar, 0.5f, 0.0f, 100),
            //    1, 2);
            //room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 3 }, goldBar, 0.5f, 0.0f, 100),
            //    1, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 3 }, goldBar, 0.5f, 0.0f, 100),
                2, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 3 }, goldBar, 0.5f, 0.0f, 100),
                3, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 3 }, goldBar, 0.5f, 0.0f, 100),
                4, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 5, 3 }, goldBar, 0.5f, 0.0f, 100),
                5, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 6, 3 }, goldBar, 0.5f, 0.0f, 100),
                6, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 3 }, goldBar, 0.5f, 0.0f, 100),
                7, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 3 }, goldBar, 0.5f, 0.0f, 100),
                8, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 2 }, goldBar, 0.5f, 0.0f, 100),
                8, 2);
            room3[1].SetGridObject(new Chest("finalchest", "finalchest", new Barrier(false), new Vector2(), new int[2] { 5, 2 }, rchestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1000),
                5, 2);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32),
                new int[6, 2] { { 1,1}, { 2,1}, {3,1 }, {4,1 }, {5,1 }, { 6,1} }),
                1, 1);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 3 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32),
               new int[6, 2] { { 1, 3 }, { 2, 3 }, { 3, 3 }, { 4, 3 }, { 5, 3 }, { 6, 3 } }),
               1, 3);

            PPObject[,] ground3 = new PPObject[room3[2].GetObjects().GetLength(0), room3[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground3.GetLength(0); r++)
            {
                for (int c = 0; c < ground3.GetLength(1); c++)
                {

                    ground3[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer4 = new Grid(ground3);
            Level1.GetFloor(2).ReplaceLayer(2, layer4);


            return Level1;
        }

        // written out placement of every object for every room in a Pyramid
        public Pyramid CreatePyramid2()
        {
            Pyramid Level1 = new Pyramid("level2", new Room[3] {
                new Room(new Grid[3] { new Grid(6, 8), new Grid(6, 8), new Grid(6, 8) }),
                new Room(new Grid[3] { new Grid(8, 11), new Grid(8, 11), new Grid(8, 11) }),
                new Room(new Grid[3] { new Grid(10, 14), new Grid(10, 14), new Grid(10, 14) })
            });
            Level1.SetWalls(Level1.GetRooms()[0], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[1], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[2], wallTxt); // fills the outside of the room with walls

            Grid[] room1 = Level1.GetFloor(0).GetLayers();
            room1[1].SetGridObject(new HiddenStairs("finalstairs", "hiddenstairs", new Barrier(true), new Vector2(), new int[2] { 2, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 3, 8 }),
                2, 1);
            room1[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 1, 3 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
                1, 3);
            room1[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 2, 2 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
               2, 2);
            room1[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 4, 6 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 5, 9 }),
                4, 6);
            room1[1].SetGridObject(new PPObject("hole", "hole", new Barrier(false), new Vector2(), new int[2] { 4, 5 }, holeTx, 0.5f, 0.0f),
                4, 5);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 6 }, wallTxt, 0.5f, 0.0f),
                3, 6);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 5 }, wallTxt, 0.5f, 0.0f),
                3, 5);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 1 }, wallTxt, 0.5f, 0.0f),
                3, 1);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 2 }, wallTxt, 0.5f, 0.0f),
                3, 2);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 1 }, wallTxt, 0.5f, 0.0f),
                4, 1);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 2 }, wallTxt, 0.5f, 0.0f),
                4, 2);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 3 }, goldBar, 0.5f, 0.0f, 100),
                4, 3);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 6 }, goldBar, 0.5f, 0.0f, 100),
                1, 6);
            room1[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 6 }, goldBar, 0.5f, 0.0f, 100),
                2, 6);
            room1[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 2, 2 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[5, 2] { { 2, 2 }, { 2, 3 }, { 2, 4 }, { 2, 5 }, { 2, 6 } }),
              2, 2);

            PPObject[,] ground = new PPObject[room1[2].GetObjects().GetLength(0), room1[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground.GetLength(0); r++)
            {
                for (int c = 0; c < ground.GetLength(1); c++)
                {

                    ground[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer2 = new Grid(ground);
            Level1.GetFloor(0).ReplaceLayer(2, layer2);

            Grid[] room2 = Level1.GetFloor(1).GetLayers();
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 6, 9 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 4, 5 }),
                6, 9);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 8 }, wallTxt, 0.5f, 0.0f),
                 6, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 7 }, wallTxt, 0.5f, 0.0f),
                 1, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 7 }, wallTxt, 0.5f, 0.0f),
                 2, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 8 }, wallTxt, 0.5f, 0.0f),
                 3, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 9 }, wallTxt, 0.5f, 0.0f),
                 3, 9);
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 9 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 2, 11 }),
                1, 9);
            room2[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 1, 8 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                 new PPObject("key4", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keygTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                1, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 7 }, wallTxt, 0.5f, 0.0f),
                 6, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 7 }, wallTxt, 0.5f, 0.0f),
                 5, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 6 }, wallTxt, 0.5f, 0.0f),
                 5, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 5 }, wallTxt, 0.5f, 0.0f),
                 5, 5);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 4 }, wallTxt, 0.5f, 0.0f),
                 5, 4);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 3 }, wallTxt, 0.5f, 0.0f),
                 5, 3);
            room2[1].SetGridObject(new PPObject("hole", "hole", new Barrier(false), new Vector2(), new int[2] { 4, 3 }, holeTx, 0.5f, 0.0f),
                 4, 3);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 3 }, wallTxt, 0.5f, 0.0f),
                 3, 3);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 3 }, wallTxt, 0.5f, 0.0f),
                 2, 3);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 3 }, wallTxt, 0.5f, 0.0f),
                 1, 3);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 5 }, wallTxt, 0.5f, 0.0f),
                 2, 5);
            room2[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 3, 6 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
               3, 6);
            room2[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 3, 4 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                3, 4);
            room2[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 3, 5 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                3, 5);
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 6, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 7, 1 }),
                6, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, goldBar, 0.5f, 0.0f, 100),
                1, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 1 }, goldBar, 0.5f, 0.0f, 100),
                2, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 1 }, goldBar, 0.5f, 0.0f, 100),
                3, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 2 }, goldBar, 0.5f, 0.0f, 100),
                1, 2);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 2, 2 }, goldBar, 0.5f, 0.0f, 100),
                2, 2);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 2 }, goldBar, 0.5f, 0.0f, 100),
                3, 2);
            room2[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 6, 6 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               6, 6);

            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 3, 6 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, true,
              new int[4, 2] { { 3, 6 }, { 3, 5 }, { 4, 5 }, { 4, 6 } }),
              3, 6);



            PPObject[,] ground2 = new PPObject[room2[2].GetObjects().GetLength(0), room2[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground2.GetLength(0); r++)
            {
                for (int c = 0; c < ground2.GetLength(1); c++)
                {

                    ground2[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer3 = new Grid(ground2);
            Level1.GetFloor(1).ReplaceLayer(2, layer3);


            Grid[] room3 = Level1.GetFloor(2).GetLayers();
            room3[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 8, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 6, 2 }),
                8, 1);
            room3[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 8, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 2, 9 }),
                2, 12);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 8, 2 }, wallTxt, 0.5f, 0.0f),
                8, 2);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 7, 2 }, wallTxt, 0.5f, 0.0f),
                7, 2);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 2 }, wallTxt, 0.5f, 0.0f),
                6, 2);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 2 }, wallTxt, 0.5f, 0.0f),
                3, 2);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 2 }, wallTxt, 0.5f, 0.0f),
                2, 2);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 3 }, wallTxt, 0.5f, 0.0f),
                2, 3);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 4 }, wallTxt, 0.5f, 0.0f),
                2, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 5 }, wallTxt, 0.5f, 0.0f),
                2, 5);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 6 }, wallTxt, 0.5f, 0.0f),
                2, 6);
            room3[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 3, 8 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
               3, 8);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 1, 8 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                1, 8);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 1, 9 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                1, 9);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 8 }, wallTxt, 0.5f, 0.0f),
                2, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 9 }, wallTxt, 0.5f, 0.0f),
                2, 9);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 10 }, wallTxt, 0.5f, 0.0f),
                2, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 10 }, wallTxt, 0.5f, 0.0f),
                3, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 11 }, wallTxt, 0.5f, 0.0f),
                3, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 12 }, wallTxt, 0.5f, 0.0f),
                3, 12);
            room3[1].SetGridObject(new Chest("finalchest", "finalchest", new Barrier(false), new Vector2(), new int[2] { 8, 12 }, rchestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 2000),
                8, 12);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 11 }, goldBar, 0.5f, 0.0f, 100),
               8, 11);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 11 }, goldBar, 0.5f, 0.0f, 100),
               7, 11);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 12 }, goldBar, 0.5f, 0.0f, 100),
               7, 12);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 8, 10 }, wallTxt, 0.5f, 0.0f),
                8, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 7, 10 }, wallTxt, 0.5f, 0.0f),
                7, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 10 }, wallTxt, 0.5f, 0.0f),
                6, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 11 }, wallTxt, 0.5f, 0.0f),
                6, 11);
            room3[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 6, 12 }, doorTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key5", true),
                6, 12);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 4, 12 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               4, 12);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 4, 11 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               4, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 10 }, wallTxt, 0.5f, 0.0f),
                4, 10);
            room3[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 5, 10 }, doorrTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key4", true),
                5, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 7 }, wallTxt, 0.5f, 0.0f),
                5, 7);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 7 }, wallTxt, 0.5f, 0.0f),
                6, 7);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 6 }, wallTxt, 0.5f, 0.0f),
                5, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 8 }, wallTxt, 0.5f, 0.0f),
                5, 8);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 4 }, goldBar, 0.5f, 0.0f, 100),
                3, 4);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 3 }, goldBar, 0.5f, 0.0f, 100),
               4, 3);
            room3[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 8, 3 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                new PPObject("key5", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                8, 3);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 3, 3 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               3, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 3 }, goldBar, 0.5f, 0.0f, 100),
                7, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 4 }, goldBar, 0.5f, 0.0f, 100),
               8, 4);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 4, 6 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                4, 6);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 3, 6 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
                3, 6);
            room3[1].SetGridObject(new Trap("floortrap", "trap", new Barrier(true), new Vector2(), new int[2] { 8, 7 }, floorSpike, 0.5f, 0.0f, new Rectangle(32, 0, 32, 32), 1),
               8, 7);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 8, 9 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               8, 9);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 9 }, goldBar, 0.5f, 0.0f, 100),
                7, 9);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 8 }, goldBar, 0.5f, 0.0f, 100),
               8, 8);

            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 4, 5 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, true,
              new int[14, 2] { { 4, 5 }, { 5, 5 }, { 6, 5 }, { 7, 5 }, { 7, 6 }, { 7, 7 }, { 7, 8 }, { 7, 9 },
              { 6, 9 }, { 5, 9 }, { 4, 9 }, { 4, 8 }, { 4, 7 }, { 4, 6 } }),
              4, 5);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 7, 9 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 7, true,
              new int[14, 2] { { 4, 5 }, { 5, 5 }, { 6, 5 }, { 7, 5 }, { 7, 6 }, { 7, 7 }, { 7, 8 }, { 7, 9 },
              { 6, 9 }, { 5, 9 }, { 4, 9 }, { 4, 8 }, { 4, 7 }, { 4, 6 } }),
              7, 9);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 6, 4 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 3, true,
              new int[20, 2] { { 3, 4 }, { 4, 4 }, { 5, 4 }, { 6, 4 }, { 7, 4 }, { 8, 4 }, { 8, 5 }, { 8, 6 },
              { 8, 7 }, { 8, 8 }, { 7, 8 }, { 6, 8 }, { 6, 9 }, { 5, 9 }, { 4, 9 }, { 3, 9 }, { 3, 8 }, { 3, 7 }, { 3, 6 }, { 3, 5 } }),
              6, 4);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 5, 9 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 13, true,
              new int[20, 2] { { 3, 4 }, { 4, 4 }, { 5, 4 }, { 6, 4 }, { 7, 4 }, { 8, 4 }, { 8, 5 }, { 8, 6 },
              { 8, 7 }, { 8, 8 }, { 7, 8 }, { 6, 8 }, { 6, 9 }, { 5, 9 }, { 4, 9 }, { 3, 9 }, { 3, 8 }, { 3, 7 }, { 3, 6 }, { 3, 5 } }),
              5, 9);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32),
                new int[7, 2] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 } }),
                1, 1);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 2 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 1, false,
                new int[7, 2] { { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 5 }, { 1, 6 }, { 1, 7 } }),
                1, 2);






            PPObject[,] ground3 = new PPObject[room3[2].GetObjects().GetLength(0), room3[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground3.GetLength(0); r++)
            {
                for (int c = 0; c < ground3.GetLength(1); c++)
                {

                    ground3[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            Grid layer4 = new Grid(ground3);
            Level1.GetFloor(2).ReplaceLayer(2, layer4);


            return Level1;
        }

        // written out placement of every object for every room in a Pyramid
        public Pyramid CreatePyramid3()
        {
            Pyramid Level1 = new Pyramid("level3", new Room[3] {
                new Room(new Grid[3] { new Grid(6, 8), new Grid(6, 8), new Grid(6, 8) }),
                new Room(new Grid[3] { new Grid(8, 11), new Grid(8, 11), new Grid(8, 11) }),
                new Room(new Grid[5] { new Grid(10, 14), new Grid(10, 14), new Grid(10, 14), new Grid(10, 14), new Grid(10, 14) })
            });
            Level1.SetWalls(Level1.GetRooms()[0], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[1], wallTxt); // fills the outside of the room with walls
            Level1.SetWalls(Level1.GetRooms()[2], wallTxt); // fills the outside of the room with walls

            Grid[] room1 = Level1.GetFloor(0).GetLayers();
            room1[1].SetGridObject(new HiddenStairs("finalstairs", "hiddenstairs", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 3, 8 }),
                1, 1);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 0, 3 }, holewTx, 0.5f, 0.0f),
                0, 3);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 0, 3 }, holewTx, 0.5f, 0.0f),
                5, 4);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 1 }, wallTxt, 0.5f, 0.0f),
                4, 1);
            room1[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 4, 2 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               4, 2);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 2 }, wallTxt, 0.5f, 0.0f),
                1, 2);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 2 }, wallTxt, 0.5f, 0.0f),
                2, 2);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 5 }, wallTxt, 0.5f, 0.0f),
                1, 5);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 5 }, wallTxt, 0.5f, 0.0f),
                2, 5);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 5 }, wallTxt, 0.5f, 0.0f),
                4, 5);
            room1[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 6 }, wallTxt, 0.5f, 0.0f),
                4, 6);
            room1[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 6 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 2, 9 }),
                1, 6);



            room1[0].SetGridObject(new Arrow("trap1", "arrow", new Barrier(true), new Vector2(), new int[2] { 0, 3 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[5, 2] { { 0, 3 }, { 1, 3 }, { 2, 3 }, { 3, 3 }, { 4, 3 } }),
              0, 3);
            room1[0].SetGridObject(new Arrow("trap1", "arrow", new Barrier(true), new Vector2(), new int[2] { 5, 4 }, arrowTx, 0.91f, 0.0f, new Rectangle(32, 0, 32, 32), 0, false,
              new int[5, 2] { { 5, 4 }, { 4, 4 }, { 3, 4 }, { 2, 4 }, { 1, 4 } }),
              5, 4);
            room1[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 3 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, true,
              new int[8, 2] { { 1, 3 }, { 2, 3 }, { 3, 3 }, { 4, 3 }, { 4, 4 }, { 3, 4 }, { 2, 4 }, { 1, 4 } }),
              1, 3);


            PPObject[,] ground = new PPObject[room1[2].GetObjects().GetLength(0), room1[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground.GetLength(0); r++)
            {
                for (int c = 0; c < ground.GetLength(1); c++)
                {

                    ground[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            ground[3, 2] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 3, 2 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap1");
            Grid layer2 = new Grid(ground);
            Level1.GetFloor(0).ReplaceLayer(2, layer2);



            Grid[] room2 = Level1.GetFloor(1).GetLayers();
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 9 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 2, 6 }),
                1, 9);
            room2[1].SetGridObject(new Door("door", "door", new Barrier(false), new Vector2(), new int[2] { 1, 5 }, doorrTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), "key6", true),
                1, 5);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 8 }, wallTxt, 0.5f, 0.0f),
                 2, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 7 }, holewTx, 0.5f, 0.0f),
                 2, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 6 }, holewTx, 0.5f, 0.0f),
                 2, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 5 }, holewTx, 0.5f, 0.0f),
                 2, 5);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 4 }, wallTxt, 0.5f, 0.0f),
                 2, 4);
            room2[1].SetGridObject(new PPObject("hole", "hole", new Barrier(false), new Vector2(), new int[2] { 1, 3 }, holeTx, 0.5f, 0.0f),
                 1, 3);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 4 }, wallTxt, 0.5f, 0.0f),
                 3, 4);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 2 }, wallTxt, 0.5f, 0.0f),
                 2, 2);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 1 }, wallTxt, 0.5f, 0.0f),
                 2, 1);
            room2[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), true, new int[2] { 1, 2 }),
                1, 1);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 8 }, wallTxt, 0.5f, 0.0f),
                3, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 8 }, wallTxt, 0.5f, 0.0f),
                 5, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 9 }, wallTxt, 0.5f, 0.0f),
                 5, 9);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 8 }, wallTxt, 0.5f, 0.0f),
                 6, 8);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 7 }, holewTx, 0.5f, 0.0f),
                 6, 7);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 6 }, holewTx, 0.5f, 0.0f),
                 6, 6);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 5 }, holewTx, 0.5f, 0.0f),
                 6, 5);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 4 }, wallTxt, 0.5f, 0.0f),
                 6, 4);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 4 }, wallTxt, 0.5f, 0.0f),
                 5, 4);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 6, 9 }, wallTxt, 0.5f, 0.0f),
                 6, 9);
            room2[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 1, 8 }, wallTxt, 0.5f, 0.0f),
                 1, 8);
            room2[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 5, 2 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
               5, 2);
            room2[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 6, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               6, 1);
            room2[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 3, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               3, 1);
            room2[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 1, 7 }, rchestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 500),
               1, 7);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 1, 6 }, goldBar, 0.5f, 0.0f, 100),
                1, 6);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 1 }, goldBar, 0.5f, 0.0f, 100),
                4, 1);
            room2[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 5, 1 }, goldBar, 0.5f, 0.0f, 100),
                5, 1);


            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 2, 7 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 2, 7 }, { 3, 7 }, { 4, 7 }, { 5, 7 } }),
              2, 7);
            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 2, 6 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 2, 6 }, { 3, 6 }, { 4, 6 }, { 5, 6 } }),
              2, 6);
            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 2, 5 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 2, 5 }, { 3, 5 }, { 4, 5 }, { 5, 5 } }),
              2, 5);

            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 7 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 6, 7 }, { 5, 7 }, { 4, 7 }, { 3, 7 } }),
              6, 7);
            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 6 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 6, 6 }, { 5, 6 }, { 4, 6 }, { 3, 6 } }),
              6, 6);
            room2[0].SetGridObject(new Arrow("trap2", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 5 }, arrowTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[4, 2] { { 6, 5 }, { 5, 5 }, { 4, 5 }, { 3, 5 } }),
              6, 5);

            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 3, 5 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[3, 2] { { 3, 5 }, { 3, 6 }, { 3, 7 } }),
              3, 5);
            room2[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 3, 5 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[3, 2] { { 5, 7 }, { 5, 6 }, { 5, 5 } }),
              5, 7);


            PPObject[,] ground2 = new PPObject[room2[2].GetObjects().GetLength(0), room2[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground2.GetLength(0); r++)
            {
                for (int c = 0; c < ground2.GetLength(1); c++)
                {

                    ground2[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            ground2[4, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 4, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap2");
            Grid layer3 = new Grid(ground2);
            Level1.GetFloor(1).ReplaceLayer(2, layer3);


            Grid[] room3 = Level1.GetFloor(2).GetLayers();
            room3[1].SetGridObject(new Stairs("stair", "stair", new Barrier(true), new Vector2(), new int[2] { 1, 1 }, stairsTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), false, new int[2] { 1, 2 }),
                1, 1);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 1 }, wallTxt, 0.5f, 0.0f),
                2, 1);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 2 }, wallTxt, 0.5f, 0.0f),
                2, 2);
            room3[1].SetGridObject(new PPObject("hole", "hole", new Barrier(false), new Vector2(), new int[2] { 2, 3 }, holeTx, 0.5f, 0.0f),
                 2, 3);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 4 }, wallTxt, 0.5f, 0.0f),
                2, 4);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 5 }, wallTxt, 0.5f, 0.0f),
                2, 5);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 6 }, wallTxt, 0.5f, 0.0f),
                2, 6);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 7 }, wallTxt, 0.5f, 0.0f),
                2, 7);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 8 }, wallTxt, 0.5f, 0.0f),
                2, 8);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 9 }, wallTxt, 0.5f, 0.0f),
                2, 9);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 10 }, wallTxt, 0.5f, 0.0f),
                2, 10);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 2, 11 }, wallTxt, 0.5f, 0.0f),
                2, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 11 }, wallTxt, 0.5f, 0.0f),
                3, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 4, 11 }, wallTxt, 0.5f, 0.0f),
                4, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 5, 11 }, wallTxt, 0.5f, 0.0f),
                5, 11);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 7, 11 }, wallTxt, 0.5f, 0.0f),
                7, 11);
            
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 8, 11 }, wallTxt, 0.5f, 0.0f),
                8, 11);

            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 3, 0);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 4, 0);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 5, 0);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 6, 0);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 7, 0);
            room3[1].SetGridObject(new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { 3, 0 }, holewTx, 0.5f, 0.0f),
                 8, 0);

            room3[1].SetGridObject(new Movable("rock", "movable", new Barrier(false), new Vector2(), new int[2] { 3, 3 }, rockTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32)),
               3, 3);

            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 10 }, goldBar, 0.5f, 0.0f, 100),
               3, 10);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 10 }, goldBar, 0.5f, 0.0f, 100),
               4, 10);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 5, 10 }, goldBar, 0.5f, 0.0f, 100),
               5, 10);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 9 }, goldBar, 0.5f, 0.0f, 100),
               3, 9);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 9 }, goldBar, 0.5f, 0.0f, 100),
               4, 9);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 10 }, goldBar, 0.5f, 0.0f, 100),
               8, 10);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 10 }, goldBar, 0.5f, 0.0f, 100),
               7, 10);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 9 }, goldBar, 0.5f, 0.0f, 100),
               8, 9);
            room3[1].SetGridObject(new Chest("finalchest", "finalchest", new Barrier(false), new Vector2(), new int[2] { 8, 12 }, rchestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 3000),
                8, 12);
            room3[1].SetGridObject(new CrackedWall("crackedwall", "crackedwall", new Barrier(false), new Vector2(), new int[2] { 6, 11 }, wcrackTx, 0.5f, 0.0f),
                6, 11);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 3, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               3, 1);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 4, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               4, 1);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 5, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               5, 1);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 6, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               6, 1);
            room3[1].SetGridObject(new ObjectChest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 7, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32),
                new PPObject("key6", "key", new Barrier(false), new Vector2(), new int[2] { 0, 0 }, keyTx, 1.0f, 2.356f, new Rectangle(0, 0, 32, 32))),
                7, 1);
            room3[1].SetGridObject(new Chest("chest", "chest", new Barrier(false), new Vector2(), new int[2] { 8, 1 }, chestTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 250),
               8, 1);

            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 5 }, goldBar, 0.5f, 0.0f, 100),
               8, 5);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 5 }, goldBar, 0.5f, 0.0f, 100),
               7, 5);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 4 }, goldBar, 0.5f, 0.0f, 100),
               8, 4);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 7, 4 }, goldBar, 0.5f, 0.0f, 100),
               7, 4);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 8, 3 }, goldBar, 0.5f, 0.0f, 100),
               8, 3);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 5 }, goldBar, 0.5f, 0.0f, 100),
               3, 5);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 4, 5 }, goldBar, 0.5f, 0.0f, 100),
               4, 5);
            room3[1].SetGridObject(new Gold("goldbar", "gold", new Barrier(true), new Vector2(), new int[2] { 3, 4 }, goldBar, 0.5f, 0.0f, 100),
               3, 4);




            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 1, 12 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, false,
              new int[7, 2] { { 1, 12 }, { 2, 12 }, { 3, 12 }, { 4, 12 }, { 5, 12 }, { 6, 12 }, { 7, 12 } }),
              1, 12);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 5, 2 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 2, false,
              new int[6, 2] { { 3, 2 }, { 4, 2 }, { 5, 2 }, { 6, 2 }, { 7, 2 }, { 8, 2 } }),
              5, 2);

            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] { 3, 4 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 0, true,
              new int[20, 2] { { 3, 4 }, { 4, 4 }, { 5, 4 }, { 6, 4 }, { 7, 4 }, { 8, 4 }, { 7, 5 }
              , { 6, 6 }, { 5, 7 }, { 4, 8 }, { 3, 9 }, { 4, 9 }, { 5, 9 }, { 6, 9 }, { 7, 9 }, { 8, 9 }
                  , { 7, 8 }, { 6, 7 }, { 5, 6 }, { 4, 5 }}),
              3, 4);
            room3[0].SetGridObject(new Enemy("skele", "enemy", new Barrier(true), new Vector2(), new int[2] {7, 9 }, skeletonTx, 0.91f, 0.0f, new Rectangle(0, 0, 32, 32), 14, true,
              new int[20, 2] { { 3, 4 }, { 4, 4 }, { 5, 4 }, { 6, 4 }, { 7, 4 }, { 8, 4 }, { 7, 5 }
              , { 6, 6 }, { 5, 7 }, { 4, 8 }, { 3, 9 }, { 4, 9 }, { 5, 9 }, { 6, 9 }, { 7, 9 }, { 8, 9 }
                  , { 7, 8 }, { 6, 7 }, { 5, 6 }, { 4, 5 }}),
              7, 9);


            room3[3].SetGridObject(new Arrow("trap3", "arrow", new Barrier(true), new Vector2(), new int[2] { 4, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 4, 6 }, { 4, 7 }, { 4, 8 }, { 4, 9 }, { 4, 10 } }),
              4, 0);
            room3[3].SetGridObject(new Arrow("trap3", "arrow", new Barrier(true), new Vector2(), new int[2] { 5, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 }, { 5, 6 }, { 5, 7 }, { 5, 8 }, { 5, 9 }, { 5, 10 } }),
              5, 0);
            room3[3].SetGridObject(new Arrow("trap3", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[13, 2] { { 6, 0 }, { 6, 1 }, { 6, 2 }, { 6, 3 }, { 6, 4 }, { 6, 5 }, { 6, 6 }, { 6, 7 }, { 6, 8 }, { 6, 9 }, { 6, 10 }, { 6, 11 }, { 6, 12 } }),
              6, 0);
            room3[3].SetGridObject(new Arrow("trap3", "arrow", new Barrier(true), new Vector2(), new int[2] { 7, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 7, 0 }, { 7, 1 }, { 7, 2 }, { 7, 3 }, { 7, 4 }, { 7, 5 }, { 7, 6 }, { 7, 7 }, { 7, 8 }, { 7, 9 }, { 7, 10 } }),
              7, 0);
            room3[4].SetGridObject(new Arrow("trap3", "arrow", new Barrier(true), new Vector2(), new int[2] { 8, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 8, 0 }, { 8, 1 }, { 8, 2 }, { 8, 3 }, { 8, 4 }, { 8, 5 }, { 8, 6 }, { 8, 7 }, { 8, 8 }, { 8, 9 }, { 8, 10 } }),
              8, 0);

            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 3, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 3, 4 }, { 3, 5 }, { 3, 6 }, { 3, 7 }, { 3, 8 }, { 3, 9 }, { 3, 10 } }),
              3, 0);
            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 4, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 4, 6 }, { 4, 7 }, { 4, 8 }, { 4, 9 }, { 4, 10 } }),
              4, 0);
            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 5, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 }, { 5, 6 }, { 5, 7 }, { 5, 8 }, { 5, 9 }, { 5, 10 } }),
              5, 0);
            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[13, 2] { { 6, 0 }, { 6, 1 }, { 6, 2 }, { 6, 3 }, { 6, 4 }, { 6, 5 }, { 6, 6 }, { 6, 7 }, { 6, 8 }, { 6, 9 }, { 6, 10 }, { 6, 11 }, { 6, 12 } }),
              6, 0);
            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 7, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 7, 0 }, { 7, 1 }, { 7, 2 }, { 7, 3 }, { 7, 4 }, { 7, 5 }, { 7, 6 }, { 7, 7 }, { 7, 8 }, { 7, 9 }, { 7, 10 } }),
              7, 0);
            room3[0].SetGridObject(new Arrow("trap4", "arrow", new Barrier(true), new Vector2(), new int[2] { 8, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 8, 0 }, { 8, 1 }, { 8, 2 }, { 8, 3 }, { 8, 4 }, { 8, 5 }, { 8, 6 }, { 8, 7 }, { 8, 8 }, { 8, 9 }, { 8, 10 } }),
              8, 0);

            room3[4].SetGridObject(new Arrow("trap5", "arrow", new Barrier(true), new Vector2(), new int[2] { 3, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 3, 0 }, { 3, 1 }, { 3, 2 }, { 3, 3 }, { 3, 4 }, { 3, 5 }, { 3, 6 }, { 3, 7 }, { 3, 8 }, { 3, 9 }, { 3, 10 } }),
              3, 0);
            room3[4].SetGridObject(new Arrow("trap5", "arrow", new Barrier(true), new Vector2(), new int[2] { 4, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 4, 0 }, { 4, 1 }, { 4, 2 }, { 4, 3 }, { 4, 4 }, { 4, 5 }, { 4, 6 }, { 4, 7 }, { 4, 8 }, { 4, 9 }, { 4, 10 } }),
              4, 0);
            room3[4].SetGridObject(new Arrow("trap5", "arrow", new Barrier(true), new Vector2(), new int[2] { 5, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 5, 0 }, { 5, 1 }, { 5, 2 }, { 5, 3 }, { 5, 4 }, { 5, 5 }, { 5, 6 }, { 5, 7 }, { 5, 8 }, { 5, 9 }, { 5, 10 } }),
              5, 0);
            room3[4].SetGridObject(new Arrow("trap5", "arrow", new Barrier(true), new Vector2(), new int[2] { 6, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[13, 2] { { 6, 0 }, { 6, 1 }, { 6, 2 }, { 6, 3 }, { 6, 4 }, { 6, 5 }, { 6, 6 }, { 6, 7 }, { 6, 8 }, { 6, 9 }, { 6, 10 }, { 6, 11 }, { 6, 12 } }),
              6, 0);
            room3[4].SetGridObject(new Arrow("trap5", "arrow", new Barrier(true), new Vector2(), new int[2] { 8, 0 }, arrowTx, 0.91f, 0.0f, new Rectangle(64, 0, 32, 32), 0, false,
              new int[11, 2] { { 8, 0 }, { 8, 1 }, { 8, 2 }, { 8, 3 }, { 8, 4 }, { 8, 5 }, { 8, 6 }, { 8, 7 }, { 8, 8 }, { 8, 9 }, { 8, 10 } }),
              8, 0);



            PPObject[,] ground3 = new PPObject[room3[2].GetObjects().GetLength(0), room3[2].GetObjects().GetLength(1)];
            for (int r = 0; r < ground3.GetLength(0); r++)
            {
                for (int c = 0; c < ground3.GetLength(1); c++)
                {

                    ground3[r, c] = new PPObject("ground", "ground", new Barrier(true), new Vector2(), new int[2] { r, c }, sandStuff, 0.0f, 0.0f, new Rectangle(64, 180, 32, 32));

                }
            }
            ground3[4, 9] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 4, 9 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap3");
            ground3[7, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 7, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap4");
            ground3[8, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 8, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap4");
            ground3[4, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 4, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap4");
            ground3[3, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 3, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap4");
            ground3[6, 6] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 6, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap4");
            ground3[8, 4] = new FloorTrap("foottrap", "foottrap", new Barrier(true), new Vector2(), new int[2] { 6, 6 }, foottrapTx, 0.5f, 0.0f, new Rectangle(0, 0, 32, 32), 1, "trap5");

            Grid layer4 = new Grid(ground3);
            Level1.GetFloor(2).ReplaceLayer(2, layer4);


            return Level1;
        }
    }

    // class for handling if object is able to be walked on
public class Barrier
    {
        private bool[] blocked = new bool[4];

        public Barrier(bool[] bp)
        {
            blocked = bp;
        }

        public Barrier(bool bp)
        {
            blocked = new bool[4] { bp,bp,bp,bp};
        }

        public bool IsBlocked(String dir)
        {
            if (dir == "left")
            {
                return blocked[0];
            }
            else if (dir == "right")
            {
                return blocked[1];
            }
            else if (dir == "up")
            {
                return blocked[2];
            }
            else if (dir == "down")
            {
                return blocked[3];
            }
            return false;
        }
    }

    // base call for all objects
    public class PPObject
    {
        public Vector2 position { get; set; }
        public Barrier walkable;
        public String name { get; set; }
        public String classname { get; set; }
        public Texture2D image { get; set; }
        public Rectangle? rect { get; set; }
        public float depth { get; set; }
        public float rotation { get; set; }
        public int[] gridloc { get; set; }

        public PPObject()
        {
            name = "";
            classname = "";
            walkable = new Barrier(new bool[4] { false, false, false, false });
            position = new Vector2();
            image = null;
            depth = 0.0f;
            rotation = 0.0f;
            rect = null;
        }

        public PPObject(String nam, String classn, Barrier walka, Vector2 pos,int[] gl, Texture2D ima, float dep, float rot)
        {
            name = nam;
            classname = classn;
            walkable = walka;
            position = pos;
            gridloc = gl;
            image = ima;
            depth = dep;
            rotation = rot;
            rect = null;
        }

        public PPObject(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec)
        {
            name = nam;
            classname = classn;
            walkable = walka;
            position = pos;
            gridloc = gl;
            image = ima;
            depth = dep;
            rotation = rot;
            rect = rec;
        }

        public void SetImage(Texture2D ima, Rectangle? rec)
        {
            image = ima;
            rect = rec;
        }

        public void SetRotation(float rot)
        {
            rotation = rot;
        }

    }

    // player object
    public class Player : PPObject
    {
        public int health { get; set; }
        public int score { get; set; }
        public int floor { get; set; }
        public int[] playerLocation { get; set; }
        public int[] newLocation { get; set; }
        public Vector2 newpos { get; set; }
        public Vector2 oldpos { get; set; }
        public int interpolate { get; set; }
        public bool moving { get; set; }
        public bool transforing { get; set; }
        public String direction { get; set; }
        public PPObject[] inventory = new PPObject[24];


        public Player(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int[] loc)
            : base(nam, classn, walka,pos,gl,ima,dep, rot, rec)
        {
            health = 3;
            score = 0;
            floor = 0;
            playerLocation = loc;
            newLocation = loc;
            newpos = pos;
            oldpos = pos;
            moving = false;
            transforing = false;
        }

        public Player(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep,float rot, int[] loc)
            : base(nam, classn, walka, pos,gl, ima, dep,rot)
        {
            health = 3;
            score = 0;
            floor = 0;
            playerLocation = loc;
            newLocation = loc;
            newpos = pos;
            oldpos = pos;
            moving = false;
            transforing = false;
        }

        public Player()
        {
            health = 3;
            score = 0;
            floor = 0;
            playerLocation = new int[2] { 0, 0 };
            newLocation = new int[2] { 0, 0 };
            newpos = new Vector2();
            oldpos = new Vector2();
            moving = false;
            transforing = false;
        }


        // used for Stairs
        public void SwapRooms(bool down)
        {
            transforing = true;
            if (down)
            {
                floor = floor + 1;
            }
            else
            {
                floor = floor - 1;
            }
        }

        public PPObject[] GetInv()
        {
            return inventory;
        }

        public void SetInv(PPObject[] invent)
        {
            inventory = invent;
        }

        // removes gaps in inventory list;
        public void ShiftInv()
        {
            PPObject[] newinv = new PPObject[24];
            int index = 0;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null)
                {
                    newinv[index] = inventory[i];
                    index++;
                   
                }
            }
            inventory = newinv;
        }

        public bool AddItem(PPObject item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(String nam)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].name == nam)
                {
                    inventory[i] = null;
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(String nam)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].name == nam)
                {
                    return true;
                }
            }
            return false;
        }

        public PPObject FetchItem(String nam)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].name == nam)
                {
                    return inventory[i];
                }
            }
            return null;
        }

        public void SetPosition(Vector2 npos)
        {
            position = npos;
            oldpos = npos;
            newpos = npos;
        }

        // check if player can walk here
        public bool CheckWakable(int[] spot,Room rom, String input)
        {
            Grid[] layers = rom.GetLayers();
            PPObject[,] layer = layers[0].GetObjects();

            if (spot[0] < 0 || spot[0] >= layer.GetLength(0) ||
                spot[1] < 0 || spot[1] >= layer.GetLength(1))
            {
                return false;
            }
            for (int l = 0; l < layers.Length; l++)
            {
                if (layers[l].GetGridObject(spot[0], spot[1]) != null && !(layers[l].GetGridObject(spot[0],spot[1]).walkable.IsBlocked(input)))
                {
                    return false;
                }
            }
            return true;
        }

        // function for telling the draw function to interpolate the players movement and where too.
        public String MovePlayer(String input, Room rom)
        {

            if (input == "left")
            {
                int[] spot = new int[2] { playerLocation[0],playerLocation[1]-1 };
                if (CheckWakable(spot, rom,input))
                {
                    
                    newLocation = spot;
                    newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
                    interpolate = 8;
                    direction = input;
                    moving = true;
                }
                else
                {
                    direction = input;
                    return "ui";
                }
            }
            else if (input == "right")
            {
                int[] spot = new int[2] { playerLocation[0], playerLocation[1]+1 };
                if (CheckWakable(spot, rom, input))
                {
                    
                    newLocation = spot;
                    newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
                    interpolate = 8;
                    direction = input;
                    moving = true;
                }
                else
                {
                    direction = input;
                    return "ui";
                }
            }
            else if (input == "up")
            {
                int[] spot = new int[2] { playerLocation[0]-1, playerLocation[1] };
                if (CheckWakable(spot, rom, input))
                {
                    
                    newLocation = spot;
                    newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
                    interpolate = 8;
                    direction = input;
                    moving = true;
                }
                else
                {
                    direction = input;
                    return "ui";
                }
            }
            else if (input == "down")
            {
                int[] spot = new int[2] { playerLocation[0]+1, playerLocation[1] };
                if (CheckWakable(spot, rom, input))
                {
                    
                    newLocation = spot;
                    newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
                    interpolate = 8;
                    direction = input;
                    moving = true;
                }
                else
                {
                    direction = input;
                    return "ui";
                }
            }
            return "";
        }


    }

    public class Gold : PPObject
    {
        public int value { get; set; }
        public bool looted { get; set; }


        public Gold(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int val)
            : base(nam, classn, walka,pos,gl,ima,dep,rot, rec)
        {
            value = val;
            looted = false;
           
        }

        public Gold(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int val)
            : base(nam, classn, walka, pos,gl, ima, dep,rot)
        {
           
            value = val;
            looted = false;
            
        }

        public void Collected(Player py)
        {
            py.score = py.score + value;
            looted = true;
            image = null;

        }
    }

    public class Movable : PPObject
    {
        public bool pushed { get; set; }


        public Movable(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            pushed = false;

        }

        public Movable(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot)
            : base(nam, classn, walka, pos, gl, ima, dep, rot)
        {

            pushed = false;

        }

        public virtual void Pushed(Player py)
        {
            pushed = true;
            //rect = new Rectangle(32, 0, 32, 32);

        }
    }

    // Enemies use a Path system to move, where the path is provided in the Constructor.
    public class Enemy : PPObject
    {
        public int[,] path { get; set; }
        public int cpath { get; set; }
        public bool active { get; set; }
        public int interpolate { get; set; }
        public int[] playerLocation { get; set; }
        public int[] newLocation { get; set; }
        public Vector2 newpos { get; set; }
        public Vector2 oldpos { get; set; }
        public bool moving { get; set; }
        public bool looppath { get; set; }
        public int direction { get; set; }


        public Enemy(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            active = false;
            interpolate = 0;
            cpath = 0;
            path = pat;
            looppath = false;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;
        }

        public Enemy(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int cpos, bool lp, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            active = false;
            interpolate = 0;
            cpath = cpos;
            path = pat;
            looppath = lp;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;
        }

        public Enemy(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot)
        {

            active = false;
            interpolate = 0;
            cpath = 0;
            path = pat;
            looppath = false;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;

        }

        public void SetPosition(Vector2 npos)
        {
            position = npos;
            oldpos = npos;
            newpos = npos;
        }

        public virtual void Trigger(Room rom)
        {
            active = true;

            SetPosition(rom.GetLayers()[2].GetGridObject(playerLocation[0], playerLocation[1]).position);
            if (cpath + direction > path.GetLength(0) - 1)
            {
                if (!looppath)
                {
                    direction = -1;
                }
                else
                {
                    cpath = -1;
                }
            }
            else if (cpath + direction < 0)
            {
                direction = 1;
            }
            cpath = cpath+direction;
            int[] spot = new int[2] { path[cpath, 0], path[cpath, 1] };
            newLocation = spot;
            newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
            interpolate = 10;
            moving = true;
            //rect = new Rectangle(32, 0, 32, 32);
        }
    }

    public class Arrow : PPObject
    {
        public int[,] path { get; set; }
        public int cpath { get; set; }
        public bool active { get; set; }
        public bool fired { get; set; }
        public int interpolate { get; set; }
        public int[] playerLocation { get; set; }
        public int[] newLocation { get; set; }
        public Vector2 newpos { get; set; }
        public Vector2 oldpos { get; set; }
        public bool moving { get; set; }
        public bool looppath { get; set; }
        public int direction { get; set; }


        public Arrow(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            active = false;
            fired = false;
            interpolate = 0;
            cpath = 0;
            path = pat;
            looppath = false;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;
        }

        public Arrow(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int cpos, bool lp, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            active = false;
            fired = false;
            interpolate = 0;
            cpath = cpos;
            path = pat;
            looppath = lp;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;
        }

        public Arrow(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int[,] pat)
            : base(nam, classn, walka, pos, gl, ima, dep, rot)
        {

            active = false;
            fired = false;
            interpolate = 0;
            cpath = 0;
            path = pat;
            looppath = false;
            playerLocation = gl;
            newLocation = gl;
            newpos = pos;
            oldpos = pos;
            moving = false;
            direction = 1;

        }

        public void SetPosition(Vector2 npos)
        {
            position = npos;
            oldpos = npos;
            newpos = npos;
        }

        public virtual void Trigger(Room rom)
        {
            active = true;

            SetPosition(rom.GetLayers()[2].GetGridObject(playerLocation[0], playerLocation[1]).position);
            if (cpath + direction > path.GetLength(0) - 1)
            {
                fired = true;
                active = false;
            }
            else if (cpath + direction < 0)
            {
                fired = true;
                active = false;
            }
            cpath = cpath + direction;
            int[] spot = new int[2] { path[cpath, 0], path[cpath, 1] };
            newLocation = spot;
            newpos = rom.GetLayers()[2].GetGridObject(spot[0], spot[1]).position;
            interpolate = 3;
            moving = true;
            //rect = new Rectangle(32, 0, 32, 32);
        }
    }

    public class Chest : PPObject
    {
        public int value { get; set; }
        public bool looted { get; set; }


        public Chest(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int val)
            : base(nam, classn, walka, pos, gl, ima, dep,rot, rec)
        {
            value = val;
            looted = false;

        }

        public Chest(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int val)
            : base(nam, classn, walka, pos, gl, ima, dep,rot)
        {

            value = val;
            looted = false;

        }

        public virtual void Opened(Player py)
        {
            py.score = py.score + value;
            looted = true;
            rect = new Rectangle(32, 0, 32, 32);

        }
    }


    public class ObjectChest : Chest
    {
        public PPObject contains { get; set; }


        public ObjectChest(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, PPObject val)
            : base(nam, classn, walka, pos, gl, ima, dep, rot,rec, 0)
        {
            contains = val;
            looted = false;

        }

        public ObjectChest(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, PPObject val)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, 0)
        {

            contains = val;
            looted = false;

        }

        public override void Opened(Player py)
        {
            //py.score = py.score + value;
            looted = true;
            py.AddItem(contains);
            rect = new Rectangle(32, 0, 32, 32);

        }
    }

    public class Door : PPObject
    {
        public String keyname { get; set; }
        public bool locked { get; set; }


        public Door(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, String val, bool lockedd)
            : base(nam, classn, walka, pos, gl, ima, dep,rot, rec)
        {
            keyname = val;
            locked = lockedd;

        }

        public Door(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, String val, bool lockedd)
            : base(nam, classn, walka, pos, gl, ima, dep,rot)
        {

            keyname = val;
            locked = lockedd;

        }

        public void Unlock(Player py)
        {
            if (py.HasItem(keyname))
            {
                py.RemoveItem(keyname);
                locked = false;
                walkable = new Barrier(true);
                image = null;
            }

        }
    }

    public class CrackedWall : PPObject
    {
        public bool triggered { get; set; }


        public CrackedWall(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            triggered = false;

        }

        public CrackedWall(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot)
            : base(nam, classn, walka, pos, gl, ima, dep, rot)
        {

            triggered = false;

        }

        public virtual void Trigger(Player py)
        {
            triggered = true;
            walkable = new Barrier(true);
            //rect = new Rectangle(0, 0, 32, 32);

        }
    }

    public class Trap : PPObject
    {
        public int damage { get; set; }
        public bool triggered { get; set; }


        public Trap(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int val)
            : base(nam, classn, walka, pos, gl, ima, dep,rot, rec)
        {
            damage = val;
            triggered = false;

        }

        public Trap(String nam,String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int val)
            : base(nam,classn, walka, pos, gl, ima, dep,rot)
        {

            damage = val;
            triggered = false;

        }

        public virtual void Trigger(Player py)
        {
            triggered = true;
            py.health = py.health - damage;
            walkable = new Barrier(new bool[4] { false,false,false,false});
            rect = new Rectangle(0, 0, 32, 32);

        }
    }

    public class FloorTrap : Trap
    {
        public String traptoactivate { get; set; }

        public FloorTrap(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, int val, String trapname)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec,val)
        {
            damage = val;
            triggered = false;
            traptoactivate = trapname;

        }

        public FloorTrap(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, int val, String trapname)
            : base(nam, classn, walka, pos, gl, ima, dep, rot,val)
        {

            damage = val;
            triggered = false;
            traptoactivate = trapname;

        }

        public override void Trigger(Player py)
        {
            triggered = true;
            //walkable = new Barrier(new bool[4] { false, false, false, false });
            //rect = new Rectangle(0, 0, 32, 32);

        }
    }

    public class Stairs : PPObject
    {
        public bool direction { get; set; }
        public bool on { get; set; }
        public int[] transportPos { get; set; }


        public Stairs(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, bool val, int[] tp)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec)
        {
            direction = val;
            on = false;
            transportPos = tp;

        }

        public Stairs(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, bool val, int[] tp)
            : base(nam, classn, walka, pos, gl, ima, dep, rot)
        {

            direction = val;
            on = false;
            transportPos = tp;

        }

        public virtual void Walked(Player py)
        {
            //on = true;
            py.SwapRooms(direction);
            //walkable = new Barrier(new bool[4] { false, false, false, false });
            //rect = new Rectangle(0, 0, 32, 32);

        }
    }

    public class HiddenStairs : Stairs
    {
        public bool hidden { get; set; }


        public HiddenStairs(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, Rectangle rec, bool val, int[] tp)
            : base(nam, classn, walka, pos, gl, ima, dep, rot, rec,val,tp)
        {
            direction = val;
            on = false;
            transportPos = tp;
            hidden = true;

        }

        public HiddenStairs(String nam, String classn, Barrier walka, Vector2 pos, int[] gl, Texture2D ima, float dep, float rot, bool val, int[] tp)
            : base(nam, classn, walka, pos, gl, ima, dep, rot,val,tp)
        {

            direction = val;
            on = false;
            transportPos = tp;
            hidden = true;

        }

        public void Reveal()
        {
            hidden = false;
        }

        public override void Walked(Player py)
        {
            //on = true;
            if (!hidden)
            {
                py.SwapRooms(direction);
            }
            //walkable = new Barrier(new bool[4] { false, false, false, false });
            //rect = new Rectangle(0, 0, 32, 32);

        }
    }

    public class Pyramid
    {
        private Room[] Floors;
        public String name { get; set; }
        public int cfloor { get; set; }
        public int maxfloor { get; set; }
        public bool fTrap { get; set; }

        public Pyramid(String nam, Room[] levels)
        {
            Floors = levels;
            cfloor = levels.Length;
            maxfloor = cfloor;
            name = nam;
            fTrap = false;
        }

        public void TriggerFTrap()
        {
            fTrap = true;
            // activate enemys
        }

        public Room[] GetRooms()
        {
            return Floors;
        }
        public void SetRooms(Room[] rms)
        {
            Floors = rms;
        }
        public Room GetFloor(int level)
        {
            return Floors[level];
        }

        public void SetFloor(int level, Room rm)
        {
            Floors[level] = rm;
        }

        public void SetWalls(Room rom, Texture2D wallTx)
        {
            Grid[] layers = rom.GetLayers();
            PPObject[,] walls = new PPObject[layers[1].GetObjects().GetLength(0), layers[1].GetObjects().GetLength(1)];
            for (int r = 0; r < walls.GetLength(0); r++)
            {
                for (int c = 0; c < walls.GetLength(1); c++)
                {
                    if (r == 0 || r == walls.GetLength(0) - 1 || c == 0 || c == walls.GetLength(1) - 1)
                    {
                        walls[r, c] = new PPObject("wall", "wall", new Barrier(false), new Vector2(), new int[2] { r, c }, wallTx, 0.5f, 0.0f);
                    }
                }
            }
            Grid layer1 = new Grid(walls);
            rom.ReplaceLayer(1, layer1);
        }


    }

    public class Room
    {
        private Grid[] layers;
        // layer0 is for dynamic objects such as enemies
        // layer1 is for static objects like walls
        // layer2 is for background stuff like ground or pictures
        

        // using a layer system, multiple objects can be physically located in the same spot.

        public Room(Grid[] layer)
        {
            layers = layer;
        }
        public Room(Grid layer0, Grid layer1, Grid layer2)
        {
            layers = new Grid[3] { layer0, layer1, layer2 };
        }

        public void ReplaceLayer(int layerNum, Grid grid)
        {
            layers[layerNum] = grid;
        }

        public Grid[] GetLayers()
        {
            return layers;
        }

        public void SetObjectAtLoc(int layer, int row, int col, PPObject objec)
        {
            layers[layer].SetGridObject(objec, row, col);
        }
        public void RemoveObjectAtLoc(int layer, int row, int col)
        {
            layers[layer].RemoveGridObject(row, col);
        }

        public PPObject[] GetObjectsAtLoc(int row,int col)
        {
            PPObject[] obs = new PPObject[layers.Length];
            for (int l = 0; l < layers.Length; l++)
            {
                PPObject ob = layers[l].GetGridObject(row, col);
                if (ob != null)
                {
                    obs[l] = ob;
                }
            }
            return obs;
        }
    }

    public class Grid
    {
        private PPObject[,] objs;
        public bool set { get; set; }
        public int cols { get; set; }
        public int rows { get; set; }

        public Grid(int row, int col)
        {
            objs = new PPObject[row, col];
            cols = col;
            rows = row;
            set = false;
        }

        public Grid(PPObject[,] obs) 
        {
            objs = obs;
            cols = obs.GetLength(1);
            rows = obs.GetLength(0);
            set = true;
        }

        public PPObject[,] GetObjects()
        {
            return objs;
        }

        public void FillGrid(PPObject[,] obs)
        {
            objs = obs;
            set = true;
        }

        public void SetGridObject(PPObject obj, int row, int col)
        {
            objs[row, col] = obj;
        }

        public void RemoveGridObject(int row, int col)
        {
            objs[row, col] = null;
        }

        public PPObject GetGridObject(int row, int col)
        {
            return objs[row, col];
        }

        
    }
}
