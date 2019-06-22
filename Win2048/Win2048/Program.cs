using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win2048
{
    public static class Program
    {
        public static int[,] num44 = new int[4, 4];
//        public static 

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class RunPro
    { 
        Random random = new System.Random();

        public Boolean newDataAppear(int[,] num44)
        {
            int random24 = 2;
            random24 = random24 * random.Next(1, 2);

            System.Collections.ArrayList arrNum = new System.Collections.ArrayList();

            arrNum.Clear();
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (num44[x, y] == 0 && (x == 0 || y == 0 || x == 3 || y == 3))
                    {
                        arrNum.Add(new int[] { x, y });
                    }
                }
            }

            int randomPos = -1;
            if (arrNum.Count > 0)
            {
                randomPos = random.Next(0, arrNum.Count - 1);
                int[] temp = (int[])arrNum[randomPos];
                num44[temp[0], temp[1]] = random24;
                return true;
            } else
            {
                return false;
            }
        }

        public Boolean move(int[,] num44, int direction) // direction 1:上 2:右 3:下 4:左
        {
            Boolean movedFlg = false;
            int levle = -1; // 加算されたアリアのＩｎｄｅｘ、再び加算を禁止

            if (direction == 1)
            {
                for (int y=0; y<4; y++)
                {
                    levle = -1;
                    for (int x = 1; x < 4; x++)
                    {
                        if (num44[x,y] != 0)
                        {
                            if (num44[x - 1, y] == 0)
                            {
                                num44[x - 1, y] = num44[x, y];
                                num44[x, y] = 0;
                                x = 0;
                                movedFlg = true;
                            } else if (num44[x - 1, y] == num44[x, y])
                            {
                                if (x > levle + 1) { 
                                    num44[x - 1, y] = num44[x - 1, y] + num44[x, y];
                                    num44[x, y] = 0;
                                    movedFlg = true;
                                    levle = x - 1;
                                    x = 0;
                                }
                            }
                        }
                    }
                }
            }
            else if (direction == 3)
            {
                for (int y = 0; y < 4; y++)
                {
                    levle = 4;
                    for (int x = 2; x > -1; x--)
                    {
                        if (num44[x, y] != 0)
                        {
                            if (num44[x + 1, y] == 0)
                            {
                                num44[x + 1, y] = num44[x, y];
                                num44[x, y] = 0;
                                x = 3;
                                movedFlg = true;
                            }
                            else if (num44[x + 1, y] == num44[x, y])
                            {
                                if (x < levle - 1)
                                {
                                    num44[x + 1, y] = num44[x + 1, y] + num44[x, y];
                                    num44[x, y] = 0;
                                    movedFlg = true;
                                    levle = x + 1;
                                    x = 3;
                                }
                            }
                        }
                    }
                }
            } else if (direction == 4)
            {
                for (int x = 0; x < 4; x++)
                {
                    levle = -1;
                    for (int y = 1; y < 4; y++)
                    {
                        if (num44[x, y] != 0)
                        {
                            if (num44[x, y - 1] == 0)
                            {
                                num44[x, y - 1] = num44[x, y];
                                num44[x, y] = 0;
                                y = 0;
                                movedFlg = true;
                            }
                            else if (num44[x, y - 1] == num44[x, y])
                            {
                                if (y > levle + 1)
                                {
                                    num44[x, y - 1] = num44[x, y - 1] + num44[x, y];
                                    num44[x, y] = 0;
                                    movedFlg = true;
                                    levle = y - 1;
                                    y = 0;
                                }
                            }
                        }
                    }
                }
            }
            else if (direction == 2)
            {
                for (int x = 0; x < 4; x++)
                {
                    levle = 4;
                    for (int y = 2; y > -1; y--)
                    {
                        if (num44[x, y] != 0)
                        {
                            if (num44[x, y + 1] == 0)
                            {
                                num44[x, y + 1] = num44[x, y];
                                num44[x, y] = 0;
                                y = 3;
                                movedFlg = true;
                            }
                            else if (num44[x, y + 1] == num44[x, y])
                            {
                                if (y < levle - 1)
                                {
                                    num44[x, y + 1] = num44[x, y + 1] + num44[x, y];
                                    num44[x, y] = 0;
                                    movedFlg = true;
                                    levle = y + 1;
                                    y = 3;
                                }
                            }
                        }
                    }
                }
            }
            return movedFlg;
        }

        public void lose1(int[,] num44)
        {

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (num44[x, y] % 2 == 1)
                    {
                        num44[x, y] = num44[x, y] - 1;
                    }

                }
            }
        }

        public void reSetNum(int[,] num44)
        {

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    num44[x, y] = 0;
                }
            }
        }
    }
}
