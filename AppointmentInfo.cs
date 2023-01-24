public class AppointmentInfo {
  public Doctor doctorId {get; set;}
  public int personId {get; set;}
  public DateTime appointmentTime {get; set;}
  public bool isNewPatientAppointment {get; set;}

  public AppointmentInfo(Doctor doctorId, int personId, DateTime time, bool isNew) {
    this.doctorId = doctorId;
    this.personId = personId;
    this.appointmentTime = time;
    this.isNewPatientAppointment = isNew;
  }
}