namespace RentCycleWebApp.Models
{
    public class UserRideInfoForMobile
    {
        public UserRideInfoForMobile()
        {
            UserRideCosts = new HashSet<UserRideCost>();
        }

        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public string DeviceId { get; set; }
        public string ScanStartTime { get; set; }
        public string ScanEndTime { get; set; }
        public string UnlockTime { get; set; }
        public string LockTime { get; set; }
        public string StartPosition { get; set; }
        public string EndPosition { get; set; }
        public string RideDate { get; set; }
        public string RideCost { get; set; }
        public string RideRate { get; set; }
        public string RideDuration { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<UserRideCost> UserRideCosts { get; set; }
    }
}
