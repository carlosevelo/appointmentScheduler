public interface iApiService {
  List<AppointmentInfo> GetSchedule();
  void Start();
  void Stop();
  AppointmentRequest? GetAppointmentRequest();
  void Schedule(AppointmentInfoRequest appt);
}