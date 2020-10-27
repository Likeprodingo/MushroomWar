namespace Spawn
{
    public struct Wave
    {
        public int WolfCount { get; }

        public int OgrCount { get; }

        public int ShamanCount { get; }

        public Wave(int wolfCount, int ogrCount, int shamanCount)
        {
            WolfCount = wolfCount;
            OgrCount = ogrCount;
            ShamanCount = shamanCount;
        }
    }
}