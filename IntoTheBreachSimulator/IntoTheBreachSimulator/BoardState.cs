using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public class BoardState
    {
        private PlayArea mPlayArea;

        private int mBuildingDamage;
        private int mPlayerDamage;
        private int mEnemyDamage;
        private int mEnemyCount;

        private int mPlayerHasAttacks;
        private int mPlayerHasMoves;

        private static int BUILDING_DAMAGE_SCORE = -500;
        private static int PLAYER_DAMAGE_SCORE = -1;
        private static int ENEMY_DAMAGE_SCORE = 5;
        private static int ENEMY_COUNT_SCORE = -50;

        public int Score => mBuildingDamage * BUILDING_DAMAGE_SCORE + mPlayerDamage * PLAYER_DAMAGE_SCORE +
            mEnemyDamage * ENEMY_DAMAGE_SCORE + mEnemyCount * ENEMY_COUNT_SCORE;

        public BoardState(PlayArea pPlayArea)
        {
            mPlayArea = pPlayArea;
            mBuildingDamage = mPlayArea.GetGridBuildingDamage();
            mPlayerDamage = mPlayArea.GetPlayerDamage();
            mEnemyDamage = mPlayArea.GetEnemyDamage();
            mEnemyCount = mPlayArea.GetEnemyCount();

            mPlayerHasAttacks = mPlayArea.GetPlayerHasAttacks();
            mPlayerHasMoves = mPlayArea.GetPlayerHasMoves();
        }

        public bool IsBetterThan(BoardState pOther)
        {
            if (Score > pOther.Score)
                return true;
            if (Score < pOther.Score)
                return false;

            if (mPlayerHasAttacks > pOther.mPlayerHasAttacks)
                return true;
            else if (mPlayerHasAttacks < pOther.mPlayerHasAttacks)
                return false;
            else
            {
                if (mPlayerHasMoves > pOther.mPlayerHasMoves)
                    return true;
            }
            return false;
        }
    }
}
