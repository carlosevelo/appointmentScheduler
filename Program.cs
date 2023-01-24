namespace AppoinmentScheduler
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Starting Scheduler...");
      Console.WriteLine("Initializing Schedule...");
      Scheduler scheduler = new Scheduler(new ApiService());
      
      // foreach (AppointmentInfo appt in scheduler.doctorSchedule[Doctor.doc1]) {
      //   Console.WriteLine(appt.doctorId);
      // }
      // foreach (AppointmentInfo appt in scheduler.personSchedule[1]) {
      //   Console.WriteLine(appt.personId);
      // }
      AppointmentRequest? request = scheduler.GetNewRequest();
      if (request != null) {
        scheduler.ScheduleAppointment(request);
      }
      // while(request != null) {
      //   scheduler.ScheduleAppointment(request);
      //   request = scheduler.GetNewRequest();
      // }
    }
  }
}