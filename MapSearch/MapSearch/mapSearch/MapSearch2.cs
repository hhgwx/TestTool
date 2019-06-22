using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//優先範囲で、探す
namespace MapSearch.mapSearch
{
    class MapSearch2
    {

        private int lengthY; // mapのheight
        private int lengthX; // mapのwidth

        private int endPointY = -1; // endPointY
        private int endPointX = -1; // endPointX

        private int startPointY = -1; // startPointY
        private int startPointX = -1; // startPointX


        private PieceArea[,] map = null;

        private List<PointXY> arrListNow = new List<PointXY>();
        private List<PointXY> arrListOut = new List<PointXY>();

        private int distanceOnePiece = 10; // 直線距離
        private int distanceOnePiecePlus = 14; // 斜線距離

        private int keyIndex = 0; // loop single key

        private PointXY areaFrom = new PointXY(-1, -1); // 探す範囲
        private PointXY areaTo = new PointXY(-1, -1); // 探す範囲

        /**
         * @param lengthY 行目
         * @param lengthX 列目
         */
        public MapSearch2(int lengthY, int lengthX)
        {
            this.lengthY = lengthY;
            this.lengthX = lengthX;

            // 初期化Map
            map = new PieceArea[lengthY,lengthX];
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    map[y,x] = new PieceArea();
                }
            }
        }

        /**
         * @param args
         */
        public static void main(String[] args)
        {
            MapSearch2 mSearch = new MapSearch2(100, 100);

            /*		for (int i=0;i<40;i++) {
                        mSearch.setWallPoint(2,5+i);
                        mSearch.setWallPoint(4,4+i);
                    }
                    mSearch.setWallPoint(2,3);
                    mSearch.setWallPoint(3,4);
            */
            int ty = 1, tx = 0;
            int tDir = 1;
            while (true)
            {
                if (tDir == 1)
                {
                    mSearch.setWallPoint(ty, tx);
                    if (tx + 2 >= mSearch.lengthX || mSearch.map[ty,tx + 2].wallFlg)
                    {
                        tDir = 2;
                        ty = ty + 1;
                        if (mSearch.map[ty,tx].wallFlg || mSearch.map[ty + 1,tx].wallFlg)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tx = tx + 1;
                    }
                }
                else if (tDir == 2)
                {
                    mSearch.setWallPoint(ty, tx);
                    if (ty + 2 >= mSearch.lengthY || mSearch.map[ty + 2,tx].wallFlg)
                    {
                        tDir = 3;
                        tx = tx - 1;
                        if (mSearch.map[ty,tx].wallFlg || mSearch.map[ty,tx - 1].wallFlg)
                        {
                            break;
                        }
                    }
                    else
                    {
                        ty = ty + 1;
                    }
                }
                else if (tDir == 3)
                {
                    mSearch.setWallPoint(ty, tx);
                    if (tx - 2 < 0 || mSearch.map[ty,tx - 2].wallFlg)
                    {
                        tDir = 4;
                        ty = ty - 1;
                        if (mSearch.map[ty,tx].wallFlg || mSearch.map[ty - 1,tx].wallFlg)
                        {
                            break;
                        }
                    }
                    else
                    {
                        tx = tx - 1;
                    }
                }
                else if (tDir == 4)
                {
                    mSearch.setWallPoint(ty, tx);
                    if (ty - 2 < 0 || mSearch.map[ty - 2,tx].wallFlg)
                    {
                        tDir = 1;
                        tx = tx + 1;
                        if (mSearch.map[ty,tx].wallFlg || mSearch.map[ty,tx + 1].wallFlg)
                        {
                            break;
                        }
                    }
                    else
                    {
                        ty = ty - 1;
                    }
                }
            }



            // Start point
            int startY = 0;
            int startX = 0;
            mSearch.setStartPoint(startY, startX);

            // End point
            int endY = 50;
            int endX = 48;
            mSearch.setEndPoint(endY, endX);

            //		mSearch.drawMap();
            System.Console.WriteLine("Start time:" + DateTime.Now.ToString());
            for (int aa = 0; aa < 1; aa++)
            {

                mSearch.searchStart();

            }
            System.Console.WriteLine("End time  :" + DateTime.Now.ToString());

            mSearch.drawMap();
        }

        /**
         * @param args
         */
        public void setWallPoint(int wallY, int wallX)
        {
            clearPoint(wallY, wallX);
            map[wallY,wallX].wallFlg = true;
        }

        /**
         * @param args
         */
        public Boolean isWallPoint(int wallY, int wallX)
        {
            if (map[wallY, wallX].wallFlg)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * @param args
         */
        public void setStartPoint(int startY, int startX)
        {
            clearPoint(startY, startX);

            map[startY,startX].level = 1;

            // 前回Clear
            if (startPointY != -1 && startPointX != -1)
            {
                clearStartPoint(startPointY, startPointX);
            }
            startPointY = startY;
            startPointX = startX;
        }

        /**
         * @param args
         */
        public Boolean isStartPoint(int startY, int startX)
        {
            if (map[startY, startX].level == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * @param args
         */
        public void clearStartPoint(int startY, int startX)
        {
            clearPoint(startY, startX);

            startPointY = -1;
            startPointX = -1;
        }

        /**
         * @param args
         */
        public void clearPoint(int startY, int startX)
        {
            map[startY, startX].wallFlg = false;
            map[startY, startX].level = -1;
        }

        /**
         * @param args
         */
        public void setEndPoint(int endY, int endX)
        {
            clearPoint(endY, endX);

            // 他EndPointを全部クリア
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    if (map[y,x].level == 0)
                    {
                        map[y,x].level = -1;
                    }
                }
            }
            map[endY,endX].level = 0;
        }

        /**
         * @param args
         */
        public void setEndPoints(int endY, int endX)
        {
            map[endY,endX].level = 0;
        }

        /**
         * @param args
         */
        public Boolean isEndPoint(int endY, int endX)
        {
            if(map[endY, endX].level == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }


        /**
         * @param args
         */
        public List<PointXY> searchStart()
        {
            if (startPointY == -1 || startPointX == -1)
            {
                return null;
            }

            // 検索優先エリアを取得
            areaFrom.pointY = startPointY;
            areaFrom.pointX = startPointX;
            areaTo.pointY = startPointY;
            areaTo.pointX = startPointX;

            // 全部EndPointをloop
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    if (map[y,x].level == 0)
                    {
                        areaFrom.pointY = Math.Min(areaFrom.pointY, y);
                        areaFrom.pointX = Math.Min(areaFrom.pointX, x);
                        areaTo.pointY = Math.Max(areaTo.pointY, y);
                        areaTo.pointX = Math.Max(areaTo.pointX, x);
                    }
                }
            }
/*          areaFrom.pointY = Math.Min(startPointY, endPointY);
            areaFrom.pointX = Math.Min(startPointX, endPointX);
            areaTo.pointY = Math.Max(startPointY, endPointY);
            areaTo.pointX = Math.Max(startPointX, endPointX);
*/
            PointXY temp = new PointXY(startPointY, startPointX);

            arrListNow.Clear();
            arrListOut.Clear();
            arrListNow.Add(temp);

            keyIndex = keyIndex + 1;
            map[startPointY,startPointX].keyIndex = keyIndex;

            Boolean endFlg = false;
            while (arrListNow.Count > 0)
            {
                if (searchSurrounding(arrListNow[0]))
                {
                    endFlg = true;
                }

                if (endFlg)
                {
                    // true:EndPoint 見つかれました
                    if (arrListNow.Count() < 2 || map[arrListNow[0].pointY,arrListNow[0].pointX].level != map[arrListNow[1].pointY,arrListNow[1].pointX].level)
                    {
                        // 検索Pointが最終Point || 現在のLevelは全部LOOP完了
                        break;
                    }
                }
                arrListNow.RemoveAt(0);

                if (arrListNow.Count == 0 && arrListOut.Count > 0)
                {

                    arrListNow = arrListOut;
                    arrListOut = new List<PointXY>();

                    int areaFromY = areaFrom.pointY;
                    int areaFromX = areaFrom.pointX;
                    int areaToY = areaTo.pointY;
                    int areaToX = areaTo.pointX;
                    foreach (PointXY pTemp in arrListNow)
                    {
                        areaFromY = Math.Min(areaFromY, pTemp.pointY);
                        areaFromX = Math.Min(areaFromX, pTemp.pointX);
                        areaToY = Math.Max(areaToY, pTemp.pointY);
                        areaToX = Math.Max(areaToX, pTemp.pointX);
                    }
                    areaFrom.pointY = areaFromY;
                    areaFrom.pointX = areaFromX;
                    areaTo.pointY = areaToY;
                    areaTo.pointX = areaToX;
                }


            }
            arrListNow.Clear();

            if (endFlg)
            {
                //			System.out.println("成功");
                return getPathList();
            }
            else
            {
                //			System.out.println("失敗");
                return null;
            }
        }

        /**
         * @param args
         * @return true:   false:終了
         */
        private Boolean searchSurrounding(PointXY temp)
        {
            int startY = temp.pointY;
            int startX = temp.pointX;
            int level = map[startY,startX].level + 1;

            Boolean endFlg = false;

            // -------------------左上piece
            //Param5  1:右下 2:下 3:左下 4:左 5:左上 6:上 7:右上 8:右
            endFlg = readyToSearchPointInit(startY, startX, startY - 1, startX - 1, 1, map[startY,startX].distance + distanceOnePiecePlus, level, endFlg);
            // -------------------上piece
            endFlg = readyToSearchPointInit(startY, startX, startY - 1, startX, 2, map[startY,startX].distance + distanceOnePiece, level, endFlg);
            // -------------------右上piece
            endFlg = readyToSearchPointInit(startY, startX, startY - 1, startX + 1, 3, map[startY,startX].distance + distanceOnePiecePlus, level, endFlg);
            // -------------------右piece
            endFlg = readyToSearchPointInit(startY, startX, startY, startX + 1, 4, map[startY,startX].distance + distanceOnePiece, level, endFlg);
            // -------------------右下piece
            endFlg = readyToSearchPointInit(startY, startX, startY + 1, startX + 1, 5, map[startY,startX].distance + distanceOnePiecePlus, level, endFlg);
            // -------------------下piece
            endFlg = readyToSearchPointInit(startY, startX, startY + 1, startX, 6, map[startY,startX].distance + distanceOnePiece, level, endFlg);
            // -------------------左下piece
            endFlg = readyToSearchPointInit(startY, startX, startY + 1, startX - 1, 7, map[startY,startX].distance + distanceOnePiecePlus, level, endFlg);
            // -------------------左piece
            endFlg = readyToSearchPointInit(startY, startX, startY, startX - 1, 8, map[startY,startX].distance + distanceOnePiece, level, endFlg);

            return endFlg;
        }

        /**
         * 当該点は有効点ですかどうか
         * @param args
         */
        private Boolean valuablePiece(int indexY, int indexX)
        {
            if (indexY >= 0 && indexX >= 0 && indexY < lengthY && indexX < lengthX && !map[indexY,indexX].wallFlg)
            {
                return true;
            }
            else
                return false;
        }

        /**
         * 当該点は対象範囲内点ですかどうか
         * @param args
         */
        private Boolean inAreaCheck(int indexY, int indexX)
        {
            if (indexY >= areaFrom.pointY && indexY <= areaTo.pointY && indexX >= areaFrom.pointX && indexX <= areaTo.pointX)
            {
                return true;
            }
            else
                return false;
        }

        /**
         * 発見される点初期化とListに追加
         * @param startY,startX 検索開始点
         * @param indexY,indexX 検索予定点
         * @param direct 方向  1:右下 2:下 3:左下 4:左 5:左上 6:上 7:右上 8:右
         * @param newDistance Start点までの距離
         * @param level 点のLevel
         */
        private Boolean readyToSearchPointInit(int startY, int startX, int indexY, int indexX, int direct, double newDistance, int level, Boolean endFlg)
        {

            if (valuablePiece(indexY, indexX))
            {
                // 有効Pointでしたら

                if (direct == 1 || direct == 3 || direct == 5 || direct == 7)
                {
                    //対角線
                    if (map[startY,indexX].wallFlg && map[indexY,startX].wallFlg)
                    {
                        //対角線通行不可
                        return endFlg;
                    }
                }

                if (map[indexY,indexX].keyIndex == keyIndex)
                {
                    // 計算したことがある 
                    if (map[indexY,indexX].distance > newDistance)
                    {
                        map[indexY,indexX].distance = newDistance;
                        map[indexY,indexX].direction = direct;
                    }
                    // TODO equals の場合

                }
                else
                {
                    // 計算したことがない
                    map[indexY,indexX].keyIndex = keyIndex;
                    map[indexY,indexX].direction = direct;
                    map[indexY,indexX].distance = newDistance;
                    if (map[indexY,indexX].level == 0)
                    {
                        // End Point
                        endFlg = true;

                        //発見した終了点をセット
                        endPointY = indexY;
                        endPointX = indexX;
                    }
                    else
                    {
                        map[indexY,indexX].level = level;
                    }

                    if (inAreaCheck(indexY, indexX))
                    {
                        // 範囲内
                        arrListNow.Add(new PointXY(indexY, indexX));
                    }
                    else
                    {
                        // 範囲外
                        arrListOut.Add(new PointXY(indexY, indexX));
                    }
                }
            }

            return endFlg;
        }

        /**
         * 完成パスを取得
         * @param args
         */
        private List<PointXY> getPathList()
        {
            List<PointXY> arrayPathList = new List<PointXY>();
            int pointY = endPointY;
            int pointX = endPointX;

            PointXY temp = new PointXY(pointY, pointX);
            arrayPathList.Add(temp);

            PieceArea piecearea;

            while (true)
            {
                piecearea = map[pointY,pointX];
                if (piecearea.level == 1)
                {
                    break;
                }

                // 1:右下 2:下 3:左下 4:左 5:左上 6:上 7:右上 8:右
                if (piecearea.direction == 1)
                {
                    pointY = pointY + 1;
                    pointX = pointX + 1;
                }
                else if (piecearea.direction == 2)
                {
                    pointY = pointY + 1;
                }
                else if (piecearea.direction == 3)
                {
                    pointY = pointY + 1;
                    pointX = pointX - 1;
                }
                else if (piecearea.direction == 4)
                {
                    pointX = pointX - 1;
                }
                else if (piecearea.direction == 5)
                {
                    pointY = pointY - 1;
                    pointX = pointX - 1;
                }
                else if (piecearea.direction == 6)
                {
                    pointY = pointY - 1;
                }
                else if (piecearea.direction == 7)
                {
                    pointY = pointY - 1;
                    pointX = pointX + 1;
                }
                else if (piecearea.direction == 8)
                {
                    pointX = pointX + 1;
                }
                else
                {
                    return null;
                }
                temp = new PointXY(pointY, pointX);
                arrayPathList.Insert(0, temp);
            }

            return arrayPathList;
        }

        /**
         * 地図を絵
         * @param args
         */
        private void drawMap()
        {
            /*		System.out.println("___dire______");
                    for (int y = 0;y <lengthY; y++) {
                        for (int x = 0;x <lengthX; x++) {
                            if (keyIndex == map[y,x].keyIndex) {
                                if (map[y,x].level == 1) {
                                    System.Console.Write("|S");
            //					} else if (map[y,x].level == 0) {
            //						System.Console.Write("|E");
                                } else {
                                    System.Console.Write("|" + map[y,x].direction);
                                }
                            } else if (map[y,x].wallFlg) {
                                System.Console.Write("|X");
                            } else {
                                System.Console.Write("| ");
                            }
                        }
                        System.out.println("|");
            //			System.out.println("_____________");
                    }
                    */

            System.Console.WriteLine("___level_____");
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    if (map[y,x].level == 1)
                    {
                        System.Console.Write("| S");
                    }
                    else if (map[y,x].level == 0)
                    {
                        System.Console.Write("| E");
                    }
                    else if (map[y,x].wallFlg)
                    {
                        System.Console.Write("| X");
                    }
                    else if (map[y,x].level != -1)
                    {
                        if (map[y,x].level < 10)
                        {
                            System.Console.Write("| " + map[y,x].level);
                        }
                        else
                        {
                            System.Console.Write("|" + map[y,x].level);
                        }
                    }
                    else
                    {
                        System.Console.Write("|  ");
                    }


                }
                System.Console.WriteLine("|");
                //			System.out.println("_____________");
            }
            /*		
                    System.out.println("___diste_____");
                    for (int y = 0;y <lengthY; y++) {
                        for (int x = 0;x <lengthX; x++) {
                            System.Console.Write("|" + map[y,x].distance);
                        }
                        System.out.println("|");
            //			System.out.println("_____________");
                    }*/
        }

    }
}
