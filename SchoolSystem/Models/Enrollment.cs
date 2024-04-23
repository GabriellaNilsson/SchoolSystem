using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
        [ForeignKey("Student")]
        public int FkStudentId { get; set; }
        public Student? Student { get; set; }
        
        [ForeignKey("Course")]
        public int FkCourseId { get; set; }
        public Course? Course { get; set; }
        
        [ForeignKey("Teacher")]
        public int FkTeacherId { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
