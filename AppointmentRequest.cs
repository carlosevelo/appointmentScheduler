public class AppointmentRequest {
  public int requestId {get; set;}
  public int personId {get; set;}
  public List<DateTime>? preferredDays {get; set;}
  public List<Doctor>? preferredDocs {get; set;}
  public bool isNew {get; set;}
}