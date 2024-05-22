namespace LazyPan {
    public class MathUtil : Singleton<MathUtil> {
        public int[] GetRandNoRepeatIndex(int length, int resultCount) {
            return GetIndexs(length, resultCount);
        }

        private int[] GetIndexs(int arrayCount, int requireIntLength) {
            if (arrayCount < requireIntLength) {
                return null;
            }

            int[] tempArray = new int[arrayCount];
            for (int i = 0; i < arrayCount; i++) {
                tempArray[i] = i;
            }

            int[] tempRetArray = new int[requireIntLength]; //结果数组
            int tmpRequireIntLength = requireIntLength;
            int index = 0;
            while (tmpRequireIntLength > 0) {
                int randIndex = UnityEngine.Random.Range(0, tempArray.Length - 1);
                GetIndexNotEqualMinusOne(ref tempArray, ref tempRetArray, index, randIndex);
                index++;
                tmpRequireIntLength--;
            }

            return tempRetArray;
        }

        private void GetIndexNotEqualMinusOne(ref int[] parentIndexs, ref int[] insertIndexs, int insertIndex,
            int index) {
            if (parentIndexs[index] != -1) {
                //如果顺序数组当前的值不为-1则加入
                insertIndexs[insertIndex] = parentIndexs[index];
                parentIndexs[index] = -1;
            } else {
                GetIndexNotEqualMinusOne(ref parentIndexs, ref insertIndexs, insertIndex, (index + 1) % parentIndexs.Length);
            }
        }
    }
}