public static class TensorUtility
{
    public static T[] CreateArray<T>(int[] dimensions) {
        int totalLen = 1;
        for(int i = 0; i < dimensions.Length; i++) {
            totalLen *= dimensions[i];
        }
        return new T[totalLen];
    }
}
