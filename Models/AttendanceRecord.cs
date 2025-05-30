namespace Geno.Models
{
    public class AttendanceRecord
    {
        public int I3D { get; set; }
        public int EmployeeI3D { get; set; }
        public int IssuedByDeviceI3D { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime? TimeOverIn { get; set; }
        public DateTime? TimeOverOut { get; set; }
    }
}