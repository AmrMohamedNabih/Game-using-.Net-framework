using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace longGame
{
    public partial class Form1 : Form
    {
        public class CAdvImgActor
        {
            public Bitmap img;
            public Rectangle rcDst;
            public Rectangle rcSrc;
            public int dir = 1;
            public int type;
        }

        public class CAnimationImgActor
        {
            public List<Bitmap> img = new List<Bitmap>();
            public Rectangle rcDst;
            public Rectangle rcSrc;
            public int index = 0;
        }

        public class Hero
        {
            public List<Bitmap> imgDnRun = new List<Bitmap>();

            public List<Bitmap> imgRun = new List<Bitmap>();
            public List<Bitmap> imgJump = new List<Bitmap>();
            public List<Bitmap> imgSlide = new List<Bitmap>();
            public List<Bitmap> imgFall = new List<Bitmap>();
            public int dir = 1;

            public int index = 0;
            public int x, y;
            public int xCheckPoint, yCheckPoint, gravity, health, wichImg;
        }

        private Bitmap blockImageSlide = new Bitmap("block1-slope.png");

        private Timer tt = new Timer();
        private CAdvImgActor Background = new CAdvImgActor();
        private List<CAdvImgActor> blocks = new List<CAdvImgActor>();
        private List<CAdvImgActor> elavator = new List<CAdvImgActor>();
        private List<CAnimationImgActor> water = new List<CAnimationImgActor>();
        private List<CAdvImgActor> boat = new List<CAdvImgActor>();
        private List<CAdvImgActor> spike = new List<CAdvImgActor>();
        private List<CAdvImgActor> ladder = new List<CAdvImgActor>();
        private List<CAdvImgActor> sgn = new List<CAdvImgActor>();
        private List<CAdvImgActor> trap2 = new List<CAdvImgActor>();
        private List<CAdvImgActor> weapon = new List<CAdvImgActor>();

        private List<CAdvImgActor> BulletTrap2 = new List<CAdvImgActor>();
        private List<Hero> hero = new List<Hero>();

        private Bitmap off;
        private int XB = 0;
        private int YB = 0;
        private int ctwater = 0;
        private int ctSlide = 0;
        private int ctJump;
        private int ctJumpTime = 0;
        private int flagGrav;
        private int ctBullet1;
        private int ctBullet1Time;

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;

            this.Paint += Form1_Paint; ;
            this.Load += Form1_Load;
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
            this.MouseMove += Form1_MouseMove;
            this.KeyDown += Form1_KeyDown;
            tt.Tick += Tt_Tick;
            tt.Interval = 1;
            tt.Start();
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            if (ctwater % 3 == 0)
            {
                AnimateWater();
            }

            checkGravity();
            graviteHero();
            AnimateTrap2();
            AnimateElevator();
            AnimateBoat();
            isTouchBoat();
            isLadderBoat();
            if (ctBullet1Time % 50 == 0)
            {
                ctBullet1++;
            }
            AnimateBulletTrap();

            ctBullet1Time++;
            ctwater++;
            DrawDubb(this.CreateGraphics());
        }

        private void AnimateBulletTrap()
        {
            for (int i = 0; i < BulletTrap2.Count; i++)
            {
                if (i < ctBullet1 || i + 500 < ctBullet1 + 500)
                {
                    BulletTrap2[i].rcDst.X += 5;
                    BulletTrap2[i].rcDst.Y = trap2[0].rcDst.Y;
                    BulletTrap2[i + 50].rcDst.X += 5;
                    BulletTrap2[i + 50].rcDst.Y = trap2[1].rcDst.Y;
                }
                else
                {
                    break;
                }
            }
        }

        private void isLadderBoat()
        {
            int i;
            for (i = 0; i < ladder.Count; i++)
            {
                if (hero[0].x + hero[0].imgDnRun[0].Width / 2 >= ladder[i].rcDst.X && hero[0].x + hero[0].imgDnRun[0].Width / 2 <= ladder[i].rcDst.X + ladder[i].rcDst.Width
                    && hero[0].y + hero[0].imgDnRun[0].Height / 2 <= ladder[0].rcDst.Y + ladder[ladder.Count - 1].rcDst.Height && hero[0].y + hero[0].imgDnRun[0].Height >= ladder[ladder.Count - 1].rcDst.Y
                    )
                {
                    flagGrav = 0;
                    hero[0].gravity = 6; break;
                }
            }
            if (i == ladder.Count)
            {
                flagGrav = 2;
            }
        }

        private void isTouchBoat()
        {
            int i;
            for (i = 0; i < boat.Count; i++)
            {
                if (hero[0].x + hero[0].imgDnRun[0].Width / 2 >= boat[i].rcDst.X && hero[0].x + hero[0].imgDnRun[0].Width / 2 <= boat[i].rcDst.X + boat[i].rcDst.Width
                    && hero[0].y + hero[0].imgDnRun[0].Height <= boat[i].rcDst.Y + 30 && hero[0].y + hero[0].imgDnRun[0].Height >= boat[i].rcDst.Y
                    )
                {
                    flagGrav = 0;
                    hero[0].gravity = 5; break;
                }
            }
        }

        private void AnimateBoat()
        {
            if (boat[0].rcDst.X <= water[0].rcDst.X)
            {
                boat[0].img = new Bitmap("boatR.png");
                boat[0].dir = 1;
            }
            if (boat[0].rcDst.X + boat[0].rcDst.Width >= water[water.Count - 1].rcDst.X + 50)
            {
                boat[0].img = new Bitmap("boat.png");

                boat[0].dir = -1;
            }
            if (boat[0].dir == -1)
            {
                boat[0].rcDst.X -= 2;
            }
            if (boat[0].dir == 1)
            {
                boat[0].rcDst.X += 2;
            }
        }

        private void IsHeroToushelevator()
        {
            int i = 0;
            for (i = 0; i < elavator.Count; i++)
            {
                if (hero[0].x + hero[0].imgDnRun[0].Width / 2 >= elavator[i].rcDst.X && hero[0].x + hero[0].imgDnRun[0].Width / 2 <= elavator[i].rcDst.X + elavator[i].rcDst.Width
                    && hero[0].y + hero[0].imgDnRun[0].Height <= elavator[i].rcDst.Y + 10 && hero[0].y + hero[0].imgDnRun[0].Height >= elavator[i].rcDst.Y
                    )
                {
                    flagGrav = 0;
                    hero[0].gravity = 4; break;
                }
            }
            if (i == elavator.Count)
            {
                flagGrav = 2;
            }
        }

        private void AnimateElevator()
        {
            for (int i = 0; i < elavator.Count; i++)
            {
                if (elavator[i].rcDst.Y <= water[0].rcDst.Y - 30)
                {
                    elavator[i].dir = 1;
                }
                if (elavator[i].rcDst.Y >= Background.rcDst.Height - 40)
                {
                    elavator[i].dir = -1;
                }
                if (elavator[i].dir == -1)
                {
                    elavator[i].rcDst.Y -= 2;
                }
                if (elavator[i].dir == 1)
                {
                    elavator[i].rcDst.Y += 2;
                }
            }
        }

        private void jumpHero()
        {
            if (hero[0].dir == 1)
            {
                if (ctJump < 5)
                {
                    hero[0].x += 7;
                    hero[0].y -= 15;
                }
                if (ctJump >= 5)
                {
                    hero[0].x += 7;
                    hero[0].y += 7;
                }

                hero[0].index++;
                if (hero[0].index >= 4)
                {
                    hero[0].index = 0;
                }

                ctJump++;
                if (ctJump > 10)
                {
                    hero[0].gravity = 1;
                    hero[0].wichImg = 0;
                    hero[0].index = 0;
                }
            }
            if (hero[0].dir == -1)
            {
                if (ctJump < 5)
                {
                    hero[0].x -= 7;
                    hero[0].y -= 15;
                }
                if (ctJump >= 5)
                {
                    hero[0].x -= 7;
                    hero[0].y += 7;
                }

                hero[0].index++;
                if (hero[0].index >= 4)
                {
                    hero[0].index = 0;
                }

                ctJump++;
                if (ctJump > 10)
                {
                    hero[0].gravity = 1;
                    hero[0].wichImg = 0;
                    hero[0].index = 0;
                }
            }
        }

        private void graviteHero()
        {
            if (hero[0].gravity != 0)
            {
                if (ctSlide % 6 == 0)
                {
                    if (hero[0].gravity == 2)
                    {
                        hero[0].y += blocks[0].rcDst.Height;
                        hero[0].x += blocks[0].rcDst.Width;
                    }
                }
                else if (hero[0].gravity == 1)
                {
                    hero[0].y += 4;
                }
                else if (hero[0].gravity == 3)
                {
                    if (ctJumpTime % 2 == 0)
                    {
                        jumpHero();
                    }
                    ctJumpTime++;
                }
                else if (hero[0].gravity == 4)
                {
                    hero[0].y = elavator[0].rcDst.Y - hero[0].imgDnRun[0].Height;
                    IsHeroToushelevator();
                }
                else if (hero[0].gravity == 5)
                {
                    hero[0].y = boat[0].rcDst.Y - hero[0].imgDnRun[0].Height + 20;
                    if (boat[0].dir == 1)
                    {
                        hero[0].x += 2;
                    }
                    else
                    {
                        hero[0].x -= 2;
                    }

                    isTouchBoat();
                }
                ctSlide++;
            }
        }

        private void checkGravity()
        {
            if (hero[0].gravity != 3 && hero[0].gravity != 4 && hero[0].gravity != 5 && hero[0].gravity != 6)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    if (hero[0].x + hero[0].imgDnRun[0].Width >= blocks[i].rcDst.X && hero[0].x + hero[0].imgDnRun[0].Width / 2 <= blocks[i].rcDst.X + blocks[i].rcDst.Width
                        && hero[0].y + hero[0].imgDnRun[0].Height <= blocks[i].rcDst.Y && hero[0].y + hero[0].imgDnRun[0].Height >= blocks[i].rcDst.Y - 3
                        )
                    {
                        if (blocks[i].type == 1)
                        {
                            flagGrav = 0;
                            hero[0].wichImg = 3;
                            hero[0].gravity = 2; break;
                        }
                        else
                        {
                            flagGrav = 0;

                            hero[0].gravity = 0; break;
                        }
                    }
                    else
                    {
                        flagGrav = 1;
                    }
                }
                if (flagGrav == 1)
                {
                    IsHeroToushelevator();
                }
                if (flagGrav == 2)
                {
                    hero[0].gravity = 1;
                }
            }
        }

        private void AnimateTrap2()
        {
            if (trap2[0].rcDst.Y <= sgn[0].rcDst.Y)
            {
                trap2[0].dir = 1;
            }
            if (trap2[0].rcDst.Y >= 430 || trap2[0].rcDst.Y + trap2[0].rcDst.Height >= trap2[1].rcDst.Y)
            {
                trap2[0].dir = -1;
            }
            if (trap2[1].rcDst.Y <= sgn[0].rcDst.Y || trap2[1].rcDst.Y <= trap2[0].rcDst.Y + trap2[0].rcDst.Height)
            {
                trap2[1].dir = 1;
            }
            if (trap2[1].rcDst.Y >= 430)
            {
                trap2[1].dir = -1;
            }
            if (trap2[0].dir == -1)
            {
                trap2[0].rcDst.Y--;
            }
            if (trap2[0].dir == 1)
            {
                trap2[0].rcDst.Y++;
            }
            if (trap2[1].dir == -1)
            {
                trap2[1].rcDst.Y--;
            }
            if (trap2[1].dir == 1)
            {
                trap2[1].rcDst.Y++;
            }
        }

        private void AnimateWater()
        {
            for (int i = 0; i < water.Count; i++)
            {
                if (water[i].img.Count != 1)
                {
                    water[i].index++;
                }
                if (water[i].index >= 6)
                {
                    water[i].index = 0;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (hero[0].gravity == 6)
                    {
                        hero[0].y -= 2;
                    }

                    break;

                case Keys.S:
                    if (hero[0].gravity == 6)
                    {
                        hero[0].y += 2;
                    }
                    break;

                case Keys.D:
                    if (hero[0].gravity == 0 || hero[0].gravity == 4 || hero[0].gravity == 5 || hero[0].gravity == 6)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            hero[0].imgRun[i] = new Bitmap("adventurer-run-0" + i + ".png");
                        }
                        hero[0].dir = 1;
                        hero[0].wichImg = 1;
                        hero[0].index++;
                        hero[0].x += 5;
                        if (hero[0].index >= hero[0].imgRun.Count)
                        {
                            hero[0].index = 0;
                        }
                    }
                    break;

                case Keys.A:
                    if (hero[0].gravity == 0 || hero[0].gravity == 4 || hero[0].gravity == 5 || hero[0].gravity == 6)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            hero[0].imgRun[i] = new Bitmap("adventurer-runR-0" + i + ".png");
                        }
                        hero[0].dir = -1;
                        hero[0].wichImg = 1;
                        hero[0].index++;
                        hero[0].x -= 5;
                        if (hero[0].index >= hero[0].imgRun.Count)
                        {
                            hero[0].index = 0;
                        }
                    }
                    break;

                case Keys.Space:
                    if (hero[0].gravity == 0 || hero[0].gravity == 4 || hero[0].gravity == 5 || hero[0].gravity == 6)
                    {
                        ctJump = 0;
                        hero[0].index = 0;
                        hero[0].gravity = 3;
                        hero[0].wichImg = 2;
                    }
                    break;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // this.Size = new Size(700, Screen.PrimaryScreen.WorkingArea.Height);
            // this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 4, 0);
            Background.img = new Bitmap("bk.png");
            Background.rcSrc = new Rectangle(0, 0, Background.img.Width, Background.img.Height);
            Background.rcDst = new Rectangle(0, 0, this.Width, this.Height);
            YB = Background.rcDst.Height * 8 / 10;

            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            createBlocksAndWater();
            createBoat();
            createBlocksAfterWatr();
            createLadder();
            createSgn();
            createSgn();
            createTarp2();
            elevator1();
            CreateHero();
        }

        private void elevator1()
        {
            for (int i = 0; i < 2; i++) // Increment the level of rows for each new slide block
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("block33.png");
                pnn.rcDst = new Rectangle((Background.rcDst.Width / 3) - 130 + pnn.img.Width * i, Background.rcDst.Height - 45, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                pnn.dir = -1;
                elavator.Add(pnn);
            }
        }

        private void createTarp2()
        {
            for (int i = 0; i < 2; i++)
            {
                CAdvImgActor pnn = new CAdvImgActor();

                pnn.img = new Bitmap("32.png");
                pnn.rcDst = new Rectangle(760, YB - 408 + ((pnn.img.Height + 30) * i), pnn.img.Width * 8 / 10, pnn.img.Height * 8 / 10);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                for (int j = 0; j < 1000; j++)
                {
                    CAdvImgActor pn = new CAdvImgActor();

                    pn.img = new Bitmap("100.png");
                    pn.rcDst = new Rectangle(pnn.rcDst.X + 30, pnn.rcDst.Y + 20, pn.img.Width, pn.img.Height);
                    pn.rcSrc = new Rectangle(0, 0, pn.img.Width, pn.img.Height);
                    BulletTrap2.Add(pn);
                }
                trap2.Add(pnn);
            }
        }

        private void createSgn()
        {
            for (int i = 0; i < 3; i++)
            {
                CAdvImgActor pnn = new CAdvImgActor();

                pnn.img = new Bitmap("22.png");
                pnn.rcDst = new Rectangle(Background.rcDst.Width * 3 / 4, YB - 408 + pnn.img.Height * i, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                sgn.Add(pnn);
            }
            CAdvImgActor pmm = new CAdvImgActor();

            pmm.img = new Bitmap("13.png");
            pmm.rcDst = new Rectangle((Background.rcDst.Width * 3 / 4) - (pmm.img.Width / 3) - 6, sgn[sgn.Count - 1].rcDst.Y + 20, pmm.img.Width, pmm.img.Height);
            pmm.rcSrc = new Rectangle(0, 0, pmm.img.Width, pmm.img.Height);
            sgn.Add(pmm);
            CAdvImgActor pm = new CAdvImgActor();

            pm.img = new Bitmap("M1A1 Thompson.png");
            pm.rcDst = new Rectangle((Background.rcDst.Width * 3 / 4) - (pm.img.Width / 3) - 6, sgn[sgn.Count - 1].rcDst.Y + 40, pm.img.Width, pm.img.Height);
            pm.rcSrc = new Rectangle(0, 0, pm.img.Width, pm.img.Height);
            weapon.Add(pm);
        }

        private void createLadder()
        {
            for (int i = 0; i < 3; i++) // Increment the level of rows for each new slide block
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("2.png");
                pnn.rcDst = new Rectangle(Background.rcDst.Width - 200, YB + 90 - pnn.img.Height * i, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                ladder.Add(pnn);
            }
        }

        private void createBoat()
        {
            CAdvImgActor pnn = new CAdvImgActor();
            pnn.img = new Bitmap("boat.png");
            pnn.rcDst = new Rectangle(760, YB + 3, pnn.img.Width, pnn.img.Height);
            pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
            pnn.dir = -1;
            boat.Add(pnn);
        }

        private void createBlocksAfterWatr()
        {
            for (int i = 0; i < 20; i++) // Increment the level of rows for each new slide block
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("block1.png");
                pnn.rcDst = new Rectangle(1028 + pnn.img.Width * i, YB + 110, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                blocks.Add(pnn);
            }
        }

        private void createBlocksAndWater()
        {
            // Ensure blocks is initialized
            if (blocks == null)
            {
                blocks = new List<CAdvImgActor>();
            }

            // Load images once
            Bitmap blockImage = new Bitmap("block1.png");
            Bitmap blockImageSlide = new Bitmap("block1-slope.png");

            int blockWidth = blockImage.Width;
            int blockHeight = blockImage.Height;

            // Create ground blocks
            for (int c = 0; c < 6; c++)
            {
                for (int r = 0; r < 5; r++)
                {
                    CAdvImgActor pnn = new CAdvImgActor();

                    pnn.img = blockImage;
                    pnn.rcDst = new Rectangle(c * blockWidth, r * blockHeight + YB, blockWidth, blockHeight);
                    pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                    blocks.Add(pnn);
                }
            }

            // Create sliding section
            int startSlideX = 6 * blockWidth; // Starting X position for the slide section
            int startSlideY = 0; // Starting Y position for the slide section
            int slideHeightIncrement = blockHeight; // Height increment for each slide block

            for (int cSlide = 0; cSlide < 5; cSlide++) // Create 2 columns for the slide
            {
                for (int r = 0; r <= 5; r++) // Increment the level of rows for each new slide block
                {
                    CAdvImgActor pnn = new CAdvImgActor();

                    if (r == 0)
                    {
                        pnn.img = blockImageSlide;
                        pnn.rcDst = new Rectangle(startSlideX + cSlide * blockImageSlide.Width, startSlideY + r * slideHeightIncrement + YB, blockImageSlide.Width, blockImageSlide.Height);
                    }
                    else
                    {
                        pnn.img = blockImage;
                        pnn.rcDst = new Rectangle(startSlideX + cSlide * blockImage.Width, startSlideY + r * slideHeightIncrement + YB, blockImage.Width, blockImage.Height);
                    }
                    if (cSlide < 4)
                    {
                        pnn.type = 1;
                    }
                    pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                    blocks.Add(pnn);
                }
                startSlideY += slideHeightIncrement; // Increase the Y position for the next column
            }
            for (int c = 0; c < 5; c++)
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = blockImage;
                pnn.rcDst = new Rectangle(c * blockWidth + 310, Background.rcDst.Height - blockHeight - 5, blockWidth, blockHeight);
                pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                blocks.Add(pnn);
            }
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    CAdvImgActor pnn = new CAdvImgActor();

                    pnn.img = blockImage;
                    pnn.rcDst = new Rectangle(c * blockWidth + 450, r * blockHeight + YB + blockHeight, blockWidth, blockHeight);
                    pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                    blocks.Add(pnn);
                }
            }

            ///water
            for (int c = 0; c < 10; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    CAnimationImgActor pnn = new CAnimationImgActor();
                    if (r == 0)
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            Bitmap img = new Bitmap("water" + i + ".png");
                            img.MakeTransparent(img.GetPixel(0, 0));
                            pnn.img.Add(img);
                        }
                    }
                    else
                    {
                        Bitmap img = new Bitmap("water7.png");
                        pnn.img.Add(img);
                    }

                    {
                        pnn.rcDst = new Rectangle(c * blockWidth + 579, r * blockHeight + YB + blockHeight, blockWidth, blockHeight);
                        pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                        water.Add(pnn);
                    }
                }
            }
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    CAdvImgActor pnn = new CAdvImgActor();
                    pnn.img = blockImage;
                    pnn.rcDst = new Rectangle(c * blockWidth + 900, r * blockHeight + YB + blockHeight, blockWidth, blockHeight);
                    pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                    blocks.Add(pnn);
                }
            }
            for (int i = 0; i < 43; i++)
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("block2.png");
                pnn.rcDst = new Rectangle(0 + pnn.img.Width * i, Background.rcDst.Height * 6 / 10, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);

                blocks.Add(pnn);
            }
            for (int i = 0; i < 40; i++)
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("block4.png");
                pnn.rcDst = new Rectangle(260 + pnn.img.Width * i, Background.rcDst.Height * 3 / 11, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);
                blocks.Add(pnn);
            }
            for (int i = 0; i < 7; i++)
            {
                CAdvImgActor pnn = new CAdvImgActor();
                pnn.img = new Bitmap("block3.png");
                pnn.rcDst = new Rectangle(760, 258 + pnn.img.Height * i, pnn.img.Width, pnn.img.Height);
                pnn.rcSrc = new Rectangle(0, 0, blockWidth, blockHeight);
                blocks.Add(pnn);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void CreateHero()
        {
            Hero pnn = new Hero();
            for (int i = 0; i < 6; i++)
            {
                Bitmap img = new Bitmap("adventurer-run-0" + i + ".png");
                pnn.imgRun.Add(img);
            }
            for (int i = 0; i < 4; i++)
            {
                Bitmap img = new Bitmap("adventurer-smrslt-0" + i + ".png");
                pnn.imgJump.Add(img);
            }
            for (int i = 0; i < 2; i++)
            {
                Bitmap img = new Bitmap("adventurer-slide-0" + i + ".png");
                pnn.imgSlide.Add(img);
            }
            for (int i = 0; i < 2; i++)
            {
                Bitmap img = new Bitmap("adventurer-fall-0" + i + ".png");
                pnn.imgFall.Add(img);
            }
            Bitmap imgg = new Bitmap("adventurer-items-00.png");
            pnn.imgDnRun.Add(imgg);
            pnn.x = 0;
            pnn.y = blocks[0].rcDst.Y - pnn.imgDnRun[0].Height;
            pnn.gravity = 0;
            pnn.index = 0;
            pnn.xCheckPoint = 0;
            pnn.yCheckPoint = blocks[0].rcDst.Y - pnn.imgDnRun[0].Height;
            pnn.health = 10;
            pnn.wichImg = 0;
            hero.Add(pnn);
        }

        private void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);

            g.DrawImage(Background.img, Background.rcDst, Background.rcSrc, GraphicsUnit.Pixel);
            for (int i = 0; i < ladder.Count; i++)
            {
                g.DrawImage(ladder[i].img, ladder[i].rcDst, ladder[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < hero.Count; i++)
            {
                if (hero[i].wichImg == 0)
                {
                    g.DrawImage(hero[i].imgDnRun[hero[i].index], hero[i].x, hero[i].y, hero[i].imgDnRun[hero[i].index].Width, hero[i].imgDnRun[hero[i].index].Height);
                }
                else if (hero[i].wichImg == 1)
                {
                    g.DrawImage(hero[i].imgRun[hero[i].index], hero[i].x, hero[i].y, hero[i].imgRun[hero[i].index].Width, hero[i].imgRun[hero[i].index].Height);
                }
                else if (hero[i].wichImg == 2)
                {
                    g.DrawImage(hero[i].imgJump[hero[i].index], hero[i].x, hero[i].y, hero[i].imgJump[hero[i].index].Width, hero[i].imgJump[hero[i].index].Height);
                }
                else if (hero[i].wichImg == 3)
                {
                    g.DrawImage(hero[i].imgSlide[0], hero[i].x, hero[i].y, hero[i].imgSlide[0].Width, hero[i].imgSlide[0].Height);
                }
                else if (hero[i].wichImg == 4)
                {
                    g.DrawImage(hero[i].imgFall[hero[i].index], hero[i].x, hero[i].y, hero[i].imgFall[hero[i].index].Width, hero[i].imgFall[hero[i].index].Height);
                }
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                g.DrawImage(blocks[i].img, blocks[i].rcDst, blocks[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < water.Count; i++)
            {
                int index = water[i].index;
                if (water[i].img.Count != 1)
                {
                    g.DrawImage(water[i].img[index], water[i].rcDst, water[i].rcSrc, GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(water[i].img[0], water[i].rcDst, water[i].rcSrc, GraphicsUnit.Pixel);
                }
            }
            for (int i = 0; i < boat.Count; i++)
            {
                g.DrawImage(boat[i].img, boat[i].rcDst, boat[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < spike.Count; i++)
            {
                g.DrawImage(spike[i].img, spike[i].rcDst, spike[i].rcSrc, GraphicsUnit.Pixel);
            }

            for (int i = 0; i < elavator.Count; i++)
            {
                g.DrawImage(elavator[i].img, elavator[i].rcDst, elavator[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < sgn.Count; i++)
            {
                g.DrawImage(sgn[i].img, sgn[i].rcDst, sgn[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < trap2.Count; i++)
            {
                g.DrawImage(trap2[i].img, trap2[i].rcDst, trap2[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < weapon.Count; i++)
            {
                g.DrawImage(weapon[i].img, weapon[i].rcDst, weapon[i].rcSrc, GraphicsUnit.Pixel);
            }
            for (int i = 0; i < BulletTrap2.Count; i++)
            {
                g.DrawImage(BulletTrap2[i].img, BulletTrap2[i].rcDst, BulletTrap2[i].rcSrc, GraphicsUnit.Pixel);
            }
        }

        private void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}
