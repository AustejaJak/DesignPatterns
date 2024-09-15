namespace BloonLibrary.Extensions
{
    [System.Serializable]
    public class VectorExtension // Serializable coordinate class, since Point2D cannot be serialized.
    {
        public float X { get; set; }
        public float Y { get; set; }

        public VectorExtension(float X = 0, float Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}