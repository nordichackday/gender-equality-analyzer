namespace Core.DataContext
{
    public class Face
    {
        public int Id { get; set; }
        public string FaceId { get; set; }
        public virtual Article Article { get; set; }
        public Gender Gender { get; set; }
        public int? Age { get; set; }
        public double SmileFactor { get; set; }
        public double HeadRoll { get; set; }
        public double HeadYaw { get; set; }
        public double HeadPitch { get; set; }
        public double MoustacheFactor { get; set; }
        public double BeardFactor { get; set; }
        public double SideburnsFactor { get; set; }

    }
}