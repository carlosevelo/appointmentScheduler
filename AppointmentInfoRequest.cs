public class AppointmentInfoRequest {
  Doctor doctorId;
  int personId;
  DateTime appointmentTime;
  bool isNewPatientAppointment;
  int requestId;

  public AppointmentInfoRequest(AppointmentRequest apptInfo, Doctor doctorId, DateTime appointmentTime) {
    this.doctorId = doctorId;
    this.personId = apptInfo.personId;
    this.appointmentTime = appointmentTime;
    this.isNewPatientAppointment = apptInfo.isNew;
    this.requestId = apptInfo.requestId;
  }
}